using UnityEngine;

public class DragObjects : MonoBehaviour
{
    private Vector3 offset;
    private Camera mainCam;
    private bool isDragging = false;

    public LayerMask forbiddenLayer;

    private Collider2D thisCollider;

    public Rect allowedArea = new Rect(-5, -3, 10, 6);

    void Start()
    {
        mainCam = Camera.main;
        thisCollider = GetComponent<Collider2D>();
    }

    void OnMouseDown()
    {
        Vector3 mousePos = GetMouseWorldPos();
        offset = transform.position - mousePos;
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePos = GetMouseWorldPos();
            Vector3 newPos = mousePos + offset;

            newPos.x = Mathf.Clamp(newPos.x, allowedArea.xMin, allowedArea.xMax);
            newPos.y = Mathf.Clamp(newPos.y, allowedArea.yMin, allowedArea.yMax);

            transform.position = newPos;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = -mainCam.transform.position.z;
        return mainCam.ScreenToWorldPoint(mousePoint);
    }
}
