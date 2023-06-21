using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public bool PauseGame;
    public GameObject pauseGameMenu;

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
        SceneManager.LoadScene(6);
        PlayerPrefs.SetInt("Current scene", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetFloat("xPos", player.GetX());
        PlayerPrefs.SetFloat("yPos", player.GetY());
        Debug.Log(player.GetX());
        Debug.Log(player.GetY());
    }
}
