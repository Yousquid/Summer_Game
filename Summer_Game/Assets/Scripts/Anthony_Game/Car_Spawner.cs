using UnityEngine;

public class Car_Spawner : MonoBehaviour
{
    [Header("�����ɲ���")]
    public GameObject carPrefab;      // ����Ԥ����
    public float spawnY = 0f;         // ���ɸ߶�
    public float spawnX = 0f; // ���ɷ�Χ
    public float spawnInterval = 2f;  // ���ɼ��ʱ�䣨�룩
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

        // 4. ���ݷ���ת��ۣ����� Sprite �ұ���Ĭ�ϳ���
        if (!moveRight)
        {
            Vector3 scale = car.transform.localScale;
            scale.x *= -1;
            car.transform.localScale = scale;
        }

        // 5. ��ֵ�ƶ��ű�
        CarMover mover = car.GetComponent<CarMover>();
        if (mover != null)
        {
            mover.moveRight = moveRight;
        }
    }
}
