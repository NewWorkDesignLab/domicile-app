using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ParticipationButton : MonoBehaviour
{
  public Participation participation;

  public void OnClick()
  {
    CrossSceneManager.currentParticipation = participation;
    UpdateScenario();
    CreateExecution();
    SceneManager.LoadScene("MainScene");
  }

  private void UpdateScenario()
  {
    API.scenario.Show(participation.scenario_id, (success) =>
    {
      Scenario scenario = JsonUtility.FromJson<Scenario>(success);
      CrossSceneManager.currentScenario = scenario;
    }, (error) =>
    {
      Debug.LogError(error);
    });
  }

  private void CreateExecution()
  {
    API.execution.Create(participation.id, (success) =>
    {
      Execution execution = JsonUtility.FromJson<Execution>(success);
      CrossSceneManager.currentExecution = execution;
    }, (error) =>
    {
      Debug.LogError(error);
    });
  }
}
