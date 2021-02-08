using UnityEngine;

public class LocaleReceiver : MonoBehaviour {
    public string localeName;

    void Start () {
        string value = LocaleHelper.GetContent (localeName);
        GetComponent<TMPro.TextMeshProUGUI> ().text = value;
    }
}
