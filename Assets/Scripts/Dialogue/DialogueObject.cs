using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueObject : Interactable
{
    public List<string> Dialogues = new List<string>();
    public UnityEvent OnDialogue;

    public override void Interact()
    {
        StartDialogue();
    }
    public void StartDialogue()
    {
        //List<string> dialogs = new List<string>(Dialogues);
        DialogueSystem.Instance.SetDiagues(Dialogues);
        DialogueSystem.Instance.StartDialogue();
        OnDialogue.Invoke();
    }
}
