using UnityEngine;

public class IventoryClick : MonoBehaviour
{
    public int iventoryNumber;
    private Collider2D collider;
    private WorkManager workManager;

    void Start()
    {
        collider = GetComponent<Collider2D>();
        workManager = GameObject.FindWithTag("Manager").GetComponent<WorkManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnMouseDown()
    {
        if (workManager.inventoryList[iventoryNumber - 1] != null)
        {
            workManager.InventoryTakeOutInquiry(iventoryNumber - 1);
        }
    }
}
