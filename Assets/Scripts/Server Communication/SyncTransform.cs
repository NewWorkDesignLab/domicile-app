using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncTransform : MonoBehaviour
{
  private Vector3 oldPosition;
  private Vector3 oldRotation;
  private Vector3 oldScale;

  void Update()
  {
    if (oldPosition != gameObject.transform.position || oldRotation != Camera.main.transform.rotation.eulerAngles)
    {
      oldPosition = gameObject.transform.position;
      oldRotation = Camera.main.transform.rotation.eulerAngles;
      oldScale = new Vector3(1, 1, 1);

      string data = JsonUtility.ToJson(new SyncModel(oldPosition, oldRotation, oldScale));
      var message = new SendMessage("message", "SpectatorChannel", CrossSceneManager.currentParticipation.id.ToString(), "update_transform", data);
      WebsocketManager.instance.SendData(message);
    }
  }
}

[System.Serializable]
class SyncModel
{
  public SyncModel(Vector3 _position, Vector3 _rotation, Vector3 _scale)
  {
    this.position = _position;
    this.rotation = _rotation;
    this.scale = _scale;
  }
  public Vector3 position;
  public Vector3 rotation;
  public Vector3 scale;
}
