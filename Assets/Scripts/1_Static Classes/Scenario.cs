using System;
using UnityEngine;

[Serializable]
public class Scenario {
  public int id;
  public int user_id;
  public DateTime created_at;
  public DateTime updated_at;
  public string name;
  public int number_rooms;
  public int time_limit;
  public int number_damages;

  public static void Index (Action<ScenarioGroup> onSuccess) {
    Index (onSuccess, () => { });
  }
  public static void Index (Action<ScenarioGroup> onSuccess, Action onFailure) {
    ServerManager.GetRequest ("/api/scenarios", "", (body) => {
      ScenarioGroup scenarios = JsonUtility.FromJson<ScenarioGroup> (body);
      onSuccess (scenarios);
    }, onFailure);
  }

  public static void Show (int id, Action<Scenario> onSuccess) {
    Show (id, onSuccess, () => { });
  }
  public static void Show (int id, Action<Scenario> onSuccess, Action onFailure) {
    ServerManager.GetRequest (String.Format ("/api/scenarios/{0}", id), "", (body) => {
      Scenario scenario = JsonUtility.FromJson<Scenario> (body);
      onSuccess (scenario);
    }, onFailure);
  }
}

[Serializable]
public class ScenarioGroup {
  public Scenario[] scenarios;
}
