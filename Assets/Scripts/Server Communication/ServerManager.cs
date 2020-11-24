using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(ScenarioRequests))]
[RequireComponent(typeof(ParticipationRequests))]
[RequireComponent(typeof(ExecutionRequests))]
[RequireComponent(typeof(FileRequests))]
[RequireComponent(typeof(AuthRequests))]
public class ServerManager : MonoBehaviour
{
  public static ServerManager instance;
  public static ScenarioRequests scenario;
  public static ParticipationRequests participation;
  public static ExecutionRequests execution;
  public static FileRequests file;
  public static AuthRequests auth;

  [HideInInspector]
  public string host;
  public bool useSecure = false;
  public string domain = "localhost:3000";
  public string domainSecure = "domicile.tobiasbohn.com";
  public string protocol = "http";
  public string protocolSecure = "https";
  public string websocket = "ws";
  public string websocketSecure = "wss";


  void Awake()
  {
    instance = this;
  }

  void Start()
  {
    scenario = GetComponent<ScenarioRequests>();
    participation = GetComponent<ParticipationRequests>();
    execution = GetComponent<ExecutionRequests>();
    file = GetComponent<FileRequests>();
    auth = GetComponent<AuthRequests>();
    host = String.Format("{0}://{1}", useSecure ? protocolSecure : protocol, useSecure ? domainSecure : domain);
  }

  public void GetRequest(string path, string jsonData, Action<string> onSuccess, Action<string> onFailure)
  {
    string method = UnityWebRequest.kHttpVerbGET;
    JsonRequestHelper(method, path, jsonData, onSuccess, onFailure);
  }

  public void PostRequest(string path, string jsonData, Action<string> onSuccess, Action<string> onFailure)
  {
    string method = UnityWebRequest.kHttpVerbPOST;
    JsonRequestHelper(method, path, jsonData, onSuccess, onFailure);
  }

  public void PutRequest(string path, string jsonData, Action<string> onSuccess, Action<string> onFailure)
  {
    string method = UnityWebRequest.kHttpVerbPUT;
    JsonRequestHelper(method, path, jsonData, onSuccess, onFailure);
  }

  public void DeleteRequest(string path, string jsonData, Action<string> onSuccess, Action<string> onFailure)
  {
    string method = UnityWebRequest.kHttpVerbDELETE;
    JsonRequestHelper(method, path, jsonData, onSuccess, onFailure);
  }



  private void JsonRequestHelper(string method, string path, string jsonData, Action<string> onSuccess, Action<string> onFailure)
  {
    UnityWebRequest request = new UnityWebRequest(String.Format("{0}{1}", host, path));
    request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");
    request.method = method;

    if (!String.IsNullOrEmpty(jsonData))
    {
      byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
      request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
    }
    StartCoroutine(RequestHelper(request, onSuccess, onFailure));
  }


  public IEnumerator RequestHelper(UnityWebRequest request, Action<string> onSuccess, Action<string> onFailure)
  {
    string uid = DataManager.getValue.lastKnownUid;
    string client = DataManager.getValue.lastKnownClient;
    string accessToken = DataManager.getValue.lastKnownAccessToken;
    if (!String.IsNullOrEmpty(uid) && !String.IsNullOrEmpty(client) && !String.IsNullOrEmpty(accessToken))
    {
      request.SetRequestHeader("uid", uid);
      request.SetRequestHeader("client", client);
      request.SetRequestHeader("access-token", accessToken);
      Debug.Log(String.Format("[ServerManager RequestHelper] Acces-Token (now_in_use): {0} (uid: {1}, client: {2})", accessToken, uid, client));
    }
    yield return request.SendWebRequest();

    if (request.isNetworkError || request.isHttpError)
    {
      Debug.LogError("[ServerManager RequestHelper] Error in Request Execution: " + request.error + "; Requested: " + request.url);
      string body = request.downloadHandler.text;
      onFailure(body);
    }
    else
    {
      string _uid = request.GetResponseHeader("uid");
      string _client = request.GetResponseHeader("client");
      string _accessToken = request.GetResponseHeader("access-token");
      if (!String.IsNullOrEmpty(_uid) && !String.IsNullOrEmpty(_client) && !String.IsNullOrEmpty(_accessToken))
      {
        DataManager.getValue.lastKnownUid = _uid;
        DataManager.getValue.lastKnownClient = _client;
        DataManager.getValue.lastKnownAccessToken = _accessToken;
        DataManager.instance.Save();
        Debug.Log(String.Format("[ServerManager RequestHelper] Acces-Token (new): {0} (uid: {1}, client: {2})", _accessToken, _uid, _client));
      }
      string body = request.downloadHandler.text;
      Debug.Log("[ServerManager RequestHelper] Success in Request Execution: " + body + "; Request: " + request.url);
      onSuccess(body);
    }
  }
}
