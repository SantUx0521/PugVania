using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{
    public static void savePlayerData(GameManager gameManager)
    {
        SaveFile save = new SaveFile(gameManager);
        string dataPath = Application.persistentDataPath + "/player.save";
        FileStream file = new FileStream(dataPath, FileMode.Create);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(file, save);
        file.Close();
        Debug.Log("Game Saved");
    }
    public static SaveFile loadPlayerData()
    {
        string dataPath = Application.persistentDataPath + "/player.save";
        if (File.Exists(dataPath))
        {
            FileStream file = new FileStream(dataPath, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            SaveFile save = binaryFormatter.Deserialize(file) as SaveFile;
            file.Close();
            Debug.Log("Game Loaded");
            return save;
        }
        else
        {
            Debug.LogError("No save file found");
            return null;
        }
    }
}
