using UnityEngine;

// ��� ������������:
//������� ���� � ������ ����� ������� SetLanguageFile().

//������ ��� ������� � �����:

//���� � ����������� KeyStorage (����� ����, �������� "ID_002:").

//������ � ����������� TextDataReader � ������� ������ �� KeyStorage.



public class LanguageDataReader : MonoBehaviour
{

    //[SerializeField] string fileName ; // ��� .txt
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
    //    // ��������� ��������� ���� �� Resources
    //    txtAsset = Load<>(textAsset);

    //    if (txtAsset != null)
    //    {
    //        txtAssetLines = txtAsset.text.Split('\n');
    //    }
    //    else
    //    {
    //        Debug.LogError("���� �� ������: " + reader.fileName);
    //    }
    //}
    void SetLanguageFileFromResources(string file_Name)
    {
        //fileName = file_Name; // ��� .txt

        // ��������� ��������� ���� �� Resources
        TextAsset txtAsset = Resources.Load<TextAsset>(file_Name);

        if (txtAsset != null)
        {
            txtAssetLines = txtAsset.text.Split('\n');
        }
        else
        {
            Debug.LogError("���� �� ������: " + file_Name);
        }
    }
}
