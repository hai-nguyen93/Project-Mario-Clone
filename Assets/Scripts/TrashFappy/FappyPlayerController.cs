using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FappyPlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float fapPower = 10f;
    [Tooltip("Euler degree")] public float maxRotationSpeed = 1080f;
    [Tooltip("Euler degree")] public float rotationAccel = 720f;
    private float rotationSpeed;

    [Header("Reference Obj")]
    [SerializeField] private Transform _rotatingbody;
    private Rigidbody2D rb;
    [SerializeField] private ParticleSystem _thruster;
    private ParticleSystem.EmissionModule em;

    // PlayerController's properties
    private bool isDead = false;
    public bool IsDead { get => isDead; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isDead = false;
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

    private void OnValidate()
    {
        fapPower = Mathf.Abs(fapPower);
    }
}
