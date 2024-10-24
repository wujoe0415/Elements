using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    void Activate();
    void Deactivate();
}

public class Skill : MonoBehaviour, ISkill
{
    public virtual void Activate()
    {
        Debug.Log("Skill activated");
    }
    public virtual void Deactivate()
    {
        Debug.Log("Skill deactivated");
    }
}
