using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DeepLinkManager {
    public static bool activatedViaLink = false;
    public static string url;
    public static string query;
    public static int scenario;

    public static void SetupHook () {
        ImaginationOverflow.UniversalDeepLinking.DeepLinkManager.Instance.LinkActivated += Action;
    }

    static void Action (ImaginationOverflow.UniversalDeepLinking.LinkActivation activation) {
        // Link Scheme: domicile://open?s=scenario_id
        // Example: domicile://open?s=98012
        Debug.Log ("[DeepLinkManager Action] Application called via DeepLink.");
        activatedViaLink = true;
        url = activation.Uri;
        query = activation.RawQueryString;
        scenario = int.Parse (activation.QueryString["s"]);
        SessionManager.DefineScenario (scenario, () => { }, () => { });
    }
}
