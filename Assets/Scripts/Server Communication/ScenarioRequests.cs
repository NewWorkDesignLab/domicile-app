using UnityEngine;
using System;

public class ScenarioRequests : MonoBehaviour
{
  // INDEX
  public void Index()
  {
    Index((ignore) => { }, (ignore) => { });
  }
  public void Index(Action<string> onFailure)
  {
    Index((ignore) => { }, onFailure);
  }
  public void Index(Action<string> onSuccess, Action<string> onFailure)
  {
    API.instance.GetRequest("/api/scenarios", "", onSuccess, onFailure);
  }


  // SHOW
  public void Show(int id)
  {
    Show(id, (ignore) => { }, (ignore) => { });
  }
  public void Show(int id, Action<string> onFailure)
  {
    Show(id, (ignore) => { }, onFailure);
  }
  public void Show(int id, Action<string> onSuccess, Action<string> onFailure)
  {
    API.instance.GetRequest(String.Format("/api/scenarios/{0}", id), "", onSuccess, onFailure);
  }
}
