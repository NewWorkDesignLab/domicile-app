using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class ScreenshotScript : MonoBehaviour {
  public static ScreenshotScript instance;
  bool isBusy = false;

  void Awake () {
    instance = this;
  }

  public void TakeScreenshot (Action<bool> callback) {
    if (!isBusy) {
      isBusy = true;
      var notification = new List<string> ();
      notification.Add ("Screenshot in 2");
      notification.Add ("Screenshot in 1");
      notification.Add ("");
      HUD.instance.ShowNotification (notification, 1f, () => {
        StartCoroutine (SavePhoto ((success) => {
          if (success)
            HUD.instance.ShowNotification ("Screenshot gespeichert", .6f);
          else
            HUD.instance.ShowNotification ("Fehler beim Speichern des Bildes", 1f);
          isBusy = false;
          callback (success);
        }));
      });
    } else {
      Debug.LogWarning ("[ScreenshotScript TakeScreenshot] Cannot take Screenshot. Is Busy.");
    }
  }

  public IEnumerator SavePhoto (Action<bool> callback) {
    yield return new WaitForEndOfFrame ();

#if PLATFORM_ANDROID
    Texture2D image = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, false);
    image.ReadPixels (new Rect (0, 0, Screen.width, Screen.height), 0, 0);
    image.Apply ();

    string myFileName = "domicile_screenshot_" + System.DateTime.Now.Hour + System.DateTime.Now.Minute + System.DateTime.Now.Second + ".png";
    NativeGallery.Permission permission = NativeGallery.SaveImageToGallery (image, "Domicile VR", myFileName, (success, path) => {
      if (success) {
        Debug.Log ("[ScreenshotScript SavePhoto] Successfully saved Screenshot to: " + path);
      } else {
        Debug.LogError ("[ScreenshotScript SavePhoto] Error while creating Screenshot.");
      }
      callback (success);
    });
    Debug.Log ("[ScreenshotScript SavePhoto] Image Permission: " + permission);
    Destroy (image);
#else
    Debug.Log ("[ScreenshotScript SavePhoto] Would only save Screenshot on Android.");
    callback (true);
#endif
  }
}
