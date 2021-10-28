using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FappyScollingColumns : MonoBehaviour
{
    [Tooltip("The distance bewtween 2 columns")] public float columnDistance = 6.5f;
    private float speed;
    public FappyGameController gameController;
    private Vector2 cameraBottomLeft;

    private void Start()
    {
        cameraBottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
    }

    void Update()
    {
        if (!gameController.gameOver)
        {
            foreach (Transform child in transform)
            {
                // if child is out of screen -> reposition
                if (child.position.x < cameraBottomLeft.x)
                {
                    float _y = Random.Range(gameController.columnMinHeight, gameController.columnMaxHeight);
                    float newWidth = Random.Range(gameController.columnMinWidth, gameController.columnMaxWidth);
                    Vector3 newPos = new Vector3(-cameraBottomLeft.x, _y, 0);
                    child.GetComponent<FappyColumn>().Reposition(newPos, newWidth);
                }

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
        for (int i = 0; i < transform.childCount; ++i)
        {
            float _x = 3;
            float _y = Random.Range(gameController.columnMinHeight, gameController.columnMaxHeight);
            float newWidth = Random.Range(gameController.columnMinWidth, gameController.columnMaxWidth);
            transform.GetChild(i).GetComponent<FappyColumn>().Reposition(new Vector3(_x + columnDistance * i, _y, 0), newWidth);
        }
    }
}
