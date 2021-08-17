using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public float xSpeed = 7f;
    private float xVel;
    public float yForce = 3f;
    public float timeToLive = 3f;
    private float timer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        xVel = xSpeed;
        timer = timeToLive;        
    }

    private void Start()
    {
        rb.velocity = new Vector2(xVel, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(timer <= 0f)
        {
            Destroy(gameObject);
        }
        timer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(xVel, rb.velocity.y);    
    }

    public void SetDirection(bool facingRight)
    {
        xVel = facingRight ? xSpeed : -xSpeed;
        rb.velocity = new Vector2(xVel, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            var hitNormal = collision.GetContact(0).normal;
            if (hitNormal.y > 0.5f)
            {
                rb.velocity = new Vector2(rb.velocity.x, yForce);
                return;
            }          
        }
        Destroy(gameObject);
    }
}