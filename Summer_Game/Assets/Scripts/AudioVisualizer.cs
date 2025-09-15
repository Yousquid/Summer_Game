using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
public class AudioVisualizer : MonoBehaviour
{
    public Image volumeFill;
    public Image pitchFill;
    public Slider volumeSlider;
    public Slider pitchSlider;

    public AudioPitchEstimator pitchEstimator;
    public MicrophoneDetection microphoneDetection;

    private float maxVolume = 0.20f;
    private float maxPitch = 300f;

    public TextMeshProUGUI indicateWord;

    public GameObject indicator;
    public Transform indicator_1;
    public Transform indicator_2;
    public Transform indicator_3;
    public Transform indicator_4;

    public SpriteRenderer script;
    public List<Sprite> scripts;
    public string startWords_one;
    public string startWords_two;
    public string startWords_three;

    public string illustrationWords;

    public List<Sprite> tutorScripts;
    public SpriteRenderer mustachImage;

    public GameObject backgroundOpera;

    // Start is called once before the firt execution of Update after the MonoBehaviour is created
    void Start()
    {
        pitchEstimator = GetComponent<AudioPitchEstimator>();
        microphoneDetection = GetComponent<MicrophoneDetection>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSliderValue();
        UpdateInidatorPos();
        UpdateScriptImage();
        UpdateTutorialScript();

        if (Input.GetKeyDown(KeyCode.K))
        {
            ScreenShake.instance.Shake(.1f, .5f);
        }
    }

    void UpdateSliderValue()
    {
        float currentVolume = microphoneDetection.GetCurrentVolume();

        float currentPitch = pitchEstimator.GetCurrentPitch();

        if (!float.IsNaN(currentPitch))
        {
            pitchSlider.value = currentPitch;
        }
        else
        {
            pitchSlider.value = 0f;
        }

        float normalizedVolume = Mathf.Clamp01(currentVolume / maxVolume);

        volumeSlider.value = normalizedVolume;

        if (currentVolume < 0.1f)
        {
            volumeFill.color = Color.green;
        }
        else if (currentVolume >= 0.1f && currentVolume < 0.17f)
        {
            volumeFill.color = Color.yellow;
        }
        else if (currentVolume >= 0.17f)
        {
            volumeFill.color = Color.red;
        }

        if (currentPitch < 130f)
        {
            pitchFill.color = Color.green;
        }
        else if (currentPitch >= 130f && currentPitch < 230f)
        {
            pitchFill.color = Color.yellow;
        }
        else if (currentPitch >= 230f)
        {
            pitchFill.color = Color.red;
        }
    }

    void UpdateInidatorPos()
    {
        if (microphoneDetection.indicator_Pos == 1)
        {
            indicator.transform.position = indicator_1.position;
        }
        if (microphoneDetection.indicator_Pos == 2)
        {
            indicator.transform.position = indicator_2.position;

        }
        if (microphoneDetection.indicator_Pos == 3)
        {
            indicator.transform.position = indicator_3.position;

        }
        if (microphoneDetection.indicator_Pos == 4)
        {
            indicator.transform.position = indicator_4.position;

        }
    }

    void UpdateScriptImage()
    {
        if (microphoneDetection.hasStarted)
        {
            int index = microphoneDetection.currentBeatIndex / 4;
            index = Mathf.Clamp(index, 0, scripts.Count - 1);
            script.sprite = scripts[index];
        }
        
    }

    void UpdateTutorialScript()
    {
        if (microphoneDetection.tutorialSteps == 2)
        {
            indicateWord.text = startWords_one;
            script.sprite = tutorScripts[0];
        }
        if (microphoneDetection.tutorialSteps == 3)
        {
            indicateWord.text = startWords_two;
            script.sprite = tutorScripts[1];
        }
        if (microphoneDetection.tutorialSteps == 4)
        {
            indicateWord.text = startWords_three;
            script.sprite = tutorScripts[2];
        }
        if (microphoneDetection.tutorialSteps == 5)
        {
            indicateWord.text = illustrationWords;
            script.enabled = false;
            mustachImage.enabled = true;
            indicator.SetActive(false);
        }
        if (microphoneDetection.tutorialSteps == 6)
        {
            indicateWord.text = "Prepare and check the script first, sing in a high pitch to go!";
            script.enabled = true;
            mustachImage.enabled = false;
            indicator.SetActive(true);
            backgroundOpera.SetActive(true);
        }
        if (microphoneDetection.tutorialSteps == 7)
        {
            
        }
    }
    
    
}
