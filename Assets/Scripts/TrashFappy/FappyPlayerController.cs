using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FappyPlayerController : MonoBehaviour
{
    // Movement settings
    [Header("Movement Settings")]
    public float baseFapPower = 10f;
    private float fapPower;
    [Tooltip("Euler degree")] public float maxRotationSpeed = 1080f;
    [Tooltip("Euler degree")] public float rotationAccel = 720f;
    private float rotationSpeed;

    // Objects to reference
    [Header("Reference Obj")]
    [SerializeField] private Transform _rotatingbody;
    private Rigidbody2D rb;
    [SerializeField] private ParticleSystem _thruster;
    private ParticleSystem.EmissionModule em;

    // Player events
    [Header("Events")]
    public GameEvent playerScores;
    public GameEvent playerDies;

    // PlayerController's properties
    private bool isDead = false;
    public bool IsDead { get => isDead; }
    private Vector3 startPos;


    #region Unity's Monobehaviour's functions
    ///////////////////////////////////////////////////////////////////////
    /// Unity's monobehaviour's functions
    ///////////////////////////////////////////////////////

    private void Start()
    {
        startPos = transform.position;
        fapPower = baseFapPower;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        isDead = true;
        rotationSpeed = 0f;
        em = _thruster.emission;
        em.enabled = false;
    }

    private void Update()
    {
        if (!isDead)
        {
            if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Space))
            {
                rb.velocity = Vector2.zero;
                em.enabled = false;
            }

            // Rotate square faster if player is moving up (velocity.y > 0); otherwise, slow down rotate speed
            if (rb.velocity.y > 0)
                rotationSpeed += (rotationAccel * Time.deltaTime);
            else
                rotationSpeed -= (rotationAccel * Time.deltaTime);

            // Rotate square
            rotationSpeed = Mathf.Clamp(rotationSpeed, 0, maxRotationSpeed);
            _rotatingbody.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space))
            {
                rb.velocity = new Vector2(0, fapPower);
                em.enabled = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }    
    }

    //////////////////////////////////////////////////////////////////////////
    #endregion


    #region Player's functions
    //////////////////////////////////////////////////////////
    /// Player's functions
    ///////////////////////////////////////////////////////

    public void Die()
    {
        playerDies.Raise();
        isDead = true;
        rb.isKinematic = true;
    }

    public void Restart()
    {
        transform.position = startPos;
        _rotatingbody.transform.rotation = Quaternion.identity;
        rb.isKinematic = false;
        isDead = false;
    }

    public void SpeedUp(FappyGameController controller)
    {
        fapPower = (1 + controller.playerSpeedModifier) * fapPower;
    }

    //////////////////////////////////////////////////////////
    #endregion


    #region Inpsector Utility
    ///////////////////////////////////////////////////////
    // Inspector Utility
    /////////////////////////////////////////////////////
    private void OnValidate()
    {
        fapPower = Mathf.Abs(fapPower);
    }
    #endregion
}
