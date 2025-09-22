using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class BasketsGameManager : MonoBehaviour
{
    public enum gameStage { morningwork, noonbreak, afternoonwork, night }

    public gameStage currentStage;

    [System.Serializable]
    public class Texts
    {
        public List<string> texts;
    }

    public List<Texts> textsList;
    public static int day = 1;
    void Start()
    {
        currentStage = gameStage.morningwork; 
    }

    void Update()
    {
        
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
