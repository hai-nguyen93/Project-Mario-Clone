using UnityEngine;

// Damage Player if hit
public class EnemyHitBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var pc = collision.attachedRigidbody.GetComponent<PlayerController>();
        if (pc)
        {
            if (pc.godMode)
                transform.root.GetComponent<EnemyBase>().Burn();
            else
                pc.Damage();
        }
    }
}
