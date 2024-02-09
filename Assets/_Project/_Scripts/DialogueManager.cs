using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private DialogueBox dialogueBox;
    private int dialogueFrame = 0;
    public bool autoContinue = false;

    private DialogueScriptableObject currentDialogue;
    protected override void Awake()
    {
        base.Awake();
        if (dialogueBox == null) {
            Debug.LogError("MISSING DIALOGUE BOX");
        }

        dialogueBox.gameObject.SetActive(false);
    }

    public void ActivateDialogue(DialogueScriptableObject dialogue) {
        Debug.Log("Activating Dialogue");
        PauseManager.Instance.Pause();
        currentDialogue = dialogue;
        dialogueBox.gameObject.SetActive(true);
        dialogueFrame = 0;
        
        ShowNextFrame();
    }


    public void ShowNextFrame() {
        Debug.Log("Showing next frame");
        if (currentDialogue == null) {
            DeactiveDialogue();
            return;
        }

        if (dialogueBox.inProgress == true) { 
            dialogueBox.ForceFinishDialogue();
            return;
        }

        if (dialogueFrame < currentDialogue.dialogueFrames.Length)
        {
            dialogueBox.SetSpeaker(currentDialogue.dialogueSpeakers[currentDialogue.dialogueFrames[dialogueFrame].speaker]);    //Man made horrors
            dialogueBox.DisplayDialogue(currentDialogue.dialogueFrames[dialogueFrame].text);

            dialogueFrame++;
        }
        else {
            DeactiveDialogue();
        }
    }

    private void DeactiveDialogue() {
        dialogueBox.gameObject.SetActive(false);
        PauseManager.Instance.UnPause();
        currentDialogue = null;
    }


}
