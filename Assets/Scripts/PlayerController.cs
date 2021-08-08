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
    public float xMaxSpeed = 3f;

    [Header("Jump Stats")]
    public float jumpPower = 10f;
    [Range(0f, 1f)] public float jumpModifier = 0.15f;
    public float jumpButtonDelay = 0.1f;
    private float jumpTimer;
    public LayerMask platformLayer;
    public float raycastExtraHeight = 0.1f; // to check ground


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        xVelocity = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Jump
        if (Input.GetKeyDown(KeyCode.W))
        {
            jumpTimer = Time.time + jumpButtonDelay;
        }
        if (Input.GetKeyUp(KeyCode.W) && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpModifier);
        }
        if (IsGrounded() && jumpTimer > Time.time)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        // move X direction
        float xDirection = Input.GetAxisRaw("Horizontal");
        rb.AddForce(Vector2.right * xDirection * acceleration);

        if (Mathf.Abs(rb.velocity.x) > xMaxSpeed)
        {
            rb.velocity = new Vector2( Mathf.Sign(rb.velocity.x) * xMaxSpeed, rb.velocity.y);
        }
        xVelocity = rb.velocity.x;
    }

    public bool IsGrounded()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, 0, 0), Vector2.down, boxCollider.bounds.extents.y + raycastExtraHeight, platformLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x, 0, 0), Vector2.down, boxCollider.bounds.extents.y + raycastExtraHeight, platformLayer);
        Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, 0, 0), Vector2.down * (boxCollider.bounds.extents.y + raycastExtraHeight), Color.green);
        Debug.DrawRay(boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x, 0, 0), Vector2.down * (boxCollider.bounds.extents.y + raycastExtraHeight), Color.green);
        return (hitLeft.collider != null) || (hitRight.collider != null);
    }

    public void Jump() // simple jump, will fix later
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }
}
