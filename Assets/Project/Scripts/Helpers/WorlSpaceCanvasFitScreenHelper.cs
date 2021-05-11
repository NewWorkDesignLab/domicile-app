using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorlSpaceCanvasFitScreenHelper : MonoBehaviour {
    private RectTransform targetRectTransform;
    private ScreenOrientation lastOrientation = ScreenOrientation.Landscape;
    private Vector2 lastResolution = Vector2.zero;

    void Awake () {
        targetRectTransform = GetComponent<RectTransform> ();
        lastOrientation = Screen.orientation;
        lastResolution.x = Screen.width;
        lastResolution.y = Screen.height;
        ApplySafeArea ();
    }

    // Update is called once per frame
    void Update () {
        if (Application.isMobilePlatform && Screen.orientation != lastOrientation)
            OrientationChanged ();

        if (Screen.width != lastResolution.x || Screen.height != lastResolution.y)
            ResolutionChanged ();
    }

    void ApplySafeArea () {
        // New Width / Hights according to Screen Resolution
        targetRectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, Screen.width);
        targetRectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, Screen.height);

        // New Scale to fit Camera
        float distanceCameraHud = transform.localPosition.z;
        var frustumHeight = 2.0f * distanceCameraHud * Mathf.Tan (Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        var frustumWidth = frustumHeight * Camera.main.aspect;
        Vector3 newScale = new Vector3 (frustumWidth / Screen.width, frustumHeight / Screen.height, 0);
        targetRectTransform.localScale = newScale;
    }

    void OrientationChanged () {
        lastOrientation = Screen.orientation;
        ApplySafeArea ();
    }

    void ResolutionChanged () {
        lastResolution.x = Screen.width;
        lastResolution.y = Screen.height;
        ApplySafeArea ();
    }
}
