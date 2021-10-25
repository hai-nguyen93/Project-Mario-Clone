using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for tiles that player interacts with from below (brick, item box, ...)
public class InteractableTile : MonoBehaviour
{
    protected BoxCollider2D bc;
    [Range(0f, 1f)] public float hitXRelativeOffset = 0.8f;
    protected float hitXOffset;

    private void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        hitXOffset = bc.bounds.extents.x * hitXRelativeOffset;
    }

    public virtual void OnInteract(PlayerController pc)
    {
        Debug.Log("parent's OnInteract()");
        return;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 hitPosition = collision.gameObject.GetComponent<PlayerController>().GetTopCenterCollisionPoint();
            if (hitPosition.y < bc.bounds.center.y && hitPosition.x <= (bc.bounds.center.x + hitXOffset) && hitPosition.x >= (bc.bounds.center.x - hitXOffset))
            {
                OnInteract(collision.gameObject.GetComponent<PlayerController>());
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        BoxCollider2D b = this.GetComponent<BoxCollider2D>();
        float xOffset = b.bounds.extents.x * hitXRelativeOffset;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(b.bounds.center + new Vector3(xOffset, 0, 0), b.bounds.center + new Vector3(xOffset, -(b.bounds.extents.y + 0.1f), 0));
        Gizmos.DrawLine(b.bounds.center + new Vector3(-xOffset, 0, 0), b.bounds.center + new Vector3(-xOffset, -(b.bounds.extents.y + 0.1f), 0));
    }
}
