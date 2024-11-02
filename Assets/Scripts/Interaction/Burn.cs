using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Burn : MonoBehaviour
{
    public float BurnTime = 3f;

    private void Awake()
    {
        BurnTime = Random.Range(2f, 5f);
        StartCoroutine(Burning());
    }
    private IEnumerator Burning()
    {
       yield return new WaitForSeconds(BurnTime);
       Destroy(gameObject);
    }
}
