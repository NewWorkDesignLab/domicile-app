using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationEntranceScript : MonoBehaviour {
    public PermissionPopupComponent permissionPrefab;

    void Start () {
        SetupDataManager ();
        SetupCrashReportManager ();
        SetupDeepLinkManager ();
        StartProcedure ();
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

    private void SetupDataManager () {
        DataManager.Load ();
    }

    private void SetupCrashReportManager () {
        CrashReportManager.MailLog ();
        Application.logMessageReceived += CrashReportManager.LogCallback;
    }

    private void SetupDeepLinkManager () {
        DeepLinkManager.SetupHook ();
    }
}
