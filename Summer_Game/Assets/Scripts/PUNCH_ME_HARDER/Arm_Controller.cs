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

    private Vector3 initialPos;
    private Vector3 lastMousePos;
    private float currentVelocity = 0f;
    private bool isCharging = false;

    void Start()
    {
        if (arm == null) arm = transform;
        initialPos = arm.localPosition;
        lastMousePos = Input.mousePosition;
    }

    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;

        }
        Vector3 currentMousePos = Input.mousePosition;
        float deltaY = currentMousePos.y - lastMousePos.y;

        if (deltaY > 0)
        {
            isCharging = false;
        }
        else if (deltaY < 0f)
        {
            isCharging = true;
        }

        if (!isCharging)
        {
            currentVelocity += deltaY * sensitivity;
        }
        else
        {
            currentVelocity += deltaY * sensitivity / 2;
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

        lastMousePos = currentMousePos;
    }

    
}
