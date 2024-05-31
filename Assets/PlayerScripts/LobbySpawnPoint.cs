using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbySpawnPoint : MonoBehaviour
{
    
    public static LobbySpawnPoint instance;
    public Transform[] spawnPoints;
    
    void Start()
    {
        instance = this;
    }

    


    /*
    public GameObject spawnPoint;
    private static int numPlayers = 0;
    private static bool[] spotTaken = new bool[16];
    private static Vector3[] spawnPoints = new Vector3[16];


    private static Vector3 firstSpawn = new Vector3(0, 10, 25);
    private static Vector3 secondSpawn = new Vector3(6, 10, 29);
    private static Vector3 thirdSpawn = new Vector3(-10, 10, 30);
    // Start is called before the first frame update
    void Start()
    {
        spawnPoints[0] = firstSpawn;
        spawnPoints[1] = secondSpawn;
        spawnPoints[2] = thirdSpawn;

        for (int i = 3; i < spawnPoints.Length; i++)
        {
            spawnPoints[i] = new Vector3((i*3)-25, 10, 40);
        }
    }


    public int AddPlayer(PhotonView photonView)
    {
        //PhotonView photonView = PhotonView.Get(this);
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if(spotTaken[i] == false)
            {
                spotTaken[i] = true;
                photonView.RPC("SpotFilled", RpcTarget.AllBuffered, i);
                Debug.Log("index of spawn: " + i.ToString());
                return i;
            }
        }
        Debug.Log("index over 15");
        return 15;
    }

    [PunRPC]
    public void SpotFilled(int i)
    {
        spotTaken[i] = true;
    }

    public Vector3 GetSpawnPosition()
    {
        PhotonView photonView = PhotonView.Get(this);
        return spawnPoints[AddPlayer(photonView)];
    }

    [PunRPC]
    public bool[] Sync()
    {
        return spotTaken;
    }

    public void OnJoinedRoom()
    {
        PhotonView photonView = PhotonView.Get(this);
        spotTaken = photonView.RPC("Sync", RpcTarget.AllBuffered);
    }
    */


}
