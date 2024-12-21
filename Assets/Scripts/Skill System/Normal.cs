using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal : Skill
{
    public override void Activate()
    {
        Debug.Log("Skill activated");
        if (Target != null && Target.GetComponent<Interactable>() != null)
        {
            Target.GetComponent<Interactable>().Interact();
        }
    }
}
