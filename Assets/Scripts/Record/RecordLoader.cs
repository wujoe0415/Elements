using UnityEngine;

public class RecordLoader : MonoBehaviour
{
    public static RecordLoader Instance { get; private set; }

    private Vector3 respawnPoint; // 用來記錄當前的重生點
    private Quaternion respawnRotation; // 用來記錄玩家的重生方向

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // 保持單例在場景切換時不被銷毀

        // 設置初始重生點為玩家的起始位置
        respawnPoint = Vector3.zero;
        respawnRotation = Quaternion.identity;
    }

    /// <summary>
    /// 更新當前的紀錄點
    /// </summary>
    /// <param name="newPosition">新的重生位置</param>
    /// <param name="newRotation">新的重生方向</param>
    public void UpdateFlag(Vector3 newPosition, Quaternion newRotation)
    {
        respawnPoint = newPosition;
        respawnRotation = newRotation;
        Debug.Log($"紀錄點已更新至位置: {newPosition}, 方向: {newRotation}");
    }

    /// <summary>
    /// 將玩家角色移動到紀錄點
    /// </summary>
    /// <param name="player">玩家角色的 GameObject</param>
    public void TriggerFlag(GameObject player)
    {
        player.transform.position = respawnPoint;
        player.transform.rotation = respawnRotation;
        Debug.Log("玩家已重生至紀錄點。");

        PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
        if (playerStatus != null)
        {
            playerStatus.LoadFromJson(); // 重生時恢復玩家狀態
        }
    }
}