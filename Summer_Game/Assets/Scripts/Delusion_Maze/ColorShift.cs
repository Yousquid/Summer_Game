using UnityEngine;

public class ColorShift : MonoBehaviour
{
    public float colorChangeSpeed = 1f; 

    [Range(0f, 1f)] public float minBrightness = 0.5f;
    [Range(0f, 1f)] public float maxBrightness = 1f;

    private SpriteRenderer spriteRenderer;
    private Color targetColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            enabled = false;
            return;
        }

        targetColor = RandomColor();
    }

    void Update()
    {
        spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, Time.deltaTime * colorChangeSpeed);

        if (Vector4.Distance(spriteRenderer.color, targetColor) < 0.05f)
        {
            targetColor = RandomColor();
        }
    }

    Color RandomColor()
    {
        float h = Random.value; 
        float s = Random.Range(0.8f, 1f); 
        float v = Random.Range(minBrightness, maxBrightness); 

        Color color = Color.HSVToRGB(h, s, v);
        return color;
    }
}
