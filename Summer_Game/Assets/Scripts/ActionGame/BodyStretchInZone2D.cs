using UnityEngine;

public class BodyStretchInZone2D : MonoBehaviour
{
    [Header("碰撞/整体拉伸倍数（>1 越长）")]
    public float bodyStretchMultiplier = 1.5f;

    [Header("是否让物体整体朝向也对齐 Zone 方向（可选）")]
    public bool alignToZoneDirection = false;

    private Vector3 originalScale;
    private Quaternion originalRotation;

    private StretchZone currentZone;

    void Awake()
    {
        originalScale = transform.localScale;
        originalRotation = transform.localRotation;
    }

    public void EnterZone(StretchZone zone)
    {
        currentZone = zone;
        ApplyStretch();
    }

    public void ExitZone(StretchZone zone)
    {
        if (currentZone == zone)
        {
            currentZone = null;
            ResetStretch();
        }
    }

    void LateUpdate()
    {
        // 如果 Zone 会移动/旋转，每帧更新一下拉伸
        if (currentZone != null)
        {
            ApplyStretch();
        }
    }

    private void ApplyStretch()
    {
        if (currentZone == null)
            return;

        // 每次从原始状态开始，避免无限叠加
        transform.localScale = originalScale;
        transform.localRotation = originalRotation;

        // 1. 先拿到 Zone 的方向（在 2D 顶视里我们用 right，当作“走廊方向”）
        Vector2 zoneForward = currentZone.transform.right.normalized;

        // （可选）让物体朝向大致跟 Zone 一致
        if (alignToZoneDirection)
        {
            float angle = Mathf.Atan2(zoneForward.y, zoneForward.x) * Mathf.Rad2Deg;
            transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }

        // 2. 判断这个 Zone 更接近水平还是垂直：
        //    和世界坐标的水平向量 / 垂直向量做点积
        float horizDot = Mathf.Abs(Vector2.Dot(zoneForward, Vector2.right)); // 越接近 1 越水平
        float vertDot = Mathf.Abs(Vector2.Dot(zoneForward, Vector2.up));    // 越接近 1 越垂直

        Vector3 stretched = transform.localScale;

        if (horizDot >= vertDot)
        {
            // Zone 更偏水平：在 X 轴上拉长
            stretched.x *= bodyStretchMultiplier;
            // Debug.Log("Stretch body on X");
        }
        else
        {
            // Zone 更偏垂直：在 Y 轴上拉长
            stretched.y *= bodyStretchMultiplier;
            // Debug.Log("Stretch body on Y");
        }

        transform.localScale = stretched;
    }

    private void ResetStretch()
    {
        transform.localScale = originalScale;
        transform.localRotation = originalRotation;
    }
}
