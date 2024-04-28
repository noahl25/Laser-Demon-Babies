using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System;

public class Health : MonoBehaviour
{

    public enum Team {
        RED,
        BLUE,
        NONE
    }

    public Team team;

    public int health;
    public bool isLocalPlayer;

    [Header("UI")]
    public TextMeshProUGUI healthText;

    [PunRPC]
    public void TakeDamage(int damage) {

        health -= damage;

        healthText.text = health.ToString();

        if (health <= 0) {
            if (isLocalPlayer) 
                RoomManager.instance.SpawnPlayer();
            Destroy(gameObject);
        } 

    }

    [PunRPC]
    public void SyncTeams(Team _team) {
        team = _team;

        if (_team == Team.RED) {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
        else {
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }

    }

    public int GetHealth() {
        return health;
    }
}
