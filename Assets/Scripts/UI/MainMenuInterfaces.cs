using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuInterfaces : MonoBehaviour {
  public GameObject dashboardGroup;
  public GameObject availableParticipationsContent;
  public GameObject availableParticipationsNoResult;
  public GameObject availableParticipationsPrefab;
  public GameObject joinScenarioGroup;
  public InputField joinScenarioIdInput;
  public InputField joinScenarioPasswordInput;

  private GameObject[] instantiatedParticipationsPrefabs;

  void Start () {
    UpdateParticipationsList ();
    ShowDashboardGroup ();
  }

  public void UpdateParticipationsList () {
    Participation.Index ((success) => {
      RenderParticipationList (success);
    });
  }

  public void RenderParticipationList (ParticipationGroup participationGroup) {
    if (participationGroup.participations.Length < 1) {
      availableParticipationsNoResult.SetActive (true);
    } else {
      availableParticipationsNoResult.SetActive (false);
      ClearParticipationInstantiations ();
      instantiatedParticipationsPrefabs = new GameObject[participationGroup.participations.Length];
      int index = 0;
      foreach (Participation participation in participationGroup.participations) {
        GameObject instance = GameObject.Instantiate (availableParticipationsPrefab);
        instance.transform.SetParent (availableParticipationsContent.transform, false);
        instance.GetComponentInChildren<Text> ().text = String.Format ("Teilnahme (ID: {0}) an Szenario {1}", participation.id, participation.scenario_id);
        instance.GetComponent<ParticipationButton> ().participation = participation;
        instantiatedParticipationsPrefabs[index] = instance;
        index++;
      }
    }
  }

  private void ClearParticipationInstantiations () {
    if (instantiatedParticipationsPrefabs != null) {
      foreach (GameObject instance in instantiatedParticipationsPrefabs) {
        GameObject.Destroy (instance);
      }
    }
  }

  public void ShowJoinScenarioGroup () {
    DisableAllGroups ();
    joinScenarioGroup.gameObject.SetActive (true);
  }
  public void ShowDashboardGroup () {
    DisableAllGroups ();
    dashboardGroup.gameObject.SetActive (true);
  }
  private void DisableAllGroups () {
    dashboardGroup.gameObject.SetActive (false);
    joinScenarioGroup.gameObject.SetActive (false);
  }
  public void ButtonJoinScenario () {
    string id = joinScenarioIdInput.text;
    string password = joinScenarioPasswordInput.text;
    Debug.Log (String.Format ("Would join Scenario. ID: {0}", id));
  }
  public void ButtonSignOut () {
    User.SignOut ((success) => {
      DataManager.getValue.lastKnownAccessToken = "";
      DataManager.getValue.lastKnownClient = "";
      DataManager.getValue.lastKnownUid = "";
      DataManager.getValue.saveLogin = false;
      DataManager.getValue.userEmail = "";
      DataManager.instance.Save ();
      SceneManager.LoadScene ("StartScene");
    });
  }
  public void OpenBrowserDashboard () {
    Application.OpenURL (String.Format ("{0}/dashboard", ServerManager.Host ()));
  }
  public void OpenBrowserNewScenario () {
    Application.OpenURL (String.Format ("{0}/szenarios/neu", ServerManager.Host ()));
  }
}
