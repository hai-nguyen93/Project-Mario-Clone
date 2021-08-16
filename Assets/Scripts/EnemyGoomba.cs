using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGoomba : EnemyBase
{
    private Rigidbody2D rb;
    public bool facingRight = false;
    public float speed = 3f;
    private float xVel;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        xVel = -speed;
    }


    private void FixedUpdate()
    {
        rb.velocity = new Vector2(xVel, rb.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Flip()
    {
        xVel = -xVel;
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 180 : 0, 0);
    }
}
