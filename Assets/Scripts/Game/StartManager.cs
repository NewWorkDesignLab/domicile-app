using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    void Start()
    {
        CrashReportManager.instance.MailLog();

        if (DataManager.getValue.saveLogin)
        {
            API.auth.CheckToken((success) =>
            {
                SceneManager.LoadScene("MainMenuScene");
            }, (failure) =>
            {
                SceneManager.LoadScene("UserManagementScene");
            });
        }
        else
        {
            SceneManager.LoadScene("UserManagementScene");
        }
    }
}
