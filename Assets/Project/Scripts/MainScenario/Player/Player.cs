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
    public bool isPlayer;

    void Start () {
        if (isLocalPlayer) {
            Debug.Log ("Is Local Player, going to send command to Server to change info");
            CmdChangeInfo (MainScenarioScript.instance.webglScenarioID, MainScenarioScript.instance.webglIsPlayer);
            cameraHolder.AddComponent (typeof (Camera));
        }
    }

    [Command]
    private void CmdChangeInfo (int _scenarioId, bool _isPlayer) {
        // only runs on server, even if called from client
        Debug.Log ("Changed infos in server side. Values: " + _scenarioId + "; " + _isPlayer);
        belongsToScenario = _scenarioId;
        isPlayer = _isPlayer;
    }

    public void HideUnrelatedPlayers () {
        // runs on client
        if (isLocalPlayer) {
            Debug.Log ("GOING TO HIDE ALL PLAYERS THAT DO NOT BELONG TO " + belongsToScenario);

            var players = GameObject.FindGameObjectsWithTag ("Player");
            foreach (GameObject p in players) {
                var playerComp = p.GetComponent<Player> ();
                if (playerComp.belongsToScenario != belongsToScenario) {
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

    }

    public void HidePlayer () {
        foreach (MeshRenderer obj in objectToHide) {
            obj.enabled = false;
        }
    }

    public void ShowPlayer () {
        foreach (MeshRenderer obj in objectToHide) {
            obj.enabled = true;
        }
    }
}
