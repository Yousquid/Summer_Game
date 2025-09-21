using UnityEngine;

public class BeltAnimation : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Collider2D targetCollider;
    public Collider2D targetCollider2;

    public Transform teleportPosition;
    public Transform teleportPosition2;


    private void Start()
    {
        targetCollider = GameObject.FindWithTag("BeltEnd").GetComponent<BoxCollider2D>();
        targetCollider2 = GameObject.FindWithTag("BeltEnd2").GetComponent<BoxCollider2D>();

    }
    void Update()
    {
        if ( Input.GetKey(KeyCode.D) && !WorkManager.isLightOn)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) && !WorkManager.isLightOn)
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == targetCollider)
        {
            transform.position = teleportPosition.position;
        }
        if (collision == targetCollider2)
        {
            transform.position = teleportPosition2.position;
        }
    }

}
