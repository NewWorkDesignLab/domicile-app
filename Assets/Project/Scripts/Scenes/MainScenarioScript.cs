using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.SimpleWeb;
using UnityEngine;

public class MainScenarioScript : MonoBehaviour {
    public NetworkManager manager;
    public SimpleWebTransport simpleWebTransport;
    public NetworkAddressModi modi;
    string productionServerAddress = "domicile.tobiasbohn.com";

    public string webglScenarioID;
    public bool webglIsPlayer = false;

    // Import Javascript Functions from Assets/Plugins/javascript.jslib to run in Brwoser in WebGL, as
    // describet here: https://docs.unity3d.com/Manual/webgl-interactingwithbrowserscripting.html
    [DllImport ("__Internal")]
    private static extern string GetScenarioInformationFromBrwoser ();

    [DllImport ("__Internal")]
    private static extern string GetPlayerInformationFromBrwoser ();

  void Start()
  {
    SetupNetworkingInformations();
#if UNITY_ANDROID
        Debug.Log ("[MainScenarioScript Start] Plattform: Android");
        manager.StartClient ();
#elif UNITY_WEBGL
        Debug.Log ("[MainScenarioScript Start] Plattform: WebGL");
        var _scenarioId = GetScenarioInformationFromBrwoser ();
        var _isPlayer = GetPlayerInformationFromBrwoser ();
        if (_scenarioId != null && _isPlayer != null) {
            webglScenarioID = _scenarioId;
            webglIsPlayer = bool.Parse(_isPlayer);
            manager.StartClient ();
        }
#elif UNITY_STANDALONE_LINUX
        Debug.Log ("[MainScenarioScript Start] Plattform: Standalone Linux");
        manager.StartServer ();
#endif
  }

  private void SetupNetworkingInformations () {
#if UNITY_EDITOR
        var val = SetFromModi ("localhost");
        Debug.Log ("[MainScenarioScript SetupNetworkingInformations] Used " + val);
        manager.networkAddress = val;
        simpleWebTransport.clientUseWss = false;
        simpleWebTransport.sslEnabled = false;
#else
        var val = SetFromModi (productionServerAddress);
        Debug.Log ("[MainScenarioScript SetupNetworkingInformations] Used " + val);
        manager.networkAddress = val;
        simpleWebTransport.clientUseWss = true;
        simpleWebTransport.sslEnabled = true;
#endif
    }

    private string SetFromModi (string plattformDependent) {
        switch (modi) {
            case NetworkAddressModi.auto:
                return plattformDependent;
            case NetworkAddressModi.local:
                return "localhost";
            case NetworkAddressModi.server:
                return productionServerAddress;
            default:
                return plattformDependent;
        }
    }
}

public enum NetworkAddressModi { auto, local, server }
