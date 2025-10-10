using UnityEngine;

public class Red_Light : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // 指向你的SpriteRenderer组件
    public Color redColor = Color.red;
    public Color yellowColor = Color.yellow;
    public Color greenColor = Color.green;

    private float timer = 0f;
    private bool isRunning = true;

    void Start()
    {
        // 如果没手动指定SpriteRenderer，就自动找自己身上的
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        // 初始化为红色
        spriteRenderer.color = redColor;
    }

    void Update()
    {
        if (!isRunning) return;

        timer += Time.deltaTime;

        // 0~25秒：红灯
        if (timer < 25f)
        {
            spriteRenderer.color = redColor;
        }
        // 25~30秒：黄灯
        else if (timer < 30f)
        {
            spriteRenderer.color = yellowColor;
        }
        // >=30秒：绿灯
        else
        {
            spriteRenderer.color = greenColor;
            isRunning = false; // 停止计时（只切一次）
        }
    }
}
