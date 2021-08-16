using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameKillZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var t = collision.transform;
        while (t.parent != null) t = t.parent;
        Destroy(t.gameObject);
    }
}
