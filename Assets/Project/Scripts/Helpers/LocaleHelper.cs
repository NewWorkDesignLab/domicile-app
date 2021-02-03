using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocaleHelper : MonoBehaviour {
    public static LocaleSet getLocale;
    public LocaleSet localeSet;

    void Awake () {
        getLocale = localeSet;
    }
}
