using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    public UnityEvent OnEnter;

    private void OnTriggerEnter(Collider other)
    {
        if ( other.gameObject.tag == "Player")
        {
            OnEnter.Invoke();
        }
    }
}
