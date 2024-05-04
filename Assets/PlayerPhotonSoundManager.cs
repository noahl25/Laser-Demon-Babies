using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerPhotonSoundManager : MonoBehaviour
{
    public AudioSource footstepSource;
    public AudioClip footstepSFX;

    public AudioSource laserShootSource;
    public AudioClip laserSFX;

    private PhotonView photonView;

    void Start() {
        photonView = GetComponent<PhotonView>();
    }
    

    public void PlayFootstepSFX() {
        photonView.RPC("PlayFootstepSFX_RPC", RpcTarget.All);
       
    }

    public void PlayLaserSFX() {
        photonView.RPC("PlayLaserSFX_RPC", RpcTarget.All);
    }

    [PunRPC]
    public void PlayFootstepSFX_RPC() {
        footstepSource.clip = footstepSFX;

        footstepSource.pitch = UnityEngine.Random.Range(0.7f, 1.2f);
        footstepSource.volume = UnityEngine.Random.Range(0.2f, 0.35f);

        footstepSource.Play();
    }

    [PunRPC]
    public void PlayLaserSFX_RPC() {

        laserShootSource.clip = laserSFX;

        footstepSource.pitch = UnityEngine.Random.Range(0.7f, 1.2f);
        footstepSource.volume = UnityEngine.Random.Range(0.2f, 0.35f);

        laserShootSource.Play();

    }



}
