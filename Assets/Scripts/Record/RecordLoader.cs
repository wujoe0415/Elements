using UnityEngine;

public class RecordLoader : MonoBehaviour
{
    public static RecordLoader Instance { get; private set; }

    private Vector3 respawnPoint; // �ΨӰO����e�������I
    private Quaternion respawnRotation; // �ΨӰO�����a�����ͤ�V

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // �O����Ҧb���������ɤ��Q�P��

        // �]�m��l�����I�����a���_�l��m
        respawnPoint = Vector3.zero;
        respawnRotation = Quaternion.identity;
    }

    /// <summary>
    /// ��s��e�������I
    /// </summary>
    /// <param name="newPosition">�s�����ͦ�m</param>
    /// <param name="newRotation">�s�����ͤ�V</param>
    public void UpdateFlag(Vector3 newPosition, Quaternion newRotation)
    {
        respawnPoint = newPosition;
        respawnRotation = newRotation;
        Debug.Log($"�����I�w��s�ܦ�m: {newPosition}, ��V: {newRotation}");
    }

    /// <summary>
    /// �N���a���Ⲿ�ʨ�����I
    /// </summary>
    /// <param name="player">���a���⪺ GameObject</param>
    public void TriggerFlag(GameObject player)
    {
        player.transform.position = respawnPoint;
        player.transform.rotation = respawnRotation;
        Debug.Log("���a�w���ͦܬ����I�C");

        PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
        if (playerStatus != null)
        {
            playerStatus.LoadFromJson(); // ���ͮɫ�_���a���A
        }
    }
}