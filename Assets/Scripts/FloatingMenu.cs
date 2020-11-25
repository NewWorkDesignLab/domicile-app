using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloatingMenu : MonoBehaviour {
  public GameObject buttonGroup;
  public GameObject menuGroup;
  private float centerDistaceBreakpoint = .2f;
  private float buttonDistancePlayer = .5f;
  private bool endConfirmation = false;

  void Start () {
    if (buttonGroup == null || menuGroup == null)
      Debug.LogError ("[FloatingMenu Start] GameObject Reference not set.");

  }

  void Update () {
    UpdatePosition ();
  }

  void UpdatePosition () {
    Vector3 centerPos = Camera.main.transform.position + Camera.main.transform.forward.normalized * buttonDistancePlayer;
    float dist = Vector3.Distance (transform.position, centerPos);
    if (dist > centerDistaceBreakpoint) {
      Vector3 newPoint = Vector3.MoveTowards (centerPos, transform.position, centerDistaceBreakpoint);
      transform.position = newPoint;
      CloseMenu ();
    }
    transform.rotation = Quaternion.Euler (Camera.main.transform.eulerAngles);
  }

  void CloseMenu () {
    // buttonGroup.SetActive (true);
    menuGroup.SetActive (false);
  }
  void OpenMenu () {
    // buttonGroup.SetActive (true);
    menuGroup.SetActive (true);
  }
  void ToggleMenu () {
    if (menuGroup.active)
      CloseMenu ();
    else
      OpenMenu ();
  }

  public void OnPointerEnterMenuButton () {
    GazeTimer.instance.StartGazeTimer (() => {
      ToggleMenu ();
    });
  }
  public void OnPointerEnterToggleCrawl () {
    GazeTimer.instance.StartGazeTimer (() => {
      VRControls.instance.ToggleModeCrawl ();
      CloseMenu();
    });
  }
  public void OnPointerEnterTookScreenshot () {
    GazeTimer.instance.StartGazeTimer (() => {
      CloseMenu();
      buttonGroup.SetActive(false);
      HUD.instance.HideHUD();
      ScreenshotScript.instance.TakeScreenshot ((success) => {
        buttonGroup.SetActive (true);
      HUD.instance.ShowHUD();
      });
    });
  }
  public void OnPointerEnterExitScenario () {
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
  public void OnPointerExit () {
    GazeTimer.instance.StopGazeTimer ();
  }
}
