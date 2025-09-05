using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class QueuedTextTyper : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public Image talkingBar;
    public float displayTime = 2f;      
    public float typeSpeed = 0.05f;    

    private Queue<string> messageQueue = new Queue<string>();
    private bool isShowing = false;

    private void Start()
    {
        textUI.enabled = false;
        talkingBar.enabled = false;
        GameManager.canInput = true;
    }
    public void AddMessage(string message)
    {
        messageQueue.Enqueue(message);

        if (!isShowing)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    public void AddContinuingMessage(string message)
    {
        StopAllCoroutines();
        messageQueue.Clear();
        messageQueue.Enqueue(message);

        if (!isShowing)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    private IEnumerator ProcessQueue()
    {
        isShowing = true;
        talkingBar.enabled = true;
        textUI.enabled = true;

        while (messageQueue.Count > 0)
        {
            string message = messageQueue.Dequeue();

            GameManager.canInput = false;

            textUI.enabled = true;
            textUI.text = "";

            foreach (char c in message)
            {
                textUI.text += c;
                yield return new WaitForSeconds(typeSpeed);
            }

            yield return new WaitForSeconds(displayTime);

            textUI.enabled = false;
            talkingBar.enabled = false;
            GameManager.canInput = true;
        }

        isShowing = false;
    }
}
