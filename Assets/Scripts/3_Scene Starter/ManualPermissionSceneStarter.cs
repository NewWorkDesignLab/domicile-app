using UnityEngine;
using UnityEngine.SceneManagement;

public class ManualPermissionSceneStarter : MonoBehaviour {
  public void ButtonPermissionSettings () {
    PermissionManager.OpenSettings ();
  }

  public void ButtonBack () {
    SceneManager.LoadScene ("StartScene");
  }
}
