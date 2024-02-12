using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue-Sequence", menuName = "Dialogue/DialogueScriptableObject")]
public class DialogueScriptableObject : ScriptableObject
{
    public DialogueSpeaker[] dialogueSpeakers;
    public DialogueFrame[] dialogueFrames;

}

[Serializable]
public class DialogueFrame {
    public int speaker;
    [TextArea (2,5)]public string text;
}

[Serializable]
public class DialogueSpeaker { 
    public string name;
    public Sprite sprite;
    public bool onRightSide;
}
