using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalScore : MonoBehaviour
{

    public struct Winner {
        public int score;
        public string name;
        public bool active;
    };

    public FinalScore Instance;

    public Winner first;
    public Winner second;
    public Winner third;

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
    }
}
