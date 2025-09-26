using System.Collections;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance;

    private Vector3 camOriginalPos;
    private Vector3 uiOriginalPos;

    public RectTransform uiRoot;

    void Awake()
    {
        Instance = this;
    }

    public void Shake(float intensity, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeRoutine(intensity, duration));
    }

    private IEnumerator ShakeRoutine(float intensity, float duration)
    {
        camOriginalPos = transform.localPosition;
        if (uiRoot != null) uiOriginalPos = uiRoot.localPosition;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * intensity;
            float offsetY = Random.Range(-1f, 1f) * intensity;
            Vector3 offset = new Vector3(offsetX, offsetY, 0f);

            // 相机抖
            transform.localPosition = camOriginalPos + offset;

            // UI 根节点抖
            if (uiRoot != null)
            {
                uiRoot.localPosition = uiOriginalPos + offset;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = camOriginalPos;
        if (uiRoot != null) uiRoot.localPosition = uiOriginalPos;
    }
}
