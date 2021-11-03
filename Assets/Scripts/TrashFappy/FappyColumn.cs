using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FappyColumn : MonoBehaviour
{
    [Tooltip("The size of the empty space between upper and lower columns.")]
    public float width = 2f;
    public bool special = false;
    public BoxCollider2D upperColumn;
    public BoxCollider2D lowerColumn;
    public GameObject prefab_scoreParticle;
    private ParticleSystem scoreParticle;

    private void Start()
    {
        scoreParticle = Instantiate(prefab_scoreParticle, transform).GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FappyPlayerController player = collision.GetComponent<FappyPlayerController>();
        if (player)
        {
            scoreParticle.transform.position = collision.transform.position;
            scoreParticle.Play();
            player.Score(special);
        }
    }

    public void Reposition(Vector3 pos, float width, bool specialColumn)
    {
        transform.position = pos + new Vector3(upperColumn.bounds.size.x, 0, 0);
        SetupColumn(width);
        if (specialColumn)
        {
            special = true;
            upperColumn.GetComponent<SpriteRenderer>().material.SetFloat("_Special", 1);
            lowerColumn.GetComponent<SpriteRenderer>().material.SetFloat("_Special", 1);
        }
        else
        {
            special = false;
            upperColumn.GetComponent<SpriteRenderer>().material.SetFloat("_Special", 0);
            lowerColumn.GetComponent<SpriteRenderer>().material.SetFloat("_Special", 0);
        }
    }

    public void SetupColumn(float width)
    {
        this.width = width;
        upperColumn.transform.localPosition = new Vector3(0, (width + upperColumn.bounds.size.y) / 2f, 0);
        lowerColumn.transform.localPosition = new Vector3(0, -(width + lowerColumn.bounds.size.y) / 2f, 0);
    }

    public void OnDrawGizmosSelected()
    {
        upperColumn.transform.localPosition = new Vector3(0, (width + upperColumn.bounds.size.y) / 2f, 0);
        lowerColumn.transform.localPosition = new Vector3(0, -(width + lowerColumn.bounds.size.y) / 2f, 0);
    }
}
