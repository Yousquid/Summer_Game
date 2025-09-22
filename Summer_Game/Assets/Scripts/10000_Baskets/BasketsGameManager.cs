using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class BasketsGameManager : MonoBehaviour
{
    public enum gameStage { morningwork, noonbreak, afternoonwork, night }

    public gameStage currentStage;

    public static bool isTexting = false;
    public static bool isSelecting = false;

    public GameObject workScene;
    public GameObject diningScene;
    public GameObject sleepScene;
    public GameObject telecommunicationScene;
    public GameObject uiBars;
    public GameObject sleepButton;

    public GameObject diary;

    private WorkManager workManager;

    public GameObject uiTexts;

    [System.Serializable]
    public class TextGroup
    {
        public List<Texts> textsList; 
    }

    [System.Serializable]
    public class Texts
    {
        public List<string> texts;
    }

    public List<TextGroup> allTexts;
    public static int day = 1;
    public static int peroid = 1;
    public static int textIndex = 1;
    public static int workPeroidCount = 1;

    public GameObject selectionButton_one;
    public GameObject selectionButton_two;
    public GameObject selectionButton_three;

    public static string selectionSituation;

    public TextMeshProUGUI peroidText;

    void Start()
    {
        currentStage = gameStage.morningwork;
        isTexting = true;
        workManager = GetComponent<WorkManager>();
    }

    void Update()
    {
        UpdatePeroid();
    }

    void UpdatePeroid()
    {
        if (peroid == 1)
        {
            peroidText.text = "PEROID: MORNING";
        }
        else if (peroid == 2)
        {
            peroidText.text = "PEROID: NOON";
        }
        else if (peroid == 3)
        {
            peroidText.text = "PEROID: NOON";
        }
        else if (peroid == 4)
        {
            peroidText.text = "PEROID: NIGHT";
        }
    }

    private void DestroyUnstoredObjects()
    {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Object");

        foreach (GameObject obj in allObjects)
        {
            bool isStored = false;

            foreach (GameObject storedObj in workManager.inventoryList)
            {
                if (storedObj == obj)
                {
                    isStored = true;
                    break;
                }
            }

            if (!isStored)
            {
                Destroy(obj);
            }
        }
    }
    public void GoNextGameStage()
    {
        DestroyUnstoredObjects();

        if (currentStage == gameStage.morningwork)
        {
            currentStage = gameStage.noonbreak;
            workScene.SetActive(false);
            uiBars.SetActive(false);
            diningScene.SetActive(true);
            workManager.HideAllInventoryObjects();
            uiTexts.SetActive(false);
            peroid += 1;
            textIndex = 1;
            isTexting = true;
        }
        else if (currentStage == gameStage.noonbreak)
        {
            currentStage = gameStage.afternoonwork;
            workScene.SetActive(true);
            uiBars.SetActive(true);
            diningScene.SetActive(false);
            telecommunicationScene.SetActive(false);
            workManager.ShowAllInventoryObjects();
            uiTexts.SetActive(true);
            isSelecting = false;
            workPeroidCount += 1;
            WorkManager.currentWorkProgress = 0;
            peroid += 1;
            textIndex = 1;
            isTexting = true;
            workManager.InstantiateBasket();
        }
        else if (currentStage == gameStage.afternoonwork)
        {
            currentStage = gameStage.night;
            workScene.SetActive(false);
            uiBars.SetActive(true);
            sleepScene.SetActive(true);
            textIndex = 1;
            peroid += 1;
            isTexting = true;


        }
        else if (currentStage == gameStage.night)
        {
            currentStage = gameStage.morningwork;
            isSelecting = false;
            workScene.SetActive(true);
            uiBars.SetActive(true);
            diningScene.SetActive(false);
            sleepScene.SetActive(false);
            workManager.ShowAllInventoryObjects();
            uiTexts.SetActive(true);
            isSelecting = false;
            workPeroidCount += 1;
            WorkManager.currentWorkProgress = 0;
            day += 1;
            peroid = 1;
            textIndex = 1;
            sleepButton.SetActive(false);
            isTexting = true;

        }
    }

    public void NoonChoice()
    {
        selectionButton_one.SetActive(true);
        selectionButton_one.GetComponentInChildren<TextMeshProUGUI>().text = "T-CENTER";
        selectionButton_one.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_one.GetComponent<Button>().onClick.AddListener(OnClickSelectGoToTCenter);
        selectionButton_two.SetActive(true);
        selectionButton_two.GetComponentInChildren<TextMeshProUGUI>().text = "???";
        selectionButton_two.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_three.SetActive(true);
        selectionButton_three.GetComponentInChildren<TextMeshProUGUI>().text = "???";
        selectionButton_three.GetComponent<Button>().onClick.RemoveAllListeners();



        workManager.talkingBar.SetActive(true);
        workManager.currentTalkingText = "FIVE LEFT MINUTES FOR ME, SHOULD I GO TO THE TELECOMMUNICATION CENTER OR MEET SOMEONE?";
        selectionSituation = "noon";

    }

    public void OnClickSelectGoToTCenter()
    {
        if (selectionSituation == "noon")
        {
            selectionButton_one.GetComponent<Button>().onClick.RemoveAllListeners();
            selectionButton_one.SetActive(false);
            selectionButton_two.SetActive(false);
            selectionButton_three.SetActive(false);
            uiBars.SetActive(true);
            diningScene.SetActive(false);
            workManager.ShowAllInventoryObjects();
            telecommunicationScene.SetActive(true);
            workManager.currentTalkingText = "WORKER 8940, WHAT OBJECT DO YOU WANT TO TELECOMMUNICATE? CLICK THE OBJECT IN YOUR INVENTORY TO TAKE IT OUT, AND THEN DRAG YOUR OBJECT TO MY HAND.";
            workManager.talkingBar.SetActive(true);
            workManager.yesButton.SetActive(true);
            
            isSelecting = true;
        }

    }

    public void OnClickSelectMOfLove()
    {
        workManager.currentTalkingText =
            "THANK YOU FOR YOUR CONTRIBUTION TO THE CONSTRUCTION OF LOVE. YOU MAY GET YOUR REPLY IF THE OBJECT YOU SENT DOES MATTER FOR BOP, YOU MIGHT EVEN GET A PROMOTION. GLORY BELONGS TO BOP!";
        selectionButton_one.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_one.GetComponent<Button>().onClick.AddListener(GoNextGameStage);
        selectionButton_one.GetComponentInChildren<TextMeshProUGUI>().text = "GO BACK TO WORK";

        selectionButton_two.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_two.SetActive(false);
        selectionButton_three.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_three.SetActive(false);
    }

    public void OnClickSelectMOfTruth()
    {
        workManager.currentTalkingText =
            "THANK YOU FOR YOUR CONTRIBUTION TO THE CONSTRUCTION OF TRUTH. YOU MAY GET YOUR REPLY IF THE OBJECT YOU SENT DOES MATTER FOR BOP, YOU MIGHT EVEN GET A PROMOTION. GLORY BELONGS TO BOP!";
        selectionButton_one.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_one.GetComponent<Button>().onClick.AddListener(GoNextGameStage);
        selectionButton_one.GetComponentInChildren<TextMeshProUGUI>().text = "GO BACK TO WORK";

        selectionButton_two.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_two.SetActive(false);
        selectionButton_three.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_three.SetActive(false);
        workManager.yesButton.SetActive(true);
    }

    public void CancelSelections()
    {
        selectionButton_one.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_one.SetActive(false);
        selectionButton_two.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_two.SetActive(false);
        selectionButton_three.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_three.SetActive(false);
        workManager.talkingBar.SetActive(false);

    }
    public void ShowTCommunicationSelections()
    {
        workManager.currentTalkingText = 
       "WHERE DO YOU WANT TO SEND TO? ACCORDING TO BOP RULE 4, PEOPLE ARE REQUIRED TO BE HONEST AND RESPONSIBLE. REPORT YOUR SUSPICIOUS COLEAGUES TO THE MINISTRY OF LOVE, REPORT HISTORY AND CULTURE RELATED AFFAIRS TO THE MINISTRY OF TRUTH. GLORY BELOGNS TO BOP!";
        workManager.talkingBar.SetActive(true);
        selectionButton_one.SetActive(true);
        selectionButton_one.GetComponentInChildren<TextMeshProUGUI>().text = "MINISTRY OF LOVE";
        selectionButton_one.GetComponent<Button>().onClick.AddListener(OnClickSelectMOfLove);
        selectionButton_two.SetActive(true);
        selectionButton_two.GetComponentInChildren<TextMeshProUGUI>().text = "MINISTRY OF TRUTH";
        selectionButton_two.GetComponent<Button>().onClick.AddListener(OnClickSelectMOfTruth);
        selectionButton_three.SetActive(true);
        selectionButton_three.GetComponentInChildren<TextMeshProUGUI>().text = "SEND ANOTHER ONE";
        selectionButton_three.GetComponent<Button>().onClick.AddListener(CancelSelections);

    }

    public void ShowFirstNightSelections()
    {
        workManager.currentTalkingText = "What do I want to write about?";
        selectionSituation = "night";
        workManager.talkingBar.SetActive(true);
        selectionButton_one.SetActive(true);
        selectionButton_one.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_one.GetComponentInChildren<TextMeshProUGUI>().text = "The FINGER & ARM I found...";
        selectionButton_one.GetComponent<Button>().onClick.AddListener(WriteAboutFinger);
        selectionButton_two.SetActive(true);
        selectionButton_two.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_two.GetComponentInChildren<TextMeshProUGUI>().text = "The BULLETS I found...";
        selectionButton_two.GetComponent<Button>().onClick.AddListener(WriteAboutBullets);
        selectionButton_three.SetActive(true);
        selectionButton_three.GetComponentInChildren<TextMeshProUGUI>().text = "The NEW POLICY...";
        selectionButton_three.GetComponent<Button>().onClick.AddListener(WriteAboutPolicy);
    }

    public void WriteAboutFinger()
    {
        workManager.currentTalkingText = "I found a cut finger and an arm in the baskets today. This is insane, I really want to know what happened. The arm seems very familiar to me, but I cannot remember whom it belongs to, we cannot make friends, sorry.";
        selectionButton_two.SetActive(false);
        selectionButton_three.SetActive(false);
        selectionButton_one.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_one.GetComponentInChildren<TextMeshProUGUI>().text = "Finish";
        selectionButton_one.GetComponent<Button>().onClick.AddListener(FinishFinger);

    }

    public void WriteAboutBullets()
    {
        workManager.currentTalkingText = "I found a bar of bullets in the basket today. I might need them one day. But how could it get inside of the basket? Am I acutually working for the Ministry of Peace?";
        selectionButton_two.SetActive(false);
        selectionButton_three.SetActive(false);
        selectionButton_one.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_one.GetComponentInChildren<TextMeshProUGUI>().text = "Finish";
        selectionButton_one.GetComponent<Button>().onClick.AddListener(FinishBullet);
    }

    public void WriteAboutPolicy()
    {
        workManager.currentTalkingText = "The new policy in the factory adds more working hours for every worker. Now I have to work 12 hours a day, I think I might need to use my social credit to exchange for some focusing water to handle these works.";
        selectionButton_two.SetActive(false);
        selectionButton_three.SetActive(false);
        selectionButton_one.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_one.GetComponentInChildren<TextMeshProUGUI>().text = "Finish";
        selectionButton_one.GetComponent<Button>().onClick.AddListener(FinishPolicy);
    }


    public void FinishFinger()
    {
        GameObject spwaned_diary = Instantiate(diary, new Vector3(0, 0, 0), Quaternion.identity);
        spwaned_diary.GetComponent<DragObjects>().description = "THE DIARY ABOUT THE FINGER AND ARM I FOUND";
        workManager.currentTalkingText = "I can carry the diary I wrote to the factory. I really want to find someone to discuss about this. But I should be careful, I must find one I trust, and give this piece of paper to that one without letting BOP know.";
        isSelecting = true;
        selectionButton_one.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_one.SetActive(false);
        selectionButton_two.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_two.SetActive(false);
        selectionButton_three.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_three.SetActive(false);
        workManager.yesButton.SetActive(true);
    }

    public void FinishBullet()
    {
        GameObject spwaned_diary = Instantiate(diary, new Vector3(0, 0, 0), Quaternion.identity);
        spwaned_diary.GetComponent<DragObjects>().description = "THE DIARY ABOUT THE BULLETS I FOUND";
        workManager.currentTalkingText = "I can carry the diary I wrote to the factory. I really want to find someone to discuss about this. But I should be careful, I must find one I trust, and give this piece of paper to that one without letting BOP know.";
        isSelecting = true;
        selectionButton_one.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_one.SetActive(false);
        selectionButton_two.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_two.SetActive(false);
        selectionButton_three.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_three.SetActive(false);
        workManager.yesButton.SetActive(true);
    }

    public void FinishPolicy()
    {
        GameObject spwaned_diary = Instantiate(diary, new Vector3(0, 0, 0), Quaternion.identity);
        spwaned_diary.GetComponent<DragObjects>().description = "THE DIARY ABOUT THE POLICY";
        workManager.currentTalkingText = "I can carry the diary I wrote to the factory. I really want to find someone to discuss about this. But I should be careful, I must find one I trust, and give this piece of paper to that one without letting BOP know.";
        isSelecting = true;
        selectionButton_one.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_one.SetActive(false);
        selectionButton_two.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_two.SetActive(false);
        selectionButton_three.GetComponent<Button>().onClick.RemoveAllListeners();
        selectionButton_three.SetActive(false);
        workManager.yesButton.SetActive(true);
    }
   
    public void ShowSecondNightSelections()
    { 
        
    }

}
