using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPiranhaPlant : EnemyBase
{
    private Vector3 startPos;
    private Vector3 endPos;
    public float moveDuration = 2f;
    public bool movingUp = false;

    [Header("Range for time delay between movement")]
    public float timeDelayMin = 1.75f;
    public float timeDelayMax = 3.5f;
    private float timeDelay;
    private float timer, t;

    // Start is called before the first frame update
    void Start()
    {
        timeDelay = Random.Range(timeDelayMin, timeDelayMax);
        timer = timeDelay;
        t = moveDuration;
        startPos = transform.position;
        endPos = startPos - new Vector3(0, GetComponent<SpriteRenderer>().bounds.size.y + 0.1f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        // Moving
        if (timer <= 0)
        {
            // finished moving
            if (t <= 0)
            {
                t = moveDuration;
                timer = Random.Range(timeDelayMin, timeDelayMax);

                // adjust position
                if (movingUp) transform.position = startPos;
                else transform.position = endPos;

                movingUp = !movingUp;
            }
            else // moving
            {
                // moving Up
                if (movingUp)
                    transform.position = Vector3.Lerp(endPos, startPos, (moveDuration - t) / moveDuration);
                else // moving down
                    transform.position = Vector3.Lerp(startPos, endPos, (moveDuration - t) / moveDuration);

                t -= Time.deltaTime;
            }
        }
        else
            timer -= Time.deltaTime;
    }
}
