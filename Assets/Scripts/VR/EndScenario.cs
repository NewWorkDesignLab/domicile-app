using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScenario : MonoBehaviour
{
  public static EndScenario instance;
  public GameObject errorMassage;

  void Awake()
  {
    instance = this;
  }
  void Start()
  {
    API.execution.UploadImages(CrossSceneManager.currentExecution.id, CrossSceneManager.instance.unsavedScreenshots, (success) =>
    {
      Debug.Log(success);
      CrossSceneManager.instance.ResetEnvironment();
      SceneManager.LoadScene("MainMenuScene");
    }, (error) =>
    {
      Debug.LogError(error);
      errorMassage.SetActive(true);
    });
  }

}
