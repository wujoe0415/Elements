using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : Skill
{
    public GameObject Fire;
    public Transform StartPosition;

    public override void Activate()
    {
        Instantiate(Fire, StartPosition.position, transform.rotation);
    }
    public override void Deactivate() {
        Debug.Log("Skill deactivated");
    }
}
