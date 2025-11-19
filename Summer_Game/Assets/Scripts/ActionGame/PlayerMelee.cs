using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [Header("挥砍 Prefab（拖进做好的 Slash 预制体）")]
    public GameObject slashPrefab;

    [Header("挥砍距离（离玩家多远生成）")]
    public float slashDistance = 0.7f;

    [Header("挥砍存在时间（会覆盖 prefab 里的 lifeTime，可选）")]
    public float slashDuration = 0.15f;

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

        // 1. 计算鼠标世界坐标
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // 2. 计算指向鼠标的方向
        Vector2 dir = ((Vector2)(mouseWorldPos - transform.position)).normalized;
        if (dir.sqrMagnitude < 0.0001f)
            return;

        // 3. 挥砍生成位置：在玩家前方 slashDistance 的地方
        Vector3 spawnPos = transform.position + (Vector3)(dir * slashDistance);

        // 4. 计算旋转角度（让 slash 看起来朝向鼠标）
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0f, 0f, angle);

        // 5. 生成挥砍
        GameObject slash = Instantiate(slashPrefab, spawnPos, rot);

        // 6. 可选：覆盖 slash 自身的 lifeTime
        var hitbox = slash.GetComponent<SplashHitBox>();
        if (hitbox != null)
        {
            hitbox.lifeTime = slashDuration;
        }
        else
        {
            // 如果你懒得挂 SlashHitbox，也可以直接在这里销毁
            Destroy(slash, slashDuration);
        }
    }
}
