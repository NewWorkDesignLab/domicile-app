using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlattformDisableHelper : MonoBehaviour {
    public DisablePlattform disablePlattform;

    void Start () {
#if UNITY_ANDROID
        if (disablePlattform == DisablePlattform.android) {
            gameObject.SetActive (false);
        }
#elif UNITY_WEBGL
        if (disablePlattform == DisablePlattform.webgl) {
            gameObject.SetActive (false);
        }
#elif UNITY_STANDALONE_LINUX
        if (disablePlattform == DisablePlattform.server) {
            gameObject.SetActive (false);
        }
#endif
    }
}

public enum DisablePlattform { none, android, webgl, server }
