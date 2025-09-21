using UnityEngine;

public class StoveChangeColor : MonoBehaviour
{
    private SpriteRenderer spriteRender;
    private GameObject objectInRange;
    private WorkManager workManager;
    private void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        spriteRender.enabled = false;
        workManager = GameObject.FindWithTag("Manager").GetComponent<WorkManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Object") && DragObjects.isDragging)
        {
            spriteRender.enabled = true;
            objectInRange = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Object"))
        {
            spriteRender.enabled = false;

            if (objectInRange == collision.gameObject)
            {
                objectInRange = null;
            }
        }
    }

   

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && objectInRange != null && this.tag == "BurnPlace")
        {
            Destroy(objectInRange);
            objectInRange = null;
            spriteRender.enabled = false;
        }

        if (Input.GetMouseButtonUp(0) && objectInRange != null && this.tag == "SavePlace")
        {
            workManager.AddToIventoryList(objectInRange);
            objectInRange = null;
            spriteRender.enabled = false;
        }

        if (!DragObjects.isDragging)
        {
            spriteRender.enabled = false;
        }
    }
}
