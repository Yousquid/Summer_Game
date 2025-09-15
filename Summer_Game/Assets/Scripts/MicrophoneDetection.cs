using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class MicrophoneDetection : MonoBehaviour
{
    private float lowVolume = 0.08f;
    private float lowVolumeOffset = 0.05f;

    private float middleVolume = 0.15f;
    private float middleVolumeOffset = 0.05f;

    private float highVolume = 0.17f;

    private float lowPitch = 90f;
    private float lowPitchOffset = 40f;

    private float middlePitch = 180f;
    private float middlePitchOffset = 50f;

    private float highPitch = 230f;

    private float minimalSoundJudgePeroid = .83f;

    public AudioPitchEstimator audioPitchEstimator;

    private AudioSource audioSource;
    private int sampleRate = 48000;
    private const int sampleSize = 1024;
    private float[] samples = new float[sampleSize];

    private List<float> collectedVolumes = new List<float>();
    private List<float> collectedPitches = new List<float>();

    private float beatTime = 0;
    private int beatCount = 0;

    private bool hasStarted = false;
    public enum VolumeLevel { Low, Middle, High, None, Any }
    public enum PitchLevel { Low, Middle, High, None, Any }

    public AudioVisualizer audioVisualizer;

    [System.Serializable]
    public class BeatRequirement
    {
        public VolumeLevel requiredVolume;
        public PitchLevel requiredPitch;
    }
    public BeatRequirement[] score;
    private int currentBeatIndex = 0;

    public int indicator_Pos = 1;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioPitchEstimator = GetComponent<AudioPitchEstimator>();
        audioVisualizer = GetComponent<AudioVisualizer>();
        audioSource.loop = true;
        audioSource.clip = Microphone.Start(null, true, 5, sampleRate);
       

    }

    

    void Update()
    {
        if (hasStarted)
        {
            RunPeroidVolumeDictation();
            RunPeroidPitchDetection();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            hasStarted = true;
        }
        
        //print(audioPitchEstimator.Estimate(audioSource));
    }


    void RunPeroidVolumeDictation()
    {
        beatTime += Time.deltaTime;
        if (beatTime < minimalSoundJudgePeroid)
        {
            float rms = GetCurrentVolume();
           // currentVolumeRMS = rms;
            collectedVolumes.Add(rms);
            
        }
        else if (beatTime >= minimalSoundJudgePeroid)
        {
            
        }
       
    }

    void RunPeroidPitchDetection()
    {
        if (beatTime < minimalSoundJudgePeroid)
        {
            float newHZ = audioPitchEstimator.Estimate(audioSource);
            collectedPitches.Add(newHZ);

        }
        else if (beatTime >= minimalSoundJudgePeroid)
        {
            EndofBeat();
        }
    }

    void EndofBeat()
    {
        beatCount++;
        indicator_Pos++;
        float avgVolume = GetAveragePeroidVolume();
        float avgPitch = GetAveragePeroidPitch();

        List<VolumeLevel> playerVolumes = JudgeVolume(avgVolume);
        List<PitchLevel> playerPitches = JudgePitch(avgPitch);

        if (currentBeatIndex < score.Length)
        {
            var expected = score[currentBeatIndex];

            if (playerVolumes.Contains(expected.requiredVolume) && playerPitches.Contains(expected.requiredPitch) )
            {
                audioVisualizer.indicateWord.text = "Good";
            }
            else
            {
                audioVisualizer.indicateWord.text = "Wrong";
            }

            currentBeatIndex++;
        }
        if (beatCount == 1)
        {
            beatCount = 0;
            SoundSystem.instance.PlaySound("beat");
        }
        if (indicator_Pos == 5)
        {
            indicator_Pos = 1;
        }

        beatTime = 0f;
    }
    public float GetAveragePeroidPitch()
    {
        collectedPitches.Sort();
        var trimmed = collectedPitches.GetRange(2, collectedPitches.Count - 4);
        float sum = 0f;
        foreach (var v in trimmed) sum += v;
        float averagePitch = sum / trimmed.Count;
        collectedPitches.Clear();
        return averagePitch;
    }
    public float GetAveragePeroidVolume()
    {
        collectedVolumes.Sort();
        var trimmed = collectedVolumes.GetRange(2, collectedVolumes.Count - 4);
        float sum = 0f;
        foreach (var v in trimmed) sum += v;
        float averageVolume = sum / trimmed.Count;
        collectedVolumes.Clear();
        return averageVolume;
     

    }

    public float GetCurrentVolume()
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

    bool InRange(float value, float center, float offset)
    {
        return value >= (center - offset) && value <= (center + offset);
    }

    List<VolumeLevel> JudgeVolume(float volume)
    {
        List<VolumeLevel> results = new List<VolumeLevel>();

        if (volume <= 0.001f)  
        {
            results.Add(VolumeLevel.None);
            return results;
        }
        if (InRange(volume, lowVolume, lowVolumeOffset)) results.Add(VolumeLevel.Low); results.Add(VolumeLevel.Any); 
        if (InRange(volume, middleVolume, middleVolumeOffset)) results.Add(VolumeLevel.Middle); results.Add(VolumeLevel.Any);
        if (volume >= highVolume) results.Add(VolumeLevel.High); results.Add(VolumeLevel.Any);


        return results;
    }

    List<PitchLevel> JudgePitch(float pitch)
    {
        List<PitchLevel> results = new List<PitchLevel>();

        if (float.IsNaN(pitch) || pitch <= 20f)
        {
            results.Add(PitchLevel.None);
            return results;
        }

        if (InRange(pitch, lowPitch, lowPitchOffset)) results.Add(PitchLevel.Low); results.Add(PitchLevel.Any);
        if (InRange(pitch, middlePitch, middlePitchOffset)) results.Add(PitchLevel.Middle); results.Add(PitchLevel.Any);
        if (pitch >= highPitch) results.Add(PitchLevel.High); results.Add(PitchLevel.Any);

        return results;
    }
}
