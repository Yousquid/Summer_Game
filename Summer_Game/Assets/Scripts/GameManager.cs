using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float temperature = 40;
    public float thirsty;
    public float urination;
    public float excitement;
    public float satisfaction;

    public string input;

    private float temper_change;
    private bool isAcTurnedOn;
    private float ac_temperature;

    public bool canSleep;

    public float sleepTime;

    public float total_temp = 100;

    public Slider temperature_slider;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        temperature_slider.value = temperature;
    }
}
