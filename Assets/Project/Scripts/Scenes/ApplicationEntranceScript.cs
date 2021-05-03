using System;
#if UNITY_WEBGL
using System.Runtime.InteropServices;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationEntranceScript : MonoBehaviour {
    public PermissionPopupComponent permissionPrefab;

#if UNITY_WEBGL
    // Import Javascript Functions from Assets/Plugins/javascript.jslib to run in Brwoser in WebGL, as
    // describet here: https://docs.unity3d.com/Manual/webgl-interactingwithbrowserscripting.html
    [DllImport ("__Internal")]
    private static extern string GetParameterFromBrwoser (string parameterName);
#endif

    void Start () {
#if UNITY_ANDROID
        Debug.Log ("[ApplicationEntranceScript Start] Plattform: Android");
        DataManager.Load ();
        SetupCrashReportManager ();
        DeepLinkManager.SetupHook ();
        StartProcedure (CheckAndroidPermissions);
#elif UNITY_WEBGL
        Debug.Log ("[ApplicationEntranceScript Start] Plattform: WebGL");
        DataManager.Load ();
        SetupCrashReportManager ();
        StartProcedure (CheckWebglUrlParameter);
#elif UNITY_STANDALONE_LINUX
        Debug.Log ("[ApplicationEntranceScript Start] Plattform: Standalone Linux");
        SceneManager.LoadScene ("3_MainScenarioScene");
#endif
    }

    // check for Server Availabillity
    private void StartProcedure (Action methodToInvoke) {
        ServerManager.CheckServerAvailabillity (() => {
            // Servers available
            methodToInvoke ();
        }, () => {
            // Servers not available
            // TODO
        });
    }

#if UNITY_ANDROID
    // check android permissions for storage read/write (required for screenshots)
    public void CheckAndroidPermissions () {
        PermissionManager.ManagePermissions (() => {
            // on permission denied
            permissionPrefab.OpenPopupDenied ();
        }, () => {
            // on permission should ask
            permissionPrefab.OpenPopupShouldAsk ();
        }, () => {
            // on permission granted
            CheckUserAuthentification (AndroidFinnishEntrance);
        });
    }
#endif

#if UNITY_WEBGL
#if UNITY_EDITOR
    private void CheckWebglUrlParameter () {
        User.SignIn (EditorSimulator.instance.debugWebglUrlParameterEmail, EditorSimulator.instance.debugWebglUrlParameterPassword, (success) => {
            CheckUserAuthentification (WebglFinnishEntrance);
        }, () => { });
    }
#else
    private void CheckWebglUrlParameter () {
        string _uid = GetParameterFromBrwoser ("uid");
        string _client = GetParameterFromBrwoser ("client");
        string _accessToken = GetParameterFromBrwoser ("access-token");
        if (!String.IsNullOrEmpty (_uid) && !String.IsNullOrEmpty (_client) && !String.IsNullOrEmpty (_accessToken)) {
            DataManager.persistedData.lastKnownUid = _uid;
            DataManager.persistedData.lastKnownClient = _client;
            DataManager.persistedData.lastKnownAccessToken = _accessToken;
            DataManager.Save ();
        }
        CheckUserAuthentification (WebglFinnishEntrance);
    }
#endif
#endif

    // check if user is logged in and session still valid
    private void CheckUserAuthentification (Action<bool> methodToInvoke) {
        User.CheckToken ((success) => {
            // on user login success
            methodToInvoke (true);
        }, () => {
            // on user login failure
            methodToInvoke (false);
        });
    }

#if UNITY_ANDROID
    private void AndroidFinnishEntrance (bool authSuccess) {
        if (authSuccess)
            SceneManager.LoadScene ("2_PreScenarioScene");
        else
            SceneManager.LoadScene ("1_LoginScene");
    }
#endif

#if UNITY_WEBGL
    private void WebglFinnishEntrance (bool authSuccess) {
        if (authSuccess) {
#if UNITY_EDITOR
            int scenario = EditorSimulator.instance.debugWebglUrlParameterScenario;
#else
            int scenario = int.Parse (GetParameterFromBrwoser ("scenario"));
#endif
            SessionManager.DefineScenario (scenario, () => {
                // Session Load Success
                Debug.Log ("[DEBUG ApplicationEntranceScript WebglFinnishEntrance] Loaded Scenario. Is Owner: " + SessionManager.IsOwner ());
                SceneManager.LoadScene ("3_MainScenarioScene");
            }, () => {
                // Session Load Error
                Debug.LogError ("[ApplicationEntranceScript WebglFinnishEntrance] Session could not be Loaded. Exit.");
            });
        } else {
            Debug.Log ("[ApplicationEntranceScript WebglFinnishEntrance] User could not be Authenticated. Exit.");
        }
    }
#endif

    private void SetupCrashReportManager () {
        CrashReportManager.MailLog ();
        Application.logMessageReceived += CrashReportManager.LogCallback;
    }
}
