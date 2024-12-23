using UnityEngine;

public class GameQuit : MonoBehaviour
{
    void Update()
    {
        // 檢查是否按下 ESC 鍵
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        // 如果在 Unity 編輯器中運行，則停止播放模式
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // 在實際執行檔中運行時，關閉應用程式
            Application.Quit();
#endif
    }
}