using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    public float letterSpeed = .01f;
    public float startBufferTime = .1f;
    public float endBufferTime = .5f;
    public int maxStringSize = 69;

    [SerializeField] private GameObject textBoxObject;
    private TextMeshProUGUI textBoxText;
    [SerializeField] private GameObject speakerNameObject;
    private TextMeshProUGUI speakerNameText;
    [SerializeField] private GameObject speakerPicObject;
    private Image speakerPicSprite;

    public bool inProgress { get; private set; }
    private string activeText;
    private Coroutine activeCoroutine;

    private void Awake()
    {
        textBoxText = textBoxObject.GetComponent<TextMeshProUGUI>();
        speakerNameText = speakerNameObject.GetComponent<TextMeshProUGUI>();
        speakerPicSprite = speakerPicObject.GetComponent<Image>();
    }

    public void SetSpeaker(DialogueSpeaker speaker) { 
        speakerNameText.text = speaker.name;
        speakerPicSprite.sprite = speaker.sprite;

        if (speaker.onRightSide)
        {
            speakerPicObject.transform.SetAsLastSibling();
        }
        else {
            speakerPicObject.transform.SetAsFirstSibling();
        }
    }

    public void DisplayDialogue(string text) { 
        activeText = text;

        if (activeText.Length > maxStringSize) { 
            activeText = activeText.Substring(0, maxStringSize);
        }

        textBoxText.text = "";
        inProgress = true;
        activeCoroutine = StartCoroutine(ShowText());
    }

    public void ForceFinishDialogue() {
        if (activeCoroutine != null) { 
            StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }

        textBoxText.text = activeText;
        inProgress = false;
    }

    IEnumerator ShowText()
    {
        yield return new WaitForSecondsRealtime(startBufferTime);


        foreach (char c in activeText.ToCharArray())
        {
            textBoxText.text += c;
            if (!Input.GetMouseButton(1))
            {
                yield return new WaitForSecondsRealtime(letterSpeed);
            }
            else
            {
                Debug.Log("Speed up");
                yield return new WaitForSecondsRealtime(.005f);
            }
        }
        yield return new WaitForSecondsRealtime(endBufferTime);
        activeCoroutine = null;
        inProgress = false;

        if (DialogueManager.Instance.autoContinue) { 
            DialogueManager.Instance.ShowNextFrame();
        }
    }
}
