using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MicrophoneDetection : MonoBehaviour
{
    private float lowVolume = 0.07f;
    private float lowVolumeOffset = 0.04f;

    private float middleVolume = 0.15f;
    private float middleVolumeOffset = 0.05f;

    private float highVolume = 0.2f;

    private float minimalSoundJudgePeroid = 0.667f;

    private float currentVolumeRMS = 0f;

    public AudioPitchEstimator audioPitchEstimator;

    private AudioSource audioSource;
    private int sampleRate = 48000;
    private const int sampleSize = 1024;
    private float[] samples = new float[sampleSize];

    private float accumulatedVolume = 0f;
    private int sampleCount = 0;

    private float beatTime = 0;



    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioPitchEstimator = GetComponent<AudioPitchEstimator>();
        audioSource.loop = true;
        audioSource.clip = Microphone.Start(null, true, 5, sampleRate);
    }

    

    void Update()
    {
        RunPeroidVolumeDictation();
        //print(audioPitchEstimator.Estimate(audioSource));
    }


    void RunPeroidVolumeDictation()
    {
        beatTime += Time.deltaTime;
        if (beatTime < minimalSoundJudgePeroid)
        {
            float rms = GetCurrentVolume();
            currentVolumeRMS = rms;
            accumulatedVolume += rms;
            sampleCount++;
        }
        else if (beatTime >= minimalSoundJudgePeroid)
        {
            //print(GetAveragePeroidVolume());
            beatTime = 0f;
        }
       
    }
    float GetAveragePeroidVolume()
    {
        float averageVolume = accumulatedVolume / sampleCount;
        currentVolumeRMS = 0;
        accumulatedVolume = 0;
        sampleCount = 0;
        return averageVolume;

    }

    float GetCurrentVolume()
    {
        float[] data = new float[1024];
        int micPos = Microphone.GetPosition(null) - data.Length + 1;
        if (micPos < 0) return 0f;

        audioSource.clip.GetData(data, micPos);

        float sum = 0f;
        for (int i = 0; i < data.Length; i++)
            sum += data[i] * data[i];

        return Mathf.Sqrt(sum / data.Length);
    }

    
}
