using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;

[RequireComponent(typeof(PlayerStatusRecord))]
public class RecordLoader : MonoBehaviour
{
    public static RecordLoader Instance { get; private set; }
    public PlayerStatusRecord Recorder;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // 保持單例在場景切換時不被銷毀
        Recorder = GetComponent<PlayerStatusRecord>();
    }
    /// <summary>
    /// 更新當前的紀錄點
    /// </summary>
    /// <param name="newPosition">新的重生位置</param>
    /// <param name="newRotation">新的重生方向</param>
    public void UpdateFlag(Vector3 newPosition, Quaternion newRotation)
    {
        RecordData data = Recorder.LoadFromJson();
        data.RespawnPosition = newPosition;
        data.RespawnRotation = newRotation;
        Recorder.SaveToJson(data);
    }
    public void UpdatePlayerStatus()
    {

        RecordData data = Recorder.LoadFromJson();
        data.Status = PlayerStatusManager.Instance.Status;
        Debug.Log(PlayerStatusManager.Instance.Status);
        Recorder.SaveToJson(data);
    }
    public void AddDeathNum()
    {
        RecordData data = Recorder.LoadFromJson();
        data.DeathNum++;
        Recorder.SaveToJson(data);
    }
    /// <summary>
    /// 將玩家角色移動到紀錄點
    /// </summary>
    /// <param name="player">玩家角色的 GameObject</param>
    public void TriggerFlag(GameObject player)
    {

        PlayerStatusManager playerStatus = player.GetComponent<PlayerStatusManager>();
        if (playerStatus != null)
        {
            RecordData status = Recorder.LoadFromJson();
            if (status.Status != null)
                playerStatus.SetPlayerStatus(status.Status); // 重生時恢復玩家狀態
            else
                Debug.LogError("PlayerStatus 讀取錯誤！");

            Debug.Log(status.RespawnPosition);

            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = status.RespawnPosition;
            player.transform.rotation = status.RespawnRotation;
            player.GetComponent<CharacterController>().enabled = true;
            Debug.Log("玩家已重生至紀錄點。");
        }
    }
}

    