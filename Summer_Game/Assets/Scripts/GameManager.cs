using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private float temperature;
    private float thirsty;
    private float urination;
    private float excitement;
    private float satisfaction;
    private float sleep_progress = 0f;

    public string input;

    private bool isAcTurnedOn;

    public bool canSleep;

    public float sleepTime;

    public Slider temperature_slider;
    public Slider thirsty_slider;
    public Slider urination_slider;
    public Slider excitement_slider;
    public Slider satisfaction_slider;
    public Slider sleepProgress_slider;

    public Image tem_fill;
    public Image thirsty_fill;
    public Image urination_fill;
    public Image excitement_fill;
    public Image satisfaction_fill;



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
    bool isSleepingMessageRepeating = false;

    //public TextMeshProUGUI text;
    public QueuedTextTyper textSystem;
    public static bool canInput = true;
    public TextMeshProUGUI inputText;

    private bool hasReportedTem = false;
    private bool hasReportedThir = false;
    private bool hasReportedUri = false;
    private bool hasReportedExci = false;
    private bool hasReportedSati = false;

    void UpdateReportMessages()
    {
        if (!hasReportedTem && temperature <= 20f)
        {
            textSystem.AddMessage("It's too cold to sleep now.");
            hasReportedTem = true;

        }
        if (!hasReportedTem && temperature >= 80f)
        {
            textSystem.AddMessage("It's too hot to sleep now.");
            hasReportedTem = true;

        }
        if (temperature > 20f && temperature < 80f && hasReportedTem)
        {
            hasReportedTem = false;
        }

        if (!hasReportedThir && thirsty >= 115f)
        {
            textSystem.AddMessage("I'm too thirsty to sleep now.");
            hasReportedThir = true;


        }
        if (hasReportedThir && thirsty < 115f)
        {
            hasReportedThir = false;
        }

        if (!hasReportedUri && urination >= 115f)
        {
            textSystem.AddMessage("I want to go to the bathroom before sleep.");
            hasReportedUri = true;

        }
        if (hasReportedUri && urination < 115f) hasReportedUri = false;

        if (!hasReportedExci && excitement >= 90f)
        {
            textSystem.AddMessage("I'm high! I dont want to sleep now.");
            hasReportedExci = true;

        }
        if (hasReportedExci && excitement < 90f) hasReportedExci = false;

        if (!hasReportedSati && satisfaction <= 10f)
        {
            textSystem.AddMessage("I feel so frustrated, cant sleep now.");
            hasReportedSati = true;

        }
        if (hasReportedSati && satisfaction > 10f) hasReportedSati = false;
    }
    void Start()
    {
        UpdateAvatarStatus();
        RandomInitialValue();
        //SoundSystem.instance.PlayMusic("music");
        SoundSystem.instance.PlaySound("music_1");
    }

    // Update is called once per frame
    void Update()
    {
        SetSliderValue();
        UpdateInpute();
        UpdateDisplay();
        UpdateValuesPerSecond();
        UpdateNormalValueChange();
        UpdateSleepCheck();
        UpdateSliderColor();
        UpdateSleepMessage();
        UpdateConfineValue();
        UpdateReportMessages();
        EndTest();
    }

    void SetSliderValue()
    {
        temperature_slider.value = temperature;
        thirsty_slider.value = thirsty;
        urination_slider.value = urination;
        excitement_slider.value = excitement;
        satisfaction_slider.value = satisfaction;
        sleepProgress_slider.value = sleep_progress;
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
        if (GameManager.canInput)
        {
            if (Input.GetKeyDown(KeyCode.W)) RegisterInput(KeyCode.W);
            if (Input.GetKeyDown(KeyCode.A)) RegisterInput(KeyCode.A);
            if (Input.GetKeyDown(KeyCode.S)) RegisterInput(KeyCode.S);
            if (Input.GetKeyDown(KeyCode.D)) RegisterInput(KeyCode.D);
        }

        if (currentInput.Count > 0 && Time.time - lastInputTime > MaxtimeLimit)
        {
            textSystem.AddMessage("Oh no, I have forgotten what I should do now.");
            satisfaction -= 10;
            currentInput.Clear();
        }
       
    }
    void RegisterInput(KeyCode key)
    {

        if (lastInputTime > 0f && Time.time - lastInputTime < MintimeLimit)
        {
            textSystem.AddMessage("It's too fast. Forgive me, please.");
            excitement += 10;
            currentInput.Clear();
            return;
        }

        currentInput.Add(key);
        lastInputTime = Time.time;

        PlayActionSound();

        if (CheckCombo(sitAction))
        {
            currentInput.Clear();

            if (currentIdle == PlayerIdle.Stand || currentIdle == PlayerIdle.LayDownMiddle || currentIdle == PlayerIdle.LayDownLeft || currentIdle == PlayerIdle.LayDownRight)
            {
                textSystem.AddMessage("So I sit up, it is not easy, and being able to do so excites me.");
                excitement += 10;
                currentIdle = PlayerIdle.Sit;
                UpdateAvatarStatus();
            }
            else if (currentIdle == PlayerIdle.PlayPhone)
            {
                textSystem.AddMessage("I stop playing my phone, sitting on my bed now.");
                excitement += 10;
                currentIdle = PlayerIdle.Sit;
                UpdateAvatarStatus();
            }
            else if (currentIdle == PlayerIdle.LayOnGround)
            {
                textSystem.AddMessage("I cannot sit on the bed before I stand up.");
            }
            else
            {
                textSystem.AddMessage("no.");
            }
        }
        else if (CheckCombo(standAction))
        {
            currentInput.Clear();

            if (currentIdle == PlayerIdle.Sit || currentIdle == PlayerIdle.LayOnGround)
            {
                textSystem.AddMessage("So I stand up, it is hard for me, and I'm happy I can still stand up.");
                excitement += 10;
                currentIdle = PlayerIdle.Stand;
                UpdateAvatarStatus();
            }
            else if (currentIdle == PlayerIdle.PlayPhone)
            {
                textSystem.AddMessage("I stop playing my phone, standing in my room now.");
                excitement += 10;
                currentIdle = PlayerIdle.Stand;
                UpdateAvatarStatus();
            }
            else if (currentIdle == PlayerIdle.LayDownMiddle || currentIdle == PlayerIdle.LayDownLeft || currentIdle == PlayerIdle.LayDownRight)
            {
                textSystem.AddMessage("I cannot stand up before I can sit up, I'm sorry.");
            }
            else {
                textSystem.AddMessage("no.");
            }
        }
        else if (CheckCombo(layDownAction))
        {
            currentInput.Clear();

            if (currentIdle == PlayerIdle.Sit)
            {
                textSystem.AddMessage("So I lay on my bed, feeling excited about that I may sleep.");
                excitement += 10;
                currentIdle = PlayerIdle.LayDownMiddle;
                UpdateAvatarStatus();
            }
            else if (currentIdle == PlayerIdle.PlayPhone)
            {
                textSystem.AddMessage("I stop playing my phone, laying on my bed.");
                excitement += 10;
                currentIdle = PlayerIdle.LayDownMiddle;
                UpdateAvatarStatus();
            }
            else if (currentIdle == PlayerIdle.Stand || currentIdle == PlayerIdle.LayOnGround)
            {
                textSystem.AddMessage("I cannot lay down before I sit on the bed first, I'm sorry.");
            }
            else
            {
                textSystem.AddMessage("no.");
            }
        }
        else if (CheckCombo(turnLeftAction))
        {
            currentInput.Clear();

            if (currentIdle == PlayerIdle.LayDownMiddle || currentIdle == PlayerIdle.LayDownRight)
            {
                textSystem.AddMessage("So I turn left on my bed. It becomes more comfortable, and I feel more exciting.");
                excitement += 10;
                satisfaction += 5;
                currentIdle = PlayerIdle.LayDownLeft;
                UpdateAvatarStatus();
            }
            else if (currentIdle == PlayerIdle.Stand || currentIdle == PlayerIdle.LayOnGround || currentIdle == PlayerIdle.Sit || currentIdle == PlayerIdle.PlayPhone)
            {
                textSystem.AddMessage("I should lay down on my bed first.");
            }
            else
            {
                textSystem.AddMessage("no.");
            }
        }
        else if (CheckCombo(turnRightAction))
        {
            currentInput.Clear();

            if (currentIdle == PlayerIdle.LayDownMiddle || currentIdle == PlayerIdle.LayDownLeft)
            {
                textSystem.AddMessage("So I turn right on my bed. It becomes more comfortable, and I feel more exciting.");
                excitement += 10;
                satisfaction += 5;
                currentIdle = PlayerIdle.LayDownRight;
                UpdateAvatarStatus();
            }
            else if (currentIdle == PlayerIdle.Stand || currentIdle == PlayerIdle.LayOnGround || currentIdle == PlayerIdle.Sit || currentIdle == PlayerIdle.PlayPhone)
            {
                textSystem.AddMessage("I should lay down on my bed first.");
            }
            else
            {
                textSystem.AddMessage("no.");
            }
        }
        else if (CheckCombo(turnMidAction))
        {
            currentInput.Clear();

            if (currentIdle == PlayerIdle.LayDownRight || currentIdle == PlayerIdle.LayDownLeft)
            {
                textSystem.AddMessage("So I turn my body straight on the bed.");
                excitement += 5;
                currentIdle = PlayerIdle.LayDownMiddle;
                UpdateAvatarStatus();
            }
            else if (currentIdle == PlayerIdle.Stand || currentIdle == PlayerIdle.LayOnGround || currentIdle == PlayerIdle.Sit || currentIdle == PlayerIdle.PlayPhone)
            {
                textSystem.AddMessage("I should lay down on my bed first.");
            }
            else
            {
                textSystem.AddMessage("no.");
            }
        }
        else if (CheckCombo(turnOnACAction))
        {
            currentInput.Clear();

            if (currentIdle == PlayerIdle.Stand)
            {
                textSystem.AddMessage("I turn on the AC, I'm glad that I can.");
                excitement += 10;
                satisfaction += 5;
                isAcTurnedOn = true;            
            }
            else if (currentIdle == PlayerIdle.PlayPhone)
            {
                textSystem.AddMessage("I should stop playing phone first.");
            }
            else
            {
                textSystem.AddMessage("I need stand up to reach the AC controller.");
            }
        }
        else if (CheckCombo(turnOffACAction))
        {
            currentInput.Clear();

            if (currentIdle == PlayerIdle.Stand)
            {
                textSystem.AddMessage("I turn off the AC, I will miss it.");
                excitement += 10;
                satisfaction -= 5;
                isAcTurnedOn = false;
            }
            else if (currentIdle == PlayerIdle.PlayPhone)
            {
                textSystem.AddMessage("I should stop playing phone first.");
            }
            else
            {
                textSystem.AddMessage("I need to stand up to reach the AC controller.");
            }
        }
        else if (CheckCombo(playPhoneAction))
        {
            currentInput.Clear();

            if (currentIdle == PlayerIdle.Sit)
            {
                textSystem.AddMessage("Yes, I love phone time.");
                excitement += 10;
                currentIdle = PlayerIdle.PlayPhone;
                UpdateAvatarStatus();
            }
            else if (currentIdle == PlayerIdle.PlayPhone)
            {
                textSystem.AddMessage("No more phone tonight, I swear.");
                currentIdle = PlayerIdle.Sit;
                UpdateAvatarStatus();
            }
            else
            {
                textSystem.AddMessage("I only want to play my phone if I am sitting on my bed.");
            }
        }
        else if (CheckCombo(drinkAction))
        {
            currentInput.Clear();

            if (currentIdle == PlayerIdle.Stand)
            {
                textSystem.AddMessage("Cheer? I guess.");
                excitement += 10;
                satisfaction += 10;
                thirsty = 0;
            }
            else if (currentIdle == PlayerIdle.PlayPhone)
            {
                textSystem.AddMessage("I should stop playing phone first.");
            }
            else
            {
                textSystem.AddMessage("I need to stand up to reach my cup.");
            }
        }
        else if (CheckCombo(urinationAction))
        {
            currentInput.Clear();

            if (currentIdle == PlayerIdle.Stand)
            {
                textSystem.AddMessage("Release time.");
                excitement += 15;
                satisfaction += 20;
                urination = 0;
            }
            else if (currentIdle == PlayerIdle.PlayPhone)
            {
                textSystem.AddMessage("I should stop playing phone first.");
            }
            else
            {
                textSystem.AddMessage("I need to stand up first.");
            }
        }
        else if (CheckCombo(layOnGroundAction))
        {
            currentInput.Clear();

            if (currentIdle == PlayerIdle.Stand)
            {
                textSystem.AddMessage("Ehh. At least it's cooler.");
                currentIdle = PlayerIdle.LayOnGround;
            }
            else if (currentIdle == PlayerIdle.PlayPhone)
            {
                textSystem.AddMessage("I should stop playing phone first.");
            }
            else
            {
                textSystem.AddMessage("I need to stand up first.");
            }
        }
        else if (!IsPrefixValid(currentInput))
        {
            textSystem.AddMessage("No, sorry, I can't.");
            currentInput.Clear();
        }

    }

    void PlayActionSound()
    {
        int randomer = Random.Range(0, 4);
        if (randomer == 0) SoundSystem.instance.PlaySound("action_1");
        if (randomer == 1) SoundSystem.instance.PlaySound("action_2");
        if (randomer == 2) SoundSystem.instance.PlaySound("action_3");
        if (randomer == 3) SoundSystem.instance.PlaySound("action_4");
       


    }

    void RandomInitialValue()
    {
        temperature = Random.Range(40, 80);
        thirsty = Random.Range(20, 70);
        urination = Random.Range(20, 70);
        excitement = Random.Range(10, 70);
        satisfaction = Random.Range(15, 30);

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
            normal_satisfaction_increase = 4f;
            normal_excitement_increase = 2f;
        }
        else if (currentIdle != PlayerIdle.PlayPhone && currentIdle != PlayerIdle.LayOnGround)
        {
            normal_satisfaction_increase = -1f;
            normal_excitement_increase = -1.25f;
        }
        else if (currentIdle == PlayerIdle.LayOnGround)
        {
            normal_satisfaction_increase = -1.5f;
            normal_excitement_increase = -1.25f;
        }
        else
        {
            normal_satisfaction_increase = -.75f;
            normal_excitement_increase = -2f;
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

    void UpdateSleepCheck()
    {
        if (IfCanSleep())
        {
            if (currentIdle == PlayerIdle.LayDownLeft || currentIdle == PlayerIdle.LayDownMiddle || currentIdle == PlayerIdle.LayDownRight || currentIdle == PlayerIdle.LayOnGround)
            {
                sleep_progress += 1f * Time.deltaTime;
            }
        }
    }

    void UpdateSleepMessage()
    {
        if (IfCanSleep() && !isSleepingMessageRepeating)
        {
            StartCoroutine(RepeatSleepMessage());
        }
    }
    IEnumerator RepeatSleepMessage()
    {
        isSleepingMessageRepeating = true;
        while (IfCanSleep())
        {
            textSystem.AddMessage("zzzzz");
            yield return new WaitForSeconds(2.5f);
        }
        isSleepingMessageRepeating = false; 
    }

    bool IfCanSleep()
    {
        if (temperature >= 20 && temperature <= 80 && thirsty < 115 && urination < 115 && excitement < 90 && satisfaction > 10)
        {
            if (currentIdle == PlayerIdle.LayDownLeft || currentIdle == PlayerIdle.LayDownMiddle || currentIdle == PlayerIdle.LayDownRight || currentIdle == PlayerIdle.LayOnGround)
            {
                return true;

            }
            return false;
        }
        else
        {
            return false;
        }
    }

    void UpdateSliderColor()
    {
        if (temperature >= 80)
        {
            tem_fill.color = Color.red;
        }
        else if (temperature <= 20)
        {
            tem_fill.color = Color.blue;
        }
        else
        {
            tem_fill.color = Color.white;
        }

        if (thirsty >= 115)
        {
            thirsty_fill.color = Color.red;
        }
        else thirsty_fill.color = Color.white;

        if (urination >= 115) urination_fill.color = Color.red;
        else urination_fill.color = Color.white;

        if (excitement >= 90) excitement_fill.color = Color.red;
        else excitement_fill.color = Color.white;

        if (satisfaction <= 10) satisfaction_fill.color = Color.red;
        else satisfaction_fill.color = Color.white;


    }

    void UpdateConfineValue()
    {
        temperature = Mathf.Clamp(temperature, 0f, 120f);
        thirsty = Mathf.Clamp(thirsty, 0f, 120f);
        urination = Mathf.Clamp(urination, 0f, 120f);
        excitement = Mathf.Clamp(excitement, 0f, 120f);
        satisfaction = Mathf.Clamp(satisfaction, 0f, 120f);

    }

    void EndTest()
    {
        if (sleep_progress >= 45)
        {
            textSystem.AddMessage("I finally can have a good night, I guess.");
        }
    }
}
