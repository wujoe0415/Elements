using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour
{
    public float Duration = 5f;
    private AudioSource _audio;
    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        
    }

    public void OnEnable()
    {
        StartCoroutine(Raining());
    }
    private IEnumerator Raining()
    {
        // ¬Iªk
        yield return new WaitForSeconds(1f);
        for(int i = 0;i<transform.childCount;i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        float fadeDuration = 1f;
        _audio.volume = 0;
        _audio.Play();
        for(float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            _audio.volume = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        _audio.volume = 1;
        
        yield return new WaitForSeconds(Duration - fadeDuration);
        
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            _audio.volume = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }
        _audio.volume = 0;
        Destroy(gameObject);

    }
}
