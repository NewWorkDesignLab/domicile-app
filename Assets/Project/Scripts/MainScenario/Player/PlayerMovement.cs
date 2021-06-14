using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    public float movementFactor = 1f;
    private PlayerMovementStatus movementStatus;
    private KeyboardWalkDirections keyboardWalkDirections;

    public void SetModeIdle () {
        if (movementStatus == PlayerMovementStatus.idle)
            return;

        movementStatus = PlayerMovementStatus.idle;
        Player.localPlayer.Stand ();
        if (HUD.instance != null)
            HUD.instance.positionText.text = "Stehen";

        if (keyboardWalkDirections == null)
            keyboardWalkDirections = new KeyboardWalkDirections ();
        keyboardWalkDirections.Reset ();
    }

    public void SetModeWalk () {
        if (movementStatus == PlayerMovementStatus.walk)
            return;

        movementStatus = PlayerMovementStatus.walk;
        Player.localPlayer.Stand ();
        if (HUD.instance != null)
            HUD.instance.positionText.text = "Laufen";
    }

    public void SetModeCrawl () {
        if (movementStatus == PlayerMovementStatus.crawl)
            return;

        movementStatus = PlayerMovementStatus.crawl;
        Player.localPlayer.Crawl ();
        if (HUD.instance != null)
            HUD.instance.positionText.text = "Hocken";

        if (keyboardWalkDirections == null)
            keyboardWalkDirections = new KeyboardWalkDirections ();
        keyboardWalkDirections.Reset ();
    }

    void Start () {
        SetModeIdle ();
    }

    void Update () {
        CheckKeyboardInput ();
        UpdateMovement ();
    }

    void CheckKeyboardInput () {
        if (keyboardWalkDirections == null)
            keyboardWalkDirections = new KeyboardWalkDirections ();

        var keyboard = Keyboard.current;
        var mouse = Mouse.current;
        if (keyboard != null) {
            if (mouse != null)
                keyboardWalkDirections.Forward (keyboard.wKey.isPressed || mouse.leftButton.isPressed);
            else
                keyboardWalkDirections.Forward (keyboard.wKey.isPressed);
            keyboardWalkDirections.Backward (keyboard.sKey.isPressed);
            keyboardWalkDirections.Left (keyboard.aKey.isPressed);
            keyboardWalkDirections.Right (keyboard.dKey.isPressed);
        }

        if (keyboardWalkDirections.AnyDirectionActive ()) {
            // Key currently pressed
            SetModeWalk ();
        } else if (keyboardWalkDirections.keyboardActive) {
            // no Key currently pressed, but Keyboard was active recently
            SetModeIdle ();
        }
    }

    void UpdateMovement () {
        if (movementStatus == PlayerMovementStatus.walk) {
            Vector3 forwardMovement = Camera.main.transform.forward;
            forwardMovement.y = 0;
            Vector3 sidewayMovement = Camera.main.transform.right;
            sidewayMovement.y = 0;

            if (keyboardWalkDirections.forward || !keyboardWalkDirections.AnyDirectionActive ()) {
                transform.position += forwardMovement * movementFactor * Time.deltaTime;
            }
            if (keyboardWalkDirections.backward) {
                transform.position -= forwardMovement * movementFactor * Time.deltaTime;
            }
            if (keyboardWalkDirections.right) {
                transform.position += sidewayMovement * movementFactor * Time.deltaTime;
            }
            if (keyboardWalkDirections.left) {
                transform.position -= sidewayMovement * movementFactor * Time.deltaTime;
            }
        }
    }

    public void ToggleModeCrawl () {
        if (movementStatus != PlayerMovementStatus.crawl) {
            SetModeCrawl ();
        } else {
            SetModeIdle ();
        }
    }
}

public enum PlayerMovementStatus { load, idle, walk, crawl }
public class KeyboardWalkDirections {
    public bool keyboardActive = false;
    public bool forward = false;
    public bool backward = false;
    public bool left = false;
    public bool right = false;

    public bool AnyDirectionActive () {
        return forward || backward || left || right;
    }

    public void Forward (bool status) {
        SetKeyboardActive (status);
        forward = status;
    }

    public void Backward (bool status) {
        SetKeyboardActive (status);
        backward = status;
    }

    public void Left (bool status) {
        SetKeyboardActive (status);
        left = status;
    }

    public void Right (bool status) {
        SetKeyboardActive (status);
        right = status;
    }

    private void SetKeyboardActive (bool stauts) {
        if (stauts == true)
            keyboardActive = true;
    }

    public void Reset () {
        keyboardActive = false;
        forward = false;
        backward = false;
        left = false;
        right = false;
    }
}
