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

    private string[] texts = { "Не хорошо лезть в чужие вещи.", "Что ж вы там разглядываете?!", "Ну и что мы там стоим?" };
    private string[] winTexts = { "Кажется там написана цифра ", "Мне показалось или это ", "Не очень хорошо видно, но вроде бы это " };

    public void PeekNum()
    { 
        anim.SetBool("startOpen", true);
        startPeek.SetBool("startOpen", false);

        int index = Random.Range(0, 3);

        if (!missions.mathFlag)
        {
            missions.mathFlag = true;
            text.text = winTexts[index] + code.code[1];
            person.text = "Ибрагим";
        }
        else
        {
            
            text.text = texts[index];
            person.text = "Даммер Д.Д.";
        }
    }

    public void EndPeek()
    {
        anim.SetBool("startOpen", false);
        startPeek.SetBool("startOpen", false);
    }
}
