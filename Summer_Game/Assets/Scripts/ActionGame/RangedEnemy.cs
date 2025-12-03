using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;           // 追击速度
    public float attackRange = 6f;         // 攻击射程

    [Header("Attack")]
    public GameObject projectilePrefab;    // 敌人的子弹预制体
    public float chargeTime = 0.8f;        // 蓄力时间
    public float fireCooldown = 1.5f;      // 发射后冷却时间

    private Transform player;
    private Rigidbody2D rb;
    private PlayerZone stretchTracker;

    private float chargeTimer = 0f;
    private float cooldownTimer = 0f;

    private enum State
    {
        Chase,      // 追击玩家
        Charging,   // 蓄力
        Cooldown    // 冷却
    }

    private State state = State.Chase;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stretchTracker = GetComponent<PlayerZone>();

        GameObject p = GameObject.FindWithTag("Player");
        if (p != null)
            player = p.transform;
    }

    void Update()
    {
        if (player == null) return;

        // 敌人朝向玩家（可选）
        Vector2 lookDir = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        float dist = Vector2.Distance(transform.position, player.position);

        switch (state)
        {
            case State.Chase:
                // 只有在「追击」状态，才根据距离切换到蓄力
                if (dist <= attackRange)
                {
                    state = State.Charging;
                    chargeTimer = 0f;
                }
                break;

            case State.Charging:
                // 一旦进入 Charging，就不会再因为距离改变跳回 Chase
                chargeTimer += Time.deltaTime;

                if (chargeTimer >= chargeTime)
                {
                    Fire();
                    state = State.Cooldown;
                    cooldownTimer = fireCooldown;
                }
                break;

            case State.Cooldown:
                cooldownTimer -= Time.deltaTime;

                if (cooldownTimer <= 0f)
                {
                    // 冷却结束后再看距离：近 → 继续蓄力，远 → 回去追
                    if (dist <= attackRange)
                    {
                        state = State.Charging;
                        chargeTimer = 0f;
                    }
                    else
                    {
                        state = State.Chase;
                    }
                }
                break;
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        if (state == State.Chase)
        {
            // 只在「追击」状态中移动
            Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
            Vector2 delta = dir * moveSpeed * Time.fixedDeltaTime;

            if (stretchTracker != null && stretchTracker.currentZone != null)
            {
                delta = stretchTracker.currentZone.WarpDisplacement(rb.position, delta);
            }

            rb.MovePosition(rb.position + delta);
        }
        else
        {
            // Charging / Cooldown 阶段站桩不动
        }
    }

    void Fire()
    {
        if (projectilePrefab == null || player == null) return;

        // 1. 计算从敌人指向玩家的方向（发射瞬间）
        Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;

        // 2. 计算子弹的朝向（假设子弹默认朝右）
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0f, 0f, angle -90F);

        // 3. 生成子弹（位置在敌人当前位置）
        GameObject proj = Instantiate(projectilePrefab, transform.position, rot);

        // 4. 把方向传给子弹脚本
        EnemyProject sp = proj.GetComponent<EnemyProject>();
        if (sp != null)
        {
            sp.Init(dir);
        }
    }

    // 在 Scene 里画个射程圈方便调试
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
