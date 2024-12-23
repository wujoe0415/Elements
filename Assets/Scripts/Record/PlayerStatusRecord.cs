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
        // �w�q�x�s JSON �ɮת����|
        savePath = Path.Combine(Application.persistentDataPath, "PlayerStatus.json");
        Debug.Log($"�s�ɸ��|: {savePath}");
        if (!File.Exists(savePath))
        {
            try
            {
                // �����ձq Resources Ū���w�]�ɮ�
                TextAsset defaultJson = Resources.Load<TextAsset>("PlayerStatus");
                string jsonContent;

                if (defaultJson != null)
                {
                    jsonContent = defaultJson.text;
                }
                else
                {
                    // �p�G Resources ���S���w�]�ɮסA�ϥε{���X�����w�]��
                    RecordData defaultData = RecordData.CreateDefault();
                    jsonContent = JsonUtility.ToJson(defaultData, true);
                }

                File.WriteAllText(savePath, jsonContent);
                Debug.Log("�w�Ыعw�]�s��");
            }
            catch (IOException e)
            {
                Debug.LogError($"�Ыعw�]�s�ɮɵo�Ϳ��~: {e.Message}");
            }
        }
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
            if (!File.Exists(savePath))
            {
                Debug.LogWarning("�䤣��s���ɮסA�N��^�w�]��");
                return RecordData.CreateDefault();
            }

            string json = File.ReadAllText(savePath);
            RecordData data = JsonUtility.FromJson<RecordData>(json);

            // �p�G�ѪR���ѡA��^�w�]��
            if (data == null)
            {
                Debug.LogWarning("�s���ɮ׷l�a�A�N��^�w�]��");
                return RecordData.CreateDefault();
            }

            return data;
        }
        catch (IOException e)
        {
            Debug.LogError($"Ū�����a���A�ɵo�Ϳ��~: {e.Message}");
            return RecordData.CreateDefault();
        }
    }
}
