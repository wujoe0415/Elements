using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayableDirector))]
public class SceneEnder : MonoBehaviour
{
    public GameObject MainCharacter;
    private PlayableDirector director;  // ��� PlayableDirector

    public string NextSceneName;

    private void Awake()
    {
        MainCharacter.SetActive(false);
        director = GetComponent<PlayableDirector>();

        // ���U���񧹦����ƥ�
        director.played += OnPlayableDirectorPlayyed;
        director.stopped += OnPlayableDirectorStopped;
    }
    public void PlayAnimation()
    {
        director.Play();
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
            SceneManager.LoadScene(NextSceneName);
            Destroy(gameObject);  // ���񧹦���R���o�Ӫ���
        }
    }
    private void OnPlayableDirectorPlayyed(PlayableDirector aDirector)
    {
        if (aDirector == director)  // �T�{�O�P�@�� director
        {
            MainCharacter.SetActive(false);
        }
    }
}
