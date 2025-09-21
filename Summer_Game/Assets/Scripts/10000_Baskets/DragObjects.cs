using UnityEngine;

public class DragObjects : MonoBehaviour
{
    private Vector3 offset;
    private Camera mainCam;
    public static bool isDragging = false;


    private Collider2D thisCollider;

    private Rect allowedArea = new Rect(-5.7F, -2.5F, 11F, 9F);

    public string description;
    public Vector3 scale;


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
        if (isDragging )
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

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
