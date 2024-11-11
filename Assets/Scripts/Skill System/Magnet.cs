using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : Skill
{
    public Polar SouthPolar;

    public void OnEnable()
    {
        SouthPolar.gameObject.SetActive(false);
    }
    public override void Activate()
    {
        base.Activate();
        Polar t = Target.gameObject.AddComponent<Polar>();
        t.SetPolarType(PolarType.North);
        SouthPolar.gameObject.SetActive(true);
    }
    public override void Deactivate()
    {
        base.Deactivate();
        SouthPolar.gameObject.SetActive(false);
    }
}
