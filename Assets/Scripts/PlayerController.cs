using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public BoxCollider2D boxCollider;

    [Header("Movement Stats")]
    public float acceleration = 3f; 
    public float slideAccel = 2f; // acceleration for slow down
    public float xVelocity = 0f;
    public float maxSpeed = 3f;
    public float jumpPower = 10f;
    public LayerMask platformLayer;
    public float extraHeight = 0.1f; // to check ground


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        xVelocity = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();

        // Jump
        if (IsGrounded() && Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
    }

    void UpdateMovement()
    {
        float xDirection = Input.GetAxisRaw("Horizontal");
        float t = Time.deltaTime;
        if (xDirection > 0.5f) // move right
        {
            xVelocity += (acceleration * t);
            if (xVelocity >= maxSpeed) xVelocity = maxSpeed;
        }
        else if (xDirection < -0.5f) // move left
        {
            xVelocity -= (acceleration * t);
            if (xVelocity <= -maxSpeed) xVelocity = -maxSpeed;
        }
        else // slow -> stop
        {
            if (xVelocity > 0)
            {
                xVelocity -= (slideAccel * t);
                if (xVelocity < 0f) xVelocity = 0f;
            }
            if (xVelocity < 0)
            {
                xVelocity += (slideAccel * t);
                if (xVelocity > 0f) xVelocity = 0f;
            }
        }
        transform.position += new Vector3(xVelocity * t, 0, 0);
    }

    public bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y + extraHeight, platformLayer);
        Debug.DrawRay(boxCollider.bounds.center, Vector2.down * (boxCollider.bounds.extents.y + extraHeight), Color.green);
        return hit.collider != null;
    }

    public void Jump() // simple jump, will fix later
    {
        rb.velocity = Vector2.up * jumpPower;
    }
}
