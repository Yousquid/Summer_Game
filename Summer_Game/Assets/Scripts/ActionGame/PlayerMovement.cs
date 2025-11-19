using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 input;
    private PlayerZone stretchTracker;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stretchTracker = GetComponent<PlayerZone>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        input = new Vector2(x, y).normalized;
    }

    void FixedUpdate()
    {
        // 本来要移动的位移
        Vector2 delta = input * moveSpeed * Time.fixedDeltaTime;

        // 如果在拉伸区，就让拉伸区来“修改这段位移”
        if (stretchTracker != null && stretchTracker.currentZone != null)
        {
            delta = stretchTracker.currentZone.WarpDisplacement(rb.position, delta);
        }

        // 用 MovePosition 实际移动（不需要 velocity 也能玩）
        rb.MovePosition(rb.position + delta);
    }
}
