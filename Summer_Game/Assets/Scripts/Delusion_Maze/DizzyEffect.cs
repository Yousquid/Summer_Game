using UnityEngine;

public class DizzyEffect : MonoBehaviour
{
    public float horizontalAmplitude = 1.5f;  // ���һζ�����
    public float horizontalSpeed = 2f;        // ���һζ��ٶ�

    public float verticalAmplitude = 0.8f;    // ���»ζ�����
    public float verticalSpeed = 1.5f;        // ���»ζ��ٶ�

    public float rotationAmplitude = 5f;      // ������ת�Ƕȷ�Χ����λ���ȣ�
    public float rotationSpeed = 2f;          // ��ת�ٶ�

    public float jitterAmount = 0.1f;         // ����ǿ��
    public float jitterSpeed = 20f;           // �����ٶ�

    private Vector3 startPos;
    private Quaternion startRot;
    private float timeOffset;

    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        timeOffset = Random.value * 100f; // �����������ͬƵ
    }

    void Update()
    {
        float t = Time.time + timeOffset;

        // ���һζ�
        float offsetX = Mathf.Sin(t * horizontalSpeed) * horizontalAmplitude;

        // ���»ζ�����λ����
        float offsetY = Mathf.Sin(t * verticalSpeed + Mathf.PI / 3f) * verticalAmplitude;

        // ����
        float jitterX = (Mathf.PerlinNoise(t * jitterSpeed, 0f) * 2f - 1f) * jitterAmount;
        float jitterY = (Mathf.PerlinNoise(0f, t * jitterSpeed) * 2f - 1f) * jitterAmount;

        // ��תҡ�Σ�Z�ᣩ
        float rotZ = Mathf.Sin(t * rotationSpeed + Mathf.PI / 6f) * rotationAmplitude;

        // Ӧ��λ��
        transform.position = startPos + new Vector3(offsetX + jitterX, offsetY + jitterY, 0f);

        // Ӧ����ת���� Z �ᣩ
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
    }
}
