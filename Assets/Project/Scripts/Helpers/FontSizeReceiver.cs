using UnityEngine;

public class FontSizeReceiver : MonoBehaviour {
    public string fontSizeType;

    void Start () {
        var field = FontSizeHelper.getFontSize.GetType ().GetField (fontSizeType);
        int value = 0;
        if (field == null) {
            Debug.LogError ("[FontSizeReceiver Start] Did not found FontSize named " + fontSizeType);
        } else {
            value = (int) field.GetValue (FontSizeHelper.getFontSize);
        }
        if (value == 0) {
            Debug.LogError ("[FontSizeReceiver Start] Requested FontSize is defined but not set (0): " + fontSizeType);
        }
        GetComponent<TMPro.TextMeshProUGUI> ().fontSize = value;
    }
}
