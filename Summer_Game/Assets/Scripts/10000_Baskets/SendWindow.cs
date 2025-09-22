using UnityEngine;

public class SendWindow : MonoBehaviour
{
    private GameObject objectInRange;
    private BasketsGameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindWithTag("Manager").GetComponent<BasketsGameManager>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Object") && DragObjects.isDragging)
        {
            objectInRange = collision.gameObject;

        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Object") && DragObjects.isDragging)
        {
            objectInRange = null;

        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && objectInRange != null)
        {
            objectInRange = null;
            gameManager.ShowTCommunicationSelections();
        }
    }
}
