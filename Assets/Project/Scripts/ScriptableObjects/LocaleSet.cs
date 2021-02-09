using UnityEngine;

[CreateAssetMenu (fileName = "defaultLocaleSet", menuName = "Domicile/LocaleSet", order = 1)]
public class LocaleSet : ScriptableObject {
    [Header ("Android Permissions")]
    public string manualPermissionHeading;
    public string manualPermissionDescriptionShouldAsk;
    public string manualPermissionDescriptionDenied;
    public string manualPermissionOpenSettingsButton;
    public string manualPermissionSettingsDone;
    public string manualPermissionRecheckPermissionsButton;
    public string manualPermissionOpenSettingsError;

    [Header ("Login Scene")]
    public string loginSceneHeader;
    public string loginSceneDescription;
    public string loginSceneEmailInputLabel;
    public string loginSceneEmailInputPlaceholder;
    public string loginSceneEmailInputSmall;
    public string loginScenePasswordInputLabel;
    public string loginScenePasswordInputPlaceholder;
    public string loginScenePasswordInputSmall;
    public string loginSceneSubmitButton;
    public string loginSceneLoginError;
    public string loginSceneDashboardButton;
    public string loginSceneRegistrationButton;
    public string loginSceneForgotPasswordButton;

    [Header ("Participation Popup")]
    public string participationPopupHeader;
    public string participationPopupText;
    public string participationPopupInputLabel;
    public string participationPopupInputPlaceholder;
    public string participationPopupInputSmall;
    public string participationPopupButton;
    public string participationPopupJoinScenarioButton;
    public string participationDashboardButton;
    public string participationPopupError;

    [Header ("Scenario Overview")]
    public string scenarioOverviewHeading;
    public string scenarioOverviewDescription;
}
