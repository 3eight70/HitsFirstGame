using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame(VectorValue position)
    {
        position.initialValue.x = 1;
        position.initialValue.y = 0;
        position.initialValue.z = 0;
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadGame(VectorValue position)
    {
        position.initialValue.x = PlayerPrefs.GetFloat("xPos");
        position.initialValue.y = PlayerPrefs.GetFloat("yPos");
        position.initialValue.z = 0;
        SceneManager.LoadScene(PlayerPrefs.GetInt("Current scene"));
        PlayerPrefs.DeleteAll();
    }
}
