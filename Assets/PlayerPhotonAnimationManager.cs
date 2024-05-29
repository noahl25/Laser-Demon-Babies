using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerPhotonAnimationManager : MonoBehaviour
{
    public AnimationClip walk;
    public AnimationClip idle;

    public Animation anim;

    public PhotonView view;
    
    private bool animationsSet = false;

    public void PlayWalkAnimationSynced() {
        view.RPC("PlayAnimation", RpcTarget.Others, "walk");
    }

    public void PlayIdleAnimationSynced() {
        view.RPC("PlayAnimation", RpcTarget.Others, "idle");
    }


    [PunRPC]
    private void PlayAnimation(string clip) {

        if (anim != null && !animationsSet) {
            anim.AddClip(walk, "walk");
            anim.AddClip(idle, "idle");
            animationsSet = true;
        }

        anim.CrossFade(clip, 0.5f);
        
    }

}
