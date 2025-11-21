using UnityEngine;

public class SlashParticle : MonoBehaviour
{
    public GameObject hitEffectPrefab;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }
    }
}
