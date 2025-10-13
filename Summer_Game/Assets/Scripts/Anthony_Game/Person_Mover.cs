using UnityEngine;

public class Person_Mover : MonoBehaviour
{
    public float speed = 2f;
    private Car_Spawner[] spawners;
    private bool isFool;
    private bool canMove = true;
    private bool canMoveAgain =false;
    private void Start()
    {
        GameObject manager = GameObject.FindWithTag("Manager");

        spawners = manager.GetComponents<Car_Spawner>();

        int randomFoolNumber = Random.Range(0, 6);

        if (randomFoolNumber < 5)
        {
            isFool = false;
        }
        else if (randomFoolNumber >=5)
        {
            isFool = true;
        }
    }
    void Update()
    {
        if (canMove)
        {
            float v = 1f;

            transform.position += (Vector3)(transform.up * v * speed * Time.deltaTime);
        }

        for (int i = 0; i < spawners.Length; i++)
        {
            if (spawners[i].randomTimeRange > 4f)
            { 
                continue;
            }
            if (spawners[i].randomTimeRange > 4f && i == spawners.Length)
            { 
                canMoveAgain = true;
            }
        }

        if (canMoveAgain && !isFool && !canMove)
        {
            canMove = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Start") && !isFool && !canMoveAgain)
        { 
            canMove = false;
        }
    }
}
