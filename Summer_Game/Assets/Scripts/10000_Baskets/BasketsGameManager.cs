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

    // Update is called once per frame
    void Update()
    {
        
    }
}
