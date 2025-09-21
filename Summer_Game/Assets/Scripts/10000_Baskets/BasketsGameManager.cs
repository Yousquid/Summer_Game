using UnityEngine;

public class BasketsGameManager : MonoBehaviour
{
    public enum gameStage { morningwork, noonbreak, afternoonwork, night }

    public gameStage currentStage;


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
