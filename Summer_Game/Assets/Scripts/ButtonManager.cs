using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ButtonManager : MonoBehaviour
{
    public GameObject guideNote;
    public TextMeshProUGUI buttonSign;
    void Start()
    {
        buttonSign.text = "?";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInformationButtonClicked()
    {
        if (!guideNote.activeSelf && GameManager.canInput)
        {
            guideNote.SetActive(true);
            buttonSign.text = "X";
            GameManager.canInput = false; 
        }
        else if (guideNote.activeSelf)
        {
            guideNote.SetActive(false);
            buttonSign.text = "?";
            GameManager.canInput = true; 
        }
    }
}
