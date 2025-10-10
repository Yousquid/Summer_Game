using UnityEngine;

public class Red_Light : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // ָ�����SpriteRenderer���
    public Color redColor = Color.red;
    public Color yellowColor = Color.yellow;
    public Color greenColor = Color.green;

    private float timer = 0f;
    private bool isRunning = true;

    void Start()
    {
        // ���û�ֶ�ָ��SpriteRenderer�����Զ����Լ����ϵ�
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        // ��ʼ��Ϊ��ɫ
        spriteRenderer.color = redColor;
    }

    void Update()
    {
        if (!isRunning) return;

        timer += Time.deltaTime;

        // 0~25�룺���
        if (timer < 25f)
        {
            spriteRenderer.color = redColor;
        }
        // 25~30�룺�Ƶ�
        else if (timer < 30f)
        {
            spriteRenderer.color = yellowColor;
        }
        // >=30�룺�̵�
        else
        {
            spriteRenderer.color = greenColor;
            isRunning = false; // ֹͣ��ʱ��ֻ��һ�Σ�
        }
    }
}
