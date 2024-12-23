using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerDialogue : MonoBehaviour
{
    public List<string> Dialogues = new List<string>();

    private void Update()
    {
        if (Input.GetKey(KeyCode.G))
        {
            OnTrigger();
        }
    }
    public void OnTrigger()
    {
        Debug.Log("trigger");
        DialogueSystem.Instance.SetDiagues(Dialogues);
        DialogueSystem.Instance.StartDialogue();
    }
}
