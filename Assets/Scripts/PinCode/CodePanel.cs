using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodePanel : MonoBehaviour
{
    public bool PauseGame;

    public GameObject codeGameMenu;
    public GameObject button;
    public GameObject PauseMenu;
    public LevelChanger levelChanger;

    public Animator anim;
    public string code;
    public Text text;
    public CodeValue codeValue;

    void Update()
    {

        if (code.Length < 5)
        {
            text.text = code;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseGame)
            {
                Resume();
            }
        }
    }

    public void Resume()
    {
        codeGameMenu.SetActive(false);
        Time.timeScale = 1f;
        PauseGame = false;
        PauseMenu.SetActive(false);
        button.SetActive(true);
        anim.SetTrigger("isTriggered");
    }

    public void Pause()
    {
        codeGameMenu.SetActive(true);
        Time.timeScale = 0;
        PauseGame = true;
        button.SetActive(false);
    }


    public void ButtonZero()
    {
        if (code !="NO") code += 0;
    }

    public void ButtonOne()
    {
        if (code != "NO") code += 1;
    }

    public void ButtonTwo()
    {
        if (code != "NO") code += 2;
    }

    public void ButtonThree()
    {
        if (code != "NO") code += 3;
    }

    public void ButtonFour()
    {
        if (code != "NO") code += 4;
    }

    public void ButtonFive()
    {
        if (code != "NO") code += 5;
    }

    public void ButtonSix()
    {
        if (code != "NO") code += 6;
    }

    public void ButtonSeven()
    {
        if (code != "NO") code += 7;
    }

    public void ButtonEight()
    {
        if (code != "NO") code += 8;
    }

    public void ButtonNine()
    {
        if (code != "NO") code += 9;
    }

    public void EnterButton()
    {
        if (codeValue.code == code)
        {
            code = "SHIT";
            levelChanger.FadeToLevel();
            Time.timeScale = 1f;
        }
        else
        {
            code = "NO";
        }
    }

    public void DeleteButton()
    {
        code = "";
    }
}
