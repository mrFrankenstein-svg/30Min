using TMPro;
using UnityEngine;
public static class TMProTextManager 
{
    public static void ChangeText(object text,TMP_Text textField)
    {
        if (textField == null)
            Debug.LogError("TMProTextManager: textField == null");

        else
        {
            string text2 = text?.ToString() ?? "NULL" ;
            if(text2 == "NULL")
                Debug.LogError("TMProTextManager: text == null");
            else
                textField.SetText(text2);

        }
    }

}
