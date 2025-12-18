using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [Header("生成设置")]
    public GameObject[] enemyPrefabs;
    public Transform player;
    public int maxEnemiesInScene = 3;

    [Header("生成范围（以玩家为中心）")]
    public float minSpawnRadius = 5f;
    public float maxSpawnRadius = 10f;

    [Header("检查间隔")]
    public float checkInterval = 1f;

    private float checkTimer = 0f;

    void OnEnable()
    {
        BindPlayer();
        checkTimer = 0f; // 重进场景时重置计时器，避免异常间隔
    }

    void Start()
    {
        BindPlayer();
    }

    void BindPlayer()
    {
        GameObject p = GameObject.FindWithTag("Player");
        player = (p != null) ? p.transform : null;
        // Debug.Log("[EnemySpawner] BindPlayer: " + (player ? player.name : "NOT FOUND"));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            PlayerMovement.score = 0;
            PlayerMovement.lives = 3;
            Time.timeScale = 1;
        }

        // Player 可能比 Spawner 晚生成：找不到就持续尝试
        if (player == null)
        {
            BindPlayer();
            if (player == null) return;
        }

        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
            return;

        checkTimer += Time.deltaTime;
        if (checkTimer >= checkInterval)
        {
            checkTimer = 0f;
            CheckAndSpawnEnemies();
        }

       
    }

    void CheckAndSpawnEnemies()
    {
        int currentCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (currentCount >= maxEnemiesInScene) return;

        int canSpawn = maxEnemiesInScene - currentCount;
        int spawnCount = Mathf.Min(Random.Range(1, 4), canSpawn);

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = GetRandomPositionAroundPlayer();
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Instantiate(prefab, spawnPos, Quaternion.identity);
        }
    }

    Vector3 GetRandomPositionAroundPlayer()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float radius = Random.Range(minSpawnRadius, maxSpawnRadius);
        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

        Vector3 pos = player.position + (Vector3)offset;
        pos.z = 0f;
        return pos;
    }
}
