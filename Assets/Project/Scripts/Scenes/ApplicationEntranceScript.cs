using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationEntranceScript : MonoBehaviour {
    public PermissionPopupComponent permissionPrefab;

  void Start()
  {
#if UNITY_ANDROID
        Debug.Log ("[ApplicationEntranceScript Start] Plattform: Android");
        DataManager.Load ();
        SetupCrashReportManager ();
        DeepLinkManager.SetupHook ();
        StartProcedure ();
#elif UNITY_WEBGL
        Debug.Log ("[ApplicationEntranceScript Start] Plattform: WebGL");
        SceneManager.LoadScene ("3_MainScenarioScene");
#elif UNITY_STANDALONE_LINUX
        Debug.Log ("[ApplicationEntranceScript Start] Plattform: Standalone Linux");
        SceneManager.LoadScene ("3_MainScenarioScene");
#endif
  }

  // check for Server Availabillity
  public void StartProcedure () {
        ServerManager.CheckServerAvailabillity (() => {
            // Servers available
            CheckAndroidPermissions ();
        }, () => {
            // Servers not available
            // TODO
        });
    }

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
            CheckUserAuthentification ();
        });
    }

    // check if user is logged in and session still valid
    public void CheckUserAuthentification () {
        User.CheckToken ((success) => {
            // on user login success
            SceneManager.LoadScene ("2_PreScenarioScene");
        }, () => {
            // on user login failure
            SceneManager.LoadScene ("1_LoginScene");
        });
    }

    private void SetupCrashReportManager () {
        CrashReportManager.MailLog ();
        Application.logMessageReceived += CrashReportManager.LogCallback;
    }
}
