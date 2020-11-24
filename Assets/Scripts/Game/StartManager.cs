using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour {
    void Start () {
        DataManager.Load();
        DataManager.ResetSessionData();
        CrashReportManager.instance.MailLog ();

        if (DataManager.persistedData.saveLogin) {
            User.CheckToken ((success) => {
                SceneManager.LoadScene ("MainMenuScene");
            }, () => {
                SceneManager.LoadScene ("UserManagementScene");
            });
        } else {
            SceneManager.LoadScene ("UserManagementScene");
        }
    }
}
