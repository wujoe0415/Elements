using UnityEngine;

public class UnlockSkill : MonoBehaviour
{
    public int skillID = 1; // �]�m��������ꪺ�ޯ� ID
    private bool isPlayerInRange = false; // �O�����a�O�_�b���ʽd��

    private void Update()
    {
        // ���a�b�d�򤺥B���U E ��ɡAĲ�o����ޯ�
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PlayerStatus playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                UnlockPlayerSkill(playerStatus);
            }
        }
    }

    /// <summary>
    /// �N���a���ޯ�ƶq�W�[�@�ӡA�Y���F��ޯ�W���C
    /// </summary>
    private void UnlockPlayerSkill(PlayerStatus playerStatus)
    {
        if (playerStatus.unlockedSkills < 5) // �ޯ�W���� 5
        {
            playerStatus.unlockedSkills++;
            Debug.Log($"�ޯ�w����I��e���ꪺ�ޯ�ƶq: {playerStatus.unlockedSkills}");
            playerStatus.SaveToJson(); // �O�s���a���A��s�� JSON �ɮ�

            // �i�H�[�J��L�ĪG�A�Ҧp�������ʵe�B���ĵ�

            Destroy(gameObject); // �����R���Ӫ���A����Ƹ���
        }
        else
        {
            Debug.Log("�ޯ�w�F��̤j�ƶq�A�L�k�A����C");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �ˬd�i�J�d�򪺬O�_�O���a
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("�i�� E �����ޯ�C");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ���a���}�d��ɭ��m���A
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("�w���}����d��C");
        }
    }
}