using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetupNamesAndScore : MonoBehaviour
{

    public TextMeshPro name;
    
    public void Setup(string _name, int score) {
        name.SetText(_name + "\nScore: " + score);
    }
}
