using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour {
    public GameObject cameraHolder;
    public MeshRenderer[] objectToHide;

    [SyncVar (hook = nameof (HookScenario))]
    public int belongsToScenario;

    [SyncVar (hook = nameof (HookPlayer))]
    public bool isOwner;

    void Start () {
        if (isLocalPlayer) {
            Debug.Log ("Is Local Player, going to send command to Server to change info");
            CmdChangeInfo (SessionManager.scenario.id, SessionManager.IsOwner ());
            cameraHolder.AddComponent (typeof (Camera));
            // Hide own Body
            HidePlayer ();
        }
    }

    [Command]
    private void CmdChangeInfo (int _scenarioId, bool _isOwner) {
        // only runs on server, even if called from client
        Debug.Log ("Changed infos in server side. Values: " + _scenarioId + "; " + _isOwner);
        belongsToScenario = _scenarioId;
        isOwner = _isOwner;
    }

    public void HideUnrelatedPlayers () {
        // runs on client
        if (isLocalPlayer) {
            Debug.Log ("GOING TO HIDE ALL PLAYERS THAT DO NOT BELONG TO " + belongsToScenario + "; IS_OWNER: " + isOwner);

            var players = GameObject.FindGameObjectsWithTag ("Player");
            foreach (GameObject p in players) {
                var playerComp = p.GetComponent<Player> ();
                if (playerComp.belongsToScenario != belongsToScenario || isOwner) {
                    playerComp.HidePlayer ();
                } else {
                    playerComp.ShowPlayer ();
                }
            }
        }
    }

    void HookScenario (int oldValue, int newValue) {
        // Update all Client-Side Playerprefabs
        var players = GameObject.FindGameObjectsWithTag ("Player");
        foreach (GameObject p in players) {
            var playerComp = p.GetComponent<Player> ();
            playerComp.HideUnrelatedPlayers ();
        }
    }
    void HookPlayer (bool oldValue, bool newValue) {
        // Update all Client-Side Playerprefabs
        var players = GameObject.FindGameObjectsWithTag ("Player");
        foreach (GameObject p in players) {
            var playerComp = p.GetComponent<Player> ();
            playerComp.HideUnrelatedPlayers ();
        }
    }

    public void HidePlayer () {
        foreach (MeshRenderer obj in objectToHide) {
            obj.enabled = false;
        }
    }

    public void ShowPlayer () {
        if (!isLocalPlayer) {
            foreach (MeshRenderer obj in objectToHide) {
                obj.enabled = true;
            }
        }
    }
}
