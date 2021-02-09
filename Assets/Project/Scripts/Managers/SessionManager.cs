using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SessionManager {
    private static bool isDefined = false;
    private static int definedParticipation;
    public static Scenario scenario { get; private set; }
    public static Participation participation { get; private set; }
    public static Execution execution { get; private set; }

    public static void LoadSession (Action onSuccess, Action onError) {
        if (CurrentStatus () == SessionStates.Defined) {
            LoadParticipation (onSuccess, onError);
        } else if (CurrentStatus () == SessionStates.Loaded) {
            onSuccess ();
        } else {
            onError ();
        }
    }

    private static void LoadParticipation (Action onSuccess, Action onError) {
        Participation.Show (definedParticipation, (Participation success) => {
            // Load Participation Success
            participation = success;
            LoadScenario (onSuccess, onError);
        }, () => {
            // Load Participation Error
            onError ();
        });
    }

    private static void LoadScenario (Action onSuccess, Action onError) {
        Scenario.Show (participation.scenario_id, (Scenario success) => {
            // Load Scenario Success
            scenario = success;
            onSuccess ();
        }, () => {
            // Load Scenario Error
            onError ();
        });
    }

    public static void ResetSession () {
        isDefined = false;
        scenario = null;
        participation = null;
        execution = null;
    }

    public static void DefineParticipation (int id) {
        // TODO what to do when deepLink activated while in Scenario
        ResetSession ();
        definedParticipation = id;
        isDefined = true;
    }

    public static SessionStates CurrentStatus () {
        if (isDefined == true && scenario != null && participation != null && definedParticipation == participation.id && participation.scenario_id == scenario.id) {
            return SessionStates.Loaded;
        } else if (isDefined == true) {
            return SessionStates.Defined;
        } else {
            return SessionStates.Null;
        }
    }
}

public enum SessionStates { Null, Defined, Loaded }
