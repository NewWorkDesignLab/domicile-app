using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SpatialTracking;

public class Player : NetworkBehaviour {
    public static Player localPlayer;
    public GameObject cameraPrefabForLocalPlayer;
    public GameObject cameraHolderIfLocalPlayer;

    public PlayerNetworking playerNetworking;
    public PlayerVisuals playerVisuals;
    public PlayerMovement playerMovement;
    public PlayerRotation playerRotation;

    private bool setupReady = false;

    void Start () {
        if (isLocalPlayer) {
            Debug.Log ("[Player Start] New Player Instance (LOCALE PLAYER) spawned!");
            PrepareLocalePlayer ();
        } else {
            Debug.Log ("[Player Start] New Player Instance spawned.");
        }
        setupReady = true;
    }

    void PrepareLocalePlayer () {
        localPlayer = this;
        transform.gameObject.tag = "LocalPlayer";
        playerNetworking.RegisterLocalPlayer ();
        playerMovement.SetModeIdle ();
        playerVisuals.Hide ();
        AddCamera ();
    }

    void Update () {
        if (isLocalPlayer && setupReady) {
            playerRotation.ApplyMouseRotation ();
            playerMovement.CheckKeyboardInput ();
            playerMovement.UpdateMovement ();
        }
    }

    public void HideUnrelatedPlayers () {
        if (isLocalPlayer) {
            Debug.Log ("[Player HideUnrelatedPlayers] Going to hide all unrelated Players. (Local Player Data: " + playerNetworking.belongsToScenario + "; " + playerNetworking.isOwner + ")");
            var players = GameObject.FindGameObjectsWithTag ("Player");
            foreach (GameObject p in players) {
                var otherPlayer = p.GetComponent<Player> ();
                var otherPlayerNet = otherPlayer.playerNetworking;

                if (!playerNetworking.isOwner || otherPlayerNet.isOwner) {
                    // This (Local)Player is Guest OR the iterated Player is Owner
                    otherPlayer.playerVisuals.Hide ();
                } else if (otherPlayerNet.belongsToScenario == playerNetworking.belongsToScenario) {
                    // This (Local)Player is Owner AND the iterated Player is Guest
                    // AND This (Local)Player and Iterated Player are in same Scenario
                    otherPlayer.playerVisuals.Show ();
                } else {
                    // Else Hide
                    otherPlayer.playerVisuals.Hide ();
                }
            }
        }
    }

    public void AddCamera () {
        if (isLocalPlayer) {
            Instantiate (cameraPrefabForLocalPlayer, cameraHolderIfLocalPlayer.transform, false);
            Camera.main.transform.localPosition = new Vector3 (0, 0, 0);
        }
    }
}
