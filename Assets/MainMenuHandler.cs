using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{

    public Transform playButton;
    public Transform settingsButton;
    public Transform roomList;

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

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }
}
