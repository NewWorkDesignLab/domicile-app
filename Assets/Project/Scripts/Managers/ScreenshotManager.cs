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

        Camera cam = GameObject.FindGameObjectWithTag ("ScreenshotCamera").GetComponent<Camera> ();
        var resWidth = (int) cam.pixelWidth;
        var resHeight = (int) cam.pixelHeight;
        RenderTexture rt = new RenderTexture (resWidth, resHeight, 24);
        cam.targetTexture = rt;
        cam.Render ();
        Texture2D image = new Texture2D (resWidth, resHeight, TextureFormat.RGB24, false);
        RenderTexture.active = rt;
        image.ReadPixels (cam.pixelRect, 0, 0);
        image.Apply ();

        string fileName = $"domicile_screenshot_{System.DateTime.Now.ToString("o")}.png";
        bool imageSuccess = false;
        string path = null;

#if UNITY_EDITOR
        byte[] bytes = image.EncodeToPNG ();
        path = $"{Application.persistentDataPath}/{fileName}";
        System.IO.File.WriteAllBytes (path, bytes);
        Debug.Log ($"[ScreenshotScript SavePhoto] Saved Screenshot to: {path}");
        imageSuccess = true;
#else
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery (image, "Domicile VR", fileName, (success, nativeGalleryPath) => {
            if (success) {
                Debug.Log ("[ScreenshotScript SavePhoto] Successfully saved Screenshot to: " + nativeGalleryPath);
                imageSuccess = true;
                path = nativeGalleryPath;
            } else {
                Debug.LogError ("[ScreenshotScript SavePhoto] Error while creating Screenshot.");
            }
        });
        Debug.Log ("[ScreenshotScript SavePhoto] Image Permission: " + permission);
#endif
        cam.targetTexture = null;
        RenderTexture.active = null;
        rt.Release ();

        if (imageSuccess && path != null) {
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
            callback (false);
        }
    }
#else
    public static void TakeScreenshot (Action<bool> callback) {
        Debug.LogWarning ("[ScreenshotManager TakeScreenshot] Will only took Screenshot on Android.");
        callback (false);
    }
#endif
}
