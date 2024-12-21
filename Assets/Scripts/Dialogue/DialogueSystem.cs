using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance { get; private set; }
    public GameObject DialogueUI;
    public TextMeshProUGUI text;
    public List<string> Dialogues;

    [Header("Settings")]
    public float typingSpeed = 0.05f;
    public float dialogueInterval = 1f;
    public float fadeOutDuration = 0.5f;  

    private Coroutine dialogueCoroutine; 
    private Coroutine typewriterCoroutine; 
    private bool isTyping = false;         

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    // Test

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            StartDialogue();
        }
    }


    public void SetDiagues(List<string> dialogues)
    {
        if (dialogueCoroutine != null)
            EndDialogue();
        Dialogues.Clear();
        Dialogues = new List<string>(dialogues);
    }

    public void StartDialogue()
    {
        if (Dialogues.Count == 0)
            return;

        DialogueUI.SetActive(true);
        if (dialogueCoroutine != null)
            EndDialogue();
        dialogueCoroutine = StartCoroutine(DialogueLoop());
    }

    private IEnumerator DialogueLoop()
    {
        while (Dialogues.Count > 0)
        {
            // �}�l���r�ĪG
            typewriterCoroutine = StartCoroutine(TypewriterEffect(Dialogues[0]));
            yield return new WaitUntil(() => !isTyping);  // ���ݥ��r����

            yield return new WaitForSeconds(dialogueInterval);

            // �H�X�ĪG
            yield return StartCoroutine(FadeOutText());

            // ���ܤU�@�q���
            Dialogues.RemoveAt(0);
        }

        EndDialogue();
    }

    private IEnumerator TypewriterEffect(string fullText)
    {
        isTyping = true;
        text.text = "";
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);

        foreach (char c in fullText)
        {
            text.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private IEnumerator FadeOutText()
    {
        float elapsedTime = 0f;
        Color startColor = text.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeOutDuration;
            text.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        text.text = "";
    }

    public void NextDialogue()
    {
        // �p�G���b���r�A�h������e��r
        if (isTyping)
        {
            if (typewriterCoroutine != null)
                StopCoroutine(typewriterCoroutine);
            text.text = Dialogues[0];
            isTyping = false;
            return;
        }

        if (Dialogues.Count == 0)
            return;

        Dialogues.RemoveAt(0);
        if (Dialogues.Count == 0)
        {
            EndDialogue();
            return;
        }

        // �}�l�s�����r�ĪG
        typewriterCoroutine = StartCoroutine(TypewriterEffect(Dialogues[0]));
    }

    public void EndDialogue()
    {
        if (dialogueCoroutine != null)
            StopCoroutine(dialogueCoroutine);
        if (typewriterCoroutine != null)
            StopCoroutine(typewriterCoroutine);

        DialogueUI.SetActive(false);
        Dialogues.Clear();
        text.text = "";
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
    }
}