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
        Debug.Log ("[MainScenarioScript Start] Switched to MainScenario Scene.");
        SetupNetworkingInformations ();
        // DebugScreens ("/home/tobias/Pictures/domicile_screenshot_104434.png");
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
        // webTransport.clientUseWss = false;
        // webTransport.sslEnabled = false;
#else
        manager.networkAddress = ServerManager.domainSecure;
        // webTransport.clientUseWss = true;
        // webTransport.sslEnabled = true;
#endif
    }

    private void DebugScreens (string path) {
        string[] paths = { path };
        Execution.UploadImages (SessionManager.execution.id, paths, (execution) => {
            Debug.Log ("[MainScenarioScript DebugScrrenshots] Success in Image Upload. Execution: " + execution);
        }, () => {
            Debug.LogError ("[MainScenarioScript DebugScrrenshots] Error in Image Upload.");
        });
    }
}
