using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerPhotonAnimationManager : MonoBehaviour
{
    public AnimationClip walk;
    public AnimationClip idle;

    public Animation anim;

    private PhotonView view;

    void Start() {
        view = GetComponent<PhotonView>();
    }

    public void Init() {

        if (anim != null) {
            anim.AddClip(walk, "walk");
            anim.AddClip(idle, "idle");
        }
    }

    public void PlayWalkAnimationSynced() {
        view.RPC("PlayAnimation", RpcTarget.Others, "walk");
    }

    public void PlayIdleAnimationSynced() {
        view.RPC("PlayAnimation", RpcTarget.Others, "idle");
    }


    [PunRPC]
    private void PlayAnimation(string clip) {
        anim.CrossFade(clip, 0.5f);
    }

}
