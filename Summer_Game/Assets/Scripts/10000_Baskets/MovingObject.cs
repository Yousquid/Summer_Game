using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float moveSpeed = .75f;          
    public Collider2D targetCollider;   

    private bool isColliding = false;

    private void Start()
    {
        targetCollider = GameObject.FindWithTag("Belt").GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        if (isColliding && Input.GetKey(KeyCode.D) && !WorkManager.isLightOn)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == targetCollider)
        {
            isColliding = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider == targetCollider)
        {
            isColliding = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider == targetCollider)
        {
            isColliding = false;
        }
    }
}
