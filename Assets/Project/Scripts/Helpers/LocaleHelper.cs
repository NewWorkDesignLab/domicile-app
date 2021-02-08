using UnityEngine;

public class LocaleHelper : Singleton<LocaleHelper> {
    public static LocaleSet getLocale;
    public LocaleSet localeSet;

    protected override void Awake () {
        base.Awake ();
        getLocale = localeSet;
    }

    public static string GetContent (string localeName) {
        var field = LocaleHelper.getLocale.GetType ().GetField (localeName);
        string value = "";
        if (field != null) {
            value = (string) field.GetValue (LocaleHelper.getLocale);
        }
        if (value == "") {
            Debug.LogError ("[LocaleHelper GetContent] Requested Local could not be found: \"" + localeName + "\". Will display localeName in App.");
            value = localeName;
        }
        return value;
    }
}
