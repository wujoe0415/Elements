using UnityEngine;
using System.IO; // �Ω��ɮ׾ާ@
using System.Collections;

[System.Serializable]
public class PlayerStatus : MonoBehaviour
{
    public int health = 100; // ���a��q
    public int unlockedSkills = 1; // ���a�w����ޯ�ƶq�A��l�� 1�A�̤j�� 5

    [SerializeField] private GameObject deathEffectPrefab; // �ɤl�S�Ĺw�s��
    [SerializeField] private Camera mainCamera; // �D�۾�
    [SerializeField] private Camera respawnCamera; // �_���۾�
    [SerializeField] private Animator playerAnimator; // ���a�ʵe���

    private string savePath;
    private Vector3 respawnPoint; // �_���I��m
    private Quaternion respawnRotation; // �_���I����

    private void Awake()
    {
        // �w�q�x�s JSON �ɮת����|
        savePath = Path.Combine(Application.dataPath, "Resources", "PlayerStatus.json");
        Debug.Log($"�s�ɸ��|: {savePath}");

        // ��l�ƴ_���I�����a��l��m
        respawnPoint = transform.position;
        respawnRotation = transform.rotation;

        // �T�O�_���۾���l���T�Ϊ��A
        if (respawnCamera != null)
        {
            respawnCamera.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Death"))
        {
            TakeDamage(10000); // �I�� "Death" �h�Ū���ɦ��� 10000 ��q
        }
    }

    /// <summary>
    /// ������ˬd���a�O�_���`
    /// </summary>
    /// <param name="damage">�ˮ`��</param>
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"���a����F {damage} �I�ˮ`�A��e��q: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// ���a���`�B�z
    /// </summary>
    private void Die()
    {
        Debug.Log("���a���`�A�ǳƭ��͡C");
        TriggerDeathEffect(); // Ĳ�o�ɤl�S��
        SaveToJson(); // ���`�e�x�s��e���A
        StartCoroutine(HandleRespawn()); // �Ұʴ_���y�{
    }

    /// <summary>
    /// Ĳ�o���`�ɤl�S��
    /// </summary>
    private void TriggerDeathEffect()
    {
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Debug.Log("�ɤl�S��Ĳ�o�I");
        }
    }

    /// <summary>
    /// �_���y�{
    /// </summary>
    private IEnumerator HandleRespawn()
    {
        // ������_���۾�
        if (respawnCamera != null && mainCamera != null)
        {
            mainCamera.enabled = false;
            respawnCamera.enabled = true;
        }

        // ���ݹB�赲��
        yield return new WaitForSeconds(2.0f);

        // �_������ܦs���I
        transform.position = respawnPoint;
        transform.rotation = respawnRotation;

        if (playerAnimator != null)
        {
            playerAnimator.Play("FallFromSky"); // ����q�Ѧӭ����ʵe
        }

        // ���ݰʵe����
        yield return new WaitForSeconds(3.0f);

        // ���^�D�۾�
        if (respawnCamera != null && mainCamera != null)
        {
            respawnCamera.enabled = false;
            mainCamera.enabled = true;
        }

        // ��_���a���A
        health = 100;
        LoadFromJson(); // �q�s�ɫ�_���A
        Debug.Log("���a�w�_���I");
    }

    /// <summary>
    /// �x�s���a���A�� JSON �ɮ�
    /// </summary>
    public void SaveToJson()
    {
        try
        {
            string json = JsonUtility.ToJson(this, true);
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
    public void LoadFromJson()
    {
        if (File.Exists(savePath))
        {
            try
            {
                string json = File.ReadAllText(savePath);
                JsonUtility.FromJsonOverwrite(json, this);
                Debug.Log("���a���A�w�q�s�ɫ�_�C");
            }
            catch (IOException e)
            {
                Debug.LogError($"Ū�����a���A�ɵo�Ϳ��~: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning("�s�ɤ��s�b�A�L�k��_���a���A�C");
        }
    }

    /// <summary>
    /// �]�w�_���I
    /// </summary>
    /// <param name="point">�_���I��m</param>
    /// <param name="rotation">�_���I����</param>
    public void SetRespawnPoint(Vector3 point, Quaternion rotation)
    {
        respawnPoint = point;
        respawnRotation = rotation;
        Debug.Log("�_���I�w��s�I");
    }
}
