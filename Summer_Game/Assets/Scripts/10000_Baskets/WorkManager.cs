using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class WorkManager : MonoBehaviour
{
    public List<GameObject> basketList;
    public GameObject light;
    public static bool isLightOn = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LightControl();
    }

    void LightControl()
    {
        if (Input.GetKeyDown(KeyCode.S) && !isLightOn)
        { 
            light.SetActive(true);
            isLightOn = true;
        }
        if (Input.GetKeyDown(KeyCode.W) && isLightOn)
        {
            light.SetActive(false);
            isLightOn = false;
        }

    }
}
