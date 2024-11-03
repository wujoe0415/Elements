using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Burn : MonoBehaviour
{
    private float _burnTime = 3f;

    private void Awake()
    {
        _burnTime = Random.Range(3f, 5f);
        StartCoroutine(Burning());
    }
    public void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.name);
        if (other.name == "FX_Rain_Collision_01")
        {
            transform.localScale -= Vector3.one * 0.1f;
            if (transform.localScale.x < 0.2f)
                Destroy(gameObject);
        }
    }
    private IEnumerator Burning()
    {
       yield return new WaitForSeconds(_burnTime);
       Destroy(gameObject);
    }
}
