using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SessionManager {
    public static Scenario scenario;
    public static Participation participation;
    public static Execution execution;
    public static bool setupReady = false;

    public static void ResetSession () {
        scenario = null;
        participation = null;
        execution = null;
    }
}
