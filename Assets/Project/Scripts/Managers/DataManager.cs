using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class DataManager {
    public static PersistedData persistedData;
    private static string fileName = "data.dat";
    private static bool useSaveFile = true; // TODO on Building

    public static void Load () {
        persistedData = Load<PersistedData> (fileName, useSaveFile);
    }
    public static T Load<T> (string _filename, bool _useSaveFile) where T : new () {
        if (File.Exists (Path.Combine (Application.persistentDataPath, _filename)) && _useSaveFile) {
            Debug.Log ("[DataManager Load] Loading  Data. Found and using File: " + _filename);
            BinaryFormatter bf = new BinaryFormatter ();
            FileStream fs = File.Open (Path.Combine (Application.persistentDataPath, _filename), FileMode.Open);
            T data = (T) bf.Deserialize (fs);
            fs.Close ();
            return data;
        } else {
            Debug.Log ("[DataManager Load] Loading  Data. No File found. (" + _filename + ")");
            T data = new T ();
            return data;
        }
    }

    public static void Save () {
        Save<PersistedData> (persistedData, fileName, useSaveFile);
    }
    public static void Save<T> (T data, string _filename, bool _useSaveFile) {
        if (data != null && _useSaveFile) {
            Debug.Log ("[DataManager Save] Saving Data. Using File: " + _filename);
            BinaryFormatter bf = new BinaryFormatter ();
            FileStream fs = File.Create (Path.Combine (Application.persistentDataPath, _filename));
            bf.Serialize (fs, data);
            fs.Close ();
        }
    }

    public static void Delete (string _filename, int retry = 0) {
        CoroutineHelper.instance.StartCoroutine (DeleteHelper (_filename, retry));
    }
    private static IEnumerator DeleteHelper (string _filename, int retry) {
        yield return new WaitForSecondsRealtime (.1f);
        if (File.Exists (Path.Combine (Application.persistentDataPath, _filename))) {
            try {
                Debug.Log ("[DataManager DeleteHelper] (Retry: " + retry + ") Deleting Data from File: " + _filename);
                File.Delete (Path.Combine (Application.persistentDataPath, _filename));
            } catch (IOException e) {
                Debug.LogWarning ("[DataManager DeleteHelper] (Retry: " + retry + ") Could not delete File: " + _filename + "\nException: " + e);
                if (retry < 5) {
                    Delete (_filename, retry + 1);
                }
            }
        }
    }
}

[Serializable]
public class PersistedData {
    // USER
    public string lastKnownUid;
    public string lastKnownClient;
    public string lastKnownAccessToken;
    public string userEmail;
}
