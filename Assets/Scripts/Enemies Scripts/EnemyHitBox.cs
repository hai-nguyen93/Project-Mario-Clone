using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Damage Player if hit
public class EnemyHitBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.CompareTag("Player"))
        {
            Debug.Log("Player: Ui daaa!!");
        }
    }
}
