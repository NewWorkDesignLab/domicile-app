using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;

public static class CrashReportManager {
    private static bool enableSave = true;
    private static string filename = "error.log";
    private static LogCollection logCollection = new LogCollection ();

    public static void LogCallback (string condition, string stackTrace, LogType type) {
        Log logInfo = new Log (condition, stackTrace, type, DateTime.Now.ToString ("yyyy-MM-ddTHH:mm:sszzz"));
        logCollection.message.Add (logInfo);

        if (type == LogType.Error || type == LogType.Exception) {
            logCollection.isUrgent = true;
            DataManager.Save<LogCollection> (logCollection, filename, enableSave);
        }
    }

    public static void MailLog () {
        LogCollection loadedData = DataManager.Load<LogCollection> (filename, enableSave);
        string date = DateTime.Now.ToString ("yyyy-MM-ddTHH:mm:sszzz");

        if (loadedData != null && loadedData.message != null && loadedData.message.Count > 0 && loadedData.isUrgent) {
            Debug.Log ("[CrashReportManager MailLog] Found log to send!");
#if UNITY_EDITOR
            Debug.Log ("[CrashReportManager MailLog] Skipped Crash Report. Will not send in Editor.");
#else
            string json = JsonUtility.ToJson (loadedData);
            ServerManager.PostRequest ("/api/bugreport", json, (success) => {
                Debug.Log ("[CrashReportManager MailLog] Successfully send Log.");
            }, () => { });
#endif
            DataManager.Delete (filename);
        }
    }
}

[Serializable]
public struct Log {
    public string condition;
    public string stackTrace;
    public LogType type;

    public string dateTime;

    public Log (string condition, string stackTrace, LogType type, string dateTime) {
        this.condition = condition;
        this.stackTrace = stackTrace;
        this.type = type;
        this.dateTime = dateTime;
    }
}

[Serializable]
public class LogCollection {
    public bool isUrgent = false;
    public List<Log> message = new List<Log> ();
}
