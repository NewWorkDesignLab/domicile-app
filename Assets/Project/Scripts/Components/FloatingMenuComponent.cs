using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingMenuComponent : MonoBehaviour {
    public GameObject menuButton;
    public GameObject menuItems;
    private float centerDistaceBreakpoint = .75f;
    private float buttonDistancePlayer = 1.5f;
    private bool endConfirmation = false;
    private bool menuIsOpen = false;
    private bool menuIsHidden = false;

    void Start () {
        CloseMenu ();
    }

    void Update () {
        if (Camera.main != null) {
            UpdatePosition ();
        }
    }

    void UpdatePosition () {
        Vector3 currentPos = transform.position;
        Vector3 camPos = Camera.main.transform.position;

        Vector3 targetPos = camPos + Camera.main.transform.forward.normalized * buttonDistancePlayer;
        float distTarget = Vector3.Distance (currentPos, targetPos);
        float distPlayer = Vector3.Distance (currentPos, camPos);

        if (distTarget > centerDistaceBreakpoint || distPlayer < buttonDistancePlayer * 0.98 || distPlayer > buttonDistancePlayer * 1.02) {
            Vector3 currentPosAwayFromPlayer = MoveTowardsWithOvershoot (camPos, currentPos, buttonDistancePlayer);
            Vector3 newPoint = Vector3.MoveTowards (targetPos, currentPosAwayFromPlayer, centerDistaceBreakpoint);
            transform.position = newPoint;
            CloseMenu ();
        }
        transform.rotation = Quaternion.Euler (Camera.main.transform.eulerAngles);
    }

    Vector3 MoveTowardsWithOvershoot (Vector3 current, Vector3 target, float distance) {
        Vector3 direction = target - current;
        Vector3 newSpot = current + (direction.normalized * distance);
        return newSpot;
    }

    void Hide () {
        menuIsHidden = true;
        menuIsOpen = false;
        menuItems.SetActive (false);
        menuButton.SetActive (false);
    }
    void Show () {
        menuIsHidden = false;
        CloseMenu ();
    }

    public void CloseMenu () {
        if (!menuIsHidden) {
            menuIsOpen = false;
            menuItems.SetActive (false);
            menuButton.SetActive (true);
        }
    }
    public void OpenMenu () {
        if (!menuIsHidden) {
            menuIsOpen = true;
            menuItems.SetActive (true);
            menuButton.SetActive (false);
        }
    }

    public void ToggleMenu () {
        if (menuIsOpen)
            CloseMenu ();
        else
            OpenMenu ();
    }

    public void ToggleCrawlButton () {
        Player.localPlayer.playerMovement.ToggleModeCrawl ();
        CloseMenu ();
    }
    public void ScreenshotButton () {
        Hide ();
        ScreenshotManager.TakeScreenshot ((success) => {
            Show ();
        });
    }
    public void ExitScenarioButton () {
        if (endConfirmation == false) {
            endConfirmation = true;
            HUD.instance.ShowNotification ("Willst du wirklich beenden? Bitte bestätige durch erneutes beenden.", 5f, () => {
                endConfirmation = false;
            });
        } else {
            // TODO: End Scenario
        }
    }
}
