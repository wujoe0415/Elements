using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cauldron : DialogueObject
{
    public UnityEvent EndLevel;
    public override void Interact()
    {
        if(CollectionBag.Instance.ContainsCollection("purple"))
        {
            EndLevel.Invoke();
        }
        else{
            // show dialogue
            base.Interact();
        }
    }
}
