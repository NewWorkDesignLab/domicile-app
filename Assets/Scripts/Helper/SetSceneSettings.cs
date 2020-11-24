using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class SetSceneSettings : MonoBehaviour {
  public ScreenOrientation orientation;
  public VRSetting vrSetting;

  void Start () {
    Screen.orientation = orientation;
    if (vrSetting == VRSetting.virtualReality) {
      StartCoroutine (SwitchToVR ());
    } else {
      StartCoroutine (SwitchTo2D ());
    }
  }

  private IEnumerator SwitchToVR () {
    // copied from https://developers.google.com/vr/develop/unity/guides/hybrid-apps
    string desiredDevice = "cardboard";
    if (String.Compare (XRSettings.loadedDeviceName, desiredDevice, true) != 0) {
      XRSettings.LoadDeviceByName (desiredDevice);
      yield return null;
    }
    XRSettings.enabled = true;
  }

  IEnumerator SwitchTo2D () {
    XRSettings.LoadDeviceByName ("");
    yield return null;
    ResetCameras ();
  }

  // Resets camera transform and settings on all enabled eye cameras.
  void ResetCameras () {
    for (int i = 0; i < Camera.allCameras.Length; i++) {
      Camera cam = Camera.allCameras[i];
      if (cam.enabled && cam.stereoTargetEye != StereoTargetEyeMask.None) {
        cam.transform.localPosition = Vector3.zero;
        cam.transform.localRotation = Quaternion.identity;
      }
    }
  }
}

public enum VRSetting { normal, virtualReality }
