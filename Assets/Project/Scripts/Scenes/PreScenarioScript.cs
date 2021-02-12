using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreScenarioScript : MonoBehaviour {
    public GameObject loadingIndicator;
    public ScenarioPopupComponent scenarioPopup;
    public GameObject overviewContent;
    public TMPro.TextMeshProUGUI scenarioIdReference;
    public TMPro.TextMeshProUGUI scenarioRoomsReference;
    public TMPro.TextMeshProUGUI scenarioTimeReference;
    public TMPro.TextMeshProUGUI scenarioDamageReference;
    public TMPro.TextMeshProUGUI scenarioJoinedAtReference;
    private bool continueSessionDataCheck = false;

    void Start () {
        loadingIndicator.SetActive (true);
        overviewContent.SetActive (false);
        CheckSessionData ();
    }

    void Update () {
        if (continueSessionDataCheck) {
            CheckSessionData ();
        }
    }

    public void CheckSessionData () {
        switch (SessionManager.status) {
            case SessionStatus.Null:
                continueSessionDataCheck = false;
                if (SessionManager.errorInLoading) {
                    scenarioPopup.ShowError ();
                } else {
                    scenarioPopup.OpenPopupEnterScenario ();
                }
                break;
            case SessionStatus.Loading:
                continueSessionDataCheck = true;
                break;
            case SessionStatus.OnHold:
                continueSessionDataCheck = false;
                SessionManager.ContinueOnHold (() => {
                    // load Session Success
                    RenderInformations ();
                }, () => {
                    // load Session Error
                    CheckSessionData ();
                });
                break;
            case SessionStatus.Ready:
                continueSessionDataCheck = false;
                RenderInformations ();
                break;
            default:
                continueSessionDataCheck = false;
                scenarioPopup.ShowError ();
                break;
        }
    }

    public void ButtonDefineScenario () {
        scenarioPopup.ClosePopups ();
        string input = scenarioPopup.enterScenarioInputGroup.inputFieldReference.text;
        int id = int.Parse (input);
        SessionManager.DefineScenario (id, () => {
            // load Session Success
            RenderInformations ();
        }, () => {
            // load Session Error
            CheckSessionData ();
        });
    }
    public void ButtonJoinScenario () {
        Application.OpenURL (String.Format ("{0}/teilnahmen/neu", ServerManager.Host ()));
    }
    public void ButtonDashboard () {
        Application.OpenURL (String.Format ("{0}/dashboard", ServerManager.Host ()));
    }

    public void RenderInformations () {
        scenarioPopup.ClosePopups ();

        scenarioIdReference.text = LocaleHelper.GetContent ("scenarioOverviewScenarioID") + " " + SessionManager.scenario.id;
        scenarioRoomsReference.text = String.Format (LocaleHelper.GetContent ("scenarioOverviewScenarioRooms"), SessionManager.scenario.number_rooms);
        scenarioTimeReference.text = String.Format (LocaleHelper.GetContent ("scenarioOverviewScenarioTime"), SessionManager.scenario.time_limit);
        scenarioDamageReference.text = String.Format (LocaleHelper.GetContent ("scenarioOverviewScenarioDamages"), SessionManager.scenario.number_damages);
        scenarioJoinedAtReference.text = LocaleHelper.GetContent ("scenarioOverviewScenarioJoinedAt") + " " + SessionManager.participation.datetime_created_at;

        loadingIndicator.SetActive (false);
        overviewContent.SetActive (true);
    }

    public void StartScenario () {

    }
}
