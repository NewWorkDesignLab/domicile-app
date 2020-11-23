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
#if PLATFORM_ANDROID
    NativeGallery.Permission permission = NativeGallery.GetImagesFromGallery ((string[] paths) => {
      if (paths != null) {
        API.execution.UploadImages (CrossSceneManager.currentExecution.id, paths, (success) => {
          Debug.Log ("[EndScenario Start] " + success);
          CrossSceneManager.instance.ResetEnvironment ();
          SceneManager.LoadScene ("MainMenuScene");
        }, (error) => {
          Debug.LogError ("[EndScenario Start] " + error);
          errorMassage.SetActive (true);
        });
      }
    }, "Screenshots zum Upload auswählen", "image/png");
    Debug.Log ("[EndScenario Start] " + permission);
#else
    Debug.Log ("[EndScenario Start] Would only upload Screenshots on Android.");
#endif
  }

}
