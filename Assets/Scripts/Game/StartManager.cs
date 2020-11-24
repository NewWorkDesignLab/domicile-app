using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour {
    void Start () {
        CrashReportManager.instance.MailLog ();

        if (DataManager.getValue.saveLogin) {
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
