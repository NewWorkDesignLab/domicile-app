using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeepLinkScript : MonoBehaviour {
    public TextMeshProUGUI textRef;

    void Start () {
        textRef.text = "before Hook";
        ImaginationOverflow.UniversalDeepLinking.DeepLinkManager.Instance.LinkActivated += LinkActivated;
        textRef.text = "after Hook";
    }

    void OnDestroy () {
        ImaginationOverflow.UniversalDeepLinking.DeepLinkManager.Instance.LinkActivated -= LinkActivated;
    }

    private void LinkActivated (ImaginationOverflow.UniversalDeepLinking.LinkActivation activation) {
        var url = activation.Uri;
        var querystring = activation.RawQueryString;
        var participationParameter = activation.QueryString["p"];
        textRef.text = participationParameter;
    }
}
