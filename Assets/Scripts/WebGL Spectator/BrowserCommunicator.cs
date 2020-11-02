using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BrowserCommunicator : MonoBehaviour
{
#if UNITY_EDITOR
  public GameObject positionReference;
  void Start()
  {
    Message(SimulateData(positionReference.transform));
  }
  string SimulateData(Transform t)
  {
    // {"position":{"x":0,"y":0,"z":0},"rotation":{"x":0,"y":0,"z":0},"scale":{"x":1,"y":1,"z":1}}
    Vector3 pos = t.position;
    Vector3 rot = t.rotation.eulerAngles;
    Vector3 scale = t.localScale;
    string blueprint = "{{\"position\":{{\"x\":{0},\"y\":{1},\"z\":{2}}},\"rotation\":{{\"x\":{3},\"y\":{4},\"z\":{5}}},\"scale\":{{\"x\":{6},\"y\":{7},\"z\":{8}}}}}";
    return String.Format(blueprint, pos.x, pos.y, pos.z, rot.x, rot.y, rot.z, scale.x, scale.y, scale.z);
  }
#endif

  public Transform playerClone;

  // Will be called from the Website which Hosts the WEbGL Player after receiving
  // Data from the current VR-Player over Websocket
  void Message(string _data)
  {
    Debug.Log("Received Data inside WebGL: " + _data);
    Data data = JsonUtility.FromJson<Data>(_data);

    playerClone.position = data.position;
    playerClone.rotation = Quaternion.Euler(data.rotation);
    playerClone.localScale = data.scale;
  }

  string FixJson(string jsonString)
  {
    var ret = String.Format("{{\"data\":{0}}}", jsonString);
    return ret;
  }

  [System.Serializable]
  class Data
  {
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
  }
}
