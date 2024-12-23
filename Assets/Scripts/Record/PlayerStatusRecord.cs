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

    public static RecordData CreateDefault()
    {
        return new RecordData
        {
            Status = new PlayerStatus
            {
                Health = 100,
                UnlockSkillNum = 3
            },
            RespawnPosition = new Vector3(-0.57f, -8.96f, -2.56f),
            RespawnRotation = new Quaternion(0f, 0.3826834f, 0f, 0.9238796f),
            DeathNum = 137
        };
    }
}

public class PlayerStatusRecord : MonoBehaviour
{
    private string savePath;
    private void Awake()
    {
        // 定義儲存 JSON 檔案的路徑
        savePath = Path.Combine(Application.persistentDataPath, "PlayerStatus.json");
        Debug.Log($"存檔路徑: {savePath}");
        if (!File.Exists(savePath))
        {
            try
            {
                // 先嘗試從 Resources 讀取預設檔案
                TextAsset defaultJson = Resources.Load<TextAsset>("PlayerStatus");
                string jsonContent;

                if (defaultJson != null)
                {
                    jsonContent = defaultJson.text;
                }
                else
                {
                    // 如果 Resources 中沒有預設檔案，使用程式碼中的預設值
                    RecordData defaultData = RecordData.CreateDefault();
                    jsonContent = JsonUtility.ToJson(defaultData, true);
                }

                File.WriteAllText(savePath, jsonContent);
                Debug.Log("已創建預設存檔");
            }
            catch (IOException e)
            {
                Debug.LogError($"創建預設存檔時發生錯誤: {e.Message}");
            }
        }
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
            if (!File.Exists(savePath))
            {
                Debug.LogWarning("找不到存檔檔案，將返回預設值");
                return RecordData.CreateDefault();
            }

            string json = File.ReadAllText(savePath);
            RecordData data = JsonUtility.FromJson<RecordData>(json);

            // 如果解析失敗，返回預設值
            if (data == null)
            {
                Debug.LogWarning("存檔檔案損壞，將返回預設值");
                return RecordData.CreateDefault();
            }

            return data;
        }
        catch (IOException e)
        {
            Debug.LogError($"讀取玩家狀態時發生錯誤: {e.Message}");
            return RecordData.CreateDefault();
        }
    }
}
