using System;
using UnityEngine;

[Serializable]
public class Participation {
    public int id;
    public int user_id;
    public int scenario_id;
    public DateTime created_at;
    public DateTime updated_at;

    public static void Index (Action<ParticipationGroup> onSuccess) {
        Index (onSuccess, () => { });
    }
    public static void Index (Action<ParticipationGroup> onSuccess, Action onFailure) {
        ServerManager.GetRequest ("/api/participations", "", (body) => {
            string editJson = "{\"participations\":" + body + "}";
            ParticipationGroup participations = JsonUtility.FromJson<ParticipationGroup> (editJson);
            onSuccess (participations);
        }, onFailure);
    }

    public static void Show (int id, Action<Participation> onSuccess) {
        Show (id, onSuccess, () => { });
    }
    public static void Show (int id, Action<Participation> onSuccess, Action onFailure) {
        ServerManager.GetRequest (String.Format ("/api/participations/{0}", id), "", (body) => {
            Participation participation = JsonUtility.FromJson<Participation> (body);
            onSuccess (participation);
        }, onFailure);
    }

    public static void Create (int scenario_id, Action<Participation> onSuccess) {
        Create (scenario_id, onSuccess, () => { });
    }
    public static void Create (int scenario_id, Action<Participation> onSuccess, Action onFailure) {
        var data = new ParticipationCreateData (scenario_id);
        string json = JsonUtility.ToJson (data);
        ServerManager.PostRequest ("/api/participations", json, (body) => {
            Participation participation = JsonUtility.FromJson<Participation> (body);
            onSuccess (participation);
        }, onFailure);
    }
}

[Serializable]
public class ParticipationGroup {
    public Participation[] participations;
}

[Serializable]
class ParticipationCreateData {
    public Participation participation = new Participation ();
    public ParticipationCreateData (int scenario_id) {
        this.participation.scenario_id = scenario_id;
    }

    [Serializable]
    public class Participation {
        public int scenario_id;
    }
}
