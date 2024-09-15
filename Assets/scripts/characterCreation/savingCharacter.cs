using System.IO;
using UnityEngine;

static public class savingCharacter
{

    static public void SaveData(saveCharacter charData)
    {
        string json = JsonUtility.ToJson(charData);

        using (StreamWriter write = new StreamWriter(Application.dataPath + Path.AltDirectorySeparatorChar + $"Save-Data/characters/{charData.name}.json"))
        {
            write.Write(json);
        }
    }

    static public saveCharacter LoadData(string name)
    {
        string json = string.Empty;

        using(StreamReader reader = new StreamReader(Application.dataPath + Path.AltDirectorySeparatorChar + $"Save-Data/characters/{name}.json"))
        {
            json = reader.ReadToEnd();
        }

        saveCharacter cData = JsonUtility.FromJson<saveCharacter>(json);
        return cData;
    }
}
