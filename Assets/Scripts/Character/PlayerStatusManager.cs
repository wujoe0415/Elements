using UnityEngine;
using System.IO; // 用於檔案操作
using System.Collections;

[System.Serializable]
public class PlayerStatus {
    public int Health = 100;
    public int UnlockSkillNum = 1;
}

public class PlayerStatusManager : MonoBehaviour
{
    public static PlayerStatusManager Instance;
    public PlayerStatus Status;

    [SerializeField] private GameObject deathEffectPrefab; // 粒子特效預製體
    [SerializeField] private Camera mainCamera; // 主相機
    [SerializeField] private Camera respawnCamera; // 復活相機
    [SerializeField] private Animator playerAnimator; // 玩家動畫控制器

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
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
        Status.Health -= damage;
        Debug.Log($"玩家受到了 {damage} 點傷害，當前血量: {Status.Health}");

        if (Status.Health <= 0)
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
        RecordLoader.Instance.AddDeathNum();
        TriggerDeathEffect(); // 觸發粒子特效
        StartCoroutine(HandleRespawn()); // 啟動復活流程
    }

    /// <summary>
    /// 觸發死亡粒子特效
    /// </summary>
    private void TriggerDeathEffect()
    {
        if (playerAnimator != null)
        {
            playerAnimator.Play("Die"); // 播放從天而降的動畫
        }
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

        RecordLoader.Instance.TriggerFlag(this.gameObject);

        if (playerAnimator != null)
        {
            playerAnimator.Play("Respawn"); // 播放從天而降的動畫
        }

        // 等待動畫完成
        yield return new WaitForSeconds(3.0f);

        // 切回主相機
        if (respawnCamera != null && mainCamera != null)
        {
            respawnCamera.enabled = false;
            mainCamera.enabled = true;
        }
        Debug.Log("玩家已復活！");
    }

    public void SetPlayerStatus(PlayerStatus status) {
        Status = status;
    }
}
