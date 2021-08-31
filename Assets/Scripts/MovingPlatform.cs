using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovingPattern { pingpong, cycle, reset}

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] public List<Transform> path;
    private List<Vector2> points;
    public MovingPattern pattern = MovingPattern.pingpong;
    public float speed = 2f;
    private int currIndex = 0;
    private Vector2 destination;
    private Rigidbody2D rb;
    private PlayerController attachedPlayer;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize points
        if (path.Count <= 0)
        {
            points = new List<Vector2>(1);
            points.Add(transform.position);
        }
        else
        {
            points = new List<Vector2>(path.Count);
            for (int i = 0; i < path.Count; ++i)
            {
                points.Add((Vector2)path[i].position);
            }
        }

        currIndex = 0;
        rb = GetComponent<Rigidbody2D>();
        destination = points[0];
    }

    private void Update()
    {
        if (points.Count <= 1) return;

        if ((Vector2)transform.position == destination) // reached destination
        {
            if (currIndex < points.Count - 1) // not reach the end yet
            {
                destination = points[++currIndex];
            }
            else // reached the end
            {
                currIndex = 0;
                switch (pattern)
                {
                    case MovingPattern.pingpong:
                        points.Reverse();
                        destination = points[++currIndex];
                        break;

                    case MovingPattern.cycle:
                        destination = points[0];
                        break;

                    case MovingPattern.reset:
                        DetachPlayer();
                        transform.position = points[0];
                        destination = points[++currIndex];
                        break;
                }
            }
        }
        else //((Vector2)transform.position != destination)
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var hit = collision.collider.attachedRigidbody.GetComponent<PlayerController>();
        if (hit && hit.onGround)
        {
            attachedPlayer = hit;
            attachedPlayer.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.attachedRigidbody.CompareTag("Player"))
        {
            DetachPlayer();
        }
    }

    public void DetachPlayer()
    {
        if (attachedPlayer)
        {
            attachedPlayer.transform.SetParent(null);
            attachedPlayer = null;
        }
    }
}
