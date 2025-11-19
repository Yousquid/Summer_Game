using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("基础移动")]
    public float moveSpeed = 5f;

    [Header("Dash 设置")]
    public float dashDistance = 3f;      // 一次 dash 走多远
    public float dashDuration = 0.12f;   // dash 持续时间（秒）
    public float dashCooldown = 0.4f;    // 冷却时间（秒）

    private Rigidbody2D rb;
    private Vector2 moveInput;

    // dash 状态
    private bool isDashing = false;
    private float dashTimeRemaining = 0f;
    private Vector2 dashDirection;
    private float lastDashTime = -999f;

    // 如果你有 StretchZone，可以选用（没有也没关系）
    private PlayerZone stretchTracker;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stretchTracker = GetComponent<PlayerZone>();  // 如果没有这个组件，会是 null，没关系
    }

    void Update()
    {
        // 获取基础移动输入
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(x, y).normalized;

        // 监听 Left Shift 开始 dash
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            TryStartDash();
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            DoDashMovement();
        }
        else
        {
            DoNormalMovement();
        }
    }

    /// <summary>
    /// 普通移动逻辑
    /// </summary>
    void DoNormalMovement()
    {
        Vector2 delta = moveInput * moveSpeed * Time.fixedDeltaTime;

        // 如果你在用 StretchZone 的位移扭曲，可以在这里加：
        if (stretchTracker != null && stretchTracker.currentZone != null)
        {
            delta = stretchTracker.currentZone.WarpDisplacement(rb.position, delta);
        }

        rb.MovePosition(rb.position + delta);
    }

    /// <summary>
    /// 尝试开始 dash
    /// </summary>
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
