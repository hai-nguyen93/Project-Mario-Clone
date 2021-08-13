using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 100;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody.gameObject.CompareTag("Player"))
        {
            GameController.instance.AddScore(value);
            Destroy(gameObject);
        }
    }
}
