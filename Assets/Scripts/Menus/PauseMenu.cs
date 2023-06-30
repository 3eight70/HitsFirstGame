using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class PauseMenu : MonoBehaviour
{
    public bool PauseGame;
    public GameObject pauseGameMenu;
    public CodeValue code;
    public FlagValue missions;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseGame)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseGameMenu.SetActive(false);
        Time.timeScale = 1f;
        PauseGame = false;
    }

    public void Pause()
    {
        pauseGameMenu.SetActive(true);
        Time.timeScale = 0;
        PauseGame=true;
    }

    public void LoadMenu(Hero player)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        PlayerPrefs.SetInt("Current scene", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetFloat("xPos", player.GetX());
        PlayerPrefs.SetFloat("yPos", player.GetY());
        PlayerPrefs.SetString("code", code.code);
        PlayerPrefs.SetInt("bulls", Convert.ToInt32(missions.bullsFlag));
        PlayerPrefs.SetInt("math", Convert.ToInt32(missions.mathFlag));
        PlayerPrefs.SetInt("path", Convert.ToInt32(missions.pathFlag));
        PlayerPrefs.SetInt("checkers", Convert.ToInt32(missions.checkersFlag));
    }
}
