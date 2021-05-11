using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlattformDependentHudSizeHelper : MonoBehaviour {
    public float webglWidthInPercent;
    public float webglHeightInPercent;
    public float androidWidthInPercent;
    public float androidHeightInPercent;
    private RectTransform targetRectTransform;

    void Awake () {
#if UNITY_ANDROID
        float vertical = androidHeightInPercent / 100f;
        float horizontal = androidWidthInPercent / 100f;
#elif UNITY_WEBGL
        float vertical = webglHeightInPercent / 100f;
        float horizontal = webglWidthInPercent / 100f;
#else
        float vertical = 1f;
        float horizontal = 1f;
#endif
        float requiredVerticalMargin = (1f - vertical) / 2;
        float requiredHorizontalMargin = (1f - horizontal) / 2;
        var anchorMin = new Vector2 (requiredHorizontalMargin, requiredVerticalMargin);
        var anchorMax = new Vector2 (1f - requiredHorizontalMargin, 1f - requiredVerticalMargin);

        targetRectTransform = GetComponent<RectTransform> ();
        targetRectTransform.anchorMin = anchorMin;
        targetRectTransform.anchorMax = anchorMax;
    }
}
