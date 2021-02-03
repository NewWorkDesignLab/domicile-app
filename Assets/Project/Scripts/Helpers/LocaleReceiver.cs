using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocaleReceiver : MonoBehaviour {
    public string localeName;
    void Start () {
        var field = LocaleHelper.getLocale.GetType ().GetField (localeName);
        string value = "";
        if (field == null) {
            Debug.LogError ("[LocaleReceiver Start] Did not found Local named " + localeName);
        } else {
            value = (string) field.GetValue (LocaleHelper.getLocale);
        }
        if (value == null || value == "") {
            Debug.LogError ("[LocaleReceiver Start] Requested Local is defined but not set (null or \"\"): " + localeName);
        }
        GetComponent<TMPro.TextMeshProUGUI> ().text = value;
    }
}
