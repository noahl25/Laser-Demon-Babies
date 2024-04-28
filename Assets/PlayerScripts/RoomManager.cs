using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;

    public GameObject player;
    [Space]
    public Transform spawnPoint;
    [Space]
    public GameObject loadingCam;

    public string roomNameToJoin = "test";

    private RoomList.GameType gameType;

    private string playerName = "unnnamed";

    void Awake() {

        instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameSelect = GameObject.FindWithTag("GameSelection");

        RoomList roomListComponent = gameSelect.GetComponent<RoomList>();

        roomNameToJoin = roomListComponent.futureRoomName;
        gameType = roomListComponent.gameType;
        playerName = roomListComponent.futurePlayerName;


        Debug.Log("Connecting...");

        RoomOptions roomOptions = new RoomOptions();
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        properties["gamemode"] = (gameType == RoomList.GameType.TDM ? "tdm" : "ffa");
        roomOptions.CustomRoomProperties = properties;

        PhotonNetwork.JoinOrCreateRoom(roomNameToJoin, roomOptions, null);
    }

    public void ChangeName(string _name) {
        name = _name;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room.");

        loadingCam.SetActive(false);

        SpawnPlayer();
  
    }

    public void SpawnPlayer() {

        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().IsLocalPlayer();
        _player.GetComponent<Health>().isLocalPlayer = true;

        Health.Team team = Health.Team.NONE;

        if ((string)PhotonNetwork.CurrentRoom.CustomProperties["gamemode"] == "tdm") {


            if (Random.value >= 0.5f) {
                team = Health.Team.BLUE;
                _player.transform.GetChild(1).gameObject.SetActive(true);
 
            }
            else {

                team = Health.Team.RED;
                _player.transform.GetChild(0).gameObject.SetActive(true);
 
            }
            team = Health.Team.RED;
            _player.GetComponent<PhotonView>().RPC("SyncTeams", RpcTarget.AllBuffered, team);
            _player.GetComponent<TeamIndicator>().SetTeamText(team.ToString());
        }

        _player.GetComponent<PhotonView>().RPC("SetName", RpcTarget.AllBuffered, playerName, team);
        _player.GetComponent<PlayerSetup>().HideName();

    }
}
