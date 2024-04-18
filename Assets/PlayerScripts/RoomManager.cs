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


    void Awake() {

        instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameSelect = GameObject.FindWithTag("GameSelection");

        roomNameToJoin = gameSelect.GetComponent<RoomList>().futureRoomName;
        gameType = gameSelect.GetComponent<RoomList>().gameType;

        Debug.Log("Connecting...");

        RoomOptions roomOptions = new RoomOptions();
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        properties["gamemode"] = (gameType == RoomList.GameType.TDM ? "tdm" : "ffa");
        roomOptions.CustomRoomProperties = properties;

        PhotonNetwork.JoinOrCreateRoom(roomNameToJoin, roomOptions, null);
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

        if ((string)PhotonNetwork.CurrentRoom.CustomProperties["gamemode"] == "tdm") {

            Health.Team team;

            if (Random.Range(0, 1) == 0) {
                team = Health.Team.BLUE;
                _player.transform.GetChild(1).gameObject.SetActive(true);
 
            }
            else {
                team = Health.Team.RED;
                _player.transform.GetChild(0).gameObject.SetActive(true);
 
            }

            _player.GetComponent<Health>().team = team;


        }

        

    }
}
