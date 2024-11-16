using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : Skill
{
    public GameObject Fire;
    public Transform StartPosition;
    public Transform CameraCenter;
    public override void Activate()
    {
        Vector3 endPosition = CameraCenter.position + CameraCenter.forward * 2.5f;
        if (Target != null)
           endPosition = Target.transform.position;

        Quaternion rot = Quaternion.LookRotation(endPosition - StartPosition.position);
        Instantiate(Fire, StartPosition.position, rot);
    }
    public override void Deactivate() {
        Debug.Log("Skill deactivated");
    }
}
