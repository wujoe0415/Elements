using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public FloorType type;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            WalkSoundManager.Instance.ChangeFloor(type);
            
    }
}
