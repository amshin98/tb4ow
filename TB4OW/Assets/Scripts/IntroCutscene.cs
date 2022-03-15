using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCutscene : MonoBehaviour
{
    public UnityEngine.Video.VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.loopPointReached += GotoMainMenu;
    }

    void Update()
    {
        // Get keypress to skip cutscene
        if (Input.anyKey)
        {
            GotoMainMenu(null);
        }
    }

    void GotoMainMenu(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene("StartScreen");
    }
}
