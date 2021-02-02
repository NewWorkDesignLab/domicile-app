using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DeepLinkManager {
    public static bool activatedViaLink = false;
    public static string refUrl;
    public static string refQuery;
    public static int refScenario;

    public static void SetupHook () {
        ImaginationOverflow.UniversalDeepLinking.DeepLinkManager.Instance.LinkActivated += Action;
    }

    static void Action (ImaginationOverflow.UniversalDeepLinking.LinkActivation activation) {
        Debug.Log ("[DeepLinkManager Action] Application called via DeepLink.");
        activatedViaLink = true;
        refUrl = activation.Uri;
        refQuery = activation.RawQueryString;
        refScenario = int.Parse(activation.QueryString["s"]);
        // TODO: store requested id in SessionManager
        // TODO: check SessionManager if ready to
    }
}
