using UnityEngine;
using UnityEngine.UI;

public class PathfinderUI : MonoBehaviour
{
    public static PathfinderUI Instance;

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
        if (WinText == null || WinAnimator == null) return;

        WinText.text = text;
        WinAnimator.SetTrigger("OpenWinPopup");
    }
}
