using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public Dialog dialog;
    public DialogManager dm;

    public void TriggerDialogue()
    {
        if (dm != null && dialog != null)
        {
            dm.StartDialog(dialog);
        }
    }
}
