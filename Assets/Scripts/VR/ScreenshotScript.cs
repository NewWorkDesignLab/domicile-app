using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class ScreenshotScript : MonoBehaviour
{
  public static ScreenshotScript instance;
  bool isBusy = false;
  void Awake()
  {
    instance = this;
  }

  // private bool isSingleTap = false;
  // private float lastTap;
  // private float doubleTapOffset = .5f;

  // void Start()
  // {
  //   lastTap = Time.realtimeSinceStartup;
  // }

  // void Update()
  // {
  //   if (Input.touchCount > 0 || Input.GetKeyDown("space") || Input.GetMouseButtonDown(0))
  //   {
  //     if (!isSingleTap)
  //     {
  //       isSingleTap = true;
  //       StartCoroutine(WaitForSecondTap());
  //     }
  //     else if ((Time.realtimeSinceStartup - lastTap) < doubleTapOffset)
  //     {
  //       // IS DOUBLE TAP
  //       isSingleTap = false;
  //       Debug.Log("DOUBLE TAP");
  //       EndScenario.instance.InvokeEndScenario();
  //     }
  //     lastTap = Time.realtimeSinceStartup;
  //   }
  // }

  // private IEnumerator WaitForSecondTap()
  // {
  //   yield return new WaitForSecondsRealtime(doubleTapOffset);
  //   if (isSingleTap)
  //   {
  //     // IS SINGLE TAP
  //     TakeScreenshot();
  //     Debug.Log("SINGLE TAP");
  //     isSingleTap = false;
  //   }
  // }

  // public void TakeScreenshot()
  // {
  //   string time = System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
  //   string name = string.Format("{0}/domicile_{1}.png", Application.persistentDataPath, time);
  //   ScreenCapture.CaptureScreenshot(name);
  //   CrossSceneManager.instance.unsavedScreenshots = CrossSceneManager.instance.unsavedScreenshots == null ? new List<string>() : CrossSceneManager.instance.unsavedScreenshots;
  //   CrossSceneManager.instance.unsavedScreenshots.Add(name);
  // }

  public void TakeScreenshot(Action callback)
  {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        }
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
#endif
    if (!isBusy)
    {
      isBusy = true;
      var notification = new List<string>();
      notification.Add("Screenshot in 3");
      notification.Add("Screenshot in 2");
      notification.Add("Screenshot in 1");
      HUD.instance.ShowNotification(notification, 1f, () =>
          {
            StartCoroutine(SaveScreenshot(() =>
            {
              HUD.instance.ShowNotification("Screenshot gespeichert", 1f);
              isBusy = false;
              callback();
            }));
          });
    }
    else
    {
      Debug.LogWarning("[ScreenshotScript TakeScreenshot] Cannot take Screenshot. Is Busy.");
    }
  }

  IEnumerator SaveScreenshot(Action callback)
  {
    yield return new WaitForEndOfFrame();

    string myFileName = "screenshot_domicile_" + System.DateTime.Now.Hour + System.DateTime.Now.Minute + System.DateTime.Now.Second + ".png";
    string myDefaultLocation = Application.persistentDataPath + "/" + myFileName;

    ScreenCapture.CaptureScreenshot(myFileName);
    yield return new WaitForSeconds(1);

    CrossSceneManager.instance.unsavedScreenshots = CrossSceneManager.instance.unsavedScreenshots == null ? new List<string>() : CrossSceneManager.instance.unsavedScreenshots;
    CrossSceneManager.instance.unsavedScreenshots.Add(myDefaultLocation);
    callback();
  }
}
