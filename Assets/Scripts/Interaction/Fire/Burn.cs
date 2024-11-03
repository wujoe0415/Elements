using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Burn : MonoBehaviour
{
    public float BurnTime = 3f;

    private void Awake()
    {
        BurnTime = Random.Range(20f, 50f);
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
       yield return new WaitForSeconds(BurnTime);
        Debug.Log("Burned");
       Destroy(gameObject);
    }
}
