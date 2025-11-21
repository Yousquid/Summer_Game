using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [Header("挥砍 Prefab（拖进做好的 Slash 预制体）")]
    public GameObject slashPrefab;

    [Header("挥砍距离（离玩家多远生成）")]
    public float slashDistance = 0.7f;

    [Header("挥砍存在时间（会覆盖 prefab 里的 lifeTime，可选）")]
    public float slashDuration = 0.15f;

    [Header("和速度相关的参数")]
    public PlayerMovement movement;
    public float maxMoveSpeed = 10f; // 你角色的“最大速度”

    void Update()
    {
        // 鼠标右键：0 = 左键, 1 = 右键, 2 = 中键
        if (Input.GetMouseButtonDown(1))
        {
            DoSlash();
        }
    }

    void DoSlash()
    {
        if (slashPrefab == null)
        {
            Debug.LogError("PlayerMelee: 没有设置 slashPrefab！");
            return;
        }

        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("PlayerMelee: 找不到主摄像机（MainCamera）");
            return;
        }

        // 1. 鼠标世界坐标
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // 2. 指向鼠标方向
        Vector2 dir = ((Vector2)(mouseWorldPos - transform.position)).normalized;
        if (dir.sqrMagnitude < 0.0001f)
            return;

        // 3. 挥砍生成位置
        Vector3 spawnPos = transform.position + (Vector3)(dir * slashDistance);

        // 4. 旋转角度
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0f, 0f, angle);

        // 5. 生成挥砍
        GameObject slash = Instantiate(slashPrefab, spawnPos, rot);

        SoundSystem.instance.PlaySound("Slash");

        // ======== 5.5 根据玩家速度放大 Slash 尺寸 ========

        if (movement != null && movement.maxSpeed > 0f)
        {
            float currentSpeed = movement.CurrentSpeed;
            float maxSpeed = movement.maxSpeed;

            // 速度/最大速度的比例
            float speedRatio = currentSpeed / maxSpeed;

            float extraPercent = 0f;

            if (speedRatio <= 1f)
            {
                // 当速度从 0% ~ 100% maxSpeed 时，挥砍从 0% ~ 20% 放大
                // 速度 == maxSpeed 时：+20%
                extraPercent = 0.2f * speedRatio;
            }
            else
            {
                // 超过最大速度的部分，每多少比例就多加多少百分比
                // 比如：currentSpeed = 1.5 * maxSpeed
                // overRatio = 0.5 → 额外 +50%
                float overRatio = speedRatio - 1f;
                extraPercent = 0.2f + overRatio;
            }

            // 防止太夸张，给一个上限（这里最多 2 倍）
            //extraPercent = Mathf.Min(extraPercent, 1.0f); // 最多 +100%

            float scaleMultiplier = 1f + extraPercent;

            slash.transform.localScale = slash.transform.localScale * scaleMultiplier;

            ScreenShake.Instance.Shake(0.08f * scaleMultiplier, 0.05f);
        }

        // 6. 可选：覆盖 slash 自身的 lifeTime
        var hitbox = slash.GetComponent<SplashHitBox>();
        if (hitbox != null)
        {
            hitbox.lifeTime = slashDuration;
        }
        else
        {
            Destroy(slash, slashDuration);
        }
    }
}
