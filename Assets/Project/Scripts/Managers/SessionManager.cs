using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SessionManager {
    private static int definedScenario;
    public static SessionStatus status { get; private set; }
    public static Scenario scenario { get; private set; }
    public static Participation participation { get; private set; }
    public static Execution execution { get; private set; }
    public static bool errorInLoading = false;

    private static void LoadSession (Action onSuccess, Action onError) {
        // Check User Auth Status
        User.CheckToken ((success) => {
            // on user login success
            LoadParticipation (onSuccess, onError);
        }, () => {
            // on user login failure
            status = SessionStatus.OnHold;
            Debug.Log ("[SessionManager LoadParticipation] User is not authenticated. Set Session Load on Hold.");
            onError ();
        });
    }

    private static void LoadParticipation (Action onSuccess, Action onError) {
        // Load Participation
        Participation.Index ((participations) => {
            // Load Participation Index Success
            var result = participations.Where<int> ("scenario_id", definedScenario);
            if (result != null) {
                // Found Participation of requested Scenario
                participation = result;
                Debug.Log ("[SessionManager LoadParticipation] Participation load success. Loaded: " + JsonUtility.ToJson (participation));
                LoadScenario (onSuccess, onError);
            } else {
                // no participation of requested scenario found
                errorInLoading = true;
                ResetSession ();
                onError ();
            }
        }, () => {
            // Load Participation Index Error
            errorInLoading = true;
            ResetSession ();
            onError ();
        });
    }

    private static void LoadScenario (Action onSuccess, Action onError) {
        Scenario.Show (definedScenario, (Scenario success) => {
            // Load Scenario Success
            scenario = success;
            Debug.Log ("[SessionManager LoadScenario] Scenario load success. Loaded: " + JsonUtility.ToJson (scenario));
            status = SessionStatus.Ready;
            onSuccess ();
        }, () => {
            // Load Scenario Error
            errorInLoading = true;
            ResetSession ();
            onError ();
        });
    }

    public static void ResetSession () {
        status = SessionStatus.Null;
        scenario = null;
        participation = null;
        execution = null;
        Debug.Log ("[SessionManager LoadParticipation] Session resetted.");
    }

    public static void DefineScenario (int id, Action onSuccess, Action onError) {
        if (status == SessionStatus.Null) {
            Debug.Log ("[SessionManager LoadParticipation] Session with Scenario ID " + id + " defined. Going to load Session.");
            errorInLoading = false;
            definedScenario = id;
            status = SessionStatus.Loading;
            LoadSession (onSuccess, onError);
        } else {
            // TODO Message to User
            Debug.LogWarning ("[SessionManager DefineScenario] Could not define Scenario because Session is currently loading or already loaded. User SessionManager.ResetSession() to load new Scenario.");
        }
    }

    public static void ContinueOnHold (Action onSuccess, Action onError) {
        if (status == SessionStatus.OnHold) {
            Debug.Log ("[SessionManager LoadParticipation] Going to continue Session Load which was OnHold.");
            status = SessionStatus.Loading;
            LoadSession (onSuccess, onError);
        }
    }
}

public enum SessionStatus { Null, Loading, OnHold, Ready }
