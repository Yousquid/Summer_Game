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
    public Slider thirsty_slider;
    public Slider urination_slider;
    public Slider excitement_slider;
    public Slider satisfaction_slider;

    private float normal_temperature_increase;
    private float normal_thirsty_increase;
    private float normal_urination_increase;
    private float normal_excitement_increase;
    private float normal_satisfaction_increase;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        temperature_slider.value = temperature;
        thirsty_slider.value = thirsty;
        urination_slider.value = urination;
        excitement_slider.value = excitement;
        satisfaction_slider.value = satisfaction;
    }
}
