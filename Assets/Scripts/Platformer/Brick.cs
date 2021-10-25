using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : InteractableTile
{
    public ParticleSystem breakPS;
    public float budgeDuration = 0.1f;
    private bool isBudging = false;

    private void Start()
    {
        isBudging = false;
    }

    public override void OnInteract(PlayerController pc)
    {
        if (pc.playerLevel == 1)
        {
            var hit = Physics2D.Raycast(bc.bounds.center, Vector2.up, bc.bounds.extents.y + 0.1f);
            if (!hit || hit.collider.gameObject.layer != LayerMask.NameToLayer("Platform") || hit.transform == transform)
            {
                if (!isBudging)
                    StartCoroutine(Budge());
            }
        }
        if (pc.playerLevel > 1)
        {
            Instantiate(breakPS, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    IEnumerator Budge()
    {
        Vector2 start = transform.position;
        Vector2 dest = start + new Vector2(0, 0.25f);
        isBudging = true;

        // move up
        float t = budgeDuration;
        while (t >= 0)
        {
            transform.position = Vector2.Lerp(start, dest, (budgeDuration - t) / budgeDuration);
            yield return null;
            t -= Time.deltaTime;
        }

        // move down
        t = budgeDuration;
        while (t >= 0)
        {
            transform.position = Vector2.Lerp(dest, start, (budgeDuration - t) / budgeDuration);
            yield return null;
            t -= Time.deltaTime;
        }

        transform.position = start;
        isBudging = false;
    }
}
