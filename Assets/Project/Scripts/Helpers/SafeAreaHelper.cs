using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaHelper : MonoBehaviour {
    // Source (Edited):
    // https://forum.unity.com/threads/canvashelper-resizes-a-recttransform-to-iphone-xs-safe-area.521107/
    private RectTransform targetRectTransform;
    private ScreenOrientation lastOrientation = ScreenOrientation.Landscape;
    private Vector2 lastResolution = Vector2.zero;
    private Rect lastSafeArea = Rect.zero;

    void Awake () {
        targetRectTransform = GetComponent<RectTransform> ();
        lastOrientation = Screen.orientation;
        lastResolution.x = Screen.width;
        lastResolution.y = Screen.height;
        lastSafeArea = Screen.safeArea;
        ApplySafeArea ();
    }

    void Update () {
        if (Application.isMobilePlatform && Screen.orientation != lastOrientation)
            OrientationChanged ();

        if (Screen.safeArea != lastSafeArea)
            SafeAreaChanged ();

        if (Screen.width != lastResolution.x || Screen.height != lastResolution.y)
            ResolutionChanged ();
    }

    void ApplySafeArea () {
        if (targetRectTransform == null)
            return;

        var safeArea = Screen.safeArea;

        var anchorMin = safeArea.position;
        var anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        targetRectTransform.anchorMin = anchorMin;
        targetRectTransform.anchorMax = anchorMax;
    }

    private void OrientationChanged () {
        Debug.Log ("[SafeAreaHelper OrientationChanged] Orientation changed from " + lastOrientation + " to " + Screen.orientation + " at " + Time.time);
        lastOrientation = Screen.orientation;
        lastResolution.x = Screen.width;
        lastResolution.y = Screen.height;
    }

    private void ResolutionChanged () {
        Debug.Log ("[SafeAreaHelper ResolutionChanged] Resolution changed from " + lastResolution + " to (" + Screen.width + ", " + Screen.height + ") at " + Time.time);
        lastResolution.x = Screen.width;
        lastResolution.y = Screen.height;
    }

    private void SafeAreaChanged () {
        Debug.Log ("[SafeAreaHelper SafeAreaChanged] Safe Area changed from " + lastSafeArea + " to " + Screen.safeArea.size + " at " + Time.time);
        lastSafeArea = Screen.safeArea;
        ApplySafeArea ();
    }
}
