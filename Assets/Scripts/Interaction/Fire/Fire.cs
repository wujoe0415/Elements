using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject[] Fires = new GameObject[3];
    private ParticleSystem _fireBall;
    public List<ParticleCollisionEvent> ParticleCollisionEvent = new List<ParticleCollisionEvent>();
    public LayerMask WaterLayer;

    public void OnEnable()
    {
        _fireBall = GetComponent<ParticleSystem>();
        StartCoroutine(FireRaying());
    }
    public void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = _fireBall.GetCollisionEvents(other, ParticleCollisionEvent);
        int i = 0;
        while (i < numCollisionEvents)
        {
            if (other.layer == 4)
            {
                Destroy(gameObject);
                return;
            }
            Vector3 pos = ParticleCollisionEvent[i].intersection;
            GameObject fire = Instantiate(Fires[Random.Range(0, Fires.Length)], pos, Quaternion.identity);
            i++;
            if(other.GetComponent<Burnable>() != null)
                other.GetComponent<Burnable>().SetFire(fire);
        }
    }

    IEnumerator FireRaying()
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
