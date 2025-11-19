using UnityEngine;

public class SpriteStretchInZone2D : MonoBehaviour
{
    [Header("视觉上的拉伸倍数（>1 越夸张）")]
    public float visualStretchMultiplier = 1.5f;

    [Header("是否让 Sprite 对齐空间方向")]
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

    void Update()
    {
        // 如果 Zone 会移动 / 旋转，可以在每帧更新一次
        if (currentZone != null)
        {
            ApplyStretch();
        }
    }

    private void ApplyStretch()
    {
        if (currentZone == null)
            return;

        // 先恢复到原始 scale 作为基准
        transform.localScale = originalScale;

        // 地图中 stretch 的方向（2D 顶视用 right）
        Vector2 zoneForward = currentZone.transform.right.normalized;

        if (alignToZoneDirection)
        {
            // 让 Sprite 的 local X 轴对齐 Zone 的 forward 方向
            float angle = Mathf.Atan2(zoneForward.y, zoneForward.x) * Mathf.Rad2Deg;
            // 因为 Sprite 一般默认朝右，这样可以转过去
            transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }

        // 在本地 X 轴上拉长（因为我们把 X 对齐到了 Zone 的方向）
        Vector3 stretched = transform.localScale;
        stretched.x *= visualStretchMultiplier;
        transform.localScale = stretched;
    }

    private void ResetStretch()
    {
        // 恢复原始的比例和旋转
        transform.localScale = originalScale;
        transform.localRotation = originalRotation;
    }
}
