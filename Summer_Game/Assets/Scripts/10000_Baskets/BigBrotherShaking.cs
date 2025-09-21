using UnityEngine;

public class BigBrotherShaking : MonoBehaviour
{
    private SpriteRenderer sprite;
    private bool isIncreasing = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Color color = sprite.color;

        if (isIncreasing)
        {
            color.a += 0.001f;
            if (color.a >= 1f)
            {
                color.a = 1f;
                isIncreasing = false;
            }
        }
        else
        {
            color.a -= 0.001f;
            if (color.a <= 0f)
            {
                color.a = 0f;
                isIncreasing = true;
            }
        }

        sprite.color = color;
    }
}
