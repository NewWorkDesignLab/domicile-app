using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermissionPopupComponent : MonoBehaviour {
    public ApplicationEntranceScript applicationEntranceScript;
    public GameObject popupBackground;
    public GameObject popupDenied;
    public GameObject popupShouldAsk;
    public GameObject openSettingsError;

    public void OpenPopupDenied () {
        popupBackground.gameObject.SetActive (true);
        popupDenied.gameObject.SetActive (true);
        popupShouldAsk.SetActive (false);
    }
    public void OpenPopupShouldAsk () {
        popupBackground.gameObject.SetActive (true);
        popupDenied.gameObject.SetActive (false);
        popupShouldAsk.SetActive (true);
    }
    public void ClosePopup () {
        popupBackground.gameObject.SetActive (false);
        popupDenied.gameObject.SetActive (false);
        popupShouldAsk.SetActive (false);
        openSettingsError.SetActive (false);
    }
    public void OpenSettings () {
        PermissionManager.OpenSettings (() => {
            ShowOpenSettingsError ();
        });
    }
    public void ShowOpenSettingsError () {
        openSettingsError.SetActive (true);
    }
    public void RecheckPermissions () {
        ClosePopup ();
        applicationEntranceScript.StartProcedure ();
    }
}
