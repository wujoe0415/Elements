using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueObject : Interactable
{
    public List<string> Dialogues = new List<string>();

    public override void Interact()
    {
        StartDialogue();
    }
    public void StartDialogue()
    {
        //List<string> dialogs = new List<string>(Dialogues);
        DialogueSystem.Instance.SetDiagues(Dialogues);
        DialogueSystem.Instance.StartDialogue();
    }
}
