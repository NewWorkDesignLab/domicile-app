using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotation : MonoBehaviour {
    public Player player;

    public float sensitivityX = 10F;
    public float sensitivityY = 10F;

    float minimumX = -360F;
    float maximumX = 360F;
    float minimumY = -90F;
    float maximumY = 90F;
    float rotationX = 0F;
    float rotationY = 0F;
    Quaternion originalRotation;

    public void ApplyMouseRotation () {
        if (originalRotation == null) {
            originalRotation = player.cameraHolderIfLocalPlayer.transform.localRotation;
        }

        var mouse = Mouse.current;
        if (mouse != null) {
            rotationX += Input.GetAxis ("Mouse X") * sensitivityX;
            rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
            rotationX = ClampAngle (rotationX, minimumX, maximumX);
            rotationY = ClampAngle (rotationY, minimumY, maximumY);
            Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, -Vector3.right);
            player.cameraHolderIfLocalPlayer.transform.localRotation = xQuaternion * yQuaternion;
        }
    }

    public static float ClampAngle (float angle, float min, float max) {
        if (angle < -360F) {
            angle += 360F;
        }
        if (angle > 360F) {
            angle -= 360F;
        }
        return Mathf.Clamp (angle, min, max);
    }
}
