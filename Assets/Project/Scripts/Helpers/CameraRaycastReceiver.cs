using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraRaycastReceiver : MonoBehaviour {

    public UnityEvent raycastEnter;
    public UnityEvent raycastExit;
    public UnityEvent raycastClick;
    public UnityEvent raycastGazedEnter;

    public void RaycastEnter () {
        raycastEnter.Invoke ();
    }
    public void RaycastExit () {
        raycastExit.Invoke ();
    }
    public void RaycastClick () {
        raycastClick.Invoke ();
    }

    public void RaycastGazedEnter () {
        raycastGazedEnter.Invoke ();
    }

    public bool GazeEventPresent () {
        for (int i = 0; i < raycastGazedEnter.GetPersistentEventCount (); i++) {
            if (raycastGazedEnter.GetPersistentTarget (i) != null) {
                return true;
            }
        }
        return false;
    }
}
