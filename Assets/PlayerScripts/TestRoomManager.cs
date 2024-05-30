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
    public TextMeshProUGUI readyButtonText;
    public TMPro.TMP_Dropdown mapSelector;
    //public TextMeshProUGUI mapSelected;
    private string playerName = "unnnamed";
    private Health.Team playerTeam = Health.Team.NONE;

    private GameObject _player;
    private int readyPlayers = 0;
    private bool playerReadyStatus = false;
    public string roomNameToJoin = "testing";
    private RoomList.GameType gameType;

    private string map = "MainMap";
    private string[] mapList = {"MainMap", "Map2"} ;


    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);

        readyButtonText = FindObjectOfType<TextMeshProUGUI>();
        //mapSelected = FindObjectOfType<TextMeshProUGUI>();

        Debug.Log("TestJoinLobby");


        //Setting room name
        GameObject gameSelect = GameObject.FindWithTag("GameSelection");
        RoomList roomListComponent = gameSelect.GetComponent<RoomList>();
        roomNameToJoin = roomListComponent.futureRoomName;
        gameType = roomListComponent.gameType;
        //playerName = roomListComponent.futurePlayerName;

        
        //Setting Room properties
        RoomOptions roomOptions = new RoomOptions();
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        properties["gamemode"] = (gameType == RoomList.GameType.TDM ? "tdm" : "ffa");
        roomOptions.CustomRoomProperties = properties;

        //Creating room
        PhotonNetwork.JoinOrCreateRoom(roomNameToJoin, roomOptions, null);

        //readyButtonText = FindObjectOfType<TextMeshProUGUI>();
        //Debug.Log("TestJoinLobby");
        //OnConnectedToMaster();
        

    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room.");
        spawnPoint = LobbySpawnPoint.Position(PhotonNetwork.CurrentRoom.PlayerCount);
        //_player = PhotonNetwork.Instantiate(player.name, spawnPoint, Quaternion.identity);
        _player = PhotonNetwork.Instantiate(player.name, spawnPoint, Quaternion.Euler(0,180,0));

        _player.GetComponent<PhotonView>().RPC("SetName", RpcTarget.AllBuffered, playerName, playerTeam);
        //_player.GetComponent<PlayerSetup>().HideName();
        //_player.GetComponent<PhotonView>().RPC("SetupMeshes", RpcTarget.OthersBuffered);
        
        _player.GetComponent<PlayerSetup>().LobbySetup();
        //_player.GetComponent<PlayerSetup>().IsLocalPlayer();
  
    }


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
    }
    public void Leave()
    {        
        PhotonView photonView = PhotonView.Get(this);
        playerReadyStatus = false;
        photonView.RPC("ReadyRPC", RpcTarget.All, playerReadyStatus);
        /*
        if (PhotonNetwork.InRoom) {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }
        */
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();

        //yield return new WaitUntil(() => !PhotonNetwork.IsConnected);
        //PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Left Room");
        SceneManager.LoadScene("Menu");
    }

    public void UpdateMap()
    {
        //mapSelector.GetComponent<dropdown>;
        map = mapList[mapSelector.value];
        Debug.Log(mapSelector.value);
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
                
                Debug.Log("The number of ");
                photonView.RPC("TransportPlayers", RpcTarget.All);
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
        //PhotonNetwork.LeaveRoom();
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LoadLevel(map);
        //SceneManager.LoadScene("MainMap");

    }

}
