using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    public float speed = 12f;
    public float lifeTime = 3f;

    private Rigidbody2D rb;
    private Vector2 moveDir;

    private StretchZone currentZone;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 direction)
    {
        moveDir = direction.normalized;
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        // 计算本来要移动的位移
        Vector2 delta = moveDir * speed * Time.fixedDeltaTime;

        // 如果在 StretchZone 里，warp 位移
        if (currentZone != null)
        {
            delta = currentZone.WarpDisplacement(rb.position, delta);
        }

        // 最终移动
        rb.MovePosition(rb.position + delta);
    }

    // ---- 接收 StretchZone 的通知 ----
    void OnTriggerEnter2D(Collider2D other)
    {
        var zone = other.GetComponent<StretchZone>();
        if (zone != null)
        {
            currentZone = zone;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var zone = other.GetComponent<StretchZone>();
        if (zone != null && currentZone == zone)
        {
            currentZone = null;
        }
    }
}
