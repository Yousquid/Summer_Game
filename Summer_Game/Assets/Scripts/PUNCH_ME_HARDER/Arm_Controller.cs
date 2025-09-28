using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class Arm_Controller : MonoBehaviour
{
    public Transform arm;

    public float minY = -2f;
    public float maxY = 2f;
    public float sensitivity = 0.01f;
    public float damping = 5f;
    public float jitterAmplitude = 0.1f;
    public float jitterFrequency = 10f;

    public Vector3 initialPos;
    private Vector3 lastMousePos;
    private float currentVelocity = 0f;

    private bool isCharging = false;
    private bool isReady = false; 

    void Start()
    {
        if (arm == null) arm = transform;
        lastMousePos = Input.mousePosition;
    }

    void OnEnable()
    {
        isReady = false;
        currentVelocity = 0f;
        arm.localPosition = initialPos;
        lastMousePos = Input.mousePosition;
    }

    void Update()
    {
        Cursor.visible = !this.gameObject.activeSelf;

        if (!isReady)
        {
            Vector3 currentMousePos = Input.mousePosition;
            float deltaY = currentMousePos.y - lastMousePos.y;

            if (deltaY < 0f)
            {
                isReady = true;
            }

            lastMousePos = currentMousePos;
            return; // 
        }

        Vector3 nowMousePos = Input.mousePosition;
        float dY = nowMousePos.y - lastMousePos.y;

        if (dY > 0)
        {
            isCharging = false; 
        }
        else if (dY < 0f)
        {
            isCharging = true; 
        }

        if (!isCharging)
        {
            currentVelocity += dY * sensitivity;
        }
        else
        {
            currentVelocity += dY * sensitivity / 2;
        }

        Vector3 newPos = arm.localPosition;
        newPos.y += currentVelocity * Time.deltaTime;
        newPos.y = Mathf.Clamp(newPos.y, initialPos.y + minY, initialPos.y + maxY);

        if (isCharging)
        {
            newPos.x = initialPos.x + Mathf.Sin(Time.time * jitterFrequency) * jitterAmplitude;
        }
        else
        {
            newPos.x = initialPos.x;
        }

        arm.localPosition = newPos;
        currentVelocity = Mathf.Lerp(currentVelocity, 0, Time.deltaTime * damping);

        lastMousePos = nowMousePos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        float impactSpeed = Mathf.Abs(currentVelocity);

        float intensity = Mathf.Clamp(impactSpeed * 0.01f, 0.05f, 0.5f); 
        float duration = Mathf.Clamp(impactSpeed * 0.002f, 0.3f, 0.6f);

        if (ScreenShake.Instance != null)
        {
            ScreenShake.Instance.Shake(intensity, duration);
            UI_Shake.Instance.Shake(intensity, duration);
        }
    }
}
