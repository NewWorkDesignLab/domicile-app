using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Text;

public class CrashReportManager : MonoBehaviour
{
    public static CrashReportManager instance;
    public bool enableSave = true;
    private string filename = "error.log";
    LogCollection logCollection = new LogCollection();


    void Awake()
    {
        instance = this;
    }

    void LogCallback(string condition, string stackTrace, LogType type)
    {
        Log logInfo = new Log(condition, stackTrace, type, DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"));
        logCollection.message.Add(logInfo);

        if (type == LogType.Error || type == LogType.Exception)
        {
            logCollection.isUrgent = true;
            DataManager.instance.Save<LogCollection>(logCollection, filename, enableSave);
        }
    }

    public void MailLog()
    {
        LogCollection loadedData = DataManager.instance.Load<LogCollection>(filename, enableSave);
        string date = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");

        if (loadedData != null && loadedData.message != null && loadedData.message.Count > 0 && loadedData.isUrgent)
        {
            Debug.Log("Found log to send!");
#if UNITY_EDITOR
            Debug.Log("Skipped Crash Report. Will only Send on Android.");
#else
      string json = JsonUtility.ToJson(loadedData);
      ServerManager.instance.PostRequest("//bugreport", json, (success) => { }, (failure) => { });
#endif

            StartCoroutine(DataManager.instance.Delete(filename));
        }
    }


    void OnEnable()
    {
        //Subscribe to Log Event
        Application.logMessageReceived += LogCallback;
    }
    void OnDisable()
    {
        //Un-Subscribe from Log Event
        Application.logMessageReceived -= LogCallback;
    }
}


[Serializable]
public struct Log
{
    public string condition;
    public string stackTrace;
    public LogType type;

    public string dateTime;

    public Log(string condition, string stackTrace, LogType type, string dateTime)
    {
        this.condition = condition;
        this.stackTrace = stackTrace;
        this.type = type;
        this.dateTime = dateTime;
    }
}


[Serializable]
public class LogCollection
{
    public bool isUrgent = false;
    public List<Log> message = new List<Log>();
}
