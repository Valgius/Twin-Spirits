using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationChanges : MonoBehaviour
{
    public AnimatorOverrideController playerSeaOverrides;
    public AnimatorOverrideController playerLeafOverrides;

    [Header("Sea Glow Animations")]
    public AnimationClip PlayerDamageGlow;
    public AnimationClip seaDashGlow;
    public AnimationClip seaFallGlow;
    public AnimationClip seaIdleGlow;
    public AnimationClip seaJumpGlow;
    public AnimationClip seaLandGlow;
    public AnimationClip seaRunGlow;
    public AnimationClip seaSwimGlow;
    public AnimationClip seaSwimIdleGlow;

    [Header("Leaf Glow Animations")]
    public AnimationClip leafClimbGlow;
    public AnimationClip leafClimbIdleGlow;
    public AnimationClip leafFallGlow;
    public AnimationClip leafIdleGlow;
    public AnimationClip leafJumpGlow;
    public AnimationClip leafLandGlow;
    public AnimationClip leafRunGlow;
    public AnimationClip playerDamageGlow;




    private void Start()
    {

        playerSeaOverrides["PlayerDamage"] = null;
        playerSeaOverrides["SeaDash"] = null;
        playerSeaOverrides["SeaFall"] = null;
        playerSeaOverrides["SeaIdle"] = null;
        playerSeaOverrides["SeaJump"] = null;
        playerSeaOverrides["SeaLand"] = null;
        playerSeaOverrides["SeaRun"] = null;
        playerSeaOverrides["SeaSwim"] = null;
        playerSeaOverrides["SeaSwimIdle"] = null;

        playerLeafOverrides["LeafClimb"] = null;
        playerLeafOverrides["LeafClimbIdle"] = null;
        playerLeafOverrides["LeafFall"] = null;
        playerLeafOverrides["LeafIdle"] = null;
        playerLeafOverrides["LeafJump"] = null;
        playerLeafOverrides["LeafLand"] = null;
        playerLeafOverrides["LeafRun"] = null;
        playerLeafOverrides["PlayerDamage"] = null;
       
    }

    public void OverideSeaGlowAnimation()
    {
        
        playerSeaOverrides["PlayerDamage"] = playerDamageGlow;
        playerSeaOverrides["SeaDash"] = seaDashGlow;
        playerSeaOverrides["SeaFall"] = seaFallGlow;
        playerSeaOverrides["SeaIdle"] = seaIdleGlow;
        playerSeaOverrides["SeaJump"] = seaJumpGlow;
        playerSeaOverrides["SeaLand"] = seaLandGlow;
        playerSeaOverrides["SeaRun"] = seaRunGlow;
        playerSeaOverrides["SeaSwim"] = seaSwimGlow;
        playerSeaOverrides["SeaSwimIdle"] = seaSwimIdleGlow;


    }

    public void OverideLeafGlowAnimation()
    {
        playerLeafOverrides["LeafClimb"] = leafClimbGlow;
        playerLeafOverrides["LeafClimbIdle"] = leafClimbIdleGlow;
        playerLeafOverrides["LeafFall"] = leafFallGlow;
        playerLeafOverrides["LeafIdle"] = leafIdleGlow;
        playerLeafOverrides["LeafJump"] = leafJumpGlow;
        playerLeafOverrides["LeafLand"] = leafLandGlow;
        playerLeafOverrides["LeafRun"] = leafRunGlow;
        playerLeafOverrides["PlayerDamage"] = playerDamageGlow;

    }
    private void Update()
    {
       if( Input.GetKeyDown(KeyCode.J))
       {
            OverideLeafGlowAnimation();
            OverideSeaGlowAnimation();
        }
    }
}
