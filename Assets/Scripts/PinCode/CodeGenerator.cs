using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeGenerator : MonoBehaviour
{
    public CodeValue codeValue;
    public GameValue newGame;
    public string code;

    public void GenerateCode()
    {
        int val = Random.Range(1000,9999);

        code = val.ToString();

        codeValue.code = code;

        newGame.newGame = true;
    }
}
