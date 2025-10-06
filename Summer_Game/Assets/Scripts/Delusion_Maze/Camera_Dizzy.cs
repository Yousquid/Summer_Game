using UnityEngine;

public class Camera_Dizzy : MonoBehaviour
{
    [Header("×óÓÒÒ¡»Î")]
    public float horizontalAmplitude = 1.5f;
    public float horizontalSpeed = 2f;

    [Header("ÉÏÏÂÒ¡»Î")]
    public float verticalAmplitude = 0.8f;
    public float verticalSpeed = 1.5f;

    [Header("Ðý×ª»Î¶¯")]
    public float rotationAmplitude = 5f;
    public float rotationSpeed = 2f;

    [Header("Ëæ»ú¶¶¶¯")]
    public float jitterAmount = 0.1f;
    public float jitterSpeed = 20f;

    private float timeOffset;
    private Vector3 baseOffset;
    private float baseRotZ;

    public Vector3 CurrentOffset => baseOffset;
    public float CurrentRotationZ => baseRotZ;

    void Start()
    {
        timeOffset = Random.value * 100f;
    }

    void Update()
    {
        float t = Time.time + timeOffset;

        float offsetX = Mathf.Sin(t * horizontalSpeed) * horizontalAmplitude;
        float offsetY = Mathf.Sin(t * verticalSpeed + Mathf.PI / 3f) * verticalAmplitude;

        float jitterX = (Mathf.PerlinNoise(t * jitterSpeed, 0f) * 2f - 1f) * jitterAmount;
        float jitterY = (Mathf.PerlinNoise(0f, t * jitterSpeed) * 2f - 1f) * jitterAmount;

        float rotZ = Mathf.Sin(t * rotationSpeed + Mathf.PI / 6f) * rotationAmplitude;

        baseOffset = new Vector3(offsetX + jitterX, offsetY + jitterY, 0f);
        baseRotZ = rotZ;
    }
}
