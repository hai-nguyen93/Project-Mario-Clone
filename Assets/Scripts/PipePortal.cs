using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePortal : MonoBehaviour
{
    public Transform destination;
    public bool isDestinationPipe;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var pc = collision.attachedRigidbody.GetComponent<PlayerController>();
            if (Input.GetAxisRaw("Vertical") < -0.5f && pc.onGround)
            {
                pc.PipeTeleport(destination, isDestinationPipe);
            }
        }
    }
}
