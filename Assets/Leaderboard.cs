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
    
    [Header("Refs")]
    public FinalScore finalScore;

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

    public void FinishGame() {
        var sortedPlayerList = (from player in PhotonNetwork.PlayerList orderby player.GetScore() descending select player).ToList();

        finalScore.first.name = sortedPlayerList[0].NickName;
        finalScore.first.score = sortedPlayerList[0].GetScore();

        if (sortedPlayerList.Count > 1) {

            finalScore.second.name = sortedPlayerList[1].NickName;
            finalScore.second.score = sortedPlayerList[1].GetScore();

        }

        if (sortedPlayerList.Count > 2) {

            finalScore.third.name = sortedPlayerList[2].NickName;
            finalScore.third.score = sortedPlayerList[2].GetScore();

        }
    }

}
