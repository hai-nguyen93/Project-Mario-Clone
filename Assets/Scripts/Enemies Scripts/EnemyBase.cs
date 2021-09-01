using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for enemies
public class EnemyBase : MonoBehaviour
{
    public int scoreValue = 100;
    public Material m_dissolve;
    public Material m_default;
    public float burnTime = 0.5f;
    public bool isDead = false;

    protected Rigidbody2D rb;
    protected BoxCollider2D bc;
    protected SpriteRenderer sr;

    protected virtual void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.material = m_default;
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Flip() { }

    public virtual void OnBulletHit()
    {
        Burn();
    }

    public virtual void Die()
    {
        GameController.instance.AddScore(scoreValue, transform);
        Destroy(gameObject);
    }

    public virtual void Burn()
    {
        isDead = true;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        bc.enabled = false;
        sr.material = m_dissolve;
        StartCoroutine(BurnToDeath());
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        var pc = collision.collider.attachedRigidbody?.GetComponent<PlayerController>();
        if (pc && collision.GetContact(0).normal.y < -0.5f)
        {            
            pc.Jump(pc.jumpPower / 2);
            OnHitHead();
        }
    }

    public virtual void OnHitHead() 
    {
        Die();
    }

    IEnumerator BurnToDeath()
    {
        float t = burnTime;
        float fade = 0.6f;
        while (t >= 0)
        {
            t -= Time.deltaTime;
            fade = Mathf.Lerp(0.7f, 0f, (burnTime - t) / burnTime);
            sr.material.SetFloat("_Fade", fade);
            yield return null;
        }
        Die();
    }
}
