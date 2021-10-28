using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FappyGameController : MonoBehaviour
{
    public int score;
    public bool gameOver = false;

    [Header("Game Settings")]
    public float playerSpeedModifier = 0.1f;
    public float baseScrollingSpeed = 2f;
    public float scrollingSpeedModifier = 0.125f;
    public float scrollingSpeed;
    [Tooltip("Speed up game(level) after x columns")] public int speedUpRate = 10;
    public int level = 1;
    public float columnMinHeight = -4f;
    public float columnMaxHeight = 4.5f;
    public float columnMinWidth = 2f;
    public float columnMaxWidth = 4f;

    [Header("UI Items")]
    public TextMeshProUGUI scoreText;
    public GameObject startText;
    public GameObject gameOverText;

    [Header("Events")]
    public GameEvent speedUp;
    public GameEvent restart;

    public void Start()
    {
        ResetGame();
        gameOver = true;
        startText.SetActive(true);
    }

    public void Update()
    {
        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
            {
                ResetGame();
                restart.Raise();
            }
        }
    }

    public void GameOver()
    {
        gameOver = true;
        gameOverText.SetActive(true);
    }

    public void ResetGame()
    {
        level = 1;
        scrollingSpeed = baseScrollingSpeed;
        score = 0;
        gameOver = false;
        UpdateScore();
        gameOverText.SetActive(false);
        startText.SetActive(false);
    }

    public void PlayerScore()
    {
        if (gameOver) return;

        ++score;
        UpdateScore();

        // speed up after each 10 columns
        if ((score / speedUpRate) >= level)
        {
            ++level;
            scrollingSpeed *= (1 + scrollingSpeedModifier);
            speedUp.Raise();
        }
    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    private void OnValidate()
    {
        speedUpRate = Mathf.Abs(speedUpRate);
        playerSpeedModifier = Mathf.Abs(playerSpeedModifier);
        scrollingSpeedModifier = Mathf.Abs(scrollingSpeedModifier);
    }
}
