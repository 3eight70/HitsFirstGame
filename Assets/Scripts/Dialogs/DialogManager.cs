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

    private Queue<string> sentences;

    private void Start()
    {
        sentences = new Queue<string>();
    }

    private void Update()
    {
        if (sentences.Count == 0) {
            NextButton.SetActive(false);
        }
    }

    public void StartDialog(Dialog dialog)
    {
        boxAnim.SetBool("startOpen", true);
        startAnim.SetBool("startOpen", false);
        NextButton.SetActive(true);

        nameText.text = dialog.name;
        sentences.Clear();

        foreach(string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

        if (sentences.Count == 0)
        {
            GiveChoice();
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
        YesButton.SetActive(true);
        NoButton.SetActive(true);
    }

    public void EndDialog()
    {
        boxAnim.SetBool("startOpen", false);
        YesButton.SetActive(false);
        NoButton.SetActive(false);
    }
}
