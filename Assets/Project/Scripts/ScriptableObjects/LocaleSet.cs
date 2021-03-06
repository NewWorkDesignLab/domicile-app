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

    [Header ("Scenario Popup")]
    public string scenarioPopupHeader;
    public string scenarioPopupText;
    public string scenarioPopupInputLabel;
    public string scenarioPopupInputPlaceholder;
    public string scenarioPopupInputSmall;
    public string scenarioPopupButton;
    public string scenarioPopupJoinScenarioButton;
    public string scenarioPopupDashboardButton;
    public string scenarioPopupError;

    [Header ("Scenario Overview")]
    public string scenarioOverviewHeading;
    public string scenarioOverviewDescription;
    public string scenarioOverviewError;
    public string scenarioOverviewScenarioSubheading;
    public string scenarioOverviewScenarioID;
    public string scenarioOverviewScenarioRooms;
    public string scenarioOverviewScenarioTime;
    public string scenarioOverviewScenarioDamages;
    public string scenarioOverviewScenarioJoinedAt;
    public string scenarioOverviewButtonExecute;
    public string scenarioOverviewButtonClose;
}
