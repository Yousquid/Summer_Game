using UnityEngine;

public class Car_Spawner : MonoBehaviour
{
    [Header("车生成参数")]
    public GameObject carPrefab;      // 车的预制体
    public float spawnY = 0f;         // 生成高度
    public float spawnX = 0f; // 生成范围
    public float spawnInterval;  // 生成间隔时间（秒）
    public bool moveRight = false;

    public Red_Light light;
    public int randomTimeRange;

    private float timer = 0f;


    private void Start()
    {
        spawnInterval = Random.Range(1F, randomTimeRange);
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval && light.timer < 30f)
        {
            SpawnCar();
            timer = 0f;
            spawnInterval = Random.Range(1F, randomTimeRange);
        }

        if (light.timer >= 30f)
        {
            spawnInterval = 12f;
        }
    }

    void SpawnCar()
    {
        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0f);

        int randomDir = Random.Range(0, 2);

        GameObject car;
        car = null;
        if (randomDir == 0)
        {
            moveRight = true;
        }
        else if (randomDir == 1)
        { 
            moveRight= false;
        }
        // 4. 根据方向翻转外观（假设 Sprite 右边是默认朝向）
        if (!moveRight)
        {
            spawnPos = new Vector3(-spawnX, spawnY, 0f);

             car = Instantiate(carPrefab, spawnPos, Quaternion.identity);

            Vector3 scale = car.transform.localScale;
            scale.x *= -1;
            car.transform.localScale = scale;


        }
        else if (moveRight)
        {
             car = Instantiate(carPrefab, spawnPos, Quaternion.identity);

        }

        // 5. 赋值移动脚本
        CarMover mover = car.GetComponent<CarMover>();
        if (mover != null)
        {
            mover.moveRight = moveRight;
        }
    }
}
