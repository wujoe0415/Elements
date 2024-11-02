using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindField : MonoBehaviour
{
    [Range(10f, 50f)]
    public float WindForce = 35f;
    public float Duration = 5f; 
    private void OnEnable()
    {
        StartCoroutine(Blowing());
    }
    IEnumerator Blowing()
    {
        yield return new WaitForSeconds(Duration);
        Destroy(gameObject);
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Rigidbody>() == null)
            return;
        
        other.GetComponent<Rigidbody>().AddForce(Vector3.up * WindForce);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() == null)
            return;
        Vector3 windDir = new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 3f), Random.Range(-1f, 1f));
        other.GetComponent<Rigidbody>().AddForce(windDir * WindForce);
    }
}
