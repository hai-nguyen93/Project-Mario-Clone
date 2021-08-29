using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject smallMario;
    public GameObject bigMario;
    private BoxCollider2D currCollider;
    private SpriteRenderer sr;


    [Header("Player Settings")]
    public bool godMode = false; /// god mode when eating star
    public float godModeDuration = 10f;
    public bool undamagable = false; // i-frame after getting hit
    public float undamagableDuration = 1f;
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
        SetPlayerLevel(1);
    }

    // Start is called before the first frame update
    void Start()
    {
        raycastXOffset = currCollider.bounds.extents.x * rayCastXRelativeOffset;
        rb = GetComponent<Rigidbody2D>();
        xMaxSpeed = walkSpeed;
        godMode = false;
        undamagable = false;
        onGround = IsGrounded();      
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

        // God mode test
        if (Input.GetKeyDown(KeyCode.G) && !godMode)
        {
            EnterGodMode();
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

    public void Damage()
    {
        if (undamagable) return;

        if (playerLevel > 1)
        {
            SetPlayerLevel(1);
            StartCoroutine(IsBeingDamaged());
        }
        else
        {
            Debug.Log("Player dies");
        }
    }

    public void EnterGodMode()
    {
        StartCoroutine(IsInGodMode());
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

    public void SetPlayerLevel(int level)
    {
        playerLevel = level;
        UpdateMesh(); 
    }

    public void UpdateMesh()
    {
        if (playerLevel == 1)
        {
            smallMario.SetActive(true);
            bigMario.SetActive(false);
            currCollider = smallMario.GetComponent<BoxCollider2D>();
        }
        else // player level > 1
        {
            smallMario.SetActive(false);
            bigMario.SetActive(true);
            currCollider = bigMario.GetComponent<BoxCollider2D>();
        }

        sr = currCollider.gameObject.GetComponent<SpriteRenderer>();
    }

    public void PipeTeleport(Transform dest, bool isDestinationPipe)
    {
        StartCoroutine(CoroutinePipeTeleport(dest.position, 0.5f, isDestinationPipe));        
    }

    IEnumerator CoroutinePipeTeleport(Vector2 dest, float duration, bool isDestinationPipe)
    {
        rb.isKinematic = true;
        currCollider.enabled = false;
        //Vector2 startPos = transform.position;
        
        // Enter Pipe
        yield return EnterExitPipe(transform.position, transform.position + new Vector3(0, -1, 0), duration);


        // Exit Pipe
        if (isDestinationPipe)
        {
            yield return EnterExitPipe(dest - new Vector2(0, 1), dest, duration);
        }
        else
        {
            transform.position = dest;
        }
        rb.isKinematic = false;
        currCollider.enabled = true;
    }

    IEnumerator EnterExitPipe(Vector2 start, Vector2 end, float duration)
    {
        float t = duration;
        while (t > 0)
        {
            transform.position = Vector2.Lerp(start, end, (duration - t) / duration);
            t -= Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator IsBeingDamaged()
    {
        undamagable = true;
        float t = undamagableDuration;
        float flashingDuration = 0.15f;
        float flashingTimer = flashingDuration;
        while (t > 0)
        {
            if (flashingTimer < 0)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, (sr.color.a > 0.5f) ? 0f : 1f);
                flashingTimer = flashingDuration;
            }

            flashingTimer -= Time.deltaTime;
            t -= Time.deltaTime;
            yield return null;
        }

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        undamagable = false;
    }

    IEnumerator IsInGodMode()
    {
        godMode = true;
        float t = godModeDuration;
        float tintDuration = 0.25f;
        float tintTimer = tintDuration;
        Color originalColor = sr.color;
        Color.RGBToHSV(originalColor, out _, out _, out float v) ;
        float s = 1f;
        float h = 0f;
        float destH = 100f / 360f;
        while (t > 0)
        {
            if (tintTimer < -tintDuration) tintTimer = tintDuration;
            sr.color = Color.HSVToRGB(Mathf.Lerp(h, destH, (tintDuration - Mathf.Abs(tintTimer)) / tintDuration), s, v);

            tintTimer -= Time.deltaTime;
            t -= Time.deltaTime;
            yield return null;
        }

        sr.color = originalColor;
        godMode = false;
    }
}
