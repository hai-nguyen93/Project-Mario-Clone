using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerMushroom : MonoBehaviour
{
    public int scoreValue = 100;
    public float speed = 2f;
    private float xVel;
    private Rigidbody2D rb;
    public float xCollisionDetectionRadius = 0.05f;
    public LayerMask layersToCollide;
    private SpriteRenderer sr;

    private void Awake()
    {
        xVel = speed;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 centerLeft = new Vector2(sr.bounds.center.x - sr.bounds.extents.x, sr.bounds.center.y);
        Vector2 centerRight = new Vector2(sr.bounds.center.x + sr.bounds.extents.x, sr.bounds.center.y);
        Debug.DrawLine(centerLeft, centerLeft + new Vector2(xCollisionDetectionRadius, 0));
        Debug.DrawLine(centerRight, centerRight + new Vector2(xCollisionDetectionRadius, 0));

        if (Physics2D.OverlapCircle(centerLeft, xCollisionDetectionRadius, layersToCollide) ||
            Physics2D.OverlapCircle(centerRight, xCollisionDetectionRadius, layersToCollide))
        {
            xVel = -xVel;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(xVel, rb.velocity.y);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var pc = collision.gameObject.GetComponentInParent<PlayerController>();
            if (pc && pc.playerLevel < 2) pc.playerLevel = 2;
            GameController.instance.AddScore(scoreValue);
            Destroy(gameObject);
        }
    }
}
