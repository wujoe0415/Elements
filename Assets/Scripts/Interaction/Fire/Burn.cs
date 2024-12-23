using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : MonoBehaviour
{
    public float BurnTime = 3f;
    private ParticleSystem fireParticle;
    private float originalStartSize;
    private bool isShrinking = false;

    private void Awake()
    {
        BurnTime += Random.Range(0.5f, 1f);
        fireParticle = GetComponent<ParticleSystem>();
        if (fireParticle != null)
        {
            var main = fireParticle.main;
            originalStartSize = main.startSize.constant;
        }
        StartCoroutine(Burning());
    }

    public void OnParticleCollision(GameObject other)
    {
        if (other.name == "FX_Rain_Collision_01" && !isShrinking && fireParticle != null)
        {
            var main = fireParticle.main;
            float currentSize = main.startSize.constant;
            float newSize = currentSize - 0.1f;

            // �p�G�ɤl�ؤo�p���l�ؤo��20%
            if (newSize < originalStartSize * 0.2f)
            {
                isShrinking = true;
                main.loop = false; // ����`��
                StartCoroutine(DestroyAfterParticles());
            }
            else
            {
                main.startSize = newSize;
            }
        }
    }

    private IEnumerator DestroyAfterParticles()
    {
        // ���ݲɤl�t�Χ�������
        yield return new WaitForSeconds(fireParticle.main.duration);
        StopAllCoroutines();
        Destroy(gameObject);
    }

    private IEnumerator Burning()
    {
        // burn sound
        float burnSoundTime = 0.75f;
        yield return new WaitForSeconds(BurnTime - burnSoundTime);

        if (GetComponent<AudioSource>() != null)
        {
            AudioSource audio = GetComponent<AudioSource>();
            for (float f = 0f; f < burnSoundTime; f += Time.deltaTime)
            {
                audio.volume = Mathf.Lerp(1f, 0f, f / burnSoundTime);
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(burnSoundTime);
        }

        if (!isShrinking) // �u���b�٨S�}�l�Y�p�ɤ~����
        {
            Destroy(gameObject);
        }
    }
}