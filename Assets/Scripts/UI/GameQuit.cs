using UnityEngine;

public class GameQuit : MonoBehaviour
{
    void Update()
    {
        // �ˬd�O�_���U ESC ��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        // �p�G�b Unity �s�边���B��A�h�����Ҧ�
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // �b��ڰ����ɤ��B��ɡA�������ε{��
            Application.Quit();
#endif
    }
}