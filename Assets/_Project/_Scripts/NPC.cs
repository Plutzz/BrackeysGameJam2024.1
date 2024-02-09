using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private DialogueScriptableObject npcDialogueOne;
    [SerializeField] private DialogueScriptableObject npcDialogueTwo;
    public bool talkedToOnce = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.E)) {
            ShowDialogue();
        }
    }

    private void ShowDialogue() {
        Debug.Log("Player Interacted");
        if (talkedToOnce == false)
        {
            DialogueManager.Instance.ActivateDialogue(npcDialogueOne);
            talkedToOnce = true;
        }
        else {
            DialogueManager.Instance.ActivateDialogue(npcDialogueTwo);
            
        }
    }
}
