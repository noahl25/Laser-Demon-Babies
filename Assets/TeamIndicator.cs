using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeamIndicator : MonoBehaviour
{

    public TextMeshProUGUI teamText;

    public void SetTeamText(string text) {

        teamText.text = text;

    }
}
