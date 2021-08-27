using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : InteractableTile
{
    [Header("Box Settings")]
    public bool hidden = false;
    public Sprite hiddenBoxSprite;
    public Sprite emptyBoxSprite;

    [SerializeField] private GameObject itemToSpawn;


    // Start is called before the first frame update
    void Start()
    {

    }
    public override void OnInteract(PlayerController pc)
    {
        //TODO 
        return;
    }
}
