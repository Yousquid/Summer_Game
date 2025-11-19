using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Transform targetPlayer;

    private Rigidbody2D rb;
    private PlayerZone stretchTracker;   // 如果敌人在 StretchZone 里，也会被扭曲移动

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stretchTracker = GetComponent<PlayerZone>();  // 如果敌人也有 PlayerZone
    }

    void FixedUpdate()
    {
        if (targetPlayer == null) return;

        // 1. 朝玩家方向移动
        Vector2 dir = ((Vector2)(targetPlayer.position - transform.position)).normalized;
        Vector2 delta = dir * moveSpeed * Time.fixedDeltaTime;

        // 2. 如果在 StretchZone 里，就扭曲移动
        if (stretchTracker != null && stretchTracker.currentZone != null)
        {
            delta = stretchTracker.currentZone.WarpDisplacement(rb.position, delta);
        }

        // 3. 移动
        rb.MovePosition(rb.position + delta);
    }
}
