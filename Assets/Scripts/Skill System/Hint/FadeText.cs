using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;

public class FadeText : MonoBehaviour
{
    public float Duration = 1.0f;
    public Color TargetColor = Color.white;
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 0);
        StartCoroutine(FadeTextUI());
    }
    public void SetTargetColor(Color color)
    {
        TargetColor = color;
    }
    private IEnumerator FadeTextUI()
    {
        float fadeDuration = Duration / 2;
        Color color = _text.color;
        for(float t = 0.01f; t < fadeDuration; t += Time.deltaTime)
        {
            _text.color = Color.Lerp(color, TargetColor, Mathf.Min(1, t / fadeDuration));
            yield return null;
        }
        // Stay 3 frames
        yield return null;
        yield return null;
        yield return null;
        for(float t = 0.01f; t < fadeDuration; t += Time.deltaTime)
        {
            _text.color = Color.Lerp(TargetColor, color, Mathf.Min(1, t / fadeDuration));
            yield return null;
        }
        Destroy(gameObject);
    }
}
