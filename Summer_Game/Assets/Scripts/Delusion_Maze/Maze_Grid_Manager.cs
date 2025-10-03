using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Maze_Grid_Manager : MonoBehaviour
{
    [Header("JSON 关卡文件 (放在 Resources 文件夹里)")]
    public string levelFileName = "LevelData"; // 不要加 .json 后缀
    private LevelData levelData;

    private int currentLevel= 1;

    [Header("格子预制体")]
    public GameObject emptyPrefab;   // 0 = 空地（也作为墙移动时的底层）
    public GameObject wallPrefab;    // 1 = 墙
    public GameObject startPrefab;   // 2 = 起点
    public GameObject endPrefab;     // 3 = 终点
    public GameObject trapPrefab;    // 4 = 陷阱

    [Header("格子设置")]
    public float cellSize = 1f;

    // baseObjects 保存每个格子的“基础物体”（empty/start/end/trap）
    // wallObjects 单独保存墙对象（只有当该格初始是墙时才有值）
    private GameObject[,] baseObjects;
    private GameObject[,] wallObjects;
    private List<Vector2Int> wallPositions = new List<Vector2Int>(); // 当前场景中墙的逻辑坐标

    private Vector2Int startPosition; // 起点位置
    private Vector2Int? endPosition;             // 当前终点位置
    private List<Vector2Int> trapPositions = new List<Vector2Int>(); // 当前陷阱位置列表


    void Start()
    {
        LoadLevelFromJson();
        GenerateLevel();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) ShiftMovables(Vector2Int.up);
        if (Input.GetKeyDown(KeyCode.S)) ShiftMovables(Vector2Int.down);
        if (Input.GetKeyDown(KeyCode.A)) ShiftMovables(Vector2Int.left);
        if (Input.GetKeyDown(KeyCode.D)) ShiftMovables(Vector2Int.right);
        EndGameDetection();
    }

    void LoadLevelFromJson()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(levelFileName);
        if (jsonFile == null)
        {
            Debug.LogError("未找到 JSON 文件: " + levelFileName);
            return;
        }
        levelData = JsonUtility.FromJson<LevelData>(jsonFile.text);
        Debug.Log("成功加载关卡: " + levelFileName);
    }


    void GenerateLevel()
    {
        if (levelData == null) return;

        int rows = levelData.height;
        int cols = levelData.width;

        baseObjects = new GameObject[rows, cols];
        wallObjects = new GameObject[rows, cols];
        wallPositions.Clear();
        trapPositions.Clear();
        endPosition = null;

        float offsetX = -(cols * cellSize) / 2f + cellSize / 2f;
        float offsetY = -(rows * cellSize) / 2f + cellSize / 2f;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                int value = levelData.GetCell(x, y);
                Vector3 pos = new Vector3(x * cellSize + offsetX, y * cellSize + offsetY, 0f);

                if (value == 1) // 墙
                {
                    if (emptyPrefab != null)
                        baseObjects[y, x] = Instantiate(emptyPrefab, pos, Quaternion.identity, transform);
                    if (wallPrefab != null)
                    {
                        GameObject w = Instantiate(wallPrefab, pos, Quaternion.identity, transform);
                        wallObjects[y, x] = w;
                        wallPositions.Add(new Vector2Int(x, y));
                    }
                }
                else
                {
                    GameObject prefab = GetPrefabForValue(value);
                    if (prefab != null)
                    {
                        baseObjects[y, x] = Instantiate(prefab, pos, Quaternion.identity, transform);

                        if (value == 2) startPosition = new Vector2Int(x, y);
                        if (value == 3) endPosition = new Vector2Int(x, y);
                        if (value == 4) trapPositions.Add(new Vector2Int(x, y));
                    }
                }
            }
        }
    }

    private GameObject GetPrefabForValue(int value)
    {
        switch (value)
        {
            case 0: return emptyPrefab;
            case 1: return null; // 墙单独处理
            case 2: return startPrefab;
            case 3: return endPrefab;
            case 4: return trapPrefab;
            default: return emptyPrefab;
        }
    }

    private void EndGameDetection()
    {
        if (startPosition == endPosition)
        {
            currentLevel += 1;
            levelFileName = $"LevelData_{currentLevel}";
            DestroyAllChildren(transform);
            LoadLevelFromJson();
            GenerateLevel();
        }
    }

    private void DestroyAllChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            GameObject child = parent.GetChild(i).gameObject;
            Destroy(child);
        }
    }
    private void ShiftMovables(Vector2Int dir)
    {
        if (levelData == null) return;

        int rows = levelData.height;
        int cols = levelData.width;

        // ========= 0) 检查墙是否会撞到起点 =========
        foreach (var old in wallPositions)
        {
            int newX = (old.x + dir.x + cols) % cols;
            int newY = (old.y + dir.y + rows) % rows;

            if (newX == startPosition.x && newY == startPosition.y)
            {
                Debug.Log("阻止移动：墙会撞上起点");
                return;
            }
        }

        float offsetX = -(cols * cellSize) / 2f + cellSize / 2f;
        float offsetY = -(rows * cellSize) / 2f + cellSize / 2f;

        // ========= 1) 清理旧物体 =========
        foreach (var p in wallPositions)
        {
            if (wallObjects[p.y, p.x] != null)
            {
                Destroy(wallObjects[p.y, p.x]);
                wallObjects[p.y, p.x] = null;
            }
        }
        foreach (var t in trapPositions)
        {
            if (baseObjects[t.y, t.x] != null)
            {
                Destroy(baseObjects[t.y, t.x]);
                baseObjects[t.y, t.x] = null;
            }
        }
        if (endPosition.HasValue)
        {
            var ep = endPosition.Value;
            if (baseObjects[ep.y, ep.x] != null)
            {
                Destroy(baseObjects[ep.y, ep.x]);
                baseObjects[ep.y, ep.x] = null;
            }
        }

        // ========= 2) 计算新位置并重建 =========
        List<Vector2Int> newWallPositions = new List<Vector2Int>();
        GameObject[,] newWallObjects = new GameObject[rows, cols];

        // 墙
        foreach (var old in wallPositions)
        {
            int newX = (old.x + dir.x + cols) % cols;
            int newY = (old.y + dir.y + rows) % rows;

            Vector3 newPos = new Vector3(newX * cellSize + offsetX, newY * cellSize + offsetY, 0f);
            GameObject w = Instantiate(wallPrefab, newPos, Quaternion.identity, transform);
            newWallObjects[newY, newX] = w;
            newWallPositions.Add(new Vector2Int(newX, newY));
        }

        // 陷阱
        List<Vector2Int> newTrapPositions = new List<Vector2Int>();
        foreach (var old in trapPositions)
        {
            int newX = (old.x + dir.x + cols) % cols;
            int newY = (old.y + dir.y + rows) % rows;

            Vector3 newPos = new Vector3(newX * cellSize + offsetX, newY * cellSize + offsetY, 0f);
            GameObject t = Instantiate(trapPrefab, newPos, Quaternion.identity, transform);
            baseObjects[newY, newX] = t;
            newTrapPositions.Add(new Vector2Int(newX, newY));
        }

        // 终点
        Vector2Int? newEndPosition = null;
        if (endPosition.HasValue)
        {
            int newX = (endPosition.Value.x + dir.x + cols) % cols;
            int newY = (endPosition.Value.y + dir.y + rows) % rows;

            Vector3 newPos = new Vector3(newX * cellSize + offsetX, newY * cellSize + offsetY, 0f);
            GameObject e = Instantiate(endPrefab, newPos, Quaternion.identity, transform);
            baseObjects[newY, newX] = e;
            newEndPosition = new Vector2Int(newX, newY);
        }

        // ========= 3) 更新缓存 =========
        wallObjects = newWallObjects;
        wallPositions = newWallPositions;
        trapPositions = newTrapPositions;
        endPosition = newEndPosition;
    }
}
