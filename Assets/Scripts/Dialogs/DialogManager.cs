using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public Text dialogText;
    public Text nameText;

    public Animator boxAnim;
    public Animator startAnim;

    public GameObject NextButton;
    public GameObject YesButton;
    public GameObject NoButton;

    public DialogChecker check;

    private bool gameFlag;

    private Queue<string> sentences;

    private void Start()
    {
        sentences = new Queue<string>();

        if (check != null)
        {
            gameFlag = check.CheckFlag();
        }
    }

    public void StartDialog(Dialog dialog)
    {
        boxAnim.SetBool("startOpen", true);
        startAnim.SetBool("startOpen", false);

        if (dialog.sentences.Length > 0)
        {
            NextButton.SetActive(true);
            nameText.text = dialog.name;
            sentences.Clear();

            foreach (string sentence in dialog.sentences)
            {
                sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }
    }

    public void DisplayNextSentence()
    {
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

        if (sentences.Count == 0 && YesButton != null && NoButton != null)
        {
            GiveChoice();
            return;
        }

        if (sentences.Count == 0)
        {
            EndDialog();
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogText.text = "";

        foreach(char letter  in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return null;
        }
    }

    public void GiveChoice()
    {
        NextButton.SetActive(false);
        YesButton.SetActive(true);
        NoButton.SetActive(true);
    }

    public void EndDialog()
    {
        boxAnim.SetBool("startOpen", false);
        if (!gameFlag && YesButton !=null && NoButton !=null)
        {
            YesButton.SetActive(false);
            NoButton.SetActive(false);
        }
    }
}
