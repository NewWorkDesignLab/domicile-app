using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSceneManager : MonoBehaviour
{
  public static CrossSceneManager instance;
  public static User currentUser;
  public static Scenario currentScenario;
  public static Participation currentParticipation;
  public static Execution currentExecution;
  public List<string> unsavedScreenshots;

  void Awake()
  {
    instance = this;
  }

  public void ResetEnvironment()
  {
    currentScenario = null;
    currentParticipation = null;
    currentExecution = null;
    unsavedScreenshots.Clear();
  }
}
