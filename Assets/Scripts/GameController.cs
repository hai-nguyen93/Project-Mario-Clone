using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("Game Settings")]
    public float deadY = -1f; // kill player, enemies if y < deadY 

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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addScore(int value)
    {
        score += value;
    }
}
