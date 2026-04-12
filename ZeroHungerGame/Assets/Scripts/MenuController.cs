using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string game = "GameScene";
    [SerializeField] private string aboutScene = "AboutScene";
    [SerializeField] private string menu = "menu";

    [SerializeField] private GameObject pausePanel;

    private bool isPaused = false;

    public void PlayGame()
    {
        SceneManager.LoadScene(game);
    }

    public void OpenAbout()
    {
        SceneManager.LoadScene(aboutScene);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(menu);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;

        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void OpenWebsite()
    {
        Application.OpenURL("https://www.un.org/sustainabledevelopment/hunger/");
    }
}