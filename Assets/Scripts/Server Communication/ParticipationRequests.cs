using UnityEngine;
using System;

public class ParticipationRequests : MonoBehaviour
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
    API.instance.GetRequest("/api/participations", "", onSuccess, onFailure);
  }


  // CREATE
  public void Create(int scenario_id)
  {
    Create(scenario_id, (ignore) => { }, (ignore) => { });
  }
  public void Create(int scenario_id, Action<string> onFailure)
  {
    Create(scenario_id, (ignore) => { }, onFailure);
  }
  public void Create(int scenario_id, Action<string> onSuccess, Action<string> onFailure)
  {
    var data = new ParticipationCreateData(scenario_id);
    string json = JsonUtility.ToJson(data);
    API.instance.PostRequest("/api/participations", json, onSuccess, onFailure);
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
    API.instance.GetRequest(String.Format("/api/participations/{0}", id), "", onSuccess, onFailure);
  }
}




[Serializable]
class ParticipationCreateData
{
  public Participation participation = new Participation();
  public ParticipationCreateData(int scenario_id)
  {
    this.participation.scenario_id = scenario_id;
  }

  [Serializable]
  public class Participation
  {
    public int scenario_id;
  }
}
