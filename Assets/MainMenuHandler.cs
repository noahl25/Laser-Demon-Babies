using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Lobby()
    {
        SceneManager.LoadScene("MainMap");
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }
}
