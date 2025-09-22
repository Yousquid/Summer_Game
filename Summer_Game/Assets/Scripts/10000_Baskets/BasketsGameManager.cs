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
            peroidText.text = "PEROID: AFTERNOON";
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
        //selectionButton_two.SetActive(true);
        selectionButton_three.SetActive(true);
        selectionButton_three.GetComponentInChildren<TextMeshProUGUI>().text = "???";

        
        workManager.talkingBar.SetActive(true);
        workManager.currentTalkingText = "FIVE LEFT MINUTES FOR ME, SHOULD I GO TO THE TELECOMMUNICATION CENTER OR MEET SOMEONE?";
        selectionSituation = "noon";

    }

    public void OnClickSelect()
    {
        if (selectionSituation == "noon")
        {
            diningScene.SetActive(false);
            workManager.ShowAllInventoryObjects();
            telecommunicationScene.SetActive(true);
        }
        
    }

}
