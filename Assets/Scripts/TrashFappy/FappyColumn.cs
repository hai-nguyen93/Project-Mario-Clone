using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FappyColumn : MonoBehaviour
{
    [Tooltip("The size of the empty space between upper and lower columns.")]
    public float width = 2f;
    public BoxCollider2D upperColumn;
    public BoxCollider2D lowerColumn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FappyPlayerController player = collision.GetComponent<FappyPlayerController>();
        player?.Score();
    }

    public void Reposition(Vector3 pos, float width)
    {
        transform.position = pos + new Vector3(upperColumn.bounds.size.x, 0, 0);
        SetupColumn(width);
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
