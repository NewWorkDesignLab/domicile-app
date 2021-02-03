using UnityEngine;

[CreateAssetMenu (fileName = "defaultLocaleSet", menuName = "Domicile/LocaleSet", order = 1)]
public class LocaleSet : ScriptableObject {
    [Header ("General")]

    [Header ("Android Permissions")]
    public string manualPermissionHeading;
    public string manualPermissionDescriptionShouldAsk;
    public string manualPermissionDescriptionDenied;
    public string manualPermissionOpenSettingsButton;
    public string manualPermissionSettingsDone;
    public string manualPermissionRecheckPermissionsButton;
    public string manualPermissionOpenSettingsError;
}
