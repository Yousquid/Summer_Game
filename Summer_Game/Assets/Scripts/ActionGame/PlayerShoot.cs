using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("拖进你的Projectile Prefab（需要Rigidbody2D）")]
    public GameObject projectilePrefab;

    [Header("射弹初速度")]
    public float projectileSpeed = 12f;

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
