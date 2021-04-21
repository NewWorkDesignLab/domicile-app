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
    /// <summary>Date and Time of the creation of this Participation as String. (Required for JSON deserialization)</summary>
    public string created_at;
    /// <summary>Date and Time of the last modification of the Participation as String. (Required for JSON deserialization)</summary>
    public string updated_at;
    /// <summary>Date and Time of the creation of this Participation as Unity DateTime.</summary>
    public DateTime datetime_created_at { get { return System.DateTime.Parse (created_at); } }
    /// <summary>Date and Time of the creation of this Participation as Unity DateTime.</summary>
    public DateTime datetime_updated_at { get { return System.DateTime.Parse (updated_at); } }


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

    public Participation Where<T> (string key, T target) {
        // check if the requested key is available
        var field = typeof (Participation).GetField (key);
        if (field != null) {
            // iterate thru participations and find the one
            foreach (Participation p in participations) {
                // if correct participation found
                // using .Equals() because of generic Type T
                // (https://stackoverflow.com/questions/8982645/how-to-solve-operator-cannot-be-applied-to-operands-of-type-t-and-t)
                T value = (T) field.GetValue (p);
                if (value.Equals (target)) {
                    return p;
                }
            }
            // no entry found
            Debug.LogWarning ("[ParticipationGroup Where] No Participation fits the requested value.");
            return null;
        } else {
            Debug.LogError ("[ParticipationGroup Where] Requested Key not Found.");
            return null;
        }
    }
}
