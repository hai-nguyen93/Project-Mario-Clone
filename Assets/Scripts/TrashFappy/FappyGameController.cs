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
    public float scrollingSpeedModifier = 0.15f;

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
            if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Space))
            {
                restart.Raise();
                ResetGame();
            }
        }
    }

    public void ResetGame()
    {
        score = 0;
        gameOver = false;
        UpdateScore();
        gameOverText.SetActive(false);
        startText.SetActive(false);
    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        gameOver = true;
        gameOverText.SetActive(true);
    }
}
