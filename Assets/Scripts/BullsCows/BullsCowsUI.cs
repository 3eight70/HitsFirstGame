using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class BullsCowsUI : MonoBehaviour
{
    public InputField userNumber;
    public Text errorText;
    public GameObject textFieldPrefab;
    public Transform textFieldContainer;
    private int textFieldCount = 0;

    private bool CheckInput() 
    {
        if (!Regex.IsMatch(userNumber.text, "^[\\d]+$")) return false;

        if (userNumber.text.Length != 3) return false;
        int[] numbers = new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

        foreach (var symbol in userNumber.text)
        {
            var number = int.Parse(symbol.ToString());
            numbers[number] += 1;
            if (numbers[number] > 1) return false;
        }

        return true;
    }

    public void OnUserTryClick()
    {
        if (!CheckInput()) 
        {
            // userNumber.text = "";
            errorText.text = "Введите трехзначное число без повторений";
            return;
        } 


        float marginTop = (50 * textFieldCount);
        textFieldCount++;
        
        GameObject newTextField = Instantiate(textFieldPrefab, textFieldContainer.transform);
        newTextField.transform.localPosition = Vector3.zero;
        Text textFieldText = newTextField.GetComponentInChildren<Text>();
        textFieldText.transform.localPosition = new Vector3(0f, -marginTop, 0f);

        textFieldText.text = algorithm(userNumber.text);

        errorText.text = "";
        userNumber.text = "";
    }

    private string algorithm(string str) 
    {
        return str;
    }
  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
