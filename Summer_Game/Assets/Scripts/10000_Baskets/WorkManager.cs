using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class WorkManager : MonoBehaviour
{
    public GameObject light;
    public static bool isLightOn = false;
    public List<GameObject> inventoryList;
    public List<Transform> inventoryListPosition;

    private BasketsGameManager gameManager;

    [System.Serializable]
    public class Basket
    {
        public List<GameObject> items;
    }

    public List<Basket> basketList;
    public Transform basketSpwanPosition;

    public static int currentWorkProgress = 0;

    public GameObject talkingBar;
    public TextMeshProUGUI talkingText;
    private string currentTalkingText;
    public GameObject buttons;
    private GameObject currentObject;
    private bool isAskingKeeping;
    private bool isAskingDestroying;
    private bool isAskingTakingOut;
    private int currentInventoryListIndex;

    public static int social_credit = 80;


    void Start()
    {
        buttons.SetActive(false);
        talkingBar.SetActive(false);
        gameManager = GetComponent<BasketsGameManager>();
        InstantiateBasket();
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
        bool added = false;
        for (int i = 0; i < inventoryList.Count; i++)
        {
            if (inventoryList[i] == null)
            {
                inventoryList[i] = gameObject;
                added = true;
                break;
            }
        }

        if (!added)
        {
            inventoryList.Add(gameObject);
        }

        SetInventoryPositions();

    }

    public void InstantiateBasket()
    {
        int dayIndex = BasketsGameManager.day - 1;

        if (dayIndex >= 0 && dayIndex < basketList.Count)
        {
            Basket currentBasket = basketList[dayIndex];

            if (currentWorkProgress >= 0 && currentWorkProgress < currentBasket.items.Count)
            {
                Instantiate(currentBasket.items[currentWorkProgress], basketSpwanPosition.position, Quaternion.identity);
            }
            else if (currentWorkProgress == currentBasket.items.Count)
            {
                gameManager.GoNextGameStage();
            }
            
        }
       
    }
    public void KeepObjectInquiry(GameObject gameObject)
    {
        talkingBar.SetActive(true);
        buttons.SetActive(true);
        if (gameObject.GetComponent<DragObjects>() != null)
        {
            currentTalkingText = gameObject.GetComponent<DragObjects>().description + " DO YOU WANT TO KEEP IT?";
            currentObject = gameObject;
            isAskingKeeping = true;
        }
        else if (gameObject.GetComponent<DragBasket>() != null)
        {
            currentTalkingText = "ACCORDING TO BOP RULE 31, WORKERS SHOULD NOT KEEP ANY PRODUCTION OUTPUT, GLORY BELONGS TO BOP!";
            buttons.SetActive(false);
            StartCoroutine(HideUIAfterDelay(2.8f));
        }
    }

    private IEnumerator HideUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        talkingBar.SetActive(false);
    }

    public void InventoryTakeOutInquiry(int iventoryIndex)
    {
        talkingBar.SetActive(true);
        buttons.SetActive(true);
        
            currentTalkingText = inventoryList[iventoryIndex].GetComponent<DragObjects>().description + " DO YOU WANT TO TAKE IT OUT?";
            isAskingTakingOut = true;
            currentObject = inventoryList[iventoryIndex];
            currentInventoryListIndex = iventoryIndex;
        
    }
    public void DestroyObjectInquiry(GameObject gameObject)
    {
        talkingBar.SetActive(true);
        buttons.SetActive(true);
        if (gameObject.GetComponent<DragObjects>() != null)
        {
            currentTalkingText = gameObject.GetComponent<DragObjects>().description + " DO YOU WANT TO DISPOSE IT?";
        }
        else if (gameObject.GetComponent<DragBasket>() != null)
        {
            currentTalkingText = "DO YOU WANT TO DISPOSE THIS BASKET?";
        }
        
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
                Collider2D collider = inventoryList[i].GetComponent<Collider2D>();
                if (rb2d != null)
                {
                    rb2d.simulated = false;   
                    rb2d.angularVelocity = 0f;
                }
                if (collider != null)
                {
                    collider.isTrigger = true;
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
            if (currentObject.GetComponent<DragObjects>() != null)
            {
                currentObject.GetComponent<DragObjects>().DestroySelf();

            }
            else if (currentObject.GetComponent<DragBasket>() != null)
            {
                currentObject.GetComponent<DragBasket>().DestroySelf();
                currentWorkProgress += 1;
                InstantiateBasket();
            }
            talkingBar.SetActive(false);
            buttons.SetActive(false);
            isAskingDestroying = false;

        }
        else if (isAskingTakingOut)
        {
            GameObject gameObject = Instantiate(currentObject,Vector3.zero,Quaternion.identity);
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            Rigidbody2D rb2d = gameObject.GetComponent<Rigidbody2D>();
            rb2d.simulated = true;
            Collider2D collider = gameObject.GetComponent<Collider2D>();
            collider.isTrigger = false;
            currentObject = null;
            Destroy(inventoryList[currentInventoryListIndex]);
            inventoryList[currentInventoryListIndex] = null;
            currentInventoryListIndex = 10;
            currentTalkingText = "";
            talkingBar.SetActive(false);
            buttons.SetActive(false);
            isAskingTakingOut = false;
        }

    }


    public void OnClickCancel()
    {
        talkingBar.SetActive(false);
        buttons.SetActive(false);
    }
}
