using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

/*
public class TestRoomManager : MonoBehaviourPunCallbacks
{
    // Old Code
    public GameObject player;
    [Space]
    public Vector3 spawnPoint;
    [Space]
    public TextMeshProUGUI readyButtonText;

    private GameObject _player;
    private int readyPlayers = 0;
    private bool playerReadyStatus = false;
    public string roomNameToJoin = "testing";

    // Start is called before the first frame update
    void Start()
    {
         
        DontDestroyOnLoad(this.gameObject);
        readyButtonText = FindObjectOfType<TextMeshProUGUI>();
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
        GameObject gameSelect = GameObject.FindWithTag("GameSelection");
        RoomList roomListComponent = gameSelect.GetComponent<RoomList>();
        roomNameToJoin = roomListComponent.futureRoomName;
        PhotonNetwork.JoinOrCreateRoom(roomNameToJoin, null, null);
    
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room.");
        spawnPoint = LobbySpawnPoint.Position(PhotonNetwork.CurrentRoom.PlayerCount);
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
        PhotonView photonView = PhotonView.Get(this);
        if(playerReadyStatus == true)
        {
            playerReadyStatus = false;
            readyButtonText.text = "Ready";
            photonView.RPC("ReadyRPC", RpcTarget.All, playerReadyStatus);
            Debug.Log("sent RPC");
        }
        else
        {
            playerReadyStatus = true;
            readyButtonText.text = "Unready";
            photonView.RPC("ReadyRPC", RpcTarget.All, playerReadyStatus);
            Debug.Log("sent RPC");
        }
        //PhotonView photonView = PhotonNetwork.MasterClient;
        

    }

    [PunRPC]
    public void ReadyRPC(bool readyState)
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Not Master");
            return;
        }
        if(readyState == true)
        {
            readyPlayers = readyPlayers + 1;
            Debug.Log("Count of players:" + PhotonNetwork.CurrentRoom.PlayerCount);
            if(readyPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                
                photonView.RPC("TransportPlayers", RpcTarget.All);
                //PhotonNetwork.AutomaticallySyncScene = true;
                //PhotonNetwork.LoadLevel("MainMap");

                //PhotonNetwork.LeaveRoom();
                //SceneManager.LoadScene("MainMap");
            }
            else
            {
                Debug.Log("else ready players:" + readyPlayers);
                return;
            }
        }
        if(readyState == false)
        {
            readyPlayers = readyPlayers - 1;
            Debug.Log("Count of players:" + PhotonNetwork.CurrentRoom.PlayerCount);
            if(readyPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                
                photonView.RPC("TransportPlayers", RpcTarget.All);
            }
            else
            {
                Debug.Log("else ready players:" + readyPlayers);
                return;
            }
        }
    }

    [PunRPC]
    public void TransportPlayers()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LoadLevel("MainMap");
        //SceneManager.LoadScene("MainMap");

    }

}
*/
