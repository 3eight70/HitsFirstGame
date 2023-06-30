using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathfinderUI : MonoBehaviour
{
    public static PathfinderUI Instance;

    [SerializeField] private Animator WinAnimator;
    [SerializeField] private Animator ErrorAnimator;
    [SerializeField] private Text ErrorText;
    [SerializeField] private Text WinText;

    public CodeValue code;
    public FlagValue missions;

    private string[] winTexts = { "Кажется выпала шоколадка", "Жаль, ничего не выпало", "Опа, банка газировки", "ДЖЕКПОТ" };

    void Awake() 
    {
        Instance = this;
    }

    public void ShowWinPopup()
    {
        if (WinText == null || WinAnimator == null) return;

        if (!missions.pathFlag)
        {
            WinText.text = "Хмм, кажется выпала записка с цифрой: " + code.code[2].ToString();
        }
        else
        {
            int index = Random.Range(0, 4);
            WinText.text = winTexts[index].ToString();
        }

        missions.pathFlag = true;

        WinAnimator.SetTrigger("OpenWinPopup");
    }
}
