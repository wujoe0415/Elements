using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aqua : Skill
{
    public Transform Anchor;
    public GameObject Rain;
    public override void Activate()
    {
       Instantiate(Rain, HintManager.Instance.HintCircle.position + Vector3.up * 3f, Quaternion.identity);
    }
    public override void Deactivate()
    {
        Debug.Log("Skill deactivated");
        
    }
}
