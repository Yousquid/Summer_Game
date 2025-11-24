using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("生成设置")]
    public GameObject enemyPrefab;   // 敌人预制体（Prefab）
    public Transform player;         // 玩家
    public int maxEnemiesInScene = 3; // 场景里允许存在的最大敌人数

    [Header("生成范围（以玩家为中心）")]
    public float minSpawnRadius = 5f;   // 离玩家至少多远
    public float maxSpawnRadius = 10f;  // 离玩家最多多远

    [Header("检查间隔")]
    public float checkInterval = 1f;    // 每隔多少秒检查一次敌人数量

    private float checkTimer = 0f;

    void Start()
    {
        // 如果没手动赋值，就自动找 Tag 为 Player 的对象
        if (player == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (player == null || enemyPrefab == null) return;

        checkTimer += Time.deltaTime;
        if (checkTimer >= checkInterval)
        {
            checkTimer = 0f;
            CheckAndSpawnEnemies();
        }
    }

    void CheckAndSpawnEnemies()
    {
        // 统计场景中有多少个 Tag 为 "Enemy" 的敌人
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int currentCount = enemies.Length;

        if (currentCount >= maxEnemiesInScene)
            return; // 已经达到或超过上限，就不生成

        // 还可以生成多少只
        int canSpawn = maxEnemiesInScene - currentCount;

        // 这次随机生成 1~3 只，但不能超过 canSpawn
        int spawnCount = Random.Range(1, 4); // 上界不含，所以是 1~3
        spawnCount = Mathf.Min(spawnCount, canSpawn);

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = GetRandomPositionAroundPlayer();
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }

    /// <summary>
    /// 在玩家周围 [minSpawnRadius, maxSpawnRadius] 的范围内随机一个点
    /// </summary>
    Vector3 GetRandomPositionAroundPlayer()
    {
        // 随机一个角度
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

        // 随机一个半径
        float radius = Random.Range(minSpawnRadius, maxSpawnRadius);

        // 极坐标 → 笛卡尔坐标
        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

        Vector3 pos = player.position + (Vector3)offset;
        pos.z = 0f; // 2D 场景一般固定 z

        return pos;
    }
}
