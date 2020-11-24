using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneStarter : MonoBehaviour {
  void Start () {
    SetupDataManager ();
    SetupCrashReportManager ();

    PermissionManager.ManagePermissions (() => {
      SceneManager.LoadScene ("ManualPermissionScene");
    }, () => {
      if (DataManager.persistedData.saveLogin) {
        User.CheckToken ((success) => {
          SceneManager.LoadScene ("MainMenuScene");
        }, () => {
          SceneManager.LoadScene ("UserManagementScene");
        });
      } else {
        SceneManager.LoadScene ("UserManagementScene");
      }
    });
  }

  private void SetupDataManager () {
    DataManager.Load ();
    DataManager.ResetSessionData ();
  }

  private void SetupCrashReportManager () {
    CrashReportManager.MailLog ();
    Application.logMessageReceived += CrashReportManager.LogCallback;
  }
}
