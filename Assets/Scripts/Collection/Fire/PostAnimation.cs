using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

public class PostAnimation : MonoBehaviour
{
    public GameObject MainCharacter;
    private PlayableDirector director;  // ��� PlayableDirector
    public UnityEvent OnPostTimelineEnd;
    private void Awake()
    {
        MainCharacter.SetActive(false);
        director = GetComponent<PlayableDirector>();

        // ���U���񧹦����ƥ�
        director.stopped += OnPlayableDirectorStopped;
    }

    private void OnDestroy()
    {
        // �����ƥ��ť�A�קK�O���鬪�|
        if (director != null)
        {
            director.stopped -= OnPlayableDirectorStopped;
        }
    }

    // Timeline ���񧹦��ɷ|�I�s�o�Ӥ�k
    private void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (aDirector == director)  // �T�{�O�P�@�� director
        {
            MainCharacter.SetActive(true);
            PlayerStatusManager.Instance.UnlockSkill();
            OnPostTimelineEnd.Invoke();
            Destroy(gameObject);
        }
    }
}
