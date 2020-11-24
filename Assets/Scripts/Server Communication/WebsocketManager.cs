using System;
using System.Collections;
using UnityEngine;
using WebSocketSharp;

public class WebsocketManager : MonoBehaviour {
  public static WebsocketManager instance;
  WebSocket webSocket;
  bool isConnected = false;
  bool isSubscribed = false;
  bool isBussy = false;

  void Awake () {
    instance = this;
  }

  void Start () {
    Connect ();
  }

  void Connect () {
    User.CheckToken ((success) => {
      string uid = DataManager.persistedData.lastKnownUid;
      string client = DataManager.persistedData.lastKnownClient;
      string accessToken = DataManager.persistedData.lastKnownAccessToken;
      string _protocol = ServerManager.useSecure ? ServerManager.websocketSecure : ServerManager.websocket;
      string _domain = ServerManager.useSecure ? ServerManager.domainSecure : ServerManager.domain;

      string url = String.Format ("{0}://{1}/cable/?access-token={2}&client={3}&uid={4}&encoding=text", _protocol, _domain, accessToken, client, uid);
      webSocket = new WebSocket (url);

      // Fixed Error: Websocket REquest was not able to pass reverse proxy - so no logs at all on Rails side
      // In Caddy Logs there was an Error: "http: TLS handshake error from <IP>: tls: client offered only unsupported versions: [301]"
      // Fixed as describet in https://github.com/Microsoft/BotBuilder-Samples/issues/279
      webSocket.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;

      webSocket.OnOpen += (sender, e) => {
        Debug.Log ("[WebsocketManager onOpen] WebSocket-> Open:");
        Debug.Log ("[WebsocketManager onOpen] Open socket-> OnOpen: " + webSocket.ReadyState);
        isConnected = true;
        SendData (new SendMessage ("subscribe", "SpectatorChannel", DataManager.sessionData.participation.id.ToString ()), true);
      };
      webSocket.OnMessage += (sender, e) => {
        if (e.IsText) {
          ReceiveMessage data = new ReceiveMessage (e.Data);
          switch (data.type) {
            case "welcome":
              Debug.Log (String.Format ("[WebsocketManager onMessage] [{0}]", data.type));
              break;
            case "ping":
              // Debug.Log(String.Format("[WebsocketManager onMessage] [{0}]", data.type));
              break;
            case "confirm_subscription":
              Debug.Log (String.Format ("[WebsocketManager onMessage] [{0}]", data.type));
              isSubscribed = true;
              break;
            case "message":
              // Debug.Log(String.Format("[WebsocketManager onMessage] [{0}] {1}", data.type, e.Data));
              break;
            default:
              // Debug.LogWarning(String.Format("[WebsocketManager onMessage] [{0}] Unknown Message Type. Data:\n{1}", data.type, e.Data));
              break;
          }
        }
      };
      webSocket.OnError += (sender, e) => {
        Debug.LogError ("[WebsocketManager onError] WebSocket-> Error: " + e.Message);
        Debug.LogError ("[WebsocketManager onError] Open socket-> OnError: " + webSocket.ReadyState);
      };
      webSocket.OnClose += (sender, e) => {
        Debug.Log ("[WebsocketManager onClose] WebSocket-> Close-code: " + e.Code);
        Debug.Log ("[WebsocketManager onClose] WebSocket-> Close-reason: " + e.Reason);
        Debug.Log ("[WebsocketManager onClose] Open socket-> OnClose: " + webSocket.ReadyState);
        isConnected = false;
        isSubscribed = false;
      };

      webSocket.Connect ();
    }, () => { });
  }

  void OnApplicationQuit () {
    if (webSocket != null) {
      webSocket.Close ();
    }
  }

  public void SendData (SendMessage message) {
    SendData (message, false);
  }
  void SendData (SendMessage message, bool forceUnsubscribed) {
    if (isConnected && (isSubscribed || forceUnsubscribed)) {
      var jsonData = JsonUtility.ToJson (message);
      webSocket.Send (jsonData);
    } else {
      Debug.LogWarning ("[WebsocketManager SendData] Should send Data via Websocket but Connection or Subscribtion not ready jet.");
    }
  }
}

[System.Serializable]
public class SendMessage {
  public SendMessage (string _command, string _channel, string _id, string _action, string _parameter) {
    this.command = _command;
    this.identifier = JsonUtility.ToJson (new Identifier (_channel, _id));
    this.data = JsonUtility.ToJson (new Data (_action, _parameter));
  }
  public SendMessage (string _command, string _channel, string _id) {
    this.command = _command;
    this.identifier = JsonUtility.ToJson (new Identifier (_channel, _id));
    this.data = JsonUtility.ToJson (new Data ("", ""));
  }
  public string command;
  public string identifier;
  public string data;
}

[System.Serializable]
class ReceiveMessage {
  public ReceiveMessage (string data) {
    this.helper = JsonUtility.FromJson<ReceiveMessageHelper> (data);
    this.type = this.helper.type;
    this.message = this.helper.message;
    this.identifier = String.IsNullOrEmpty (this.helper.identifier) ? new Identifier ("", "") : JsonUtility.FromJson<Identifier> (this.helper.identifier);
    this.data = String.IsNullOrEmpty (this.helper.data) ? new Data ("", "") : JsonUtility.FromJson<Data> (this.helper.data);
  }
  public string type;
  public string message;
  public Identifier identifier;
  public Data data;
  private ReceiveMessageHelper helper;
}

[System.Serializable]
class ReceiveMessageHelper {
  public string type;
  public string message;
  public string identifier;
  public string data;
}

[System.Serializable]
public class Identifier {
  public Identifier (string _channel, string _id) {
    this.channel = _channel;
    this.id = _id;
  }
  public string channel;
  public string id;
}

[System.Serializable]
public class Data {
  public Data (string _action, string _parameter) {
    this.action = _action;
    this.parameter = _parameter;
  }
  public string action;
  public string parameter;
}
