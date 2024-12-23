using UnityEngine;
using UnityEngine.EventSystems;  // 需要這個來處理 UI 事件
using UnityEngine.SceneManagement;  // 需要這個來處理場景切換

[RequireComponent(typeof(AudioSource))]
public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Header("Scene Settings")]
    public string SceneName;  // 要切換到的場景名稱

    [Header("Audio Settings")]
    public AudioClip hoverSound;    // hover 時的音效
    public AudioClip selectSound;   // 選擇時的音效

    private AudioSource audioSource;

    private void Awake()
    {
        // 獲取或添加 AudioSource 組件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // 當滑鼠懸停在按鈕上時
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    // 當按鈕被點擊時
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(PlaySelectSoundAndChangeScene());
    }

    // 協程：先播放音效，再切換場景
    private System.Collections.IEnumerator PlaySelectSoundAndChangeScene()
    {
        if (selectSound != null)
        {
            audioSource.PlayOneShot(selectSound);
            // 等待音效播放完畢
            yield return new WaitForSeconds(selectSound.length);
        }

        // 確保場景名稱不為空
        if (!string.IsNullOrEmpty(SceneName))
        {
            SceneManager.LoadScene(SceneName);
        }
        else
        {
            Debug.LogWarning("Scene name is not set!");
        }
    }
}