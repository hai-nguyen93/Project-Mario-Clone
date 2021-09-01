using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndFlag : MonoBehaviour
{
    public float slideDownDuration = 2f;
    public float moveToDestDuration = 4f;
    public Transform destination;
    private PlayerController pc;
    private Rigidbody2D pc_rb;
    private BoxCollider2D bc;

    private void Start()
    {
        bc = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        pc = collision.attachedRigidbody?.GetComponent<PlayerController>();
        if (pc)
        {
            pc_rb = collision.attachedRigidbody;
            pc_rb.velocity = Vector2.zero;
            StartCoroutine(FinishLevel());
        }
    }

    IEnumerator FinishLevel()
    {
        GameController.instance.gameState = GameState.Finish;
        yield return SlideDown();
        yield return MoveToDest();
    }

    IEnumerator SlideDown()
    {
        pc_rb.isKinematic = true;
        float t = slideDownDuration;
        float destY = bc.bounds.center.y - bc.bounds.extents.y + pc.GetSpriteRenderer().bounds.extents.y;
        float startY = pc.transform.position.y;
        while (t >= 0)
        {
            pc.transform.position = new Vector2(pc.transform.position.x, Mathf.Lerp(startY, destY, (slideDownDuration - t) / slideDownDuration));
            t -= Time.deltaTime;
            yield return null;
        }
        pc_rb.isKinematic = false;
    }

    IEnumerator MoveToDest()
    {
        float t = moveToDestDuration;
        float startX = pc.transform.position.x;
        while (t >= 0)
        {
            pc.transform.position = new Vector2(Mathf.Lerp(startX, destination.position.x, (moveToDestDuration - t) / moveToDestDuration), pc.transform.position.y);
            t -= Time.deltaTime;
            yield return null;
        }
        pc_rb.velocity = Vector2.zero;
    }
}
