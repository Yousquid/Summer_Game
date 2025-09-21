using UnityEngine;

public class DragBasket : MonoBehaviour
{
    private Vector3 offset;
    private Camera mainCam;
    public static bool isDragging = false;


    private Collider2D thisCollider;

    private Rect allowedArea = new Rect(-5.7F, -2.5F, 11F, 9F);
    private GameObject innerObject;

    public bool isDefective;
    public bool hasInnderObject;
    private WorkManager workManager;


    void Start()
    {
        mainCam = Camera.main;
        thisCollider = GetComponent<Collider2D>();
        if (transform.childCount > 0)
        {
            innerObject = transform.GetChild(0).gameObject;
            hasInnderObject = true;
            innerObject.SetActive(false);

        }
        workManager = GameObject.FindWithTag("Manager").GetComponent<WorkManager>();



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
        if (collision.tag == "BasketEnd")
        {
            if (isDefective)
            {
                WorkManager.social_credit -= 5; 
            }
            if (hasInnderObject)
            {
                WorkManager.social_credit -= 5;
            }
            if (!isDefective && !hasInnderObject)
            {
                WorkManager.social_credit += 1;
            }
            WorkManager.currentWorkProgress += 1;
            workManager.InstantiateBasket();
            DestroySelf();
            
        }
        if (collision.tag == "Light")
        {
            innerObject.SetActive(true);
            hasInnderObject = false;
            innerObject.transform.SetParent(null, true);
        }
    }

    private void Update()
    {
        if (WorkManager.isLightOn)
        {
            Rigidbody2D rigidbody = this.GetComponent<Rigidbody2D>();
            rigidbody.gravityScale = 0;
            PolygonCollider2D collider = this.GetComponent<PolygonCollider2D>();
            
        }
        else if (!WorkManager.isLightOn) {
            Rigidbody2D rigidbody = this.GetComponent<Rigidbody2D>();
            rigidbody.gravityScale = 1;
            PolygonCollider2D collider = this.GetComponent<PolygonCollider2D>();
        }
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
