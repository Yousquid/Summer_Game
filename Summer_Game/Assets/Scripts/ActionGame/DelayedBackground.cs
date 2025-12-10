using UnityEngine;

public class DelayedBackground : MonoBehaviour
{
    public Transform cam;          // 主摄像机
    public float followDelay = 0.3f; // 跟随的“慢半拍”程度（数值越大越慢）

    private Vector3 velocity = Vector3.zero;
    private float smoothTime = 0.3f; // 用于 SmoothDamp 的平滑时间

    void Start()
    {
        if (cam == null && Camera.main != null)
            cam = Camera.main.transform;

        // 初始时直接跟到摄像机位置（不突兀）
        if (cam != null)
        {
            transform.position = new Vector3(
                cam.position.x,
                cam.position.y,
                transform.position.z  // 保持自身的深度
            );
        }

        // 用 followDelay 控制 smoothTime
        smoothTime = Mathf.Max(0.01f, followDelay);
    }

    void LateUpdate()
    {
        if (cam == null) return;

        // 目标位置：摄像机的 x、y，加上背景自己的 z
        Vector3 targetPos = new Vector3(
            cam.position.x,
            cam.position.y,
            transform.position.z
        );

        // 使用 SmoothDamp 做一个“有点迟到”的平滑跟随
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            smoothTime
        );
    }
}
