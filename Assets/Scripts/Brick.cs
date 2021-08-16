using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : InteractableTile
{
    public ParticleSystem breakPS;

    public override void OnInteract()
    {
        Instantiate(breakPS, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
