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

    protected void Awake()
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
        StartCoroutine(BurnToDeath());
    }

    IEnumerator BurnToDeath()
    {
        sr.material = m_dissolve;
        float t = burnTime;
        float fade = 1f;
        while (t > 0)
        {
            fade = Mathf.Lerp(1f, 0f, (burnTime - t) / burnTime);
            sr.material.SetFloat("_Fade", fade);
            yield return null;
            t -= Time.deltaTime;
        }
        Die();
    }
}
