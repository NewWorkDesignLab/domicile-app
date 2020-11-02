using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameMenuScript : MonoBehaviour
{
  public static IngameMenuScript instance;
  public GameObject mainMenu;
  private bool endConfirmation = false;

  void Awake()
  {
    instance = this;
  }

  void Start()
  {
    mainMenu.SetActive(false);
  }

  void Update()
  {
    UpdateFootMenuPosition();
  }

  void UpdateFootMenuPosition()
  {
    if (gameObject.active)
    {
      Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
      Vector3 fixedForward = new Vector3(forward.x, Camera.main.transform.position.y, forward.z).normalized;
      Quaternion menuRotation = Quaternion.LookRotation(Vector3.up * -1, fixedForward);
      transform.rotation = menuRotation;
    }
  }

  public void OpenMainMenu()
  {
    GazeTimer.instance.StartGazeTimer(() =>
    {
      UpdateMainMenuPosition();
      mainMenu.SetActive(true);
    });
  }
  public void CloseMainMenu()
  {
    GazeTimer.instance.StartGazeTimer(() =>
    {
      mainMenu.SetActive(false);
    });
  }

  void UpdateMainMenuPosition()
  {
    var target = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
    target = target.normalized * .5f;
    mainMenu.transform.position = Camera.main.transform.position + target;
    mainMenu.transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
  }


  public void ToggleCrawl()
  {
    GazeTimer.instance.StartGazeTimer(() =>
    {
      VRControls.instance.ToggleModeCrawl();
      mainMenu.SetActive(false);
    });
  }
  public void TookScreenshot()
  {
    GazeTimer.instance.StartGazeTimer(() =>
    {
      mainMenu.SetActive(false);
      ScreenshotScript.instance.TakeScreenshot(() => { });
    });
  }
  public void ExitScenario()
  {
    GazeTimer.instance.StartGazeTimer(() =>
    {
      if (endConfirmation == false)
      {
        endConfirmation = true;
        HUD.instance.ShowNotification("Willst du wirklich beenden? Bitte bestätige durch erneutes beenden.", 5f, () =>
            {
              endConfirmation = false;
            });
      }
      else
      {
        SceneManager.LoadScene("EndScene");
      }
    });

  }
  public void StopGazeTimer()
  {
    GazeTimer.instance.StopGazeTimer();
  }
}
