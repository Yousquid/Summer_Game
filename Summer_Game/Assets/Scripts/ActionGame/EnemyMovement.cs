using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Transform targetPlayer;

    private Rigidbody2D rb;
    private PlayerZone stretchTracker;   // 如果敌人在 StretchZone 里，也会被扭曲移动

    public int life = 8;
    [Header("Knockback")]
    public float knockbackDistance = 1.5f;   // 被击退的总距离
    public float knockbackDuration = 0.1f;   // 击退持续时间（越小越“瞬间”）
    private bool isKnockback = false;
    private Vector2 knockbackDir;
    private float knockbackTimeRemaining;

    public GameObject particle;
    void Awake()
    {
        targetPlayer = GameObject.FindWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        stretchTracker = GetComponent<PlayerZone>();  // 如果敌人也有 PlayerZone
    }

    void FixedUpdate()
    {
        if (targetPlayer == null) return;

        if (isKnockback)
        {
            DoKnockbackMovement();  
        }
        else
        {
            DoChaseMovement();    
        }

        if (life <= 0)
        {
            SoundSystem.instance.PlaySound("EnemyDead");
            Instantiate(particle, transform.position, Quaternion.identity);
            PlayerMovement.score += 10;
            Destroy(gameObject);
        }
    }

    // ================== 普通追踪移动 ==================
    void DoChaseMovement()
    {
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

    // ================== 击退运动 ==================
    void DoKnockbackMovement()
    {
        // knockbackSpeed = 距离 / 时间
        float knockbackSpeed = knockbackDistance / knockbackDuration;
        Vector2 delta = knockbackDir * knockbackSpeed * Time.fixedDeltaTime;

        if (stretchTracker != null && stretchTracker.currentZone != null)
        {
            delta = stretchTracker.currentZone.WarpDisplacement(rb.position, delta);
        }

        rb.MovePosition(rb.position + delta);

        knockbackTimeRemaining -= Time.fixedDeltaTime;
        if (knockbackTimeRemaining <= 0f)
        {
            isKnockback = false;
        }
    }

    // ================== 碰撞处理 ==================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 假设 Basket / Light 是你的子弹或攻击
        if (collision.gameObject.CompareTag("Basket"))
        {
            life -= 1;
            SoundSystem.instance.PlaySound("Explode");

            StartKnockback(collision); 
        }

        if (collision.gameObject.CompareTag("Light"))
        {
            life -= 3;

            StartKnockback(collision);   
        }
    }

    /// <summary>
    /// 开始一次击退
    /// </summary>
    void StartKnockback(Collision2D collision)
    {
        // 击退方向：从子弹指向敌人（敌人被打飞）
        Vector2 dir = ((Vector2)transform.position - (Vector2)collision.transform.position).normalized;

        if (dir.sqrMagnitude < 0.0001f)
        {
            // 如果实在算不出方向（重叠太近），就反向玩家
            if (targetPlayer != null)
                dir = ((Vector2)transform.position - (Vector2)targetPlayer.position).normalized;
            else
                dir = Vector2.up;
        }

        knockbackDir = dir;
        isKnockback = true;
        knockbackTimeRemaining = knockbackDuration;
    }
}
