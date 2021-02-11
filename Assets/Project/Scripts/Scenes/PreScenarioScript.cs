using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreScenarioScript : MonoBehaviour {
    public GameObject loadingIndicator;
    public ScenarioPopupComponent scenarioPopup;
    public GameObject overviewContent;
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
        // TODO display info on Screen
        loadingIndicator.SetActive (false);
        scenarioPopup.ClosePopups ();
        overviewContent.SetActive (true);
    }

    public void StartScenario () {

    }
}
