using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burnable : MonoBehaviour
{
    public GameObject Fire;
    public GameObject Fracture;
    private GameObject _attachedFire;
    public float ExplosionTime = 1.5f;
    public void SetFire(GameObject attachFire)
    {
        _attachedFire = attachFire;
        Fire = Resources.Load<GameObject>("Prefabs/Skill/Fire/FX_Fire_Big_02");
        Fracture = Resources.Load<GameObject>("Prefabs/Skill/Fire/ExplosionFracture");
        Fire = Instantiate(Fire, transform.position, Quaternion.identity);
        StartCoroutine(BurnOut());
    }
    IEnumerator BurnOut()
    {
        yield return new WaitForSeconds(ExplosionTime);
        Destroy(gameObject);
        Destroy(Fire);
        Destroy(_attachedFire);
        Instantiate(Fracture, transform.position, Quaternion.identity);
    }
}
