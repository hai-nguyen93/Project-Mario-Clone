using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBox : ItemBox
{
    public Sprite coinSprite;
    public int coinValue = 100;
    public int numberOfCoins = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnInteract(PlayerController pc)
    {
        if (numberOfCoins > 0)
        {
            GameController.instance.AddScore(coinValue);
            --numberOfCoins;
            if (numberOfCoins == 0)
            {
                GetComponent<SpriteRenderer>().sprite = emptyBoxSprite;
            }
        }
    }
}
