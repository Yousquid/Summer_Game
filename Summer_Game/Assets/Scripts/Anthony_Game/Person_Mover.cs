using UnityEngine;

public class Person_Mover : MonoBehaviour
{
    public float speed = 2f;
    private Car_Spawner[] spawners;
    private bool isFool;
    private bool isGood;
    private bool isChancer;
    private bool isLooker;
    private bool canMove = true;
    private bool canMoveAgain =false;
    private float randomMoveTime;

    private bool canStop = false;
    private float carCount;

    private float chancerMoveTimer;

    private Red_Light light;
    private void Start()
    {
        light = GameObject.FindWithTag("Object").GetComponent<Red_Light>();

        GameObject manager = GameObject.FindWithTag("Manager");

        spawners = manager.GetComponents<Car_Spawner>();

        int randomFoolNumber = Random.Range(0, 6);

        randomMoveTime = Random.Range(0, 0.6f);

        if (randomFoolNumber <= 2)
        {
            isLooker = true;
        }
        else if (randomFoolNumber ==5)
        {
            isFool = true;
        }
        else if (randomFoolNumber == 3)
        {
            isChancer = true;
            chancerMoveTimer = Random.Range(2f, 10f);
        }
        else if (randomFoolNumber == 4)
        {
            isGood = true;
        }
    }
    void Update()
    {
        if (canStop)
        {
            randomMoveTime -= Time.deltaTime;
        }
        if (isChancer)
        {
            chancerMoveTimer -= Time.deltaTime;
        }

        if (isChancer && chancerMoveTimer <= 0)
        {
            canMoveAgain = true;
            canMove = true;
        }

        if (randomMoveTime <= 0 && !canMoveAgain && !isFool)
        {
            canMove = false;
        }
        if (canMove)
        {
            float v = 1f;

            transform.position += (Vector3)(transform.up * v * speed * Time.deltaTime);
        }

        CheckIfCanGo();


        if (isGood && light.timer >= 30f)
        {
            canMove = true;
            canMoveAgain = true;
        }

        if (canMoveAgain && !isFool && !canMove && isLooker)
        {
            canMove = true;
        }

    }

    void CheckIfCanGo()
    {
        if (isLooker)
        {
            foreach (var spawner in spawners)
            {
                carCount += spawner.spawnInterval;
            }
            if (carCount >= 24f)
            {
                canMoveAgain = true;
                canMove = true;
            }
            carCount = 0;
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Start") && !isFool && !canMoveAgain)
        {
            canStop = true;
        }

        if (collision.CompareTag("Select_one")|| collision.CompareTag("Select_two"))
        {
            Destroy(gameObject);
            if (collision.CompareTag("Select_one"))
            {
                People_Spawner.score += 1;

            }
        }
    }
}
