using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class RecordData {
    public PlayerStatus Status;
    public Vector3 RespawnPosition;
    public Quaternion RespawnRotation;
    public int DeathNum;
}

public class PlayerStatusRecord : MonoBehaviour
{
    private string savePath;
    private void Awake()
    {
        // 定義儲存 JSON 檔案的路徑
        savePath = Path.Combine(Application.dataPath, "Resources", "PlayerStatus.json");
        Debug.Log($"存檔路徑: {savePath}");
    }

    /// <summary>
    /// 儲存玩家狀態為 JSON 檔案
    /// </summary>
    public void SaveToJson(RecordData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
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
    public RecordData LoadFromJson()
    {
        try
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<RecordData>(json);
        }
        catch (IOException e)
        {
            Debug.LogError($"讀取玩家狀態時發生錯誤: {e.Message}");
            return null;
        }
    }
}
