using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class TestRoomManager : MonoBehaviourPunCallbacks
{
    // Old Code
    public GameObject player;
    [Space]
    public Vector3 spawnPoint;
    [Space]
    public TextMeshProUGUI loadingText;

    private GameObject _player;
    private int readyPlayers = 0;
    // Start is called before the first frame update
    void Start()
    {
         
        DontDestroyOnLoad(this.gameObject);
        Debug.Log("TestJoinLobby");
        OnConnectedToMaster();
        

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
        spawnPoint = LobbySpawnPoint.Position(PhotonNetwork.CountOfPlayers);
        _player = PhotonNetwork.Instantiate(player.name, spawnPoint, Quaternion.identity);
        //_player.GetComponent<PlayerSetup>().IsLocalPlayer();
  
    }
    //Old code end 



    //my code 
    public void TestReady()
    {
        Debug.Log("I am test ready");
        Debug.Log(PhotonNetwork.MasterClient);
    }

    public void Ready()
    {
        //PhotonView photonView = PhotonNetwork.MasterClient.GetComponent<PhotonView>();
        PhotonView photonView = PhotonView.Get(this);
        //PhotonView photonView = PhotonNetwork.MasterClient;
        photonView.RPC("ReadyRPC", RpcTarget.All);
        Debug.Log("sent RPC");

    }

    [PunRPC]
    public void ReadyRPC()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Not Master");
            return;
        }
        readyPlayers = readyPlayers + 1;
        if(readyPlayers == PhotonNetwork.CountOfPlayers)
        {
            
            photonView.RPC("TransportPlayers", RpcTarget.All);
            //PhotonNetwork.AutomaticallySyncScene = true;
            //PhotonNetwork.LoadLevel("MainMap");

            //PhotonNetwork.LeaveRoom();
            //SceneManager.LoadScene("MainMap");
        }
        else
        {
            Debug.Log(readyPlayers);
            return;
        }
    }

    [PunRPC]
    public void TransportPlayers()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MainMap");

    }

}
