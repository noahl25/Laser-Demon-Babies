using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySpawnPoint : MonoBehaviour
{
    
    // Start is called before the first frame update
    public static LobbySpawnPoint instance;
    public Transform[] spawnPoints;
    
    void Start()
    {
        instance = this;
    }

}