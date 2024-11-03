using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : Skill
{
    public GameObject Fire;
    public Transform StartPosition;
    public Transform TargetPosition;
    public override void Activate()
    {
        Quaternion rot = Quaternion.LookRotation(TargetPosition.position - StartPosition.position);
        Instantiate(Fire, StartPosition.position, rot);
    }
    public override void Deactivate() {
        Debug.Log("Skill deactivated");
    }
}
