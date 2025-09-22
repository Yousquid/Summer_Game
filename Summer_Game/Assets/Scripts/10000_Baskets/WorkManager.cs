using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class WorkManager : MonoBehaviour
{
    public GameObject light;
    public GameObject eatingLight;
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

    public GameObject noButton;
    public GameObject yesButton;

    public static int currentWorkProgress = 0;

    public GameObject talkingBar;
    public TextMeshProUGUI talkingText;
    public string currentTalkingText;
    public GameObject buttons;
    private GameObject currentObject;
    private bool isAskingKeeping;
    private bool isAskingDestroying;
    private bool isAskingTakingOut;
    private int currentInventoryListIndex;

    public static int social_credit = 80;
    public TextMeshProUGUI social_credit_text;
    public TextMeshProUGUI work_number_text;
    public TextMeshProUGUI date_text;


    public static int work_finished_number = 0;



    void Start()
    {
        yesButton.SetActive(true);
        noButton.SetActive(false);
        gameManager = GetComponent<BasketsGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        LightControl();
        talkingText.text = currentTalkingText;
        TextMode();
        social_credit_text.text = $"SOCIAL CREDIT: {social_credit}";
        work_number_text.text = $"{10000 - work_finished_number * 1000}";
        date_text.text = $"1984.09.{21 + BasketsGameManager.day}";
    }

    void LightControl()
    {
        if (Input.GetKeyDown(KeyCode.S) && !isLightOn)
        {
            if (BasketsGameManager.peroid == 1 || BasketsGameManager.peroid == 3 || BasketsGameManager.peroid == 4)
            {
                light.SetActive(true);
                isLightOn = true;
            }
            else if (BasketsGameManager.peroid == 2)
            {
                eatingLight.SetActive(true);
                isLightOn = true;
            }


        }
        if (Input.GetKeyDown(KeyCode.W) && isLightOn)
        {
            light.SetActive(false);
            eatingLight.SetActive(false);
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
        yesButton.SetActive(true);
        noButton.SetActive(true);
        if (gameObject.GetComponent<DragObjects>() != null)
        {
            currentTalkingText = gameObject.GetComponent<DragObjects>().description + " DO YOU WANT TO KEEP IT?";
            currentObject = gameObject;
            isAskingKeeping = true;
        }
        else if (gameObject.GetComponent<DragBasket>() != null)
        {
            currentTalkingText = "ACCORDING TO BOP RULE 31, WORKERS SHOULD NOT KEEP ANY PRODUCTION OUTPUT, GLORY BELONGS TO BOP!";
            yesButton.SetActive(false);
            noButton.SetActive(false);
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
        yesButton.SetActive(true);
        noButton.SetActive(true);
        currentTalkingText = inventoryList[iventoryIndex].GetComponent<DragObjects>().description + " DO YOU WANT TO TAKE IT OUT?";
            isAskingTakingOut = true;
            currentObject = inventoryList[iventoryIndex];
            currentInventoryListIndex = iventoryIndex;
        
    }
    public void DestroyObjectInquiry(GameObject gameObject)
    {
        talkingBar.SetActive(true);
        yesButton.SetActive(true);
        noButton.SetActive(true);
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
        if (!BasketsGameManager.isTexting)
        {
            if (isAskingKeeping)
            {
                AddToIventoryList(currentObject);
                currentObject = null;
                currentTalkingText = "";
                talkingBar.SetActive(false);
                yesButton.SetActive(false);
                noButton.SetActive(false);
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
                    work_finished_number += 1;
                    InstantiateBasket();

                }
                talkingBar.SetActive(false);
                yesButton.SetActive(false);
                noButton.SetActive(false);
                isAskingDestroying = false;

            }
            else if (isAskingTakingOut)
            {
                GameObject gameObject = Instantiate(currentObject, Vector3.zero, Quaternion.identity);
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
                yesButton.SetActive(false);
                noButton.SetActive(false);
                isAskingTakingOut = false;
            }
        }
    }

    public void TextMode()
    {
        if (BasketsGameManager.isTexting)
        {
            noButton.SetActive(false);
            yesButton.SetActive(true);
            talkingBar.SetActive(true);
            currentTalkingText = gameManager.allTexts[BasketsGameManager.day - 1].textsList[BasketsGameManager.peroid - 1].texts[BasketsGameManager.textIndex-1];

        }
    }

    private void CloseTextMode()
    {
        noButton.SetActive(false);
        yesButton.SetActive(false);
        talkingBar.SetActive(false);
        BasketsGameManager.isTexting = false;
    }

    public void OnClickContinueText()
    {
        if (BasketsGameManager.isTexting)
        {
            if (BasketsGameManager.isTexting && BasketsGameManager.textIndex < gameManager.allTexts[BasketsGameManager.day - 1].textsList[BasketsGameManager.peroid - 1].texts.Count)
            {
                BasketsGameManager.textIndex += 1;
            }
            else if (BasketsGameManager.textIndex == gameManager.allTexts[BasketsGameManager.day - 1].textsList[BasketsGameManager.peroid - 1].texts.Count)
            {
                CloseTextMode();

                if (BasketsGameManager.peroid == 1 && currentWorkProgress == 0)
                {
                    InstantiateBasket();
                    currentWorkProgress += 1;
                }
            }
        }

        

    }

    public void HideAllInventoryObjects()
    {
        foreach (var obj in inventoryList)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    public void ShowAllInventoryObjects()
    {
        foreach (var obj in inventoryList)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }

    public void OnClickCancel()
    {
        talkingBar.SetActive(false);
        noButton.SetActive(false);
        yesButton.SetActive(false);
    }
}
