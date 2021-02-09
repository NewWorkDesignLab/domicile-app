using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticipationPopupComponent : MonoBehaviour {
    public GameObject popupBackground;
    public GameObject popupEnterParticipation;
    public InputGroupComponent enterParticipationInputGroup;
    public GameObject enterParticipationError;

    void Awake () {
        ClosePopups ();
    }

    public void OpenPopupEnterParticipation () {
        popupBackground.gameObject.SetActive (true);
        popupEnterParticipation.gameObject.SetActive (true);
    }

    public void ClosePopups () {
        popupBackground.gameObject.SetActive (false);
        popupEnterParticipation.gameObject.SetActive (false);
        enterParticipationError.gameObject.SetActive (false);
    }

    public void ShowError () {
        enterParticipationError.gameObject.SetActive (true);
        OpenPopupEnterParticipation ();
    }
}
