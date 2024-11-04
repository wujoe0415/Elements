using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PolarType { 
    Null,
    North,
    South,
}

public class Polar : MonoBehaviour
{
    public PolarType PolarType = PolarType.Null;
    private Rigidbody rb;
    [Range(10f, 50f)]
    public float MagnetForce = 35f;
    private Rigidbody _rb;
    public List<Polar> _affectedObject = new List<Polar>();
    private void OnEnable()
    {
        if (this.transform.parent.GetComponent<Rigidbody>() == null)
            Destroy(gameObject);
        _rb = this.transform.parent.GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        foreach (Polar polar in _affectedObject)
        {
            if (polar.PolarType == PolarType.Null)
                return;

            Vector3 direction = (polar.PolarType == PolarType ? 1f : -1f) * (polar.transform.position - transform.position).normalized;
            polar.SetMagneticForce(direction * MagnetForce);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.GetComponent<Polar>() == null)
            return;
        else
        _affectedObject.Add(other.GetComponent<Polar>());
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Polar>() == null)
            return;
        else
            _affectedObject.Remove(other.GetComponent<Polar>());
    }
    public void SetMagneticForce(Vector3 force)
    {
        _rb.AddForce(force);
    }
    public void SetPolarType(PolarType polarType)
    {
        PolarType = polarType;
    }
    public void ClearPolar()
    {
       PolarType = PolarType.Null;
    }
}
