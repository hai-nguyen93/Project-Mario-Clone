using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("Game Settings")]
    public float deadY = -3f; // kill player, enemies if y < deadY 
    public float timeLimit = 100f; ///in seconds
    private float timer;

    [Header("Player's stats")]
    public int score = 0;
    public int lives = 3;


    [Header("UI items")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public ParticleSystem scoreParticlePrefab;
    public GameObject pauseScreen;
    public GameObject gameOverScreen;
    private bool isPaused = false;
    private bool isInScreenTransition = false;

   void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        score = 0;
        lives = 3;
        timer = timeLimit;

        // Initialize UI items
        pauseScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        isPaused = false;
        isInScreenTransition = false;
    }

    private void Start()
    {
        UpdateScoreText();
    }

    private void Update()
    {
        // update timer
        if (timer < 0)
        {
            if (!gameOverScreen.activeSelf)
                GameOver();
        }
        else
            timer -= Time.deltaTime;
        timeText.text = "Time: " + Mathf.RoundToInt(timer);

        if (Input.GetKeyDown(KeyCode.Escape) && !isInScreenTransition)
        {
            if (gameOverScreen.activeSelf)
            {
                // reset UI then restart game
                pauseScreen.SetActive(false);
                gameOverScreen.SetActive(false);
                isPaused = false;
                isInScreenTransition = false;
                timer = timeLimit;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                if (!isPaused)
                    PauseGame();
                else
                    ResumeGame();
            }
        }
    }

    public void AddScore(int value, Transform particlePos = null)
    {
        if (particlePos)
        {
            var p = Instantiate(scoreParticlePrefab, particlePos.position, Quaternion.identity);
            p.transform.parent = null;
            p.GetComponent<ParticleToUI>().destinationUI = scoreText.rectTransform;
        }
        score += value;
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseScreen.SetActive(true);
        StartCoroutine(FadePanel(0.5f, pauseScreen.GetComponent<Image>(), 0f, pauseScreen.GetComponent<Image>().color.a));
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        Destroy(FindObjectOfType<PlayerController>().gameObject);
        StartCoroutine(FadePanel(0.5f, gameOverScreen.GetComponent<Image>(), 0f, 0.75f));
    }

    IEnumerator FadePanel(float duration, Image panelToFade, float startAlpha, float finalAlpha)
    {
        isInScreenTransition = true;
        float t = duration;
        while(t > 0)
        {
            panelToFade.color = new Color(panelToFade.color.r, panelToFade.color.g, panelToFade.color.b, Mathf.Lerp(startAlpha, finalAlpha, (duration - t) / duration));
            t -= Time.unscaledDeltaTime;
            yield return null;
        }
        isInScreenTransition = false;
    }
}
