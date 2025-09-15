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


    
}
