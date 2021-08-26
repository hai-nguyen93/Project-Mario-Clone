using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum GameState { Playing, Paused}

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("Game Settings")]
    public GameState gameState; 
    public float deadY = -3f; // kill player, enemies if y < deadY 
    public float timeLimit = 100f; ///in seconds
    private float timer;

    [Header("UI items")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public ParticleSystem scoreParticlePrefab;
    public GameObject pauseScreen;
    public GameObject gameOverScreen;
    public GameObject loadScreen;
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
        StartCoroutine(StartScene());
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
                ResetLevel();
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
        GameData.instance.score += value;
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Score: " + GameData.instance.score.ToString();
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

    public void ResetLevel()
    {
        pauseScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        isPaused = false;
        timer = timeLimit;
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
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

    IEnumerator StartScene()
    {
        // Set up load screen
        Time.timeScale = 0f;
        loadScreen.SetActive(true);
        isInScreenTransition = true;
        
        // wait for screen transition
        yield return new WaitForSecondsRealtime(0.25f);
        float duration = 0.25f;
        float t = duration;
        while (t > 0)
        {
            loadScreen.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, (duration - t) / duration);
            t -= Time.unscaledDeltaTime;
            yield return null;
        }
        isInScreenTransition = false;

        // resume game
        Time.timeScale = 1f;
        loadScreen.SetActive(false);
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        // pause game, prepare load screen
        Time.timeScale = 0f;
        loadScreen.SetActive(true);
        isInScreenTransition = true;

        // transition to load screen
        float duration = 0.25f;
        float t = duration;
        while (t > 0)
        {
            loadScreen.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0.7f, 1f, (duration - t) / duration);
            t -= Time.unscaledDeltaTime;
            yield return null;
        }

        // load new scene in background
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        while (!op.isDone)
        {
            yield return null;
        }
        isInScreenTransition = false;
    }
}
