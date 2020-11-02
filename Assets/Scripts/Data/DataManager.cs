using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    [HideInInspector] public static SaveData getValue;
    private string fileName = "savedata.dat";
    private bool useSaveFile = true;

    void Awake()
    {
        instance = this;
        Load();
    }


    public void Load()
    {
        getValue = Load<SaveData>(fileName, useSaveFile);
    }
    public T Load<T>(string _filename, bool _useSaveFile) where T : new()
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, _filename)) && _useSaveFile)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Path.Combine(Application.persistentDataPath, _filename), FileMode.Open);
            T data = (T)bf.Deserialize(fs);
            fs.Close();
            return data;
        }
        else
        {
            T data = new T();
            return data;
        }
    }


    public void Save()
    {
        Save<SaveData>(getValue, fileName, useSaveFile);
    }
    public void Save<T>(T data, string _filename, bool _useSaveFile)
    {
        if (data != null && _useSaveFile)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Create(Path.Combine(Application.persistentDataPath, _filename));
            bf.Serialize(fs, data);
            fs.Close();
        }
    }


    public IEnumerator Delete(string _filename, int retry = 0)
    {
        yield return new WaitForSecondsRealtime(.1f);
        if (File.Exists(Path.Combine(Application.persistentDataPath, _filename)))
        {
            try
            {
                File.Delete(Path.Combine(Application.persistentDataPath, _filename));
            }
            catch (IOException e)
            {
                Debug.Log(e);
                if (retry < 5)
                {
                    StartCoroutine(Delete(_filename, retry + 1));
                }
            }
        }
    }
}

[Serializable]
public class SaveData
{
    // USER
    public string lastKnownUid;
    public string lastKnownClient;
    public string lastKnownAccessToken;
    public string userEmail;
    public bool saveLogin;
}