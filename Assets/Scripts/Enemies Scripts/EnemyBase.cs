using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for enemies
public class EnemyBase : MonoBehaviour
{
    public int scoreValue = 100;

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
        Die();
    }

    public virtual void Die()
    {
        GameController.instance.AddScore(scoreValue, transform);
        Destroy(gameObject);
    }
}
