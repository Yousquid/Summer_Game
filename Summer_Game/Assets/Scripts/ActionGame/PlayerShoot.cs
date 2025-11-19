using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("拖进你的Projectile Prefab（需要Rigidbody2D）")]
    public GameObject projectilePrefab;

    [Header("射弹初速度")]
    public float projectileSpeed = 12f;

    void Update()
    {
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
}
