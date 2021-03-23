using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MainScenarioScript : MonoBehaviour {
    public NetworkManager manager;
    public NetworkAddressModi modi;
    string productionServerAddress = "domicile.tobiasbohn.com";

    void Start () {
        SetupNetworkingInformations ();
#if UNITY_ANDROID
        Debug.Log ("[MainScenarioScript Start] Plattform: Android");
        manager.StartClient ();
#elif UNITY_WEBGL
        Debug.Log ("[MainScenarioScript Start] Plattform: WebGL");
        manager.StartClient ();
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
#else
        var val = SetFromModi (productionServerAddress);
        Debug.Log ("[MainScenarioScript SetupNetworkingInformations] Used " + val);
        manager.networkAddress = val;
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
