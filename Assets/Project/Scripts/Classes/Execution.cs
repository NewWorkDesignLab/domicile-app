using System;
using UnityEngine;

[Serializable]
public class Execution {
    public int id;
    public int participation_id;
    public DateTime created_at;
    public DateTime updated_at;

    public static void Create (int participation_id, Action<Execution> onSuccess) {
        Create (participation_id, onSuccess, () => { });
    }
    public static void Create (int participation_id, Action<Execution> onSuccess, Action onFailure) {
        var data = new ExecutionCreateData (participation_id);
        string json = JsonUtility.ToJson (data);
        ServerManager.PostRequest ("/api/executions", json, (body) => {
            Execution execution = JsonUtility.FromJson<Execution> (body);
            onSuccess (execution);
        }, onFailure);
    }

    public static void UploadImages (int execution_id, string[] images, Action<Execution> onSuccess) {
        UploadImages (execution_id, images, onSuccess, () => { });
    }
    public static void UploadImages (int execution_id, string[] images, Action<Execution> onSuccess, Action onFailure) {
        ServerManager.ImageRequest (String.Format ("/api/executions/{0}/images", execution_id), images, (body) => {
            Execution execution = JsonUtility.FromJson<Execution> (body);
            onSuccess (execution);
        }, onFailure);
    }
}

[Serializable]
public class ExecutionGroup {
    public Execution[] executions;
}

[Serializable]
class ExecutionCreateData {
    public Execution execution = new Execution ();
    public ExecutionCreateData (int participation_id) {
        this.execution.participation_id = participation_id;
    }

    [Serializable]
    public class Execution {
        public int participation_id;
    }
}
