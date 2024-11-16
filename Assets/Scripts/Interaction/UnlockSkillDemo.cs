using UnityEngine;

public class UnlockSkillDemo : MonoBehaviour
{
    [SerializeField]
    private Color activatedColor = Color.green; // 解鎖後的顏色
    [SerializeField]
    private Renderer objectRenderer; // 對應的渲染器，設定材質顏色用

    private bool isActivated = false; // 是否已經觸發

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
                    Debug.Log($"玩家解鎖了一個技能，目前技能數量: {playerStatus.unlockedSkills}");

                    // 更換物件顏色
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
        Debug.Log("物件顏色已更改，UnlockSkill 觸發完成。");
    }
}