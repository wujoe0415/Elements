using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerDialogue : MonoBehaviour
{
    public List<string> Dialogues = new List<string>();

    public void OnTrigger()
    {
        DialogueSystem.Instance.SetDiagues(Dialogues);
        DialogueSystem.Instance.StartDialogue();
    }
}
