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

    [Header("Knockback")]
    public float knockbackDistance = 2f;   // 被击退的距离
    public float knockbackDuration = 0.1f; // 被击退持续时间

    private Transform player;
    private Rigidbody2D rb;
    private PlayerZone stretchTracker;

    private float chargeTimer = 0f;
    private float cooldownTimer = 0f;

    private bool isKnockback = false;
    private Vector2 knockbackDir;
    private float knockbackTimeRemaining;

    [Header("Aim Line")]
    public LineRenderer aimLine;
    public float lineStartWidth = 0.25f;   // 刚开始蓄力时线的宽度
    public float lineEndWidth = 0.03f;     // 发射前那一刻线的宽度
    public Color lineColor = Color.red;    // 瞄准线的颜色

    [Header("Charge Visual")]
    public float maxChargeScale = 1.3f;   // 蓄力结束时的最大放大倍数
    public float flickerSpeed = 18f;      // 闪烁频率
    public float firePopScale = 1.6f;     // 发射瞬间的爆开缩放倍数
    public float firePopDuration = 0.08f; // 发射后的爆开动画时间

    private SpriteRenderer sprite;
    private Vector3 originalScale;
    private Color originalColor;
    private bool isFirePopping = false;   // 防止开火动画过程中被别的地方改 scale

    private int health = 3;

    private bool hasPlayedChargingSound = false;

    public GameObject deadEffect; 

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

        aimLine = GetComponent<LineRenderer>();

        if (aimLine != null)
        {
            aimLine.positionCount = 2;
            aimLine.enabled = false;   // 默认不显示
        }


        sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
            originalColor = sprite.color;

        originalScale = transform.localScale;
    }

    void Update()
    {
        if (health <= 0)
        {
            Instantiate(deadEffect, transform.position, Quaternion.identity);
            SoundSystem.instance.PlaySound("EnemyDead");
            PlayerMovement.score += 20;
            Destroy(gameObject);
        }

        if (player == null) return;

        // 敌人朝向玩家（纯视觉，可选）
        Vector2 lookDir = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (isKnockback) return;

        float dist = Vector2.Distance(transform.position, player.position);

        switch (state)
        {
            case State.Chase:
                // 只有在「追击」状态，才根据距离切换到蓄力
                HideAimLine();

                if (dist <= attackRange)
                {
                    state = State.Charging;
                    chargeTimer = 0f;
                }
                break;

            case State.Charging:
                chargeTimer += Time.deltaTime;
                if (!hasPlayedChargingSound)
                {
                    SoundSystem.instance.PlaySound("EnemyCharging1");
                    hasPlayedChargingSound = true;
                }

                // 0~1 的蓄力进度
                float t = Mathf.Clamp01(chargeTimer / chargeTime);
                UpdateAimLine(t);
                UpdateChargeVisual(t);

                if (chargeTimer >= chargeTime)
                {
                    Fire();
                    state = State.Cooldown;
                    cooldownTimer = fireCooldown;
                    HideAimLine();  // 发射后把线关掉
                }
                break;

            case State.Cooldown:
                cooldownTimer -= Time.deltaTime;
                HideAimLine();
                hasPlayedChargingSound = false;
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

    void UpdateChargeVisual(float t)
    {
        if (sprite == null || isFirePopping) return;

        float flicker = (Mathf.Sin(Time.time * flickerSpeed) + 1f) * 0.5f; // 0~1
        Color targetWhite = Color.white;
        Color c = Color.Lerp(originalColor, targetWhite, flicker);
        sprite.color = c;

        float scaleMul = Mathf.Lerp(1f, maxChargeScale, t);
        transform.localScale = originalScale * scaleMul;
    }
    void UpdateAimLine(float t)
    {
        if (aimLine == null || player == null) return;

        aimLine.enabled = true;

        // 起点：敌人位置
        aimLine.SetPosition(0, transform.position);
        // 终点：玩家当前位置
        aimLine.SetPosition(1, player.position);

        // 宽度：从粗 → 细（t 从 0 → 1）
        float width = Mathf.Lerp(lineStartWidth, lineEndWidth, t);
        aimLine.startWidth = width;
        aimLine.endWidth = width * 0.7f;

        // 透明度：从几乎看不见 → 完全清晰
        Color c = lineColor;
        c.a = Mathf.Lerp(0f, 1f, t);   // 0 → 1
        aimLine.startColor = c;
        aimLine.endColor = c;
    }

    void HideAimLine()
    {
        if (aimLine != null && aimLine.enabled)
        {
            aimLine.enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        if (isKnockback)
        {
            DoKnockbackMovement();   
        }
        else if (state == State.Chase)
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

    // =================== 开火 ===================
    void Fire()
    {
        if (projectilePrefab == null || player == null) return;

        // 1. 计算从敌人指向玩家的方向（发射瞬间）
        Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;

        // 2. 计算子弹的朝向（假设子弹默认朝右；如果默认朝上就 -90f）
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0f, 0f, angle - 90f);

        // 3. 生成子弹（位置在敌人当前位置）
        GameObject proj = Instantiate(projectilePrefab, transform.position, rot);

        // 4. 把方向传给子弹脚本
        EnemyProject ep = proj.GetComponent<EnemyProject>();
        if (ep != null)
        {
            ep.Init(dir);
        }

        SoundSystem.instance.PlaySound("Shoot");

        StartCoroutine(FirePopEffect());

    }
    System.Collections.IEnumerator FirePopEffect()
    {
        isFirePopping = true;

        // 发射前最后一帧的蓄力 scale（理论上接近 originalScale * maxChargeScale）
        Vector3 startScale = transform.localScale;
        Vector3 peakScale = originalScale * firePopScale;

        float timer = 0f;

        while (timer < firePopDuration)
        {
            timer += Time.deltaTime;
            float t = timer / firePopDuration;

            // 从 peakScale 慢慢回到 originalScale
            transform.localScale = Vector3.Lerp(peakScale, originalScale, t);

            // 发射瞬间保持高亮一点
            if (sprite != null)
            {
                sprite.color = Color.Lerp(Color.white, originalColor, t);
            }

            yield return null;
        }

        // 保险：最后强行恢复
        transform.localScale = originalScale;
        if (sprite != null)
            sprite.color = originalColor;

        isFirePopping = false;
    }
    // =================== 击退 ===================
    public void StartKnockback(Vector2 fromPosition)
    {
        // 击退方向 = 敌人位置 - 攻击来源位置（被打飞）
        Vector2 dir = ((Vector2)transform.position - fromPosition).normalized;
        if (dir.sqrMagnitude < 0.0001f)
            dir = Vector2.up;

        knockbackDir = dir;
        isKnockback = true;
        knockbackTimeRemaining = knockbackDuration;

        // 可以选择强制回到追击状态（之后再重新进射程攻击）
        state = State.Chase;
    }

    void DoKnockbackMovement()
    {
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
            // 防止残余速度（虽然是 Kinematic，保险起见清零）
            rb.linearVelocity = Vector2.zero;
        }
    }

    // 你可以在这里接子弹的碰撞事件
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 比如玩家子弹 Tag 是 "Basket" 或 "PlayerBullet"
        if (collision.gameObject.CompareTag("Light") || collision.collider.CompareTag("Basket"))
        {
            // 取子弹位置作为击退来源
            StartKnockback(collision.transform.position);

            health--;

            // 在这里减血、播音效、销毁子弹等
            // life -= 1;
            // Destroy(collision.gameObject);
        }

        
    }



    // 在 Scene 里画个射程圈方便调试
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
