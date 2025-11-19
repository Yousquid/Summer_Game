using UnityEngine;

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
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stretchTracker = GetComponent<PlayerZone>(); // optional    
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
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            DoDashMovement();     // 覆盖正常移动
        }
        else
        {
            UpdateVelocity();
            ApplyMovement();
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
        lastDashTime = Time.time;
    }

    /// <summary>
    /// dash 过程中的移动
    /// </summary>
    void DoDashMovement()
    {
        // dashSpeed = 距离 / 时间
        float dashSpeed = dashDistance / dashDuration;
        Vector2 delta = dashDirection * dashSpeed * Time.fixedDeltaTime;

        // 如需空间拉伸，这里也可以扭曲 dash 的位移
        if (stretchTracker != null && stretchTracker.currentZone != null)
        {
            delta = stretchTracker.currentZone.WarpDisplacement(rb.position, delta);
        }

        rb.MovePosition(rb.position + delta);

        dashTimeRemaining -= Time.fixedDeltaTime;
        if (dashTimeRemaining <= 0f)
        {
            isDashing = false;
        }
    }
}
