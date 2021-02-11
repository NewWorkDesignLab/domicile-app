using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioPopupComponent : MonoBehaviour {
    public GameObject popupBackground;
    public GameObject popupEnterScenario;
    public InputGroupComponent enterScenarioInputGroup;
    public GameObject enterScenarioError;

    void Awake () {
        ClosePopups ();
    }

    public void OpenPopupEnterScenario () {
        popupBackground.gameObject.SetActive (true);
        popupEnterScenario.gameObject.SetActive (true);
    }

    public void ClosePopups () {
        popupBackground.gameObject.SetActive (false);
        popupEnterScenario.gameObject.SetActive (false);
        enterScenarioError.gameObject.SetActive (false);
    }

    public void ShowError () {
        enterScenarioError.gameObject.SetActive (true);
        OpenPopupEnterScenario ();
    }
}
