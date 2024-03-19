using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickFunctions : MonoBehaviour
{
    
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Lobby()
    {
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {
        SceneManager.LoadScene(2);
    }
}
