using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMatchSprites : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Continuously match the child's flip state with the parent's flip state
        SpriteRenderer parentSpriteRenderer = GetComponent<SpriteRenderer>();

        if (parentSpriteRenderer != null)
        {
            foreach (Transform child in transform)
            {
                SpriteRenderer childSpriteRenderer = child.GetComponent<SpriteRenderer>();
                if (childSpriteRenderer != null)
                {
                    childSpriteRenderer.flipX = parentSpriteRenderer.flipX;
                    childSpriteRenderer.flipY = parentSpriteRenderer.flipY;
                }
            }
        }
    }
}
