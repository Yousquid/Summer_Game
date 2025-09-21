using UnityEngine;

public class StoveChangeColor : MonoBehaviour
{
    private SpriteRenderer spriteRender;
    private GameObject objectInRange;
    private WorkManager workManager;
    private bool isLastObjectBasket = false;
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
            isLastObjectBasket = false;

        }
        if (collision.CompareTag("Basket") && DragBasket.isDragging)
        {
            spriteRender.enabled = true;
            objectInRange = collision.gameObject;
            isLastObjectBasket = true;
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
        if (collision.CompareTag("Basket"))
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
            workManager.DestroyObjectInquiry(objectInRange);
            objectInRange = null;
            spriteRender.enabled = false;
        }

        if (Input.GetMouseButtonUp(0) && objectInRange != null && this.tag == "SavePlace")
        {
            workManager.KeepObjectInquiry(objectInRange);
            objectInRange = null;
            spriteRender.enabled = false;
        }

        if (!DragObjects.isDragging && !isLastObjectBasket)
        {
            spriteRender.enabled = false;
        }
        else if (!DragBasket.isDragging && isLastObjectBasket)
        {
            spriteRender.enabled = false;
        }
    }

    
}
