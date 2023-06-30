using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        if (flags.bullsFlag && SceneManager.GetActiveScene().buildIndex == 5)
        {
            startButtonWithoutFlag.SetActive(false);
            startButtonWithFlag.SetActive(true);
            DialogWithoutFlag.SetActive(false);
            DialogWithFlag.SetActive(true);
            winText.text = "� ����� ����������, ���� �� �����, �� ���� �����: " + code.code[0] + ". ����� �� ������ ������� ��� ���??";

            return true;
        }

        else if (flags.mathFlag && SceneManager.GetActiveScene().buildIndex == 6)
        {
            return true;
        }

        else if (flags.checkersFlag && SceneManager.GetActiveScene().buildIndex == 4)
        {
            startButtonWithoutFlag.SetActive(false);
            startButtonWithFlag.SetActive(true);
            DialogWithoutFlag.SetActive(false);
            DialogWithFlag.SetActive(true);
            winText.text = "��� �� �� ���� ���������, �� ��� ���� �����: " + code.code[3] + ". ������� ��� �����?";
            return true;
        }

        return false;
    }
}
