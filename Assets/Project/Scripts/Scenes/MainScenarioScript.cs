using System.Collections;
using System.Collections.Generic;
using kcp2k;
using Mirror;
using Mirror.SimpleWeb;
using UnityEngine;

public class MainScenarioScript : Singleton<MainScenarioScript> {
    public NetworkManager manager;
    public SimpleWebTransport webTransport;

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
        manager.networkAddress = "localhost";
        webTransport.clientUseWss = false;
        webTransport.sslEnabled = false;
#else
        manager.networkAddress = ServerManager.domainSecure;
        webTransport.clientUseWss = true;
        webTransport.sslEnabled = true;
#endif
    }
}
