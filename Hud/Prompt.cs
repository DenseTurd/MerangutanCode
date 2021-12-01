using UnityEngine;
using TMPro;

public class Prompt : MonoBehaviour
{
    public TMP_Text promptText;
    string currentText;

    void Start()
    {
        ClearPrompt();    
    }

    public void SetPrompt(string stringy)
    {
        if (stringy != currentText)
        {
            currentText = stringy;
            promptText.text = currentText;
        }
    }

    public void ClearPrompt()
    {
        SetPrompt("");
    }
}
