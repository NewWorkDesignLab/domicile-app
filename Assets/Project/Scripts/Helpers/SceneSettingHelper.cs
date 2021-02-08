using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Management;

public class SceneSettingHelper : MonoBehaviour {
    public VRSetting vrSetting;

    void Start () {
        if (vrSetting == VRSetting.VR) {
            StartCoroutine (SwitchToVR ());
        } else {
            SwitchTo2D ();
        }
    }

    private IEnumerator SwitchToVR () {
        // from https://forum.unity.com/threads/toggle-between-2d-and-google-cardboard.902378/
        Screen.orientation = ScreenOrientation.Landscape;
#if UNITY_EDITOR
        Debug.Log ("[SceneSettingHelper] Would Display Scene in Virtual Reality");
        yield return null;
#else
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader ();
        XRGeneralSettings.Instance.Manager.StartSubsystems ();
#endif
    }

    private void SwitchTo2D () {
#if UNITY_EDITOR
        Debug.Log ("[SceneSettingHelper] Would Display Scene in Normal Mode");
#else
        if (XRGeneralSettings.Instance.Manager.isInitializationComplete) {
            XRGeneralSettings.Instance.Manager.StopSubsystems ();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader ();
        }
#endif
        Screen.orientation = ScreenOrientation.Portrait;
    }
}

public enum VRSetting { Normal, VR }
