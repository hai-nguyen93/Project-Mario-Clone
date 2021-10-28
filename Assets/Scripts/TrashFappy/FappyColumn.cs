using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FappyColumn : MonoBehaviour
{
    [Tooltip("The size of the empty space between upper and lower columns.")]
    public float width = 2f;
    public GameObject upperColumn;
    public GameObject lowerColumn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FappyPlayerController player = collision.GetComponent<FappyPlayerController>();
        player?.Score();
    }
}
