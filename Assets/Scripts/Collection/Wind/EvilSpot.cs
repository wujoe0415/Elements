using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EvilSpot : Collectable
{
    public int LimitNum = 4;
    public GameObject MagicRing;
    public override void Interact()
    {
        base.Interact();
        for (int i = 0; i < LimitNum; i++)
        {
            if (CollectionBag.Instance.Collections.Count >= LimitNum)
                MagicRing.SetActive(true);
        }
    }
}
