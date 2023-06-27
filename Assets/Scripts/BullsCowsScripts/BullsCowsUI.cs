using UnityEngine;
using UnityEngine.UI;

public class BullsCowsUI : MonoBehaviour
{
    public static BullsCowsUI Instance;

    [SerializeField] private Animator WinAnimator;
    [SerializeField] private Animator ErrorAnimator;
    [SerializeField] private Text ErrorText;
    [SerializeField] private Text WinText;

    void Awake() 
    {
        Instance = this;
    }

    public void ShowWinPopup(string text)
    {
        WinText.text = text;
        WinAnimator.SetTrigger("OpenWinPopup");
    }

    public void ShowErrorPopup(string text)
    {
        ErrorText.text = text;
        ErrorAnimator.SetTrigger("OnErrorUserNumber");
    }
}
