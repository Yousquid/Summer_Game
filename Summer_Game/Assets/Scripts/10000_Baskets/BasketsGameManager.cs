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

    public TextMeshProUGUI peroidText;

    void Start()
    {
        currentStage = gameStage.morningwork;
        isTexting = true;
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

   
}
