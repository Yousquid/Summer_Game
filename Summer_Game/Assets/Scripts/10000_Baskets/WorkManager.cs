using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class WorkManager : MonoBehaviour
{
    public List<GameObject> basketList;
    public GameObject light;
    public static bool isLightOn = false;
    public List<GameObject> inventoryList;
    public List<Transform> inventoryListPosition;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LightControl();
    }

    void LightControl()
    {
        if (Input.GetKeyDown(KeyCode.S) && !isLightOn)
        { 
            light.SetActive(true);
            isLightOn = true;
        }
        if (Input.GetKeyDown(KeyCode.W) && isLightOn)
        {
            light.SetActive(false);
            isLightOn = false;
        }

    }

    public void AddToIventoryList(GameObject gameObject)
    {
        inventoryList.Add(gameObject);
        SetInventoryPositions(); 

    }

    private void SetInventoryPositions()
    {
        int count = Mathf.Min(inventoryList.Count, inventoryListPosition.Count);

        for (int i = 0; i < count; i++)
        {
            if (inventoryList[i] != null && inventoryListPosition[i] != null)
            {
                inventoryList[i].transform.position = inventoryListPosition[i].position;
                Rigidbody2D rb2d = inventoryList[i].GetComponent<Rigidbody2D>();
                if (rb2d != null)
                {
                    rb2d.simulated = false;   
                    rb2d.angularVelocity = 0f;
                }
                DragObjects objectScript = inventoryList[i].GetComponent<DragObjects>();
                inventoryList[i].transform.localScale = objectScript.scale;
            }
        }
    }
}
