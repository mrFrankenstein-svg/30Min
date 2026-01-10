using UnityEngine;

// Как использовать:
//Передай файл с языком через функцию SetLanguageFile().

//Создай два объекта в сцене:

//Один с компонентом KeyStorage (укажи ключ, например "ID_002:").

//Второй с компонентом TextDataReader и присвой ссылку на KeyStorage.



public class LanguageDataReader : MonoBehaviour
{

    //[SerializeField] string fileName ; // без .txt
    //[SerializeField] TextAsset txtAsset;
    string[] txtAssetLines;

    private static LanguageDataReader reader; //this LanguageDataReader()

    private void Start()
    {
        reader = GetComponent<LanguageDataReader>();
    }

    static string GetText(string keyword)
    {
        // string[] lines = reader.txtAsset.text.Split('\n');
        string result= "File error";
        foreach (string line in reader.txtAssetLines)
        {
            if (line.StartsWith(keyword))
            {
                result = line.Substring(keyword.Length).Trim();
            }
        }
        return result;

    }
    //void SetLanguageFile(TextAsset textAsset)
    //{
    //    // Загружаем текстовый файл из Resources
    //    txtAsset = Load<>(textAsset);

    //    if (txtAsset != null)
    //    {
    //        txtAssetLines = txtAsset.text.Split('\n');
    //    }
    //    else
    //    {
    //        Debug.LogError("Файл не найден: " + reader.fileName);
    //    }
    //}
    void SetLanguageFileFromResources(string file_Name)
    {
        //fileName = file_Name; // без .txt

        // Загружаем текстовый файл из Resources
        TextAsset txtAsset = Resources.Load<TextAsset>(file_Name);

        if (txtAsset != null)
        {
            txtAssetLines = txtAsset.text.Split('\n');
        }
        else
        {
            Debug.LogError("Файл не найден: " + file_Name);
        }
    }
}
