using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

    public void AddFood(int scoreAmount, float hungerAmount)
    {
        score += scoreAmount;
        hunger += hungerAmount;

        // If bad food (negative values), trigger flash
        if (scoreAmount < 0 || hungerAmount < 0)
        {
            StartCoroutine(FlashRed());
        }

        score = Mathf.Max(0, score);
        hunger = Mathf.Clamp01(hunger);

        // Check if hunger is full
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

    private IEnumerator FlashRed()
    {
        if (damageFlashImage == null)
            yield break;

        // Set visible red
        Color color = damageFlashImage.color;
        color.a = flashAlpha;
        damageFlashImage.color = color;

        yield return new WaitForSeconds(flashDuration);

        // Fade out
        float t = 0f;
        float startAlpha = flashAlpha;

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