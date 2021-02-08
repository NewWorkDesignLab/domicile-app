using System;
using UnityEngine;

/// <summary>
/// Public class for participation objects. The Participation Class also exists in the web application.
/// A participation is a single subscription to a scenario. A user may only participate in a scenario once.
/// </summary>
[Serializable]
public class Participation {
    /// <summary>ID of the Participation.</summary>
    public int id;
    /// <summary>ID of the associated User.</summary>
    public int user_id;
    /// <summary>ID of the associated Scenario.</summary>
    public int scenario_id;
    /// <summary>Date and Time of the creation of this Participation.</summary>
    public DateTime created_at;
    /// <summary>Date and Time of the last modification of the Participation.</summary>
    public DateTime updated_at;

    /// <summary>
    /// Generates an API query to retrieve a list of all participations of a User.
    /// </summary>
    /// <param name="onSuccess">Required. Callback for a successfull operation. Returns a Collection of Participations.</param>
    /// <param name="onFailure">Required. Callback for a unsuccessfull operation.</param>
    public static void Index (Action<ParticipationGroup> onSuccess, Action onFailure) {
        ServerManager.GetRequest ("/api/participations", "", (body) => {
            // successfull server request
            string editJson = "{\"participations\":" + body + "}";
            ParticipationGroup participations = JsonUtility.FromJson<ParticipationGroup> (editJson);
            onSuccess (participations);
        }, onFailure);
    }

    /// <summary>
    /// Generates an API query to retrieve information of a single Participation.
    /// </summary>
    /// <param name="id">Required. ID for which the Information should be queried.</param>
    /// <param name="onSuccess">Required. Callback for a successfull operation. Returns a Participation.</param>
    /// <param name="onFailure">Required. Callback for a unsuccessfull operation.</param>
    public static void Show (int id, Action<Participation> onSuccess, Action onFailure) {
        ServerManager.GetRequest (String.Format ("/api/participations/{0}", id), "", (body) => {
            // successfull server request
            Participation participation = JsonUtility.FromJson<Participation> (body);
            onSuccess (participation);
        }, onFailure);
    }

    /// <summary>
    /// Generates an API call to the web server, which creates an Participation.
    /// </summary>
    /// <param name="scenario_id">Required. ID of the Scenario which the User will participate.</param>
    /// <param name="onSuccess">Required. Callback for a successfull operation. Returns a Participation.</param>
    /// <param name="onFailure">Required. Callback for a unsuccessfull operation.</param>
    public static void Create (int scenario_id, Action<Participation> onSuccess, Action onFailure) {
        string json = String.Format ("{{\"participation\":{{\"scenario_id\":{0}}}}}", scenario_id);
        ServerManager.PostRequest ("/api/participations", json, (body) => {
            // successfull server request
            Participation participation = JsonUtility.FromJson<Participation> (body);
            onSuccess (participation);
        }, onFailure);
    }
}

/// <summary>
/// Wrapper class for a collection of multiple Participations.
/// </summary>
[Serializable]
public class ParticipationGroup {
    public Participation[] participations;
}
