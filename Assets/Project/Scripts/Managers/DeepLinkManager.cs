using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DeepLinkManager {
    public static bool activatedViaLink = false;
    public static string refUrl;
    public static string refQuery;
    public static int refParticipation;

    public static void SetupHook () {
        ImaginationOverflow.UniversalDeepLinking.DeepLinkManager.Instance.LinkActivated += Action;
    }

    static void Action (ImaginationOverflow.UniversalDeepLinking.LinkActivation activation) {
        // Link Scheme: domicile://open?p=participation_id
        Debug.Log ("[DeepLinkManager Action] Application called via DeepLink.");
        activatedViaLink = true;
        refUrl = activation.Uri;
        refQuery = activation.RawQueryString;
        refParticipation = int.Parse (activation.QueryString["p"]);
        SessionManager.DefineParticipation (refParticipation);
    }
}
