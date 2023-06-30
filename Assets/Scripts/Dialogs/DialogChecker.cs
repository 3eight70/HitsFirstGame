using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogChecker : MonoBehaviour
{
    public GameObject startButtonWithoutFlag;
    public GameObject startButtonWithFlag;
    public GameObject DialogWithoutFlag;
    public GameObject DialogWithFlag;
    public GameObject yesButton;
    public GameObject noButton;

    public Text winText;

    public CodeValue code;
    public FlagValue flags;

    public bool CheckFlag()
    {
        if (flags.bullsFlag)
        {
            startButtonWithoutFlag.SetActive(false);
            startButtonWithFlag.SetActive(true);
            DialogWithoutFlag.SetActive(false);
            DialogWithFlag.SetActive(true);
            winText.text = "И снова здравствуй, если ты забыл, то твоя цифра: " + code.code[0] + ". Может ты хочешь сыграть еще раз??";

            return true;
        }

        else if (flags.mathFlag)
        {
            return true;
        }

        else if (flags.checkersFlag)
        {
            startButtonWithoutFlag.SetActive(false);
            startButtonWithFlag.SetActive(true);
            DialogWithoutFlag.SetActive(false);
            DialogWithFlag.SetActive(true);
            winText.text = "Раз уж ты меня переиграл, то вот твоя цифра: " + code.code[0] + ". Сыграем еще разок?";

            return true;
        }

        return false;
    }
}
