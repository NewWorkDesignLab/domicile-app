using System.Collections;
using System.Collections.Generic;
using kcp2k;
using Mirror;
using Mirror.SimpleWeb;
using UnityEngine;

public class MainScenarioScript : Singleton<MainScenarioScript> {
    public NetworkManager manager;
    public NetworkAddressModi modi;
    public GameObject productionNetworkManagerPrefab;
    public GameObject editorNetworkManagerPrefab;

    void Start () {
        SetupNetworkingInformations ();
#if UNITY_EDITOR
        Debug.Log ("[MainScenarioScript Start] Plattform: EDITOR");
#elif UNITY_ANDROID
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
        var instance = Instantiate (editorNetworkManagerPrefab, new Vector3 (0, 0, 0), Quaternion.identity);
        manager = instance.GetComponent<NetworkManager> ();
        var val = SetFromModi ("localhost");
        Debug.Log ("[MainScenarioScript SetupNetworkingInformations] Used " + val);
        manager.networkAddress = val;
#else
        var instance = Instantiate (productionNetworkManagerPrefab, new Vector3 (0, 0, 0), Quaternion.identity);
        manager = instance.GetComponent<NetworkManager> ();
        var val = SetFromModi (ServerManager.domainSecure);
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
                return ServerManager.domainSecure;
            default:
                return plattformDependent;
        }
    }
}

public enum NetworkAddressModi { auto, local, server }
