using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class SkillHintWord
{
    public Skill Skill;
    public string Word;
    public Color WordColor = Color.white;
}

public class HintManager : MonoBehaviour
{
    public GameObject ScreenWord;
    public List<SkillHintWord> HintWords = new List<SkillHintWord>();
    public Rect TextRect;
    public void ShowSkillHint(Skill skill)
    {
        foreach (SkillHintWord word in HintWords) {
            if (word.Skill == skill) {
                ShowHint(word.Word);
                return;
            }
        }
    }
    public void ShowHint(string hint)
    {
        Vector2 pos = new Vector2(TextRect.x + Random.Range(-TextRect.width/2, TextRect.width / 2), TextRect.y+ Random.Range(-TextRect.height / 2, TextRect.height / 2));
        GameObject newWord = Instantiate(ScreenWord, pos, Quaternion.identity);
        newWord.GetComponent<TextMeshProUGUI>().text = hint;
    }
}
