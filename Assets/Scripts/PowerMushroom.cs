using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerMushroom : MonoBehaviour
{
    public int scoreValue = 100;
    public float speed = 2f;
    private float xVel;
    private Rigidbody2D rb;
    public Transform forwardDetection;
    public float forwardDetectionRange = 0.05f;
    public LayerMask layersToDetect;

    private void Awake()
    {
        xVel = speed;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(forwardDetection.position, forwardDetection.position + new Vector3(Mathf.Sign(xVel)*forwardDetectionRange,0,0));
        var hit = Physics2D.Raycast(forwardDetection.position, new Vector2(Mathf.Sign(xVel), 0), forwardDetectionRange, layersToDetect);
        if (hit)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(xVel, rb.velocity.y);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var pc = collision.gameObject.GetComponentInParent<PlayerController>();
            if (pc && pc.playerLevel < 2) pc.playerLevel = 2;
            GameController.instance.AddScore(scoreValue);
            Destroy(gameObject);
        }
    }

    public void Flip()
    {
        xVel = -xVel;
        transform.rotation = Quaternion.Euler(0, Mathf.Sign(xVel) == 1 ? 0 : 180, 0);
    }
}
