using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameMenuScript : MonoBehaviour {
  public static IngameMenuScript instance;
  public GameObject mainMenu;
  public GameObject menuButton;
  private bool endConfirmation = false;

  void Awake () {
    instance = this;
  }

  void Start () {
    mainMenu.SetActive (false);
    menuButton.SetActive (true);
  }

  void Update () {
    UpdateFootMenuPosition ();
  }

  void UpdateFootMenuPosition () {
    if (gameObject.active) {
      Vector3 forward = Camera.main.transform.TransformDirection (Vector3.forward);
      Vector3 fixedForward = new Vector3 (forward.x, Camera.main.transform.position.y, forward.z).normalized;
      Quaternion menuRotation = Quaternion.LookRotation (Vector3.up * -1, fixedForward);
      transform.rotation = menuRotation;
    }
  }

  public void OpenMainMenu () {
    GazeTimer.instance.StartGazeTimer (() => {
      UpdateMainMenuPosition ();
      mainMenu.SetActive (true);
      menuButton.SetActive (false);
    });
  }
  public void CloseMainMenu () {
    GazeTimer.instance.StartGazeTimer (() => {
      mainMenu.SetActive (false);
      menuButton.SetActive (true);
    });
  }

  void UpdateMainMenuPosition () {
    var target = new Vector3 (Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
    target = target.normalized * .5f;
    mainMenu.transform.position = Camera.main.transform.position + target;
    mainMenu.transform.rotation = Quaternion.Euler (0, Camera.main.transform.eulerAngles.y, 0);
  }

  public void ToggleCrawl () {
    GazeTimer.instance.StartGazeTimer (() => {
      VRControls.instance.ToggleModeCrawl ();
      mainMenu.SetActive (false);
      menuButton.SetActive (true);
    });
  }
  public void TookScreenshot () {
    GazeTimer.instance.StartGazeTimer (() => {
      mainMenu.SetActive (false);
      menuButton.SetActive (false);
      ScreenshotScript.instance.TakeScreenshot ((success) => {
        menuButton.SetActive (true);
      });
    });
  }
  public void ExitScenario () {
    GazeTimer.instance.StartGazeTimer (() => {
      if (endConfirmation == false) {
        endConfirmation = true;
        HUD.instance.ShowNotification ("Willst du wirklich beenden? Bitte bestätige durch erneutes beenden.", 5f, () => {
          endConfirmation = false;
        });
      } else {
        SceneManager.LoadScene ("EndScene");
      }
    });

  }
  public void StopGazeTimer () {
    GazeTimer.instance.StopGazeTimer ();
  }
}
