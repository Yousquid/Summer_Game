using UnityEngine;
using TMPro;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 6f;          // 最大移动速度
    public float acceleration = 20f;     // 加速
    public float deceleration = 25f;     // 停止时的减速
    public float inputDeadZone = 0.1f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 currentVelocity;      // 当前真实速度

    private PlayerZone stretchTracker;


    public float dashDistance = 3f;
    public float dashDuration = 0.12f;
    public float dashCooldown = 0.4f;

    bool isDashing;
    float dashTimeRemaining;
    Vector2 dashDirection;
    float lastDashTime;

    private TrailRenderer trail;
    private Gradient inintialTrailColor;
    private SpriteRenderer thisSprite;

    private Vector3 originalScale;   // 玩家原始体型
    private float dashElapsed;       // 本次 dash 已经过去的时间

    private bool dashSoundPlayed = false;
    public float CurrentSpeed { get; private set; }

    [Header("Knockback")]
    public float knockbackDistance = 2f;     // 击退距离
    public float knockbackDuration = 0.1f;   // 击退持续时间（越小越瞬间）
    private bool isKnockback = false;
    private Vector2 knockbackDir;
    private float knockbackTimeRemaining;

    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI livesUI;

    public static int score = 0;
    public static int lives = 3;

    public GameObject GameOverUI;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stretchTracker = GetComponent<PlayerZone>(); // optional    
        originalScale = transform.localScale;
    }

    private void Start()
    {
        trail = GetComponent<TrailRenderer>();
        inintialTrailColor = trail.colorGradient;
        thisSprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(x, y);

        if (moveInput.sqrMagnitude > 1f)
            moveInput.Normalize();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            TryStartDash(); // like above
        }

        scoreUI.text = $"Score: {score}";
        livesUI.text = $"LEFT LIVES: {lives}";

        if (lives <= 0)
        {
            GameOverUI.SetActive(true);
            Time.timeScale = 0f;
            Destroy(gameObject);
        }


    }

    void FixedUpdate()
    {
        if (isKnockback)
        {
            DoKnockbackMovement();    
        }
        else if (isDashing)
        {
            DoDashMovement();         // dash
        }
        else
        {
            UpdateVelocity();
            ApplyMovement();
        }

        UpdateCurrentSpeed();

    }

    void DoKnockbackMovement()
    {
        // knockbackSpeed = 距离 / 时间
        float knockbackSpeed = knockbackDistance / knockbackDuration;
        Vector2 delta = knockbackDir * knockbackSpeed * Time.fixedDeltaTime;

        // StretchZone 支持
        if (stretchTracker != null && stretchTracker.currentZone != null)
        {
            delta = stretchTracker.currentZone.WarpDisplacement(rb.position, delta);
        }

        rb.MovePosition(rb.position + delta);

        // 计时
        knockbackTimeRemaining -= Time.fixedDeltaTime;

        if (knockbackTimeRemaining <= 0f)
        {
            isKnockback = false;  // 击退结束
        }
    }

    void UpdateCurrentSpeed()
    {
        if (isDashing)
        {
            // dash 时速度 = 距离 / 时间
            CurrentSpeed = dashDistance / dashDuration;
        }
        else
        {
            // 平时用我们自己维护的 currentVelocity
            CurrentSpeed = currentVelocity.magnitude;
        }
    }

    /// <summary>
    /// 计算 currentVelocity，根据输入进行加速或减速
    /// </summary>
    void UpdateVelocity()
    {
        if (moveInput.sqrMagnitude > inputDeadZone)
        {
            // 有输入 → 加速到 maxSpeed
            Vector2 desired = moveInput * maxSpeed;

            currentVelocity = Vector2.MoveTowards(
                currentVelocity,
                desired,
                acceleration * Time.fixedDeltaTime
            );
        }
        else
        {
            // 没有输入 → 减速到 0
            currentVelocity = Vector2.MoveTowards(
                currentVelocity,
                Vector2.zero,
                deceleration * Time.fixedDeltaTime
            );
        }
    }

    /// <summary>
    /// 将 currentVelocity 转换为位移，移动玩家
    /// </summary>
    void ApplyMovement()
    {
        Vector2 delta = currentVelocity * Time.fixedDeltaTime;

        // 如果有 StretchZone，就扭曲 delta
        if (stretchTracker != null && stretchTracker.currentZone != null)
        {
            delta = stretchTracker.currentZone.WarpDisplacement(rb.position, delta);
        }

        rb.MovePosition(rb.position + delta);
    }

    void TryStartDash()
    {
        // 冷却没好，直接返回
        if (Time.time < lastDashTime + dashCooldown) return;

        // 1. 确定 dash 方向：
        Vector2 dir = Vector2.zero;

        if (moveInput.sqrMagnitude > 0.0001f)
        {
            // 如果有移动输入，就朝移动方向 dash
            dir = moveInput.normalized;
        }
        else
        {
            // 没有输入时，朝鼠标方向 dash
            Camera cam = Camera.main;
            if (cam != null)
            {
                Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0f;
                dir = ((Vector2)(mouseWorldPos - transform.position)).normalized;
            }
        }

        if (dir.sqrMagnitude < 0.0001f)
        {
            // 实在没有方向，就不 dash
            return;
        }

        dashDirection = dir;
        isDashing = true;
        dashTimeRemaining = dashDuration;
        dashElapsed = 0f;                     
        lastDashTime = Time.time;

        transform.localScale = originalScale * 2f;
    }

    /// <summary>
    /// dash 过程中的移动
    /// </summary>
    void DoDashMovement()
    {
        // dashSpeed = 距离 / 时间
        float dashSpeed = dashDistance / dashDuration;
        Vector2 delta = dashDirection * dashSpeed * Time.fixedDeltaTime;

        // Trail & 颜色之类的效果
        Gradient g = trail.colorGradient;
        var keys = g.colorKeys;
        keys[0].color = Color.blue;
        g.colorKeys = keys;
        trail.colorGradient = g;
        trail.widthMultiplier = 2f;

        thisSprite.color = Color.blue;

        if (!dashSoundPlayed)
        {
            SoundSystem.instance.PlaySound("Dash2");
            SoundSystem.instance.PlaySound("Dash1");

            dashSoundPlayed = true;
        }

        // 如需空间拉伸，这里也可以扭曲 dash 的位移
        if (stretchTracker != null && stretchTracker.currentZone != null)
        {
            delta = stretchTracker.currentZone.WarpDisplacement(rb.position, delta);
        }

        rb.MovePosition(rb.position + delta);

        dashElapsed += Time.fixedDeltaTime;
        dashTimeRemaining -= Time.fixedDeltaTime;

        float t = Mathf.Clamp01(dashElapsed / dashDuration); // 0 → 1
        float scaleMultiplier = Mathf.Lerp(2f, 1f, t);       // 2 → 1
        transform.localScale = originalScale * scaleMultiplier;

        if (dashTimeRemaining <= 0f)
        {
            isDashing = false;
            dashSoundPlayed = false;

            transform.localScale = originalScale;

            // 还原 trail 和颜色
            trail.colorGradient = inintialTrailColor;
            trail.widthMultiplier = 1;
            thisSprite.color = Color.white;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("EnemyBullet"))
        {
            StartKnockback(collision);
            ScreenShake.Instance.Shake(.6f,.1f);
            SoundSystem.instance.PlaySound("PlayerHurt");
            lives -= 1;
        }

    }

    void StartKnockback(Collision2D collision)
    {
        // 击退方向 = 玩家位置 - 敌人位置（也就是从敌人那里被打飞）
        Vector2 dir = ((Vector2)transform.position - (Vector2)collision.transform.position).normalized;

        // 防止没有方向（重叠时）
        if (dir.sqrMagnitude < 0.0001f)
            dir = Vector2.up;

        knockbackDir = dir;
        isKnockback = true;
        knockbackTimeRemaining = knockbackDuration;
    }
}
