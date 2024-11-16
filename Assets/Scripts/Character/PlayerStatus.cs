using UnityEngine;
using System.IO; // 用於檔案操作
using System.Collections;

[System.Serializable]
public class PlayerStatus : MonoBehaviour
{
    public int health = 100; // 玩家血量
    public int unlockedSkills = 1; // 玩家已解鎖技能數量，初始為 1，最大為 5

    [SerializeField] private GameObject deathEffectPrefab; // 粒子特效預製體
    [SerializeField] private Camera mainCamera; // 主相機
    [SerializeField] private Camera respawnCamera; // 復活相機
    [SerializeField] private Animator playerAnimator; // 玩家動畫控制器

    private string savePath;
    private Vector3 respawnPoint; // 復活點位置
    private Quaternion respawnRotation; // 復活點旋轉

    private void Awake()
    {
        // 定義儲存 JSON 檔案的路徑
        savePath = Path.Combine(Application.dataPath, "Resources", "PlayerStatus.json");
        Debug.Log($"存檔路徑: {savePath}");

        // 初始化復活點為玩家初始位置
        respawnPoint = transform.position;
        respawnRotation = transform.rotation;

        // 確保復活相機初始為禁用狀態
        if (respawnCamera != null)
        {
            respawnCamera.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Death"))
        {
            TakeDamage(10000); // 碰到 "Death" 層級物件時扣除 10000 血量
        }
    }

    /// <summary>
    /// 扣血並檢查玩家是否死亡
    /// </summary>
    /// <param name="damage">傷害值</param>
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"玩家受到了 {damage} 點傷害，當前血量: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 玩家死亡處理
    /// </summary>
    private void Die()
    {
        Debug.Log("玩家死亡，準備重生。");
        TriggerDeathEffect(); // 觸發粒子特效
        SaveToJson(); // 死亡前儲存當前狀態
        StartCoroutine(HandleRespawn()); // 啟動復活流程
    }

    /// <summary>
    /// 觸發死亡粒子特效
    /// </summary>
    private void TriggerDeathEffect()
    {
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Debug.Log("粒子特效觸發！");
        }
    }

    /// <summary>
    /// 復活流程
    /// </summary>
    private IEnumerator HandleRespawn()
    {
        // 切換到復活相機
        if (respawnCamera != null && mainCamera != null)
        {
            mainCamera.enabled = false;
            respawnCamera.enabled = true;
        }

        // 等待運鏡結束
        yield return new WaitForSeconds(2.0f);

        // 復活角色至存檔點
        transform.position = respawnPoint;
        transform.rotation = respawnRotation;

        if (playerAnimator != null)
        {
            playerAnimator.Play("FallFromSky"); // 播放從天而降的動畫
        }

        // 等待動畫完成
        yield return new WaitForSeconds(3.0f);

        // 切回主相機
        if (respawnCamera != null && mainCamera != null)
        {
            respawnCamera.enabled = false;
            mainCamera.enabled = true;
        }

        // 恢復玩家狀態
        health = 100;
        LoadFromJson(); // 從存檔恢復狀態
        Debug.Log("玩家已復活！");
    }

    /// <summary>
    /// 儲存玩家狀態為 JSON 檔案
    /// </summary>
    public void SaveToJson()
    {
        try
        {
            string json = JsonUtility.ToJson(this, true);
            File.WriteAllText(savePath, json);
            Debug.Log($"玩家狀態已儲存至 {savePath}");
        }
        catch (IOException e)
        {
            Debug.LogError($"儲存玩家狀態時發生錯誤: {e.Message}");
        }
    }

    /// <summary>
    /// 從 JSON 檔案讀取玩家狀態
    /// </summary>
    public void LoadFromJson()
    {
        if (File.Exists(savePath))
        {
            try
            {
                string json = File.ReadAllText(savePath);
                JsonUtility.FromJsonOverwrite(json, this);
                Debug.Log("玩家狀態已從存檔恢復。");
            }
            catch (IOException e)
            {
                Debug.LogError($"讀取玩家狀態時發生錯誤: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning("存檔不存在，無法恢復玩家狀態。");
        }
    }

    /// <summary>
    /// 設定復活點
    /// </summary>
    /// <param name="point">復活點位置</param>
    /// <param name="rotation">復活點旋轉</param>
    public void SetRespawnPoint(Vector3 point, Quaternion rotation)
    {
        respawnPoint = point;
        respawnRotation = rotation;
        Debug.Log("復活點已更新！");
    }
}
