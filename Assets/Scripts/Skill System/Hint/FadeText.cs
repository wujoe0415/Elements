using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;

public class FadeText : MonoBehaviour
{
    public float Duration = 0.8f;
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 0);
        StartCoroutine(FadeTextUI());
    }
    private IEnumerator FadeTextUI()
    {
        float showDuration = 0.1f;
        float fadeDuration = Duration - showDuration;
        Color _initColor = new Color(_text.color.r, _text.color.g, _text.color.b, 0f);
        Color _targertColor = new Color(_text.color.r, _text.color.g, _text.color.b, 1f);
        for (float t = 0.01f; t < showDuration; t += Time.deltaTime)
        {
            _text.color = Color.Lerp(_initColor, _targertColor, Mathf.Min(1, t / showDuration));
            yield return null;
        }
        // Stay 5 frames
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        for (float t = 0.01f; t < fadeDuration; t += Time.deltaTime)
        {
            _text.color = Color.Lerp(_targertColor, _initColor, Mathf.Min(1, t / fadeDuration));
            yield return null;
        }
        Destroy(gameObject);
    }
}
