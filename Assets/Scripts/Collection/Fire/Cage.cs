using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cage : DialogueObject
{
    public UnityEvent OnReleaseCage;

    public override void Interact()
    {
        if(CollectionBag.Instance.ContainsCollection("Key"))
        {
            OnReleaseCage.Invoke();
        }
        else{
            // show dialogue
            base.Interact();
        }
    }
}
