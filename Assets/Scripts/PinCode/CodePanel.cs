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
        code += 0;
    }

    public void ButtonOne()
    {
        code += 1;
    }

    public void ButtonTwo()
    {
        code += 2;
    }

    public void ButtonThree()
    {
        code += 3;
    }

    public void ButtonFour()
    {
        code += 4;
    }

    public void ButtonFive()
    {
        code += 5;
    }

    public void ButtonSix()
    {
        code += 6;
    }

    public void ButtonSeven()
    {
        code += 7;
    }

    public void ButtonEight()
    {
        code += 8;
    }

    public void ButtonNine()
    {
        code += 9;
    }

    public void EnterButton()
    {
        if (codeValue.code == code)
        {
            code = "GG";
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
