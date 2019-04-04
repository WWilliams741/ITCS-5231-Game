using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveGameData : MonoBehaviour
{
    [HideInInspector] public string dataPath;
    public string extenstion;
    [HideInInspector] public PlayerData playerData;

    public static SaveGameData instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            CreateSaveObjects();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        LoadData();
    }

    public void CreateSaveObjects()
    {
        playerData = new PlayerData("/Player Data");
        dataPath = Application.persistentDataPath + playerData.GetFileName() + extenstion;
        LoadData();
    }

    public void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(dataPath);
        bf.Serialize(file, playerData);
        file.Close();
    }

    public void LoadData()
    {
        if (File.Exists(dataPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(dataPath, FileMode.Open);
            playerData = (PlayerData)bf.Deserialize(file);
            file.Close();
        }
    }
}
