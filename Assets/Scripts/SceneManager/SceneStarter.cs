using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;  // 需要加入這個命名空間

[RequireComponent(typeof(PlayableDirector))]
public class SceneStarter : MonoBehaviour
{
    public GameObject MainCharacter;
    private PlayableDirector director;  // 改用 PlayableDirector

    private void Awake()
    {
        MainCharacter.SetActive(false);
        director = GetComponent<PlayableDirector>();

        // 註冊播放完成的事件
        director.stopped += OnPlayableDirectorStopped;
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
            MainCharacter.SetActive(true);
            Destroy(gameObject);  // 播放完成後刪除這個物件
        }
    }
}