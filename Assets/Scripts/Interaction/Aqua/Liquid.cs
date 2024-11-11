using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    public Transform Surface;
    public float MaxHeight = 1f;
    public void OnParticleCollision(GameObject other)
    {
        if (other.name == "FX_Rain_Collision_01")
        {
            if (Surface.position.y < MaxHeight)
                Surface.position += Vector3.up * 0.00035f;
        }
    }
}
