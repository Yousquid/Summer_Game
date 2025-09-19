using UnityEngine;

public class BeltAnimation : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Collider2D targetCollider;
    public Transform teleportPosition;

    private void Start()
    {
        targetCollider = GameObject.FindWithTag("BeltEnd").GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        if ( Input.GetKey(KeyCode.D) && !WorkManager.isLightOn)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == targetCollider)
        {
            transform.position = teleportPosition.position;
        }
    }

}
