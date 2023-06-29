using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinNums : MonoBehaviour
{
    public CodeValue code;
    public FlagValue flags;

    public Text pin1;
    public Text pin2;
    public Text pin3;
    public Text pin4;

    void Update()
    {
        if (flags.bullsFlag)
        {
            pin1.text = code.code[0].ToString();
        }

        if (flags.mathFlag)
        {
            pin2.text = code.code[1].ToString();
        }

        if (flags.pathFlag)
        {
            pin3.text = code.code[2].ToString();
        }

        if (flags.checkersFlag)
        {
            pin4.text = code.code[3].ToString();
        }
    }
}
