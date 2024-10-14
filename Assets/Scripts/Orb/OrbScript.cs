using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbScript : MonoBehaviour
{
    public OrbManager orbManager;
    public bool isLeafOrb;
    public OrbDrop orbDrop;



    private void Start()
    {
        orbManager = FindObjectOfType<OrbManager>();
        orbDrop = GameObject.Find("DropZone").GetComponent<OrbDrop>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == orbManager.playerLeaf)
        {

            if (isLeafOrb)
            {
                orbManager.LeafLeafOrbCollision();
            }
            else
            {
                orbManager.LeafSeaOrbCollision();
            }
        }
        else if (collision.gameObject == orbManager.playerSea)
        {
            if (isLeafOrb)
            {
                orbManager.SeaLeafOrbCollision();
            }
            else
            {
                orbManager.SeaSeaOrbCollision();
            }
        }
    }

    public void AfterLeafAnimationTrigger()
    {
        orbDrop.AfterLeafAnimation();
    }

    public void AfterSeaAnimationTrigger()
    {
        orbDrop.AfterSeaAnimation();
        print("afterseaAnimation");
    }


}
