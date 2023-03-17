using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoad {

    public static List<GameObject> savedGameObjects = new List<GameObject>();

    public static void Save() {
        savedGameObjects.Clear();
        // Add all the game objects you want to save to the list
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Dice")) {
            savedGameObjects.Add(gameObject);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData();
        data.savedGameObjects = savedGameObjects.ToArray();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void Load() {
        string path = Application.persistentDataPath + "/save.fun";

        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            savedGameObjects = new List<GameObject>(data.savedGameObjects);

            // Instantiate the saved game objects
            foreach (GameObject gameObject in savedGameObjects) {
                GameObject.Instantiate(gameObject);
            }
        }
    }
}

[System.Serializable]
class SaveData {
    public GameObject[] savedGameObjects;
}

// THIS IS HOW YOU USE THIS SCRIPT
///// SaveLoadScripts.SaveLoad.Save();
///// SaveLoadScripts.SaveLoad.Load();