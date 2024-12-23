using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayableDirector))]
public class SceneEnder : MonoBehaviour
{
    public GameObject MainCharacter;
    private PlayableDirector director;  // 改用 PlayableDirector

    public string NextSceneName;

    private void Awake()
    {
        MainCharacter.SetActive(false);
        director = GetComponent<PlayableDirector>();

        // 註冊播放完成的事件
        director.played += OnPlayableDirectorPlayyed;
        director.stopped += OnPlayableDirectorStopped;
    }
    public void PlayAnimation()
    {
        director.Play();
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
            SceneManager.LoadScene(NextSceneName);
            Destroy(gameObject);  // 播放完成後刪除這個物件
        }
    }
    private void OnPlayableDirectorPlayyed(PlayableDirector aDirector)
    {
        if (aDirector == director)  // 確認是同一個 director
        {
            MainCharacter.SetActive(false);
        }
    }
}
