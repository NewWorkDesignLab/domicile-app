using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebControls : MonoBehaviour {
    public GameObject body;
    public GameObject head;
    public float sensitivityX = 10F;
    public float sensitivityY = 10F;
    public float movementFactor = 1f;

    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    float rotationX = 0F;
    float rotationY = 0F;
    Quaternion originalRotation;

    void Start () {
        originalRotation = transform.localRotation;
    }

    void Update () {
        // ROTATE
        rotationX += Input.GetAxis ("Mouse X") * sensitivityX;
        rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
        rotationX = ClampAngle (rotationX, minimumX, maximumX);
        rotationY = ClampAngle (rotationY, minimumY, maximumY);
        Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, -Vector3.right);
        head.transform.localRotation = originalRotation * xQuaternion * yQuaternion;

        // MOVE
        Vector3 forwardMovement = Camera.main.transform.forward;
        forwardMovement.y = 0;

        Vector3 sidewayMovement = Camera.main.transform.right;
        sidewayMovement.y = 0;

        if (Input.GetKey (KeyCode.W) || Input.GetMouseButton (0)) {
            body.transform.position += forwardMovement * movementFactor * Time.deltaTime;
        }
        if (Input.GetKey (KeyCode.S)) {
            body.transform.position -= forwardMovement * movementFactor * Time.deltaTime;
        }
        if (Input.GetKey (KeyCode.D)) {
            body.transform.position += sidewayMovement * movementFactor * Time.deltaTime;
        }
        if (Input.GetKey (KeyCode.A)) {
            body.transform.position -= sidewayMovement * movementFactor * Time.deltaTime;
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
