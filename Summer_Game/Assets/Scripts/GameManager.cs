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

    private float normal_temperature_increase = 1.5f;
    private float normal_thirsty_increase = 1f;
    private float normal_urination_increase = .75f;
    private float normal_excitement_increase = -1.5f;
    private float normal_satisfaction_increase = -1.5f;

    enum PlayerIdle { Stand, Sit, LayDownMiddle, LayDownLeft, LayDownRight, LayOnGround, PlayPhone}
    PlayerIdle currentIdle = PlayerIdle.LayOnGround;

    public GameObject standIdle;
    public GameObject sitIdle;
    public GameObject layDownMidIdle;
    public GameObject layDownLeftIdle;
    public GameObject layDownRightIdle;
    public GameObject layOngroundIdle;
    public GameObject playPhoneIdle;



    // INPUT SYSTEM

    private List<KeyCode> sitAction = new List<KeyCode>() { KeyCode.D, KeyCode.S, KeyCode.W, KeyCode.W };
    private List<KeyCode> standAction = new List<KeyCode>() { KeyCode.W, KeyCode.W, KeyCode.D, KeyCode.D, KeyCode.S, KeyCode.S };
    private List<KeyCode> layDownAction = new List<KeyCode>() { KeyCode.S, KeyCode.S, KeyCode.A, KeyCode.A, KeyCode.D, KeyCode.D };
    private List<KeyCode> layOnGroundAction = new List<KeyCode>() { KeyCode.S, KeyCode.S, KeyCode.S, KeyCode.S, KeyCode.S };
    private List<KeyCode> turnLeftAction = new List<KeyCode>() { KeyCode.A, KeyCode.A, KeyCode.A};
    private List<KeyCode> turnRightAction = new List<KeyCode>() { KeyCode.D, KeyCode.D, KeyCode.D };
    private List<KeyCode> turnMidAction = new List<KeyCode>() { KeyCode.S, KeyCode.S, KeyCode.S };
    private List<KeyCode> turnOnACAction = new List<KeyCode>() { KeyCode.D, KeyCode.D, KeyCode.W, KeyCode.S, KeyCode.S };
    private List<KeyCode> turnOffACAction = new List<KeyCode>() { KeyCode.D, KeyCode.D, KeyCode.S, KeyCode.W, KeyCode.W };
    private List<KeyCode> playPhoneAction = new List<KeyCode>() { KeyCode.S, KeyCode.S, KeyCode.W, KeyCode.W, KeyCode.A, KeyCode.A };
    private List<KeyCode> drinkAction = new List<KeyCode>() { KeyCode.D, KeyCode.D, KeyCode.W, KeyCode.A, KeyCode.A };
    private List<KeyCode> urinationAction = new List<KeyCode>() { KeyCode.A, KeyCode.A, KeyCode.D, KeyCode.D, KeyCode.S, KeyCode.S, KeyCode.S };


    private List<KeyCode> currentInput = new List<KeyCode>();

    public float MaxtimeLimit = 1.5f;
    public float MintimeLimit = .5f;
    private float lastInputTime = 0f;

    //DIALOGUE SYSTEM

    public TextMeshProUGUI text;
    public TextMeshProUGUI inputText;

    void Start()
    {
        UpdateAvatarStatus();
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
            satisfaction -= 10;
            currentInput.Clear();
        }
       
    }
    void RegisterInput(KeyCode key)
    {
        if (lastInputTime > 0f && Time.time - lastInputTime < MintimeLimit)
        {
            text.text = "It's too fast. Forgive me, please.";
            excitement += 10;
            currentInput.Clear();
            return;
        }

        currentInput.Add(key);
        lastInputTime = Time.time; 

        if (CheckCombo(sitAction))
        {
            currentInput.Clear();

            if (currentIdle == PlayerIdle.Stand || currentIdle == PlayerIdle.LayDownMiddle || currentIdle == PlayerIdle.LayDownLeft || currentIdle == PlayerIdle.LayDownRight)
            {
                text.text = "So I sit up, it is not easy, and being able to do so excites me.";
                excitement += 10;
                currentIdle = PlayerIdle.Sit;
                UpdateAvatarStatus();
            }
            else if (currentIdle == PlayerIdle.PlayPhone)
            {
                text.text = "I stop playing my phone, sitting on my bed now.";
                excitement += 10;
                currentIdle = PlayerIdle.Sit;
                UpdateAvatarStatus();
            }
            else if (currentIdle == PlayerIdle.LayOnGround)
            {
                text.text = "I cannot sit on the bed before I stand up.";
            }
            else
            {
                text.text = "no.";
            }
        }
        else if (CheckCombo(standAction))
        {
            currentInput.Clear();

            if (currentIdle == PlayerIdle.Sit || currentIdle == PlayerIdle.LayOnGround)
            {
                text.text = "So I stand up, it is hard for me, and I'm happy I can still stand up.";
                excitement += 10;
                currentIdle = PlayerIdle.Stand;
                UpdateAvatarStatus();
            }
            else if (currentIdle == PlayerIdle.PlayPhone)
            {
                text.text = "I stop playing my phone, standing in my room now.";
                excitement += 10;
                currentIdle = PlayerIdle.Stand;
                UpdateAvatarStatus();
            }
            else if (currentIdle == PlayerIdle.LayDownMiddle || currentIdle == PlayerIdle.LayDownLeft || currentIdle == PlayerIdle.LayDownRight)
            {
                text.text = "I cannot stand up before I can sit up, I'm sorry.";
            }
            else {
                text.text = "no.";
            }
        }
        else if (CheckCombo(layDownAction))
        {
            currentInput.Clear();

            if (currentIdle == PlayerIdle.Sit)
            {
                text.text = "So I lay on my bed, feeling excited about that I may sleep.";
                excitement += 10;
                currentIdle = PlayerIdle.LayDownMiddle;
                UpdateAvatarStatus();
            }
            else if (currentIdle == PlayerIdle.PlayPhone)
            {
                text.text = "I stop playing my phone, laying on my bed.";
                excitement += 10;
                currentIdle = PlayerIdle.LayDownMiddle;
                UpdateAvatarStatus();
            }
            else if (currentIdle == PlayerIdle.Stand || currentIdle == PlayerIdle.LayOnGround)
            {
                text.text = "I cannot lay down before I sit on the bed first, I'm sorry.";
            }
            else
            {
                text.text = "no.";
            }
        }
        else if (CheckCombo(turnLeftAction))
        {
            currentInput.Clear();

            if (currentIdle == PlayerIdle.LayDownMiddle || currentIdle == PlayerIdle.LayDownRight)
            {
                text.text = "So I turn left on my bed. It becomes more comfortable, and I feel more exciting.";
                excitement += 10;
                satisfaction += 5;
                currentIdle = PlayerIdle.LayDownLeft;
                UpdateAvatarStatus();
            }
            else if (currentIdle == PlayerIdle.Stand || currentIdle == PlayerIdle.LayOnGround || currentIdle == PlayerIdle.Sit || currentIdle == PlayerIdle.PlayPhone)
            {
                text.text = "I should lay down on my bed first.";
            }
            else
            {
                text.text = "no.";
            }
        }
        else if (CheckCombo(turnRightAction))
        {
            currentInput.Clear();

            if (currentIdle == PlayerIdle.LayDownMiddle || currentIdle == PlayerIdle.LayDownLeft)
            {
                text.text = "So I turn right on my bed. It becomes more comfortable, and I feel more exciting.";
                excitement += 10;
                satisfaction += 5;
                currentIdle = PlayerIdle.LayDownRight;
                UpdateAvatarStatus();
            }
            else if (currentIdle == PlayerIdle.Stand || currentIdle == PlayerIdle.LayOnGround || currentIdle == PlayerIdle.Sit || currentIdle == PlayerIdle.PlayPhone)
            {
                text.text = "I should lay down on my bed first.";
            }
            else
            {
                text.text = "no.";
            }
        }
        else if (CheckCombo(turnMidAction))
        {
            currentInput.Clear();

            if (currentIdle == PlayerIdle.LayDownRight || currentIdle == PlayerIdle.LayDownLeft)
            {
                text.text = "So I turn my body straight on the bed.";
                excitement += 5;
                currentIdle = PlayerIdle.LayDownMiddle;
                UpdateAvatarStatus();
            }
            else if (currentIdle == PlayerIdle.Stand || currentIdle == PlayerIdle.LayOnGround || currentIdle == PlayerIdle.Sit || currentIdle == PlayerIdle.PlayPhone)
            {
                text.text = "I should lay down on my bed first.";
            }
            else
            {
                text.text = "no.";
            }
        }
        else if (CheckCombo(turnOnACAction))
        {
            currentInput.Clear();

            if (currentIdle == PlayerIdle.Stand)
            {
                text.text = "I turn on the AC, I'm glad that I can.";
                excitement += 10;
                satisfaction += 5;
                isAcTurnedOn = true;            
            }
            else if (currentIdle == PlayerIdle.PlayPhone)
            {
                text.text = "I should stop playing phone first.";
            }
            else
            {
                text.text = "I need stand up to reach the AC controller.";
            }
        }
        else if (CheckCombo(turnOffACAction))
        {
            currentInput.Clear();

            if (currentIdle == PlayerIdle.Stand)
            {
                text.text = "I turn off the AC, I will miss it.";
                excitement += 10;
                satisfaction -= 5;
                isAcTurnedOn = false;
            }
            else if (currentIdle == PlayerIdle.PlayPhone)
            {
                text.text = "I should stop playing phone first.";
            }
            else
            {
                text.text = "I need to stand up to reach the AC controller.";
            }
        }
        else if (CheckCombo(playPhoneAction))
        {
            currentInput.Clear();

            if (currentIdle == PlayerIdle.Sit)
            {
                text.text = "Yes, I love phone time.";
                excitement += 10;
                currentIdle = PlayerIdle.PlayPhone;
                UpdateAvatarStatus();
            }
            else if (currentIdle == PlayerIdle.PlayPhone)
            {
                text.text = "No more phone tonight, I swear.";
                currentIdle = PlayerIdle.Sit;
                UpdateAvatarStatus();
            }
            else
            {
                text.text = "I only want to play my phone if I am sitting on my bed.";
            }
        }
        else if (CheckCombo(drinkAction))
        {
            currentInput.Clear();

            if (currentIdle == PlayerIdle.Stand)
            {
                text.text = "Cheer? I guess.";
                excitement += 10;
                satisfaction += 10;
                thirsty = 0;
            }
            else if (currentIdle == PlayerIdle.PlayPhone)
            {
                text.text = "I should stop playing phone first.";
            }
            else
            {
                text.text = "I need to stand up to reach my cup.";
            }
        }
        else if (CheckCombo(urinationAction))
        {
            currentInput.Clear();

            if (currentIdle == PlayerIdle.Stand)
            {
                text.text = "Release time.";
                excitement += 15;
                satisfaction += 20;
                urination = 0;
            }
            else if (currentIdle == PlayerIdle.PlayPhone)
            {
                text.text = "I should stop playing phone first.";
            }
            else
            {
                text.text = "I need to stand up first.";
            }
        }
        else if (CheckCombo(layOnGroundAction))
        {
            currentInput.Clear();

            if (currentIdle == PlayerIdle.Stand)
            {
                text.text = "Ehh. At least it's cooler.";
                currentIdle = PlayerIdle.LayOnGround;
            }
            else if (currentIdle == PlayerIdle.PlayPhone)
            {
                text.text = "I should stop playing phone first.";
            }
            else
            {
                text.text = "I need to stand up first.";
            }
        }
        else if (!IsPrefixValid(currentInput))
        {
            text.text = "No, sorry, I can't.";
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
        if (isAcTurnedOn && currentIdle != PlayerIdle.LayOnGround)
        {
            normal_temperature_increase = -1f;
        }
        else if (isAcTurnedOn && currentIdle == PlayerIdle.LayOnGround)
        {
            normal_temperature_increase = -2f;
        }
        else if (!isAcTurnedOn && currentIdle != PlayerIdle.LayOnGround)
        {
            normal_temperature_increase = 1f;

        }
        else if (!isAcTurnedOn && currentIdle == PlayerIdle.LayOnGround)
        {
            normal_temperature_increase = -.75f;

        }

        if (currentIdle == PlayerIdle.PlayPhone)
        {
            normal_satisfaction_increase = 2f;
            normal_excitement_increase = 4f;
        }
        else if (currentIdle != PlayerIdle.PlayPhone && currentIdle != PlayerIdle.LayOnGround)
        {
            normal_satisfaction_increase = -.75f;
            normal_excitement_increase = -.75f;
        }
        else if (currentIdle == PlayerIdle.LayOnGround)
        {
            normal_satisfaction_increase = -1.5f;
        }

        if (thirsty < 50)
        {
            normal_urination_increase = 1f;
        }
        else
        {
            normal_urination_increase = .75f;

        }


    }

    void UpdateAvatarStatus()
    {
        standIdle.SetActive(false);
        sitIdle.SetActive(false);
        layDownMidIdle.SetActive(false);
        layDownLeftIdle.SetActive(false);
        layDownRightIdle.SetActive(false);
        layOngroundIdle.SetActive(false);
        playPhoneIdle.SetActive(false);

        switch (currentIdle)
        {
            case PlayerIdle.Stand:
                standIdle.SetActive(true);
                break;
            case PlayerIdle.Sit:
                sitIdle.SetActive(true);
                break;
            case PlayerIdle.LayDownMiddle:
                layDownMidIdle.SetActive(true);
                break;
            case PlayerIdle.LayDownLeft:
                layDownLeftIdle.SetActive(true);
                break;
            case PlayerIdle.LayDownRight:
                layDownRightIdle.SetActive(true);
                break;
            case PlayerIdle.LayOnGround:
                layOngroundIdle.SetActive(true);
                break;
            case PlayerIdle.PlayPhone:
                playPhoneIdle.SetActive(true);
                break;

        }
    }

}
