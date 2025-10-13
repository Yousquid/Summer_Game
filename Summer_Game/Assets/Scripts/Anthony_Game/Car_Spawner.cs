using UnityEngine;

public class Car_Spawner : MonoBehaviour
{
    [Header("�����ɲ���")]
    public GameObject carPrefab;      // ����Ԥ����
    public float spawnY = 0f;         // ���ɸ߶�
    public float spawnX = 0f; // ���ɷ�Χ
    public float spawnInterval;  // ���ɼ��ʱ�䣨�룩
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
        // 4. ���ݷ���ת��ۣ����� Sprite �ұ���Ĭ�ϳ���
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

        // 5. ��ֵ�ƶ��ű�
        CarMover mover = car.GetComponent<CarMover>();
        if (mover != null)
        {
            mover.moveRight = moveRight;
        }
    }
}
