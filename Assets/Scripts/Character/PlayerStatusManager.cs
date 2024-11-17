using UnityEngine;
using System.IO; // �Ω��ɮ׾ާ@
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

    [SerializeField] private GameObject deathEffectPrefab; // �ɤl�S�Ĺw�s��
    [SerializeField] private Camera mainCamera; // �D�۾�
    [SerializeField] private Camera respawnCamera; // �_���۾�
    [SerializeField] private Animator playerAnimator; // ���a�ʵe���

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
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
        Status.Health -= damage;
        Debug.Log($"���a����F {damage} �I�ˮ`�A��e��q: {Status.Health}");

        if (Status.Health <= 0)
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
        RecordLoader.Instance.AddDeathNum();
        TriggerDeathEffect(); // Ĳ�o�ɤl�S��
        StartCoroutine(HandleRespawn()); // �Ұʴ_���y�{
    }

    /// <summary>
    /// Ĳ�o���`�ɤl�S��
    /// </summary>
    private void TriggerDeathEffect()
    {
        if (playerAnimator != null)
        {
            playerAnimator.Play("Die"); // ����q�Ѧӭ����ʵe
        }
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

        RecordLoader.Instance.TriggerFlag(this.gameObject);

        if (playerAnimator != null)
        {
            playerAnimator.Play("Respawn"); // ����q�Ѧӭ����ʵe
        }

        // ���ݰʵe����
        yield return new WaitForSeconds(3.0f);

        // ���^�D�۾�
        if (respawnCamera != null && mainCamera != null)
        {
            respawnCamera.enabled = false;
            mainCamera.enabled = true;
        }
        Debug.Log("���a�w�_���I");
    }

    public void SetPlayerStatus(PlayerStatus status) {
        Status = status;
    }
}
