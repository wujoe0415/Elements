using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField]
    private bool isOneTimeUse = true; // �p�G�u���\�X�mĲ�o�@���A�]�� true
    private bool isActivated = false; // �����ӺX�m�O�_�w�QĲ�o

    private void OnTriggerEnter(Collider other)
    {
        if (isActivated && isOneTimeUse) return;

        if (other.CompareTag("Player"))
        {
            RecordLoader.Instance.UpdateFlag(transform.position, transform.rotation);
            isActivated = true;
            Debug.Log("���a�g�L�X�m�A�����I�M���A�w��s�C");
        }
    }
}