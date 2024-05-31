using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{

    public Transform playButton;
    public Transform settingsButton;
    public Transform roomList;
    public Transform settings;

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Lobby()
    {
        playButton.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(false);
        roomList.gameObject.SetActive(true);

    }

    public void Setting()
    {
        playButton.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(false);
        settings.gameObject.SetActive(true);
    }

    public void Back() {
        playButton.gameObject.SetActive(true);
        settingsButton.gameObject.SetActive(true);
        settings.gameObject.SetActive(false);
    }

    public void PostProcessingToggle() {
        Settings.PostProcessingOff = !Settings.PostProcessingOff;
        //switch
    }
}
