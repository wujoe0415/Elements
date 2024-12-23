using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;  // �ݭn�[�J�o�өR�W�Ŷ�

[RequireComponent(typeof(PlayableDirector))]
public class SceneStarter : MonoBehaviour
{
    public GameObject MainCharacter;
    private PlayableDirector director;  // ��� PlayableDirector

    private void Awake()
    {
        MainCharacter.SetActive(false);
        director = GetComponent<PlayableDirector>();

        // ���U���񧹦����ƥ�
        director.stopped += OnPlayableDirectorStopped;
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
            MainCharacter.SetActive(true);
            Destroy(gameObject);  // ���񧹦���R���o�Ӫ���
        }
    }
}