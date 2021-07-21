using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public static class ServerManager {
    public static bool useSecure = true;
    public static string domain = "localhost:3000";
    public static string domainSecure = "domicile.tobiasbohn.com";
    public static string protocol = "http";
    public static string protocolSecure = "http";
    private static bool isBussy = false;
    private static List<RequestQueueEntry> requestQueue;

    public static void CheckServerAvailabillity (Action onSuccess, Action onFailure) {
        onSuccess ();
        // TODO
    }

    public static void GetRequest (string path, string jsonData, Action<string> onSuccess, Action onFailure) {
        string method = UnityWebRequest.kHttpVerbGET;
        JsonRequestHelper (method, path, jsonData, onSuccess, onFailure);
    }

    public static void PostRequest (string path, string jsonData, Action<string> onSuccess, Action onFailure) {
        string method = UnityWebRequest.kHttpVerbPOST;
        JsonRequestHelper (method, path, jsonData, onSuccess, onFailure);
    }

    public static void PutRequest (string path, string jsonData, Action<string> onSuccess, Action onFailure) {
        string method = UnityWebRequest.kHttpVerbPUT;
        JsonRequestHelper (method, path, jsonData, onSuccess, onFailure);
    }

    public static void DeleteRequest (string path, string jsonData, Action<string> onSuccess, Action onFailure) {
        string method = UnityWebRequest.kHttpVerbDELETE;
        JsonRequestHelper (method, path, jsonData, onSuccess, onFailure);
    }

    // https://answers.unity.com/questions/1632065/how-to-upload-multiple-files-to-a-server-using-uni.html
    public static void ImageRequest (string path, string[] images, Action<string> onSuccess, Action onFailure) {
        if (images.Length > 0) {
            WWWForm form = new WWWForm ();
            for (int i = 0; i < images.Length; i++) {
                byte[] bytes = File.ReadAllBytes (images[i]);
                form.AddBinaryData ("images[]", bytes, Path.GetFileName (images[i]), "image/png");
            }
            UnityWebRequest request = UnityWebRequest.Post (String.Format ("{0}{1}", Host (), path), form);
            AddToRequestQueue (new RequestQueueEntry (request, onSuccess, onFailure));
        } else {
            Debug.Log ("[ServerManager ImageRequest] No Images to upload. OnFailure Callback ist going to be called.");
            onFailure ();
        }
    }

    public static string Host () {
        return String.Format ("{0}://{1}", useSecure ? protocolSecure : protocol, useSecure ? domainSecure : domain);
    }

    private static void JsonRequestHelper (string method, string path, string jsonData, Action<string> onSuccess, Action onFailure) {
        UnityWebRequest request = new UnityWebRequest (String.Format ("{0}{1}", Host (), path));
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer ();
        request.SetRequestHeader ("Content-Type", "application/json");
        request.method = method;

        if (!String.IsNullOrEmpty (jsonData)) {
            byte[] jsonToSend = new System.Text.UTF8Encoding ().GetBytes (jsonData);
            request.uploadHandler = (UploadHandler) new UploadHandlerRaw (jsonToSend);
        }
        AddToRequestQueue (new RequestQueueEntry (request, onSuccess, onFailure));
    }

    private static void AddToRequestQueue (RequestQueueEntry _request) {
        if (requestQueue == null)
            requestQueue = new List<RequestQueueEntry> ();
        requestQueue.Add (_request);
        CheckRequestQueue ();
    }
    private static void CheckRequestQueue () {
        if (requestQueue == null)
            requestQueue = new List<RequestQueueEntry> ();
        if (!isBussy && requestQueue.Count > 0) {
            RequestQueueEntry process = requestQueue[0];
            requestQueue.RemoveAt (0);
            CoroutineHelper.instance.StartCoroutine (RequestHelper (process.request, process.onSuccess, process.onFailure));
        }
    }

    private static IEnumerator RequestHelper (UnityWebRequest request, Action<string> onSuccess, Action onFailure) {
        if (isBussy) {
            Debug.Log ("[ServerManager RequestHelper] Can not handle new Request. RequestHelper is bussy. OnFailure Callback ist going to be called.");
            onFailure ();
            yield break;
        }
        isBussy = true;

        string uid = DataManager.persistedData.lastKnownUid;
        string client = DataManager.persistedData.lastKnownClient;
        string accessToken = DataManager.persistedData.lastKnownAccessToken;
        if (!String.IsNullOrEmpty (uid) && !String.IsNullOrEmpty (client) && !String.IsNullOrEmpty (accessToken)) {
            request.SetRequestHeader ("uid", uid);
            request.SetRequestHeader ("client", client);
            request.SetRequestHeader ("access-token", accessToken);
            Debug.Log ("[ServerManager RequestHelper] Used saved UID(" + uid + "), Client(" + client + ") and Access-Token(" + accessToken + ").");
        }
        Debug.Log (String.Format ("[ServerManager RequestHelper] Sending new Request: {0} {1}", request.method, request.url));
        yield return request.SendWebRequest ();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) {
            string body = request.downloadHandler.text;
            Debug.LogError ("[ServerManager RequestHelper] Error in Request Execution. OnFailure Callback ist going to be called.\nRequest Error: " + request.error + "\nDownloadHandler Text: " + body);
            onFailure ();
        } else {
            string _uid = request.GetResponseHeader ("uid");
            string _client = request.GetResponseHeader ("client");
            string _accessToken = request.GetResponseHeader ("access-token");
            if (!String.IsNullOrEmpty (_uid) && !String.IsNullOrEmpty (_client) && !String.IsNullOrEmpty (_accessToken)) {
                DataManager.persistedData.lastKnownUid = _uid;
                DataManager.persistedData.lastKnownClient = _client;
                DataManager.persistedData.lastKnownAccessToken = _accessToken;
                DataManager.Save ();
                Debug.Log ("[ServerManager RequestHelper] Received new UID, Client and Access-Token.");
            }
            string body = request.downloadHandler.text;
            Debug.Log ("[ServerManager RequestHelper] Success in Request Execution: " + body);
            onSuccess (body);
        }
        isBussy = false;
        CheckRequestQueue ();
    }
}

[Serializable]
class RequestQueueEntry {
    public UnityWebRequest request;
    public Action<string> onSuccess;
    public Action onFailure;
    public RequestQueueEntry (UnityWebRequest _request, Action<string> _onSuccess, Action _onFailure) {
        this.request = _request;
        this.onSuccess = _onSuccess;
        this.onFailure = _onFailure;
    }
}
