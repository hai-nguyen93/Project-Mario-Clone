using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGoomba : EnemyBase
{
    public bool facingRight = false;
    public float speed = 3f;
    private float xVel;
    public Transform forwardDetection;
    public float forwardDetectRange = 0.05f;
    public LayerMask layerToDetect;


    // Start is called before the first frame update
    void Start()
    {
        xVel = -speed;
    }


    private void FixedUpdate()
    {
        if (!isDead)
            rb.velocity = new Vector2(xVel, rb.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        // Change direction if hit something in front of this enemy
        Debug.DrawLine(forwardDetection.position, forwardDetection.position + new Vector3((facingRight ? 1 : -1) * forwardDetectRange, 0, 0));
        var hit = Physics2D.Raycast(forwardDetection.position, new Vector2(facingRight ? 1 : -1, 0), forwardDetectRange, layerToDetect);
        if (hit)
        {
            Flip();
        }
    }

    public override void Flip()
    {
        xVel = -xVel;
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 180 : 0, 0);
    }
}
