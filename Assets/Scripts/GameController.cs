using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    }

    private void Start()
    {
        UpdateScoreText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void PauseGame()
    {

    }
}
