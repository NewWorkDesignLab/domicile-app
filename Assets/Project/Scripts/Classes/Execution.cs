using System;
using UnityEngine;

/// <summary>
/// Public class for execution objects. The Execution Class also exists in the web application. An
/// execution is a single run of a scenario and contains all the information like the corresponding
/// participation id or date and time of execution.
/// </summary>
[Serializable]
public class Execution {
    /// <summary>ID of the Execution.</summary>
    public int id;
    /// <summary>ID of the associated participation.</summary>
    public int participation_id;
    /// <summary>Date and Time of the creation of this Execution.</summary>
    public DateTime created_at;
    /// <summary>Date and Time of the last modification of the Execution.</summary>
    public DateTime updated_at;

    /// <summary>
    /// Generates an API call to the web server, which creates an execution.
    /// </summary>
    /// <param name="participation_id">Required. The ID of the Participation which gets executed.</param>
    /// <param name="onSuccess">Required. Callback for a successfull operation. Returns the created Execution.</param>
    /// <param name="onFailure">Required. Callback for a unsuccessfull operation.</param>
    public static void Create (int participation_id, Action<Execution> onSuccess, Action onFailure) {
        // Use double curly braces ({{; }}) to escape normal braces in String.Format
        string json = String.Format ("{{\"execution\":{{\"participation_id\":{0}}}}}", participation_id);
        ServerManager.PostRequest ("/api/executions", json, (body) => {
            // successfull server request
            Execution execution = JsonUtility.FromJson<Execution> (body);
            onSuccess (execution);
        }, onFailure);
    }

    /// <summary>
    /// Uploads images of an execution to the web server.
    /// </summary>
    /// <param name="execution_id">Required. ID of the execution to which the images should be uploaded.</param>
    /// <param name="images"Required. Array of image paths to be uploaded.></param>
    /// <param name="onSuccess">Required. Callback for a successfull operation. Returns the created Execution.</param>
    /// <param name="onFailure">Required. Callback for a unsuccessfull operation.</param>
    public static void UploadImages (int execution_id, string[] images, Action<Execution> onSuccess, Action onFailure) {
        ServerManager.ImageRequest (String.Format ("/api/executions/{0}/images", execution_id), images, (body) => {
            // successfull server request
            Execution execution = JsonUtility.FromJson<Execution> (body);
            onSuccess (execution);
        }, onFailure);
    }
}
