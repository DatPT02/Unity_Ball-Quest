using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class sceneManager : MonoBehaviour
{
    public void LoadStartMenu()
    {
        SceneManager.LoadScene("Start Scene");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void LoadGameOver()
    {
        SceneManager.LoadScene("End Scene");
    }

    public void LoadPlayAgain()
    {
        SceneManager.LoadScene("MainGame");
    }
}
