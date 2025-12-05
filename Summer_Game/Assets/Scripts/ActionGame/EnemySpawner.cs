using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("生成设置")]
    public GameObject[] enemyPrefabs;   
    public Transform player;            // 玩家
    public int maxEnemiesInScene = 3;   // 场景最大敌人数量

    [Header("生成范围（以玩家为中心）")]
    public float minSpawnRadius = 5f;
    public float maxSpawnRadius = 10f;

    [Header("检查间隔")]
    public float checkInterval = 1f;

    private float checkTimer = 0f;

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (player == null || enemyPrefabs == null || enemyPrefabs.Length == 0)
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
        // 当前场景中所有敌人
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int currentCount = enemies.Length;

        if (currentCount >= maxEnemiesInScene)
            return;

        // 这次最多能再生成多少
        int canSpawn = maxEnemiesInScene - currentCount;

        // 本次生成 1~3 个之间随机
        int spawnCount = Random.Range(1, 4);
        spawnCount = Mathf.Min(spawnCount, canSpawn);

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
