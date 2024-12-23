using UnityEngine;
using UnityEngine.EventSystems;  // �ݭn�o�ӨӳB�z UI �ƥ�
using UnityEngine.SceneManagement;  // �ݭn�o�ӨӳB�z��������

[RequireComponent(typeof(AudioSource))]
public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Header("Scene Settings")]
    public string SceneName;  // �n�����쪺�����W��

    [Header("Audio Settings")]
    public AudioClip hoverSound;    // hover �ɪ�����
    public AudioClip selectSound;   // ��ܮɪ�����

    private AudioSource audioSource;

    private void Awake()
    {
        // ����βK�[ AudioSource �ե�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // ��ƹ��a���b���s�W��
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    // ����s�Q�I����
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(PlaySelectSoundAndChangeScene());
    }

    // ��{�G�����񭵮ġA�A��������
    private System.Collections.IEnumerator PlaySelectSoundAndChangeScene()
    {
        if (selectSound != null)
        {
            audioSource.PlayOneShot(selectSound);
            // ���ݭ��ļ��񧹲�
            yield return new WaitForSeconds(selectSound.length);
        }

        // �T�O�����W�٤�����
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