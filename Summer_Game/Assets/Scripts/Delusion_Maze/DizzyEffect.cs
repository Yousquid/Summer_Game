using UnityEngine;

public class DizzyEffect : MonoBehaviour
{
    public float horizontalAmplitude = 1.5f;  // 左右晃动幅度
    public float horizontalSpeed = 2f;        // 左右晃动速度

    public float verticalAmplitude = 0.8f;    // 上下晃动幅度
    public float verticalSpeed = 1.5f;        // 上下晃动速度

    public float rotationAmplitude = 5f;      // 左右旋转角度范围（单位：度）
    public float rotationSpeed = 2f;          // 旋转速度

    public float jitterAmount = 0.1f;         // 抖动强度
    public float jitterSpeed = 20f;           // 抖动速度

    private Vector3 startPos;
    private Quaternion startRot;
    private float timeOffset;

    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        timeOffset = Random.value * 100f; // 避免多个摄像机同频
    }

    void Update()
    {
        float t = Time.time + timeOffset;

        // 左右晃动
        float offsetX = Mathf.Sin(t * horizontalSpeed) * horizontalAmplitude;

        // 上下晃动（相位错开）
        float offsetY = Mathf.Sin(t * verticalSpeed + Mathf.PI / 3f) * verticalAmplitude;

        // 抖动
        float jitterX = (Mathf.PerlinNoise(t * jitterSpeed, 0f) * 2f - 1f) * jitterAmount;
        float jitterY = (Mathf.PerlinNoise(0f, t * jitterSpeed) * 2f - 1f) * jitterAmount;

        // 旋转摇晃（Z轴）
        float rotZ = Mathf.Sin(t * rotationSpeed + Mathf.PI / 6f) * rotationAmplitude;

        // 应用位置
        transform.position = startPos + new Vector3(offsetX + jitterX, offsetY + jitterY, 0f);

        // 应用旋转（绕 Z 轴）
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
    }
}
