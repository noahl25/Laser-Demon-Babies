using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    private AudioSource src;
    private bool playing = true;

    private MusicPlayer Instance;

    void Awake() {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("MusicPlayer"))
        {
            Destroy(player);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        src = GetComponent<AudioSource>();

        src.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("h")) {

            if (playing) {
                src.Stop();
            }
            else {
                src.Play();
            }

        }       
        
    }
}
