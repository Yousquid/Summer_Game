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
        if (GameManager.canInput)
        {
            if (guideNote.active == false) { guideNote.SetActive(true); buttonSign.text = "X"; }
            else if (guideNote.active == true) { guideNote.SetActive(false);  buttonSign.text = "?"; }
        }
    }
}
