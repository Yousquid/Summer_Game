using UnityEngine;

public class SplashHitBox : MonoBehaviour
{
    [Header("挥砍存在时间")]
    public float lifeTime = 0.15f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 这里可以写命中逻辑，比如：
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Slash hit enemy: " + other.name);
            // 在这里调用敌人受伤脚本，比如 other.GetComponent<EnemyHealth>()?.TakeDamage(1);
        }
    }
}
