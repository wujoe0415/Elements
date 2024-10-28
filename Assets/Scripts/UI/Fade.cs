using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Fade : MonoBehaviour
{
    public static Fade Instance { get; private set; }
    public Image Cover;

    private IEnumerator _coroutine;
    
    public void FadeToColor(Color color, float duration = 1f)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = FadeTo(duration, color);
        StartCoroutine(_coroutine);
    }
    public void ClearFlag(float duration = 1f)
    {
        Color target = new Color(Cover.color.r, Cover.color.b, Cover.color.g, 0);
        if(_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = FadeTo(duration, target);
        StartCoroutine(_coroutine);
    }
    public void FadeStay(Color color, float duration = 1f, float stayTime = 0.5f)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = FadeToBack(color, duration, stayTime);
        StartCoroutine(_coroutine);
    }
    private IEnumerator FadeTo(float duration, Color color)
    {
        Color init = Cover.color;
        for (float f = 0f; f <= duration; f += Time.deltaTime)
        {
            Cover.color = Color.Lerp(init, color, f);
            yield return null;
        }
        Cover.color = color;
    }
    private IEnumerator FadeToBack(Color color, float duration = 1f, float stayTime = 0.5f)
    {
        Color initColor = Cover.color;
        for(float f = 0f; f<duration; f+=Time.deltaTime)
        {
            Cover.color = Color.Lerp(initColor, color, f);
            yield return null;
        }
        Cover.color = color;
        yield return new WaitForSeconds(stayTime);
        for (float f = 0f; f < duration; f += Time.deltaTime)
        {
            Cover.color = Color.Lerp(color, initColor, f);
            yield return null;
        }
        Cover.color = initColor;
    }
}
