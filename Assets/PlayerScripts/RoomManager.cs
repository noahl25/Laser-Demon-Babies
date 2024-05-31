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
    public Transform[] spawnPoint;
    [Space]
    public GameObject loadingCam;
    [Space]
    public Leaderboard leaderboard;


    public string roomNameToJoin = "test";

    private RoomList.GameType gameType;

    private string playerName = "unnnamed";
    private Health.Team playerTeam = Health.Team.NONE;
    private float timer = 60.0f;
    private bool endedGame = false;

    void Update() {
        timer -= Time.deltaTime;

        if (timer <= 0 && !endedGame) {
            StartCoroutine("EndGame");
            endedGame = true;
        }

        if (player.transform.position.y < -20) {
            Destroy(player);
            SpawnPlayer();
        }
    }

    void Awake() {

        instance = this;

    }

    public float GetTimer() {
        return timer;
    }

    // Start is called before the first frame update
    void Start()
    {
        /*
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
        */
        GameObject gameSelect = GameObject.FindWithTag("GameSelection");
        RoomList roomListComponent = gameSelect.GetComponent<RoomList>();
        playerName = roomListComponent.futurePlayerName;

        loadingCam.SetActive(false);

        SpawnPlayer();
    }

    public void ChangeName(string _name) {
        name = _name;
    }
/*
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room.");

        loadingCam.SetActive(false);

        SpawnPlayer();
  
    }
    */

    public void SpawnPlayer() {

        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint[Random.Range(0, spawnPoint.Length)].position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().IsLocalPlayer();
        _player.GetComponent<Health>().isLocalPlayer = true;

        //if tdm gamemode, setup that
        if ((string)PhotonNetwork.CurrentRoom.CustomProperties["gamemode"] == "tdm") {

            if (playerTeam == Health.Team.NONE) {
                if (Random.value >= 0.5f) {
                    playerTeam = Health.Team.BLUE;
                    _player.transform.GetChild(1).gameObject.SetActive(true);
    
                }
                else {

                    playerTeam = Health.Team.RED;
                    _player.transform.GetChild(0).gameObject.SetActive(true);
    
                }
            }

            _player.GetComponent<PhotonView>().RPC("SyncTeams", RpcTarget.OthersBuffered, playerTeam);
            //set team jereseys unactive for YOURSELF only, so you dont see them (very annoying if you can)
            _player.transform.GetChild(0).gameObject.SetActive(false);
            _player.transform.GetChild(1).gameObject.SetActive(false);
            _player.GetComponent<TeamIndicator>().SetTeamText(playerTeam.ToString());

        }

        _player.GetComponent<PhotonView>().RPC("SetName", RpcTarget.OthersBuffered, playerName, playerTeam);
        //hides your own name
        _player.GetComponent<PlayerSetup>().HideName();
        _player.GetComponent<PhotonView>().RPC("SetupMeshes", RpcTarget.OthersBuffered);


        PhotonNetwork.LocalPlayer.NickName = playerName;

        player = _player;

    }

    IEnumerator EndGame() {

        leaderboard.FinishGame();

        float t = 3.0f;

        PlayerSetup setup = player.GetComponent<PlayerSetup>();

        setup.End();
        while (t > 0) {
            setup.FadeInOverlay();
            t -= Time.deltaTime;
            yield return null;
        }
        setup.FadeFully();
        yield return new WaitForSeconds(1f);
        PhotonNetwork.LoadLevel("Podium");
    }
}
