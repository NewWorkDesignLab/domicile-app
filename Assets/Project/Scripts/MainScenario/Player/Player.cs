using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SpatialTracking;

public class Player : NetworkBehaviour {
    public GameObject cameraPrefabForLocalPlayer;
    public GameObject cameraHolderIfLocalPlayer;

    public PlayerMovement playerMovement;
    public PlayerRotation playerRotation;
    public PlayerVisuals playerVisuals;
    public PlayerNetworking playerNetworking;

    void Start () {
        playerMovement.SetModeIdle ();
        if (isLocalPlayer) {
            Debug.Log ("[Player Start] New Player Instance (LOCALE PLAYER) spawned!");
            playerNetworking.RegisterLocalPlayer ();
            playerVisuals.Hide ();
            AddCamera ();
        } else {
            Debug.Log ("[Player Start] New Player Instance spawned.");
            // No Network-Registration needed.
            // No hiding of own Visuals needed.
            // No Camera needed.
        }
    }

    void Update () {
        if (isLocalPlayer) {
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
                if (otherPlayerNet.belongsToScenario == playerNetworking.belongsToScenario && !otherPlayerNet.isOwner && playerNetworking.isOwner) {
                    // show other players if they belong to scenario and player is owner
                    otherPlayer.playerVisuals.Show ();
                } else {
                    // hide other players that do not belong to this scenario
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
