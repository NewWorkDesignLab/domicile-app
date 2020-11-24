using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScenario : MonoBehaviour {
  public static EndScenario instance;
  public GameObject errorMassage;

  void Awake () {
    instance = this;
  }
  void Start () {
#if UNITY_EDITOR
    Debug.Log ("[EndScenario Start] Would only upload Screenshots on Android.");
    EndScenarioHelper ();
#else
    NativeGallery.Permission permission = NativeGallery.GetImagesFromGallery ((string[] paths) => {
      if (paths != null) {
        Execution.UploadImages (DataManager.sessionData.execution.id, paths, (execution) => {
          Debug.Log ("[EndScenario Start] Success in Image Upload. Execution: " + execution);
          EndScenarioHelper ();
        }, () => {
          Debug.LogError ("[EndScenario Start] Error in Image Upload.");
          errorMassage.SetActive (true);
        });
      } else {
        EndScenarioHelper ();
      }
    }, "Screenshots zum Upload auswählen", "image/png");
    Debug.Log ("[EndScenario Start] " + permission);
#endif
  }

  void EndScenarioHelper () {
    DataManager.ResetSessionData ();
    SceneManager.LoadScene ("MainMenuScene");
  }
}
