using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FappyPlayerController : MonoBehaviour
{
    // Movement settings
    [Header("Movement Settings")]
    public float baseFapPower = 10f;
    public float fapPower;
    [Tooltip("Euler degree")] public float maxRotationSpeed = 1080f;
    [Tooltip("Euler degree")] public float rotationAccel = 720f;
    private float rotationSpeed;
    public float dissolveDuration = 1f;

    // Objects to reference
    [Header("Reference Obj")]
    [SerializeField] private Transform _rotatingbody;
    private Rigidbody2D rb;
    [SerializeField] private ParticleSystem _thruster;
    private ParticleSystem.EmissionModule em;
    private FappyAudioController audioController;
    public GameObject shockwave;

    // Player events
    [Header("Events")]
    public GameEvent playerScores;
    public GameEvent specialScore;
    public GameEvent playerDies;

    // PlayerController's properties
    private bool isDead = false;
    public bool IsDead { get => isDead; }
    private Vector3 startPos;
    private Material m_Dissolve;


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
        m_Dissolve = _rotatingbody.GetComponent<SpriteRenderer>().material;
        m_Dissolve.SetFloat("_Fade", 1);
        isDead = true;
        rotationSpeed = 0f;
        em = _thruster.emission;
        em.enabled = false;

        audioController = FindObjectOfType<FappyAudioController>();
        shockwave.SetActive(false);
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
        isDead = true;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        em.enabled = false;
        audioController?.PlaySFX("Die");
        StartCoroutine(DieCoroutine());
    }

    public void Restart()
    {
        shockwave.SetActive(false);
        fapPower = baseFapPower;
        transform.position = startPos;
        _rotatingbody.transform.rotation = Quaternion.identity;
        m_Dissolve.SetFloat("_Fade", 1);
        rb.isKinematic = false;
        isDead = false;
    }

    public void Score(bool special)
    {
        if (!isDead)
        {
            if (!special)
                playerScores.Raise();
            else
                specialScore.Raise();
        }
    }

    public void SpeedUp(FappyGameController controller)
    {
        fapPower = (1 + controller.playerSpeedModifier) * fapPower;
    }

    public IEnumerator DieCoroutine()
    {
        float duration = dissolveDuration;
        float timer = duration;
        shockwave.SetActive(true);
        var m_shockwave = shockwave.GetComponent<SpriteRenderer>().material;
        while (timer >= 0)
        {
            float fade = Mathf.Lerp(0.7f, 0f, 1 - timer / duration);
            m_Dissolve.SetFloat("_Fade", fade);
            m_shockwave.SetFloat("_NormalizedTime", 1 - timer / duration);
            timer -= Time.deltaTime;
            yield return null;
        }
        shockwave.SetActive(false);
        playerDies.Raise();
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
