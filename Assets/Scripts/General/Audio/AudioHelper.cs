using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class AudioHelper: MonoBehaviour
{
    public AudioSource Source;

    public Action OnStart;
    public Action OnEnd;

    public void PlayAudio(float fadeDuration = 0f, float delayDuration = 0f)
    {
        StopAllCoroutines();
        StartCoroutine(StartAudio(fadeDuration, delayDuration));
    }
    public void StopAudio(float fadeDuration = 0f)
    {
        StopAllCoroutines();
        StartCoroutine(EndAudio(fadeDuration));
    }
    public void Pause()
    {
        Source.Pause();
    }
    private IEnumerator StartAudio(float fadeDuration, float delayDuration)
    {
        yield return new WaitForSeconds(delayDuration);
        float targetVolume = Source.volume; 
        Source.volume = 0f;
        Source.Play();
        OnStart?.Invoke();
        for(float f = 0; f < fadeDuration; f += Time.deltaTime)
        {
            Source.volume = Mathf.Lerp(0f, targetVolume, f / fadeDuration);
            yield return null;
        }
        Source.volume = targetVolume;
        float length = Source.clip.length;
        Invoke("StopAudio", Mathf.Max(0, length - fadeDuration));
    }
    private IEnumerator EndAudio(float fadeDuration)
    {
        float initVolume = Source.volume;
        for(float f = 0; f < fadeDuration; f += Time.deltaTime)
        {
            if (!Source.isPlaying)
                break;
            Source.volume = Mathf.Lerp(initVolume, 0f, f / fadeDuration);
            yield return null;
        }
        Source.volume = 0f;
        Source.Stop();
        OnEnd?.Invoke();
    }
}
