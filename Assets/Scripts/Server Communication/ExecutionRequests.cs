using UnityEngine;
using System.Collections.Generic;
using System;

public class ExecutionRequests : MonoBehaviour
{
  // CREATE
  public void Create(int participation_id)
  {
    Create(participation_id, (ignore) => { }, (ignore) => { });
  }
  public void Create(int participation_id, Action<string> onFailure)
  {
    Create(participation_id, (ignore) => { }, onFailure);
  }
  public void Create(int participation_id, Action<string> onSuccess, Action<string> onFailure)
  {
    var data = new ExecutionCreateData(participation_id);
    string json = JsonUtility.ToJson(data);
    API.instance.PostRequest("/api/executions", json, onSuccess, onFailure);
  }


  // UPLOAD IMAGES
  public void UploadImages(int execution_id, string[] images)
  {
    UploadImages(execution_id, images, (ignore) => { }, (ignore) => { });
  }
  public void UploadImages(int execution_id, string[] images, Action<string> onFailure)
  {
    UploadImages(execution_id, images, (ignore) => { }, onFailure);
  }
  public void UploadImages(int execution_id, string[] images, Action<string> onSuccess, Action<string> onFailure)
  {
    API.file.UploadImages(String.Format("/api/executions/{0}/images", execution_id), images, onSuccess, onFailure);
  }
}


[Serializable]
class ExecutionCreateData
{
  public Execution execution = new Execution();
  public ExecutionCreateData(int participation_id)
  {
    this.execution.participation_id = participation_id;
  }

  [Serializable]
  public class Execution
  {
    public int participation_id;
  }
}
