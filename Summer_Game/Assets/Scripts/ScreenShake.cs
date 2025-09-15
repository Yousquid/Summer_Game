using System.Collections;
using UnityEngine;
public class ScreenShake : MonoBehaviour
{
    public static ScreenShake instance;

    private Vector3 originalPos;

    void Awake()
    {
        if (instance == null) instance = this;
        originalPos = transform.localPosition;
    }

    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(DoShake(duration, magnitude));
    }

    private IEnumerator DoShake(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
