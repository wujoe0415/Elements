using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aqua : Skill
{
    public Transform Anchor;
    public GameObject Rain;
    public override void Activate()
    {
       Instantiate(Rain, Anchor.position, Quaternion.identity);
    }
    public override void Deactivate()
    {
        Debug.Log("Skill deactivated");
        
    }
}
