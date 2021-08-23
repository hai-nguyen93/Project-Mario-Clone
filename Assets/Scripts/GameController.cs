using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("Game Settings")]
    public float deadY = -3f; // kill player, enemies if y < deadY 

    [Header("Player's stats")]
    public int score = 0;
    public int lives = 3;


    [Header("UI items")]
    public TextMeshProUGUI scoreText;
    public ParticleSystem scoreParticle;
    public GameObject pauseScreen;
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

        // Initialize UI items
        pauseScreen.SetActive(false);
        isPaused = false;
        isInScreenTransition = false;
    }

    private void Start()
    {
        UpdateScoreText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isInScreenTransition)
        {
            if (!isPaused) 
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void AddScore(int value, Transform particlePos = null)
    {
        if (particlePos)
        {
            var p = Instantiate(scoreParticle, particlePos.position, Quaternion.identity);
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
