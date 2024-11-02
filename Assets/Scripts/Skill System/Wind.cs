using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : Skill
{
    public GameObject WindField;
    public Transform TargetPosition;

    public override void Activate()
    {
        Instantiate(WindField, TargetPosition.position, Quaternion.identity);
    }

    public override void Deactivate()
    {
        Destroy(WindField);
    }
}
