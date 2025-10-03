using UnityEngine;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class GridEditorWindow : EditorWindow
{
    private int defaultWidth = 10;
    private int defaultHeight = 10;
    private int cellSize = 20;

    private List<LevelData> levels = new List<LevelData>();
    private int currentLevelIndex = 0;

    private LevelData currentLevel;

    [MenuItem("Tools/Grid Level Editor")]
    public static void OpenWindow()
    {
        GetWindow<GridEditorWindow>("Grid Editor");
    }

    private void OnEnable()
    {
        if (levels.Count == 0)
        {
            CreateNewLevel(defaultWidth, defaultHeight);
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("关卡设置", EditorStyles.boldLabel);

        // 默认大小输入框
        defaultWidth = EditorGUILayout.IntField("默认宽度", defaultWidth);
        defaultHeight = EditorGUILayout.IntField("默认高度", defaultHeight);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("新建关卡", GUILayout.Height(25)))
        {
            CreateNewLevel(defaultWidth, defaultHeight);
        }

        if (GUILayout.Button("保存当前关卡", GUILayout.Height(25)))
        {
            SaveCurrentLevel();
        }

        if (GUILayout.Button("切换关卡", GUILayout.Height(25)))
        {
            if (levels.Count > 0)
            {
                currentLevelIndex = (currentLevelIndex + 1) % levels.Count;
                currentLevel = levels[currentLevelIndex];
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        if (currentLevel == null) return;

        // 显示当前关卡信息
        EditorGUILayout.LabelField($"当前关卡：{currentLevelIndex + 1}/{levels.Count}");
        EditorGUILayout.LabelField($"尺寸：{currentLevel.width} × {currentLevel.height}");

        EditorGUILayout.Space();

        // 网格绘制
        for (int y = 0; y < currentLevel.height; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < currentLevel.width; x++)
            {
                int value = currentLevel.GetCell(x, y);

                Color oldColor = GUI.backgroundColor;
                GUI.backgroundColor = GetColorForValue(value);

                if (GUILayout.Button(value.ToString(), GUILayout.Width(cellSize), GUILayout.Height(cellSize)))
                {
                    int nextValue = (value + 1) % 5; 
                    currentLevel.SetCell(x, y, nextValue);
                }

                GUI.backgroundColor = oldColor;
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    private void CreateNewLevel(int w, int h)
    {
        if (w <= 0 || h <= 0)
        {
            Debug.LogError("关卡尺寸必须大于 0！");
            return;
        }

        LevelData newLevel = new LevelData(w, h);
        levels.Add(newLevel);
        currentLevel = newLevel;
        currentLevelIndex = levels.Count - 1;
    }

    private void SaveCurrentLevel()
    {
        string path = EditorUtility.SaveFilePanel("保存关卡为 JSON", Application.dataPath, $"LevelData_{levels.Count}", "json");
        if (!string.IsNullOrEmpty(path))
        {
            string json = JsonUtility.ToJson(currentLevel, true);
            System.IO.File.WriteAllText(path, json);
            AssetDatabase.Refresh();
            Debug.Log("关卡已保存到: " + path);
        }
    }

    private Color GetColorForValue(int value)
    {
        switch (value)
        {
            case 0: return Color.white;   // 空地
            case 1: return Color.black;   // 墙
            case 2: return Color.green;   // 起点
            case 3: return Color.red;     // 终点
            case 4: return Color.yellow;  // 陷阱
            default: return Color.gray;
        }
    }
}