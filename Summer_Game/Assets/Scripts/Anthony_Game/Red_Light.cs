using UnityEngine;

public class Red_Light : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // 指向你的SpriteRenderer组件
    public Sprite greenlight;

    public float timer = 0f;
    private bool isRunning = true;

    void Start()
    {
        // 如果没手动指定SpriteRenderer，就自动找自己身上的
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        // 初始化为红色
    }

    void Update()
    {
        if (!isRunning) return;

        timer += Time.deltaTime;

        // 0~25秒：红灯
        if (timer < 25f)
        {
        }
        // 25~30秒：黄灯
        else if (timer < 30f)
        {
        }
        // >=30秒：绿灯
        else
        {
            spriteRenderer.sprite = greenlight;
            isRunning = false; // 停止计时（只切一次）
        }
    }
}
