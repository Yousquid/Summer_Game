using UnityEngine;

public class StretchZone : MonoBehaviour
{
    [Header("位移拉伸倍数（移动用，不一定等于视觉/碰撞倍数）")]
    public float stretchMultiplier = 2f;

    private BoxCollider2D box;

    void Awake()
    {
        box = GetComponent<BoxCollider2D>();
        box.isTrigger = true;
    }

    // —— 如果你有 WarpDisplacement 就继续保留，这里略 —— //
    public Vector2 WarpDisplacement(Vector2 position, Vector2 delta)
    {
        Vector2 forward = ((Vector2)transform.right).normalized;
        float along = Vector2.Dot(delta, forward);
        Vector2 alongVec = along * forward;
        Vector2 sideVec = delta - alongVec;

        alongVec *= stretchMultiplier;
        return alongVec + sideVec;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 找身上的“身体拉伸组件”
        var bodyStretch = other.GetComponent<BodyStretchInZone2D>();
        if (bodyStretch != null)
        {
            bodyStretch.EnterZone(this);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var bodyStretch = other.GetComponent<BodyStretchInZone2D>();
        if (bodyStretch != null)
        {
            bodyStretch.ExitZone(this);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 start = transform.position;
        Vector3 end = start + transform.right * 2f;
        Gizmos.DrawLine(start, end);
        Gizmos.DrawSphere(end, 0.1f);
    }
}
