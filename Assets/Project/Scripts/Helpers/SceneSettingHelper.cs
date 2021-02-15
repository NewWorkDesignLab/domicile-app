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
        Debug.Log ("[SceneSettingHelper SwitchToVR] Would Display Scene in Virtual Reality");
        yield return null;
#else
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader ();
        if (XRGeneralSettings.Instance.Manager.activeLoader == null) {
            Debug.LogError ("[SceneSettingHelper SwitchToVR] Initializing XR Failed. Check Editor or Player log for details.");
        } else {
            Debug.LogError ("[SceneSettingHelper SwitchToVR] Starting XR...");
            XRGeneralSettings.Instance.Manager.StartSubsystems ();
        }
#endif
    }

    private void SwitchTo2D () {
#if UNITY_EDITOR
        Debug.Log ("[SceneSettingHelper SwitchTo2D] Would Display Scene in Normal Mode");
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
