using UnityEngine;

public class UnlockSkill : MonoBehaviour
{
    public int skillID = 1; // 設置此物件解鎖的技能 ID
    private bool isPlayerInRange = false; // 記錄玩家是否在互動範圍內

    private void Update()
    {
        // 當玩家在範圍內且按下 E 鍵時，觸發解鎖技能
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PlayerStatus playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                UnlockPlayerSkill(playerStatus);
            }
        }
    }

    /// <summary>
    /// 將玩家的技能數量增加一個，若未達到技能上限。
    /// </summary>
    private void UnlockPlayerSkill(PlayerStatus playerStatus)
    {
        if (playerStatus.unlockedSkills < 5) // 技能上限為 5
        {
            playerStatus.unlockedSkills++;
            Debug.Log($"技能已解鎖！當前解鎖的技能數量: {playerStatus.unlockedSkills}");
            playerStatus.SaveToJson(); // 保存玩家狀態更新至 JSON 檔案

            // 可以加入其他效果，例如播放解鎖動畫、音效等

            Destroy(gameObject); // 解鎖後摧毀該物件，防止重複解鎖
        }
        else
        {
            Debug.Log("技能已達到最大數量，無法再解鎖。");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 檢查進入範圍的是否是玩家
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("可按 E 鍵解鎖技能。");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 玩家離開範圍時重置狀態
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("已離開解鎖範圍。");
        }
    }
}