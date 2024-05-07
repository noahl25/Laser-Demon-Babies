using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerPhotonAnimationManager : MonoBehaviour
{
    public AnimationClip walk;
    public AnimationClip idle;

    public Animation _animation;

    private PhotonView view;

    void Start() {
        _animation.AddClip(walk, "walk");
        _animation.AddClip(idle, "idle");

        view = GetComponent<PhotonView>();
    }

    public void PlayWalkAnimationSynced() {
        view.RPC("PlayAnimation", RpcTarget.Others, "walk");
    }

    public void PlayIdleAnimationSynced() {
        view.RPC("PlayAnimation", RpcTarget.Others, "idle");
    }


    [PunRPC]
    private void PlayAnimation(string clip) {
        _animation.CrossFade(clip, 0.5f);
    }

}
