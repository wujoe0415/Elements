using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField]
    private bool isOneTimeUse = true; // 如果只允許旗幟觸發一次，設為 true
    private bool isActivated = false; // 紀錄該旗幟是否已被觸發

    private void OnTriggerEnter(Collider other)
    {
        if (isActivated && isOneTimeUse) return;

        if (other.CompareTag("Player"))
        {
            RecordLoader.Instance.UpdateFlag(transform.position, transform.rotation);
            isActivated = true;
            Debug.Log("玩家經過旗幟，紀錄點和狀態已更新。");
        }
    }
}