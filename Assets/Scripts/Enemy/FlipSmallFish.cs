using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSmallFish : GameBehaviour
{
    private NewFish fishParent;
    private SpriteRenderer fishSprite;
    private SpriteRenderer smallFishSprite;

    void Start()
    {
        fishParent = transform.parent.GetComponent<NewFish>();
        fishSprite = fishParent.gameObject.GetComponent<SpriteRenderer>();
        smallFishSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fishSprite.flipX)
        {
            smallFishSprite.flipX = true;
        }
        else
            smallFishSprite.flipX = false;

        if (fishSprite.flipY)
            smallFishSprite.flipY = true;
        else 
            smallFishSprite.flipY = false;

        if (fishSprite.enabled == false)
            smallFishSprite.enabled = false;
        else
            smallFishSprite.enabled = true;
    }
}
