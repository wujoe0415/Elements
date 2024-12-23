using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

public class PostAnimation : MonoBehaviour
{
    public GameObject MainCharacter;
    private PlayableDirector director;  // 改用 PlayableDirector
    public UnityEvent OnPostTimelineEnd;
    private void Awake()
    {
        MainCharacter.SetActive(false);
        director = GetComponent<PlayableDirector>();

        // 註冊播放完成的事件
        director.stopped += OnPlayableDirectorStopped;
    }

    private void OnDestroy()
    {
        // 移除事件監聽，避免記憶體洩漏
        if (director != null)
        {
            director.stopped -= OnPlayableDirectorStopped;
        }
    }

    // Timeline 播放完成時會呼叫這個方法
    private void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (aDirector == director)  // 確認是同一個 director
        {
            MainCharacter.SetActive(true);
            PlayerStatusManager.Instance.UnlockSkill();
            OnPostTimelineEnd.Invoke();
            Destroy(gameObject);
        }
    }
}
