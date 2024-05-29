using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    // Update is called once per frame
    void Update()
    {
        if (RoomManager.instance) {
            float timer = RoomManager.instance.GetTimer();

            if (timer <= 0) {
                timerText.text = "00:00";
                return;
            }

            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer - minutes * 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
