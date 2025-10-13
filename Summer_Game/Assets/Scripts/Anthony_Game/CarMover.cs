using UnityEngine;

public class CarMover : MonoBehaviour
{
    public float speed = 3f;        // ����
    public bool moveRight = true;   // �Ƿ�����

    private Rigidbody2D rb;

    private void Start()
    {
        Destroy(gameObject, 5f);

    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;       // �ó���������Ӱ��
        rb.freezeRotation = true;   // ��ֹ��ת
    }

    void FixedUpdate()
    {
        // ʹ�� MovePosition ƽ���ƶ�
        float dir = moveRight ? 1f : -1f;
        Vector2 move = new Vector2(dir * speed * Time.fixedDeltaTime, 0f);
        rb.MovePosition(rb.position + move);
    }

}
