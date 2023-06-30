using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MainMenu : MonoBehaviour
{
    public VectorValue position;
    public FlagValue missions;
    public CodeValue code;
    public GameValue gameFlag;

    public void PlayGame()
    {
        missions.bullsFlag = false;
        missions.checkersFlag = false;
        missions.mathFlag = false;
        missions.pathFlag = false;
        position.initialValue.x = 1;
        position.initialValue.y = 0;
        position.initialValue.z = 0;   
        SceneManager.LoadScene(9);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        gameFlag.newGame = Convert.ToBoolean(PlayerPrefs.GetInt("game"));
        Debug.Log(gameFlag.newGame);
        if (gameFlag.newGame == true)
        {
            position.initialValue.x = PlayerPrefs.GetFloat("xPos");
            position.initialValue.y = PlayerPrefs.GetFloat("yPos");
            position.initialValue.z = 0;
            code.code = PlayerPrefs.GetString("code");
            SceneManager.LoadScene(PlayerPrefs.GetInt("Current scene"));
            missions.bullsFlag = Convert.ToBoolean(PlayerPrefs.GetInt("bulls"));
            missions.mathFlag = Convert.ToBoolean(PlayerPrefs.GetInt("math"));
            missions.pathFlag = Convert.ToBoolean(PlayerPrefs.GetInt("path"));
            missions.checkersFlag = Convert.ToBoolean(PlayerPrefs.GetInt("checkers"));
            
        }
        else
        {
            PlayGame();
        }
    }
}
