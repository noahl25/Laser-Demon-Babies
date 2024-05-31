using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomList : MonoBehaviourPunCallbacks
{

    public enum GameType {
        FFA,
        TDM,
        None
    }

    public static RoomList Instance;

    [Header("UI")]
    public Transform roomListParent;
    public GameObject roomListItemPrefab;
    public TMP_InputField createRoomName;
    public Toggle TDMToggle;
    public Toggle FFAToggle;

    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();

    [HideInInspector]public string futureRoomName;
    public GameType gameType = GameType.None;
    [HideInInspector] public string futurePlayerName = "unnamed";

    bool connected = false;
    bool joinedRoom = false;


    private void Awake() {
        if (Instance == null) {
            //First run, set the Instance
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else if (Instance != this) {
            //Instance is not the same as the one we have, destroy old one, and reset to newest one
            Destroy(Instance.gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //make sure you are disconnected before proceeding
        if (PhotonNetwork.InRoom) {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }

        yield return new WaitUntil(() => !PhotonNetwork.IsConnected);

        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster() {
        base.OnConnectedToMaster();

        if (!joinedRoom) 
            PhotonNetwork.JoinLobby();

        joinedRoom = true;
        
    }

    public override void OnJoinedLobby() {
        base.OnJoinedLobby();

        connected = true;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {

        Debug.Log(roomList.Count);

        if (cachedRoomList.Count <= 0) {
            cachedRoomList = roomList;
        }
        else {
            foreach (var room in roomList) {

                for (int i = 0; i < cachedRoomList.Count; i++) {
                    //make sure cached list isn't updated with duplicates
                    if (cachedRoomList[i].Name == room.Name) {

                        List<RoomInfo> newList = cachedRoomList;

                        if (room.RemovedFromList) {
                            newList.Remove(newList[i]);
                        }
                        else {
                            newList[i] = room;
                        }

                        cachedRoomList = newList;

                    }

                }

            }
        }

        UpdateUI();

    }

    void UpdateUI() {
        //destroy each object then recreate from cache
      
        foreach (Transform roomItem in roomListParent) {
            if (roomItem.gameObject)
                Destroy(roomItem.gameObject);
        }
        

        foreach (var room in cachedRoomList) {

            GameObject roomItem = Instantiate(roomListItemPrefab, roomListParent);

            roomItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = room.Name;
            roomItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = room.PlayerCount + "/16";

            roomItem.GetComponent<RoomItemButton>().RoomName = room.Name;
        }


    }

    public void UnselectFFA() {
        FFAToggle.isOn = false;
    }
    public void UnselectTDM() {
        TDMToggle.isOn = false;
    }

    public void JoinRoomByName(string name) {

        if (!connected) return;

        futureRoomName = name;
        SceneManager.LoadScene("Lobby");
    }

    public void SetFuturePlayerName(string name) {
        futurePlayerName = name;
    }

    public void CreateRoom() {

        if (!connected) return;
        
        futureRoomName = createRoomName.text;
        
        if (FFAToggle.isOn) {
            gameType = GameType.FFA;
        }
        else if (TDMToggle.isOn) {
            gameType = GameType.TDM;
        }
        else {
            return;
        }

        SceneManager.LoadScene("Lobby");
    }

}
