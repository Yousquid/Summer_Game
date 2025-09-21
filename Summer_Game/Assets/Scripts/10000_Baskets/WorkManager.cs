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

    public GameObject talkingBar;
    public TextMeshProUGUI talkingText;
    private string currentTalkingText;
    public GameObject buttons;
    private GameObject currentObject;
    private bool isAskingKeeping;
    private bool isAskingDestroying;


    void Start()
    {
        buttons.SetActive(false);
        talkingBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        LightControl();
        talkingText.text = currentTalkingText;
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

    private void AddToIventoryList(GameObject gameObject)
    {
        inventoryList.Add(gameObject);
        SetInventoryPositions(); 

    }

    public void KeepObjectInquiry(GameObject gameObject)
    {
        talkingBar.SetActive(true);
        buttons.SetActive(true);
        currentTalkingText = gameObject.GetComponent<DragObjects>().description + " Do you want to keep it?";
        currentObject = gameObject;
        isAskingKeeping = true;
    }


    public void DestroyObjectInquiry(GameObject gameObject)
    {
        talkingBar.SetActive(true);
        buttons.SetActive(true);
        currentTalkingText = gameObject.GetComponent<DragObjects>().description + " Do you want to dispose it?";
        currentObject = gameObject;
        isAskingDestroying = true;
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

    public void OnClickKeepOrDestroyObject()
    {
        if (isAskingKeeping)
        {
            AddToIventoryList(currentObject);
            currentObject = null;
            currentTalkingText = "";
            talkingBar.SetActive(false);
            buttons.SetActive(false);
            isAskingKeeping = false;
        }
        else if (isAskingDestroying)
        {
            currentObject.GetComponent<DragObjects>().DestroySelf();
            talkingBar.SetActive(false);
            buttons.SetActive(false);
            isAskingDestroying = false;

        }
    }


    public void OnClickCancel()
    {
        talkingBar.SetActive(false);
        buttons.SetActive(false);
    }
}
