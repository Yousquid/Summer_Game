using UnityEngine;

public class Car_Spawner : MonoBehaviour
{
    [Header("车生成参数")]
    public GameObject carPrefab;      // 车的预制体
    public float spawnY = 0f;         // 生成高度
    public float spawnX = 0f; // 生成范围
    public float spawnInterval = 2f;  // 生成间隔时间（秒）
    public bool moveRight = false;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnCar();
            timer = 0f;
        }
    }

    void SpawnCar()
    {
        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0f);

        GameObject car = Instantiate(carPrefab, spawnPos, Quaternion.identity);

        // 4. 根据方向翻转外观（假设 Sprite 右边是默认朝向）
        if (!moveRight)
        {
            Vector3 scale = car.transform.localScale;
            scale.x *= -1;
            car.transform.localScale = scale;
        }

        // 5. 赋值移动脚本
        CarMover mover = car.GetComponent<CarMover>();
        if (mover != null)
        {
            mover.moveRight = moveRight;
        }
    }
}
