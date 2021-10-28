using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FappyScollingObj : MonoBehaviour
{
    private float speed;
    public FappyGameController gameController;

    void Update()
    {
        if (!gameController.gameOver)
        {
            foreach (Transform child in transform)
            {
                child.position += new Vector3(-speed * Time.deltaTime, 0, 0);
            }
        }
    }

    public void UpdateSpeed()
    {
        speed = gameController.scrollingSpeed;
    }

    public void ResetChildrenPosition()
    {
        foreach (Transform child in transform)
        {
            child.position = new Vector3(3, 0, 0);
        }
    }
}
