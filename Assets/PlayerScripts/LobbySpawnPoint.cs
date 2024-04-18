using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySpawnPoint : MonoBehaviour
{
    
    public GameObject spawnPoint;
    private static int numPlayers = 0;
    private static Vector3 firstSpawn = new Vector3(0, 10, 25);
    private static Vector3 secondSpawn = new Vector3(6, 10, 29);
    private static Vector3 thirdSpawn = new Vector3(-10, 10, 30);
    // Start is called before the first frame update
    void Start()
    {

    }
/**
    public static void AddPlayer()
    {
        var lsp = new LobbySpawnPoint();
        lsp.numPlayers = lsp.numPlayers + 1;
        Debug.Log(lsp.numPlayers);
    }
*/
    public static void AddPlayer()
    {
        Debug.Log(numPlayers);
        numPlayers = numPlayers + 1;
        Debug.Log(numPlayers);
    }
/**
    private static int getPlayers()
    {
        return numPlayers;
    }
*/
    /**
    public static Vector3 Position()
    {
        var lsp = new LobbySpawnPoint();
        if (lsp.numPlayers == 1)
        {
            return lsp.firstSpawn;
        } 
        else if (lsp.numPlayers == 2)
        {
            return lsp.secondSpawn;
        } 
        else if (lsp.numPlayers == 3)
        {
            return lsp.thirdSpawn;
        }  
        else
        {
            Debug.Log("else");
            Debug.Log(lsp.numPlayers);
            return lsp.firstSpawn;
        }
    }
*/
    public static Vector3 Position(int players)
    {
        if (players == 1)
        {
            return firstSpawn;
        } 
        else if (players == 2)
        {
            return secondSpawn;
        } 
        else if (players == 3)
        {
            return thirdSpawn;
        }  
        else
        {
            Debug.Log("else");
            Debug.Log(players);
            return firstSpawn;
        }
    }
}
