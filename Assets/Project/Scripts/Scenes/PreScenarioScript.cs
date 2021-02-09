using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreScenarioScript : MonoBehaviour {
    public GameObject loadingIndicator;
    public ParticipationPopupComponent participationPopup;
    public GameObject overviewContent;

    void Start () {
        loadingIndicator.SetActive (true);
        overviewContent.SetActive (false);
        CheckSessionData ();
    }

    public void CheckSessionData () {
        switch (SessionManager.CurrentStatus ()) {
            case SessionStates.Null:
                participationPopup.OpenPopupEnterParticipation ();
                break;
            case SessionStates.Defined:
                SessionManager.LoadSession (() => {
                    RenderInformations ();
                }, () => {
                    participationPopup.ShowError ();
                });
                break;
            case SessionStates.Loaded:
                RenderInformations ();
                break;
            default:
                participationPopup.OpenPopupEnterParticipation ();
                break;
        }
    }

    public void ButtonDefineParticipation () {
        string input = participationPopup.enterParticipationInputGroup.inputFieldReference.text;
        int id = int.Parse (input);
        SessionManager.DefineParticipation (id);
        CheckSessionData ();
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
        participationPopup.ClosePopups ();
        overviewContent.SetActive (true);
    }

    public void StartScenario () {

    }
}
