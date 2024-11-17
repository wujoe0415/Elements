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
        DontDestroyOnLoad(gameObject); // �O����Ҧb���������ɤ��Q�P��
        Recorder = GetComponent<PlayerStatusRecord>();
    }
    /// <summary>
    /// ��s��e�������I
    /// </summary>
    /// <param name="newPosition">�s�����ͦ�m</param>
    /// <param name="newRotation">�s�����ͤ�V</param>
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
    /// �N���a���Ⲿ�ʨ�����I
    /// </summary>
    /// <param name="player">���a���⪺ GameObject</param>
    public void TriggerFlag(GameObject player)
    {

        PlayerStatusManager playerStatus = player.GetComponent<PlayerStatusManager>();
        if (playerStatus != null)
        {
            RecordData status = Recorder.LoadFromJson();
            if (status.Status != null)
                playerStatus.SetPlayerStatus(status.Status); // ���ͮɫ�_���a���A
            else
                Debug.LogError("PlayerStatus Ū�����~�I");

            Debug.Log(status.RespawnPosition);

            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = status.RespawnPosition;
            player.transform.rotation = status.RespawnRotation;
            player.GetComponent<CharacterController>().enabled = true;
            Debug.Log("���a�w���ͦܬ����I�C");
        }
    }
}

    