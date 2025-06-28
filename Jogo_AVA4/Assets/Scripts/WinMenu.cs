using UnityEngine;

public class WinMenu : MonoBehaviour
{
    public GameObject winMenuUI;
    public GameObject PauseMenuUI;
   
    private void OnDestroy()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 0f;
        winMenuUI.SetActive(true);
    }
}
