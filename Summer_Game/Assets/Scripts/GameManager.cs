using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    private float temperature;
    private float thirsty;
    private float urination;
    private float excitement;
    private float satisfaction;

    public string input;

    private float temper_change;
    private bool isAcTurnedOn;
    private float ac_temperature;

    public bool canSleep;

    public float sleepTime;

    private float total_temp;

    public Slider temperature_slider;
    public Slider thirsty_slider;
    public Slider urination_slider;
    public Slider excitement_slider;
    public Slider satisfaction_slider;

    private float normal_temperature_increase = 2f;
    private float normal_thirsty_increase = 1.5f;
    private float normal_urination_increase = 0f;
    private float normal_excitement_increase = -1.5f;
    private float normal_satisfaction_increase = -1.5f;

    public bool isStand = false;
    public bool isSit = false;
    public bool isLayDownMid = false;
    public bool isLayDownLeft = false;
    public bool isLayDownRight = false;
    public bool isOnGround = true;
    public bool isPlayPhone = false;

    public GameObject standIdle;
    public GameObject sitIdle;
    public GameObject layDownMidIdle;
    public GameObject layDownLeftIdle;
    public GameObject layDownRightIdle;
    public GameObject layOngroundIdle;


    // INPUT SYSTEM

    private List<KeyCode> sitAction = new List<KeyCode>() { KeyCode.D, KeyCode.S, KeyCode.W, KeyCode.W };
    private List<KeyCode> standAction = new List<KeyCode>() { KeyCode.W, KeyCode.W, KeyCode.D, KeyCode.D, KeyCode.S, KeyCode.S };
    private List<KeyCode> layDownAction = new List<KeyCode>() { KeyCode.S, KeyCode.S, KeyCode.A, KeyCode.A, KeyCode.D, KeyCode.D };
    private List<KeyCode> layOnGroundAction = new List<KeyCode>() { KeyCode.S, KeyCode.S, KeyCode.S, KeyCode.S, KeyCode.S };
    private List<KeyCode> turnLeftAction = new List<KeyCode>() { KeyCode.A, KeyCode.A, KeyCode.A};
    private List<KeyCode> turnRightAction = new List<KeyCode>() { KeyCode.D, KeyCode.D, KeyCode.D };
    private List<KeyCode> turnMidAction = new List<KeyCode>() { KeyCode.S, KeyCode.S, KeyCode.S };
    private List<KeyCode> turnOnACAction = new List<KeyCode>() { KeyCode.D, KeyCode.D, KeyCode.D, KeyCode.S, KeyCode.S };
    private List<KeyCode> turnOffACAction = new List<KeyCode>() { KeyCode.D, KeyCode.D, KeyCode.D, KeyCode.W, KeyCode.W };
    private List<KeyCode> playPhoneAction = new List<KeyCode>() { KeyCode.S, KeyCode.S, KeyCode.W, KeyCode.W, KeyCode.A, KeyCode.A };
    private List<KeyCode> drinkAction = new List<KeyCode>() { KeyCode.D, KeyCode.D, KeyCode.D, KeyCode.W, KeyCode.A, KeyCode.A };
    private List<KeyCode> urinationAction = new List<KeyCode>() { KeyCode.A, KeyCode.A, KeyCode.D, KeyCode.D, KeyCode.S, KeyCode.S, KeyCode.S, KeyCode.S };


    private List<KeyCode> currentInput = new List<KeyCode>();

    public float MaxtimeLimit = 1.5f;
    public float MintimeLimit = .5f;
    private float lastInputTime = 0f;

    //DIALOGUE SYSTEM

    public TextMeshProUGUI text;
    public TextMeshProUGUI inputText;

    void Start()
    {
        RandomInitialValue();
    }

    // Update is called once per frame
    void Update()
    {
        SetSliderValue();
        UpdateInpute();

        UpdateDisplay();
        UpdateValuesPerSecond();
        UpdateNormalValueChange();


    }

    void SetSliderValue()
    {
        temperature_slider.value = temperature;
        thirsty_slider.value = thirsty;
        urination_slider.value = urination;
        excitement_slider.value = excitement;
        satisfaction_slider.value = satisfaction;
    }

    bool CheckCombo(List<KeyCode> combo)
    {
        if (currentInput.Count != combo.Count) return false;

        for (int i = 0; i < combo.Count; i++)
        {
            if (currentInput[i] != combo[i]) return false;
        }
        return true;
    }
    bool IsPrefixValid(List<KeyCode> input)
    {
        return IsPrefixOf(input, sitAction) || IsPrefixOf(input, standAction)|| IsPrefixOf(input, layDownAction) || IsPrefixOf(input, turnLeftAction)
            || IsPrefixOf(input, turnRightAction) || IsPrefixOf(input, turnMidAction) || IsPrefixOf(input, turnOnACAction) || IsPrefixOf(input, turnOffACAction)
            || IsPrefixOf(input, playPhoneAction) || IsPrefixOf(input, drinkAction) || IsPrefixOf(input, urinationAction) || IsPrefixOf(input, layOnGroundAction);
    }

    bool IsPrefixOf(List<KeyCode> input, List<KeyCode> combo)
    {
        if (input.Count > combo.Count) return false;
        for (int i = 0; i < input.Count; i++)
        {
            if (input[i] != combo[i]) return false;
        }
        return true;
    }

    void UpdateInpute()
    {
        if (Input.GetKeyDown(KeyCode.W)) RegisterInput(KeyCode.W);
        if (Input.GetKeyDown(KeyCode.A)) RegisterInput(KeyCode.A);
        if (Input.GetKeyDown(KeyCode.S)) RegisterInput(KeyCode.S);
        if (Input.GetKeyDown(KeyCode.D)) RegisterInput(KeyCode.D);

        if (currentInput.Count > 0 && Time.time - lastInputTime > MaxtimeLimit)
        {
            text.text = "Oh no, I have forgotten what I should do now.";
            currentInput.Clear();
        }
       
    }
    void RegisterInput(KeyCode key)
    {
        if (lastInputTime > 0f && Time.time - lastInputTime < MintimeLimit)
        {
            text.text = "It's too fast. Forgive me, please.";
            currentInput.Clear();
            return;
        }

        currentInput.Add(key);
        lastInputTime = Time.time; 

        if (CheckCombo(sitAction))
        {
            currentInput.Clear();
            if (isStand || isLayDownMid || isLayDownLeft || isLayDownRight)
            {
                text.text = "So I sit up, it is not easy.";
            }
            else
            {
             
            }
        }
        else if (CheckCombo(standAction))
        {
            Debug.Log("触发招式 B：播放对话 B");
            currentInput.Clear();
        }
        else if (CheckCombo(layDownAction))
        {
            Debug.Log("触发招式 B：播放对话 B");
            currentInput.Clear();
        }
        else if (CheckCombo(turnLeftAction))
        {
            Debug.Log("触发招式 B：播放对话 B");
            currentInput.Clear();
        }
        else if (CheckCombo(turnRightAction))
        {
            Debug.Log("触发招式 B：播放对话 B");
            currentInput.Clear();
        }
        else if (CheckCombo(turnMidAction))
        {
            Debug.Log("触发招式 B：播放对话 B");
            currentInput.Clear();
        }
        else if (CheckCombo(turnOnACAction))
        {
            Debug.Log("触发招式 B：播放对话 B");
            currentInput.Clear();
        }
        else if (CheckCombo(turnOffACAction))
        {
            Debug.Log("触发招式 B：播放对话 B");
            currentInput.Clear();
        }
        else if (CheckCombo(playPhoneAction))
        {
            Debug.Log("触发招式 B：播放对话 B");
            currentInput.Clear();
        }
        else if (CheckCombo(drinkAction))
        {
            Debug.Log("触发招式 B：播放对话 B");
            currentInput.Clear();
        }
        else if (CheckCombo(urinationAction))
        {
            Debug.Log("触发招式 B：播放对话 B");
            currentInput.Clear();
        }
        else if (CheckCombo(layOnGroundAction))
        {
            Debug.Log("触发招式 B：播放对话 B");
            currentInput.Clear();
        }
        else if (!IsPrefixValid(currentInput))
        {
            text.text = "No, I can't.";
            currentInput.Clear();
        }

    }

    void RandomInitialValue()
    {
        temperature = Random.Range(40, 80);
        thirsty = Random.Range(10, 70);
        urination = Random.Range(10, 70);
        excitement = Random.Range(10, 70);
        satisfaction = Random.Range(10, 50);

    }
    void UpdateDisplay()
    {
        if (inputText == null) return;

        inputText.text = "";
        foreach (var key in currentInput)
        {
            inputText.text += key.ToString() + " ";
        }
    }

    void UpdateValuesPerSecond()
    {
        temperature += normal_temperature_increase * Time.deltaTime;
        thirsty += normal_thirsty_increase * Time.deltaTime;
        urination += normal_urination_increase * Time.deltaTime;
        excitement += normal_excitement_increase * Time.deltaTime;
        satisfaction += normal_satisfaction_increase * Time.deltaTime;

    }

    void UpdateNormalValueChange()
    {
        if (isAcTurnedOn && !isOnGround)
        {
            normal_temperature_increase = -1.5f;
        }
        else if (isAcTurnedOn && isOnGround)
        {
            normal_temperature_increase = -3f;
        }
        else if (!isAcTurnedOn && !isOnGround)
        {
            normal_temperature_increase = 1.5f;

        }
        else if (!isAcTurnedOn && isOnGround)
        {
            normal_temperature_increase = -1f;

        }

        if (isPlayPhone)
        {
            normal_satisfaction_increase = 3f;
        }
        else if (!isPlayPhone)
        {
            normal_satisfaction_increase = -1.5f;
        }

        if (thirsty < 50)
        {
            normal_urination_increase = 1.5f;
        }
        else
        {
            normal_urination_increase = 0f;
        }
    }

}
