using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public BoxCollider2D smallCollider;
    public BoxCollider2D bigCollider;
    private BoxCollider2D currCollider;
    

    [Header("Player Settings")]
    public bool isDead = false;
    public int playerLevel = 1; // 1=small; 2=big; 3=fire
    public bool facingRight = true;
    public ParticleSystem dust;
    public Transform firePoint;
    public PlayerBullet bulletPrefab;

    [Header("Movement Stats")]
    public float acceleration = 3f;
    public float walkSpeed = 5f;
    public float sprintSpeed = 7f;
    public float xMaxSpeed;

    [Header("Jump Stats")]
    public bool onGround = false;
    public float jumpPower = 10f;
    [Range(0f, 1f)] public float jumpModifier = 0.15f;
    public float jumpButtonDelay = 0.1f;
    private float jumpTimer;
    public LayerMask platformLayer;
    public float raycastExtraHeight = 0.1f; // to check ground
    [Range(0f, 1f)] public float rayCastXRelativeOffset = 0.9f;  // to prevent collision at corner
    private float raycastXOffset;

    private void Awake()
    {
        currCollider = smallCollider;
    }

    // Start is called before the first frame update
    void Start()
    {
        raycastXOffset = currCollider.bounds.extents.x * rayCastXRelativeOffset;
        rb = GetComponent<Rigidbody2D>();
        xMaxSpeed = walkSpeed;
        onGround = IsGrounded();
        playerLevel = 1;

        // Set collider
        if (playerLevel == 1)
            currCollider = smallCollider;
        else
            currCollider = bigCollider;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player is grounded
        onGround = IsGrounded();

        // Sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (onGround)
            {
                xMaxSpeed = sprintSpeed;
            }
        }
        else
        {
            xMaxSpeed = walkSpeed;
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.W))
        {
            jumpTimer = Time.time + jumpButtonDelay;
        }
        if (Input.GetKeyUp(KeyCode.W) && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpModifier);
        }
        if (onGround && jumpTimer > Time.time)
        {
            Jump();
        }

        // Fire
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireBullet();
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
        if ((xDirection > 0 && !facingRight) || (xDirection < 0 && facingRight))
        {
            Flip(!facingRight);
        }
    }

    public bool IsGrounded()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(currCollider.bounds.center - new Vector3(raycastXOffset, 0, 0), Vector2.down, currCollider.bounds.extents.y + raycastExtraHeight, platformLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(currCollider.bounds.center + new Vector3(raycastXOffset, 0, 0), Vector2.down, currCollider.bounds.extents.y + raycastExtraHeight, platformLayer);
        Debug.DrawRay(currCollider.bounds.center - new Vector3(raycastXOffset, 0, 0), Vector2.down * (currCollider.bounds.extents.y + raycastExtraHeight), Color.green);
        Debug.DrawRay(currCollider.bounds.center + new Vector3(raycastXOffset, 0, 0), Vector2.down * (currCollider.bounds.extents.y + raycastExtraHeight), Color.green);
        return (hitLeft.collider != null) || (hitRight.collider != null);
    }

    public void Jump() // simple jump, will fix later
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }
    
    public void FireBullet()
    {
        PlayerBullet b = Instantiate<PlayerBullet>(bulletPrefab, firePoint.position, Quaternion.identity);
        b.SetDirection(facingRight);
    }

    public Vector2 GetTopCenterCollisionPoint()
    {
        return new Vector2(currCollider.bounds.center.x, currCollider.bounds.center.y + currCollider.bounds.extents.y);
    }

    public void Flip(bool right)
    {
        facingRight = right;
        transform.rotation = Quaternion.Euler(0, right ? 0 : 180, 0);
        CreateDust();
    }

    public void CreateDust()
    {
        dust.Play();
    }

    public void PipeTeleport(Transform dest, bool isDestinationPipe)
    {
        StartCoroutine(CoroutinePipeTeleport(dest.position, 0.5f));        
    }

    IEnumerator CoroutinePipeTeleport(Vector2 dest, float duration)
    {
        rb.isKinematic = true;
        currCollider.enabled = false;
        Vector2 startPos = transform.position;

        float t = duration;
        while (t > 0)
        {
            transform.position = Vector2.Lerp(startPos, startPos + new Vector2(0, -1), (duration - t) / duration) ;
            t -= Time.deltaTime;
            yield return null;
        }

        transform.position = dest;
        rb.isKinematic = false;
        currCollider.enabled = true;
    }
}
