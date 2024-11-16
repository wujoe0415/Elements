using UnityEngine;

public class UnlockSkillDemo : MonoBehaviour
{
    [SerializeField]
    private Color activatedColor = Color.green; // ����᪺�C��
    [SerializeField]
    private Renderer objectRenderer; // ��������V���A�]�w�����C���

    private bool isActivated = false; // �O�_�w�gĲ�o

    private void Awake()
    {
        if (objectRenderer == null)
        {
            objectRenderer = GetComponent<Renderer>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isActivated) return;

        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
                if (playerStatus != null)
                {
                    playerStatus.unlockedSkills += 1;
                    playerStatus.unlockedSkills = Mathf.Clamp(playerStatus.unlockedSkills, 1, 5);
                    Debug.Log($"���a����F�@�ӧޯ�A�ثe�ޯ�ƶq: {playerStatus.unlockedSkills}");

                    // �󴫪����C��
                    Activate();
                }
            }
        }
    }

    private void Activate()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = activatedColor;
        }
        isActivated = true;
        Debug.Log("�����C��w���AUnlockSkill Ĳ�o�����C");
    }
}