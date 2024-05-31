using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PodiumRoomManager : MonoBehaviour
{
    public AudioSource source;
    [Space]
    public GameObject first;
    public GameObject second;
    public GameObject third;
    // Start is called before the first frame update
    void Start()
    {
        FinalScore finalScore = GameObject.FindWithTag("FinalScore").GetComponent<FinalScore>();

        source.Play();
        first.GetComponent<SetupNamesAndScore>().Setup(finalScore.first.name, finalScore.first.score);

        //check if actually exists
        if (!finalScore.second.active) second.SetActive(false);
        else second.GetComponent<SetupNamesAndScore>().Setup(finalScore.second.name, finalScore.second.score);
        if (!finalScore.third.active) third.SetActive(false);
        else third.GetComponent<SetupNamesAndScore>().Setup(finalScore.third.name, finalScore.third.score);
        //leave after 10s

        Destroy(finalScore.gameObject);
        StartCoroutine("Exit");
    }

    IEnumerator Exit() {
        yield return new WaitForSeconds(15);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        yield return new WaitUntil(() => !PhotonNetwork.IsConnected);
        Debug.Log("Left Room");
        SceneManager.LoadScene("Menu");        
    }

}
