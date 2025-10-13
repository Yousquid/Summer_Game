using UnityEngine;

public class CarMover : MonoBehaviour
{
    public float speed = 3f;        // 车速
    public bool moveRight = true;   // 是否向右

    private Rigidbody2D rb;

    private void Start()
    {
        Destroy(gameObject, 5f);

    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;       // 让车不受重力影响
        rb.freezeRotation = true;   // 防止旋转
    }

    void FixedUpdate()
    {
        // 使用 MovePosition 平滑移动
        float dir = moveRight ? 1f : -1f;
        Vector2 move = new Vector2(dir * speed * Time.fixedDeltaTime, 0f);
        rb.MovePosition(rb.position + move);
    }

}
