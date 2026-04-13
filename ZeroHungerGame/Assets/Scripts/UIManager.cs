using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

// This script manages the UI elements of the game, including score display, hunger bar, and damage flash effect
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Hunger Bar")]
    [SerializeField] private Slider hungerBar;

    [Header("Damage Flash")]
    [SerializeField] private Image damageFlashImage;
    [SerializeField] private float flashDuration = 0.2f;
    [SerializeField] private float flashAlpha = 0.5f;

    private int score = 0;
    private float hunger = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        UpdateScoreUI();
        UpdateHungerUI();
    }

    // This method is called when the player picks up food, updating score and hunger values accordingly
    public void AddFood(int scoreAmount, float hungerAmount)
    {
        score += scoreAmount;
        hunger += hungerAmount;

        if (scoreAmount < 0 || hungerAmount < 0)
        {
            StartCoroutine(FlashRed());
        }

        score = Mathf.Max(0, score);
        hunger = Mathf.Clamp01(hunger);

        // Check if hunger bar is full and load next scene if it is
        if (hunger >= 1f)
        {
            LoadNextScene();
        }

        UpdateScoreUI();
        UpdateHungerUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    private void UpdateHungerUI()
    {
        if (hungerBar != null)
        {
            hungerBar.value = hunger;
        }
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    // This coroutine handles the red flash effect when the player takes damage from bad food
    private IEnumerator FlashRed()
    {
        if (damageFlashImage == null)
            yield break;

        Color color = damageFlashImage.color;
        color.a = flashAlpha;
        damageFlashImage.color = color;

        yield return new WaitForSeconds(flashDuration);

        // Fade out
        float t = 0f;
        float startAlpha = flashAlpha;

        // Fade the red flash out over time
        while (t < flashDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, t / flashDuration);

            color.a = alpha;
            damageFlashImage.color = color;

            yield return null;
        }

        color.a = 0f;
        damageFlashImage.color = color;
    }
}