using UnityEngine;
using System.IO;
using DefaultNamespace;



public static class SaveManager
{
    private static string savePath = Path.Combine(Application.persistentDataPath, "databin.json");
    public static SaveData Data = new SaveData();

    public static void Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            Data = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            Data = new SaveData();
        }
    }

    public static void Save()
    {
        string json = JsonUtility.ToJson(Data, true);
        File.WriteAllText(savePath, json);
    }
}