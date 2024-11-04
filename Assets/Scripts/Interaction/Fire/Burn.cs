using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Burn : MonoBehaviour
{
    public float BurnTime = 3f;

    private void Awake()
    {
        BurnTime += Random.Range(0.5f, 1f);
        StartCoroutine(Burning());
    }
    public void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.name);
        if (other.name == "FX_Rain_Collision_01")
        {
            transform.localScale -= Vector3.one * 0.1f;
            if (transform.localScale.x < 0.2f)
            {
                StopAllCoroutines();
                Destroy(gameObject);
            }
        }
    }
    private IEnumerator Burning()
    {
        // burn sound
        float burnSoundTime = 0.75f;
        yield return new WaitForSeconds(BurnTime - burnSoundTime);
        if(GetComponent<AudioSource>() != null)
        {
            AudioSource audio = GetComponent<AudioSource>();
            for(float f = 0f; f< burnSoundTime; f += Time.deltaTime)
            {
                audio.volume = Mathf.Lerp(1f, 0f, f / burnSoundTime);
                yield return null;
            }
        }
        else
            yield return new WaitForSeconds(burnSoundTime);
        Destroy(gameObject);
    }
}
