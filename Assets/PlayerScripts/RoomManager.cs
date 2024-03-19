using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{

    public GameObject player;
    [Space]
    public Transform spawnPoint;
    [Space]
    public TextMeshProUGUI loadingText;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting...");
        loadingText.text = "";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {

        base.OnConnectedToMaster();

        Debug.Log("Connected to server.");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby() {

        base.OnJoinedLobby();

        Debug.Log("Joined lobby.");
        PhotonNetwork.JoinOrCreateRoom("test", null, null);
    
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room.");
        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().IsLocalPlayer();
  
    }
}
