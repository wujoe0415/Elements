using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool isInteractable
    {
        get
        {
            return true;
        }
    }
    public void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }
    public virtual void Interact()
    {
        Debug.Log("Interacting with " + gameObject.name);
    }
}
