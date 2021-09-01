using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKoopaTroopa : EnemyBase
{
    public bool facingRight = false;

    public float speed = 2.5f;
    private float xVel;

    public float forwardDetectRange = 0.05f;
    public LayerMask layerToDetect;

    [Header("KoopaTroopa properties")]
    public bool isShell = false;
    public float shellDuration = 5f; // how long does it stay in shell
    public float shellTimer;
    public float shellSpeed = 5f;
    public GameObject shell;
    public GameObject normalSelf;

    protected override void Awake()
    {
        shell.SetActive(false);
        normalSelf.SetActive(true);
        SetupComponents(normalSelf);
        rb = GetComponent<Rigidbody2D>();
        sr.material = m_default;
    }

    void Start()
    {
        isShell = false;
        shellTimer = shellDuration;
        xVel = facingRight ? speed : -speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        if (!isShell || (isShell && xVel != 0))
        {
            var hit = Physics2D.Raycast(bc.bounds.center, new Vector2(facingRight ? 1 : -1, 0), bc.bounds.extents.x + 0.05f, layerToDetect);
            if (hit)
            {
                Flip();
            }
        }

        // update shell timer
        if (isShell && xVel == 0)
        {
            if (shellTimer <= 0)
            {
                SwitchToNormal();
            }
            shellTimer -= Time.deltaTime;            
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;
     
        rb.velocity = new Vector2(xVel, rb.velocity.y);
    }

    public override void Flip()
    {
        xVel = -xVel;
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 180 : 0, 0);
    }

    void SetupComponents(GameObject objToGet)
    {
        sr = objToGet.GetComponent<SpriteRenderer>();
        bc = objToGet.GetComponent<BoxCollider2D>();
    }

    public new void OnCollisionEnter2D(Collision2D collision)
    {
        var pc = collision.collider.attachedRigidbody?.GetComponent<PlayerController>();

        if (pc)
        {
            if (collision.GetContact(0).normal.y < -0.5f) // player hit from above
            {
                pc.Jump(pc.jumpPower / 2);

                if (!isShell)
                {
                    SwitchToShell();
                }
                else
                {
                    KickShell(pc.GetTopCenterCollisionPoint().x);
                }
            }
            else if (Mathf.Abs(collision.GetContact(0).normal.x) > 0.5f) // player hit from sides
            {
                if (xVel == 0) KickShell(pc.GetTopCenterCollisionPoint().x);
                else
                {
                    HitPlayer(pc);
                }
            }
            else if (collision.GetContact(0).normal.y > 0.5f) // player hit from below
                HitPlayer(pc);
        }      
    }
    
    public void HitPlayer(PlayerController pc)
    {
        if (pc.godMode)
            Burn();
        else
            pc.Damage();
    }

    public void KickShell(float playerX)
    {
        xVel = (playerX <= transform.position.x) ? shellSpeed : -shellSpeed;
        facingRight = (xVel > 0) ? true : false;
        transform.rotation = Quaternion.Euler(0, facingRight ? 180 : 0, 0);
    }

    public void SwitchToShell()
    {
        isShell = true;
        shellTimer = shellDuration;
        rb.velocity = Vector2.zero;
        xVel = 0;
        normalSelf.SetActive(false);
        shell.SetActive(true);
        SetupComponents(shell);
    }

    public void SwitchToNormal()
    {
        isShell = false;
        shellTimer = shellDuration;
        rb.velocity = Vector2.zero;
        normalSelf.SetActive(true);
        shell.SetActive(false);
        SetupComponents(normalSelf);
        xVel = facingRight ? speed : -speed;
    }

    private void OnValidate()
    {
        if (speed < 0) speed = -speed;
        if (shellSpeed < 0) shellSpeed = -shellSpeed;
    }
}
