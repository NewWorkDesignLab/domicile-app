using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SpatialTracking;

public class Player : NetworkBehaviour {
    public static Player localPlayer;

    [Header ("Visibillity")]
    public MeshRenderer playerHead;
    public MeshRenderer playerBody;
    public MeshRenderer playerVector;
    public Light viewportInner;
    public Light viewportOuter;

    [Header ("Local Player")]
    public PlayerMovement playerMovement;
    public PlayerRotation playerRotation;
    public GameObject movementTrigger;
    public Rigidbody rb;
    public CapsuleCollider capsuleCollider;
    public TrackedPoseDriver trackedPoseDriver;

    [Header ("Local Player Prefabs")]
    public GameObject localPlayerExtensionHead;

    [Header ("Settings")]
    public float headNormalHeight = 2.75f;
    public float headCrawlHeight = 0.5f;

    [Header ("Sync Variables")]
    [SyncVar]
    public string email;
    [SyncVar]
    public int scenario;
    [SyncVar]
    public PlayerRole role;

    void Start () {
#if UNITY_STANDALONE_LINUX
        Debug.Log ("[InactivePlayer Start] A new Player joined to Server: " + scenario + " - " + email + " - " + role);
        SetupInactivePlayer ();
#else
        if (isLocalPlayer) {
            SetupLocalPlayer ();
        } else if (ShouldBeVisable ()) {
            SetupVisablePlayer ();
        } else {
            SetupInactivePlayer ();
        }
#endif
    }

    private bool ShouldBeVisable () {
        if (!isLocalPlayer && localPlayer != null) {
            // This is not localPlayer, but a localPlayer is present
            if (scenario == localPlayer.scenario) {
                // This Instance is same Scenario as localPlayer
                if (localPlayer.role == PlayerRole.spectator || localPlayer.role == PlayerRole.owner) {
                    // localPlayer is Spectator
                    if (role == PlayerRole.player) {
                        // This Instance is a Player

                        // If all true, this Instance should be visable
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void SetupInactivePlayer () {
        RenamePlayer ("InactivePlayer");
        SetVisabillity (false);
        SetInteractabillity (false);
    }

    private void SetupLocalPlayer () {
        localPlayer = this;
        RenamePlayer ("LocalPlayer");
        SetVisabillity (false);
        SetInteractabillity (true);
        Instantiate (localPlayerExtensionHead, playerHead.transform, false);
    }

    private void SetupVisablePlayer () {
        RenamePlayer ("VisablePlayer");
        SetVisabillity (true);
        SetInteractabillity (false);
    }

    private void RenamePlayer (string type) {
        transform.gameObject.name = type + " (" + scenario + " | " + email + " | " + role + ")";
        transform.gameObject.tag = type;
    }

    private void SetVisabillity (bool value) {
        playerHead.enabled = value;
        playerBody.enabled = value;
        playerVector.enabled = value;
        playerVector.gameObject.SetActive (value);
        viewportInner.enabled = value;
        viewportInner.gameObject.SetActive (value);
        viewportOuter.enabled = value;
        viewportOuter.gameObject.SetActive (value);
    }

    private void SetInteractabillity (bool value) {
        playerMovement.enabled = value;
        playerRotation.enabled = value;

        capsuleCollider.enabled = value;
        rb.isKinematic = !value;
        rb.detectCollisions = value;

        SetMovementTriggerIfAndroid (value);
#if UNITY_ANDROID
        trackedPoseDriver.enabled = value;
#else
        trackedPoseDriver.enabled = false;
#endif
    }

    public void SetMovementTriggerIfAndroid (bool value) {
#if UNITY_ANDROID
        movementTrigger.SetActive (value);
#else
        movementTrigger.SetActive (false);
#endif
    }

    public void Stand () {
        SetMovementTriggerIfAndroid (true);
        SetHeadPosition (headNormalHeight);
    }

    public void Crawl () {
        SetMovementTriggerIfAndroid (false);
        SetHeadPosition (headCrawlHeight);
    }

    private void SetHeadPosition (float value) {
        Vector3 newHeadPos = playerHead.gameObject.transform.localPosition;
        newHeadPos.y = value;

        float valueForBody = (value - 0.25f) / 2f;
        Vector3 newBodyPos = playerBody.gameObject.transform.localPosition;
        Vector3 newBodyScale = playerBody.gameObject.transform.localScale;
        newBodyPos.y = valueForBody;
        newBodyScale.y = valueForBody;

        playerHead.gameObject.transform.localPosition = newHeadPos;
        playerBody.gameObject.transform.localPosition = newBodyPos;
        playerBody.gameObject.transform.localScale = newBodyScale;
    }
}
