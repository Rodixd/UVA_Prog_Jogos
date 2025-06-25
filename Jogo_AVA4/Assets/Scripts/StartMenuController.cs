using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public GameObject controlPanel;
   public void OnStartClick()
    {
        SceneManager.LoadScene("SampleScene");

    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void OnControlClick()
    {
        controlPanel.SetActive(true);
    }

    public void OnXClick()
    {
        controlPanel.SetActive(false);
    }
}
