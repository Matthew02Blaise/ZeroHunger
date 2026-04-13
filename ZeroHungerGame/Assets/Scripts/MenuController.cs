using UnityEngine;
using UnityEngine.SceneManagement;

// This script manages the menu interactions, including starting the game, opening the about page, pausing/resuming the game, and quitting the game
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

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    // This method opens the UN's hunger page in the users web browser
    public void OpenWebsite()
    {
        Application.OpenURL("https://www.un.org/sustainabledevelopment/hunger/");
    }
}