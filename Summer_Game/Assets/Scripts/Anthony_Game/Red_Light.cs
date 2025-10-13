using UnityEngine;

public class Red_Light : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // ָ�����SpriteRenderer���
    public Sprite greenlight;

    public float timer = 0f;
    private bool isRunning = true;

    void Start()
    {
        // ���û�ֶ�ָ��SpriteRenderer�����Զ����Լ����ϵ�
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        // ��ʼ��Ϊ��ɫ
    }

    void Update()
    {
        if (!isRunning) return;

        timer += Time.deltaTime;

        // 0~25�룺���
        if (timer < 25f)
        {
        }
        // 25~30�룺�Ƶ�
        else if (timer < 30f)
        {
        }
        // >=30�룺�̵�
        else
        {
            spriteRenderer.sprite = greenlight;
            isRunning = false; // ֹͣ��ʱ��ֻ��һ�Σ�
        }
    }
}
