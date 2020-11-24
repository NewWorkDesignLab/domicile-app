using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ParticipationButton : MonoBehaviour {
  public Participation participation;

  public void OnClick () {
    CrossSceneManager.currentParticipation = participation;
    UpdateScenario ();
    CreateExecution ();
    SceneManager.LoadScene ("MainScene");
  }

  private void UpdateScenario () {
    Scenario.Show (participation.scenario_id, (scenario) => {
      CrossSceneManager.currentScenario = scenario;
    });
  }

  private void CreateExecution () {
    Execution.Create (participation.id, (execution) => {
      CrossSceneManager.currentExecution = execution;
    });
  }
}
