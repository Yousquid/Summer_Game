using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour
{
    [Header("拖进你的Projectile Prefab（需要Rigidbody2D）")]
    public GameObject projectilePrefab;

    [Header("射弹初速度")]
    public float projectileSpeed = 12f;

    private Vector3 originalScale;
    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        RotateTowardsMouse();

        // 左键点击
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector2 dir = (mouseWorldPos - transform.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        proj.GetComponent<SimpleProjectile>().Init(dir);

        ScreenShake.Instance.Shake(.08f, .05f);

        SoundSystem.instance.PlaySound("Shoot");

        StartCoroutine(ShootRecoilEffect());

    }

    IEnumerator ShootRecoilEffect()
    {
        float duration = 0.12f;      // 效果持续时间
        float bigScale = 1.3f;      // 最大放大倍率（1.15 = 稍微变大）

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            // 体型从 1.15 倍 → 回到原始大小
            float scale = Mathf.Lerp(bigScale, 1f, t);
            transform.localScale = originalScale * scale;

            // 颜色从红色 → 渐渐回到白色
            if (sprite != null)
            {
                sprite.color = Color.Lerp(Color.red, Color.white, t);
            }

            yield return null;
        }

        // 结束后强制恢复（避免浮点误差）
        transform.localScale = originalScale;
        if (sprite != null)
            sprite.color = Color.white;
    }

    void RotateTowardsMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector2 direction = (mousePos - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 假如你的玩家默认朝右（Unity 2D 精灵默认是向右）
        //transform.rotation = Quaternion.Euler(0f, 0f, angle);

        
         transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }
}
