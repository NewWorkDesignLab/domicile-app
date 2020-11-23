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

  public void TakeScreenshot (Action callback) {
    if (!isBusy) {
      isBusy = true;
      var notification = new List<string> ();
      notification.Add ("Screenshot in 2");
      notification.Add ("Screenshot in 1");
      HUD.instance.ShowNotification (notification, 1f, () => {
        StartCoroutine (SavePhoto (() => {
          HUD.instance.ShowNotification ("Screenshot gespeichert", 1f);
          isBusy = false;
          callback ();
        }));
      });
    } else {
      Debug.LogWarning ("[ScreenshotScript TakeScreenshot] Cannot take Screenshot. Is Busy.");
    }
  }

  public IEnumerator SavePhoto (Action callback) {
#if PLATFORM_ANDROID
    Texture2D texture = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, false);
    texture.ReadPixels (new Rect (0, 0, Screen.width, Screen.height), 0, 0, false);
    texture.Apply ();
    string myFileName = "screenshot_domicile_" + System.DateTime.Now.Hour + System.DateTime.Now.Minute + System.DateTime.Now.Second;
    var imageUrl = SaveImageToGallery (texture, myFileName, "Saving photo to android");
    var imageUrl_2 = imageUrl.Replace("content:/", "");
    Debug.Log(imageUrl);
    Debug.Log(imageUrl_2);

    yield return new WaitForEndOfFrame ();
    CrossSceneManager.instance.unsavedScreenshots = CrossSceneManager.instance.unsavedScreenshots == null ? new List<string> () : CrossSceneManager.instance.unsavedScreenshots;
    CrossSceneManager.instance.unsavedScreenshots.Add (imageUrl_2);
#else
    Debug.Log ("Would Save Screenshot only on Android");
#endif
    callback ();
  }

  // following Screenshot / Java / Android Code copied from:
  // https://answers.unity.com/questions/204372/saving-screenshots-to-android-gallery.html?_ga=2.33230142.1821600655.1606136243-1256100704.1580052593
  private const string MediaStoreImagesMediaClass = "android.provider.MediaStore$Images$Media";
  public static string SaveImageToGallery (Texture2D texture2D, string title, string description) {
    using (var mediaClass = new AndroidJavaClass (MediaStoreImagesMediaClass)) {
      using (var cr = Activity.Call<AndroidJavaObject> ("getContentResolver")) {
        var image = Texture2DToAndroidBitmap (texture2D);
        var imageUrl = mediaClass.CallStatic<string> ("insertImage", cr, image, title, description);
        return imageUrl;
      }
    }
  }

  public static AndroidJavaObject Texture2DToAndroidBitmap (Texture2D texture2D) {
    byte[] encoded = texture2D.EncodeToPNG ();
    using (var bf = new AndroidJavaClass ("android.graphics.BitmapFactory")) {
      return bf.CallStatic<AndroidJavaObject> ("decodeByteArray", encoded, 0, encoded.Length);
    }
  }

  private static AndroidJavaObject _activity;
  public static AndroidJavaObject Activity {
    get {
      if (_activity == null) {
        var unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
        _activity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
      }
      return _activity;
    }
  }
}
