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
    public GameObject telecommunicationScene;
    public GameObject uiBars;
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
    public void GoNextGameStage()
    {
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
            peroid += 1;
            textIndex = 1;
            isTexting = true;
        }
        else if (currentStage == gameStage.afternoonwork)
        {
            currentStage = gameStage.night;
        }
        else if (currentStage == gameStage.night)
        {
            currentStage = gameStage.morningwork;
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

}
