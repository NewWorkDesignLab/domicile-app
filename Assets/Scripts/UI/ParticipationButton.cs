using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ParticipationButton : MonoBehaviour {
  public Participation participation;

  public void OnClick () {
    DataManager.sessionData.participation = participation;
    UpdateScenario ();
    CreateExecution ();
    SceneManager.LoadScene ("MainScene");
  }

  private void UpdateScenario () {
    Scenario.Show (participation.scenario_id, (scenario) => {
      DataManager.sessionData.scenario = scenario;
    });
  }

  private void CreateExecution () {
    Execution.Create (participation.id, (execution) => {
      DataManager.sessionData.execution = execution;
    });
  }
}
