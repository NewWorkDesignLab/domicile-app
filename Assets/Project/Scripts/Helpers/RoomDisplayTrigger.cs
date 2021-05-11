using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDisplayTrigger : MonoBehaviour {
    public string roomName;

    private void OnTriggerEnter (Collider other) {
        if (other.transform.tag == "LocalPlayer" && HUD.instance != null) {
            HUD.instance.UpdateRoomDisplay (roomName);
        }
    }
}
