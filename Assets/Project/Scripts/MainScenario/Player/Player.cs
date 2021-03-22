using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour {
    public GameObject cameraHolder;

    void Start () {
        if (isLocalPlayer) {
            cameraHolder.AddComponent (typeof (Camera));
        }
    }
}
