using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject[] Fires = new GameObject[3];
    private ParticleSystem _fireBall;
    public List<ParticleCollisionEvent> ParticleCollisionEvent = new List<ParticleCollisionEvent>();
    public LayerMask WaterLayer;
    public AudioSource PutoutFire;

    private IEnumerator _coroutine;

    public void OnEnable()
    {
        _fireBall = GetComponent<ParticleSystem>();
        StartCoroutine(FireRaying());
    }
    public void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = _fireBall.GetCollisionEvents(other, ParticleCollisionEvent);
        int i = 0;
        while (i < numCollisionEvents && _coroutine == null)
        {
            if (other.layer == 4) // Encounter water
            {
                _coroutine = KillFire();
                StartCoroutine(_coroutine);
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
    IEnumerator KillFire()
    {
        PutoutFire.Play();
        for (float f = 2.3f; f >= 0f; f -= Time.deltaTime)
        {
            _fireBall.transform.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, Mathf.Clamp(f, 0f, 0.5f) / 0.5f);
            yield return null;
        }
        Destroy(gameObject);
    }
}
