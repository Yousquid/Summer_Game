using UnityEngine;
using System.Collections;
public class UI_Shake : MonoBehaviour
{
    public static UI_Shake Instance;

    private RectTransform rectTransform;
    private Vector2 uiOriginalPos;

    void Awake()
    {
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
        uiOriginalPos = rectTransform.anchoredPosition;
    }

    public void Shake(float intensity, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeRoutine(intensity, duration));
    }

    private IEnumerator ShakeRoutine(float intensity, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * intensity*100f;
            float offsetY = Random.Range(-1f, 1f) * intensity*100f;
            Vector2 offset = new Vector2(offsetX, offsetY);

            rectTransform.anchoredPosition = uiOriginalPos + offset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = uiOriginalPos;
    }
}
