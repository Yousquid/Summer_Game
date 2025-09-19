using UnityEngine;

public class DragBasket : MonoBehaviour
{
    private Vector3 offset;
    private Camera mainCam;
    private bool isDragging = false;


    private Collider2D thisCollider;

    private Rect allowedArea = new Rect(-5.7F, -2.5F, 11F, 9F);
    private GameObject innerObject;
    



    void Start()
    {
        mainCam = Camera.main;
        thisCollider = GetComponent<Collider2D>();
        innerObject = transform.GetChild(0).gameObject;

    }

    void OnMouseDown()
    {
        Vector3 mousePos = GetMouseWorldPos();
        offset = transform.position - mousePos;
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (isDragging && !WorkManager.isLightOn)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Light" && innerObject != null)
        { 
            
        }
    }

    private void Update()
    {
        if (WorkManager.isLightOn)
        {
            Rigidbody2D rigidbody = this.GetComponent<Rigidbody2D>();
            rigidbody.gravityScale = 0;
            PolygonCollider2D collider = this.GetComponent<PolygonCollider2D>();
            collider.enabled = false;
            innerObject.SetActive(true);
            innerObject.transform.SetParent(null, true);
        }
        else if (!WorkManager.isLightOn) {
            Rigidbody2D rigidbody = this.GetComponent<Rigidbody2D>();
            rigidbody.gravityScale = 1;
            PolygonCollider2D collider = this.GetComponent<PolygonCollider2D>();
            collider.enabled = true;
        }
    }
}
