using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Peek : MonoBehaviour
{
    public Animator anim;
    public Animator startPeek;
    public Text text;
    public Text person;

    public FlagValue missions;
    public CodeValue code;

    private string[] texts = { "�� ������ ����� � ����� ����.", "��� � �� ��� �������������?!", "�� � ��� �� ��� �����?" };
    private string[] winTexts = { "������� ��� �������� ����� ", "��� ���������� ��� ��� ", "�� ����� ������ �����, �� ����� �� ��� " };

    public void PeekNum()
    { 
        anim.SetBool("startOpen", true);
        startPeek.SetBool("startOpen", false);

        int index = Random.Range(0, 3);

        if (!missions.mathFlag)
        {
            missions.mathFlag = true;
            text.text = winTexts[index] + code.code[1];
            person.text = "�������";
        }
        else
        {
            
            text.text = texts[index];
            person.text = "������ �.�.";
        }
    }

    public void EndPeek()
    {
        anim.SetBool("startOpen", false);
        startPeek.SetBool("startOpen", false);
    }
}
