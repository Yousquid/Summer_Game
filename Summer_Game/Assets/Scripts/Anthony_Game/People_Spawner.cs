using UnityEngine;

public class People_Spawner : MonoBehaviour
{
    public float spawnY = -7f;
    public new Vector2 spawnXRange = new Vector2(-7f,7f);
    public float spawnInterval;
    private float timer = 0f;
    public float randomTimeRange = 2;
    public GameObject personObject;

    void Start()
    {
        spawnInterval = Random.Range(.2F, randomTimeRange);

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpwanPerson();
            spawnInterval = Random.Range(.2F, randomTimeRange);
            timer = 0f;
        }
    }

    void SpwanPerson()
    {
        float xPos = Random.Range(spawnXRange.x, spawnXRange.y);
        Vector3 spawnPos = new Vector3(xPos, spawnY, 0f);
        Instantiate(personObject, spawnPos, Quaternion.identity);
    }
}
