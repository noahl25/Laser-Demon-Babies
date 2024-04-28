using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using TMPro;

public class Leaderboard : MonoBehaviour
{

    [Header("Options")]
    public float refreshRate = 1f;

    [Header("UI")]
    public GameObject playersHolder;
    public GameObject leaderboardItemPrefab;

    void Start() {
        InvokeRepeating(nameof(UpdateLeaderboard), 1f, refreshRate);
    }

    void UpdateLeaderboard() {

        foreach (Transform item in playersHolder.transform) {
            Destroy(item.gameObject);
        }

        var sortedPlayerList = (from player in PhotonNetwork.PlayerList orderby player.GetScore() descending select player).ToList();

        foreach (var player in sortedPlayerList) {

            GameObject item = Instantiate(leaderboardItemPrefab, playersHolder.transform);

            item.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = player.NickName;
            item.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = player.GetScore().ToString();

        }

    }

    void Update() {
        playersHolder.SetActive(Input.GetKey(KeyCode.Tab));
    }

}
