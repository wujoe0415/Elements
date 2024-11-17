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
        // �w�q�x�s JSON �ɮת����|
        savePath = Path.Combine(Application.dataPath, "Resources", "PlayerStatus.json");
        Debug.Log($"�s�ɸ��|: {savePath}");
    }

    /// <summary>
    /// �x�s���a���A�� JSON �ɮ�
    /// </summary>
    public void SaveToJson(RecordData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json);
            Debug.Log($"���a���A�w�x�s�� {savePath}");
        }
        catch (IOException e)
        {
            Debug.LogError($"�x�s���a���A�ɵo�Ϳ��~: {e.Message}");
        }
    }

    /// <summary>
    /// �q JSON �ɮ�Ū�����a���A
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
            Debug.LogError($"Ū�����a���A�ɵo�Ϳ��~: {e.Message}");
            return null;
        }
    }
}
