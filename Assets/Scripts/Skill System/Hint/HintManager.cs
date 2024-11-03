using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class HintManager : MonoBehaviour
{
    public RectTransform HintAnchor;
    public GameObject ScreenWord;
    public Rect TextRect;
    public static HintManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void ShowHint(string hint, Color hintColor)
    {
        GameObject newWord = Instantiate(ScreenWord, HintAnchor.transform.position, Quaternion.identity, HintAnchor.transform);
        newWord.GetComponent<TextMeshProUGUI>().text = hint;
        newWord.GetComponent<TextMeshProUGUI>().color = hintColor;
        newWord.AddComponent<FadeText>();
    }
}
