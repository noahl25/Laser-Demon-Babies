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

    public Team team = Team.NONE;

    public int health;
    public bool isLocalPlayer;

    [Header("UI")]
    public TextMeshProUGUI healthText;

    [PunRPC]
    public void TakeDamage(int damage) {
        health -= damage;

        healthText.text = health.ToString();

        if (health < 0) {
            if (isLocalPlayer) 
                RoomManager.instance.SpawnPlayer();
            Destroy(gameObject);
        }

    }

    public int GetHealth() {
        return health;
    }
}
