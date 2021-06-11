using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public static class ScreenshotManager {
#if UNITY_ANDROID
    private static bool isBusy = false;

    public static void TakeScreenshot (Action<bool> callback) {
        if (!isBusy) {
            isBusy = true;
            var notification = new List<string> ();
            notification.Add ("Screenshot in 2");
            notification.Add ("Screenshot in 1");
            notification.Add ("");
            HUD.instance.ShowNotification (notification, 1f, () => {
                // hide Hud
                GameObject overlayCam = GameObject.FindGameObjectsWithTag ("OverlayCamera") [0];
                if (overlayCam != null)
                    overlayCam.GetComponent<Camera> ().enabled = false;

                CoroutineHelper.instance.StartCoroutine (ScreenshotManager.SavePhoto ((success) => {
                    // Show Hud
                    if (overlayCam != null)
                        overlayCam.GetComponent<Camera> ().enabled = true;

                    if (success)
                        HUD.instance.ShowNotification ("Screenshot gespeichert", .6f);
                    else
                        HUD.instance.ShowNotification ("Fehler beim Speichern des Bildes", 1f);
                    isBusy = false;
                    callback (success);
                }));
            });
        } else {
            HUD.instance.ShowNotification ("Screenshot zur Zeit nicht möglich", 1f);
            Debug.LogWarning ("[ScreenshotScript TakeScreenshot] Cannot take Screenshot. Is Busy.");
        }
    }

    public static IEnumerator SavePhoto (Action<bool> callback) {
        yield return new WaitForEndOfFrame ();

#if UNITY_EDITOR
        Debug.Log ("[ScreenshotScript SavePhoto] Would only save Screenshot on Android.");
        yield return new WaitForSecondsRealtime (.5f);
        callback (true);
#else
        Texture2D image = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, false);
        image.ReadPixels (new Rect (0, 0, Screen.width, Screen.height), 0, 0);
        image.Apply ();

        string myFileName = "domicile_screenshot_" + System.DateTime.Now.Hour + System.DateTime.Now.Minute + System.DateTime.Now.Second + ".png";
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery (image, "Domicile VR", myFileName, (success, path) => {
            if (success) {
                Debug.Log ("[ScreenshotScript SavePhoto] Successfully saved Screenshot to: " + path);

                // Upload image
                string[] paths = { path };
                Execution.UploadImages (SessionManager.execution.id, paths, (execution) => {
                    Debug.Log ("[ScreenshotScript SavePhoto] Success in Image Upload. Execution: " + execution);
                    callback (true);
                }, () => {
                    Debug.LogError ("[EndScenario Start] Error in Image Upload.");
                    callback (false);
                });
            } else {
                Debug.LogError ("[ScreenshotScript SavePhoto] Error while creating Screenshot.");
                callback (false);
            }
        });
        Debug.Log ("[ScreenshotScript SavePhoto] Image Permission: " + permission);
        // CoroutineHelper.instance.Destroy (image);
#endif
    }
#else
    public static void TakeScreenshot (Action<bool> callback) {
        Debug.LogWarning ("[ScreenshotManager TakeScreenshot] Will only took Screenshot on Android.");
        callback (false);
    }
#endif
}
