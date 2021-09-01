using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 100;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var hit_rb = other.attachedRigidbody;
        if (hit_rb && hit_rb.CompareTag("Player"))
        {
            GameController.instance.AddScore(value);
            Destroy(gameObject);
        }
    }
}
