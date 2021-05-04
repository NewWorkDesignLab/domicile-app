using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerNetworking : NetworkBehaviour {
    public Player player;

    [SyncVar (hook = nameof (ScenarioChanged))]
    public int belongsToScenario;

    [SyncVar (hook = nameof (OwnershipChanged))]
    public bool isOwner;

    public void RegisterLocalPlayer () {
        if (isLocalPlayer) {
            RegisterPlayerOnServer (SessionManager.scenario.id, SessionManager.IsOwner ());
        }
    }

    [Command]
    private void RegisterPlayerOnServer (int _scenarioId, bool _isOwner) {
        // only runs on server, even if called from client
        belongsToScenario = _scenarioId;
        isOwner = _isOwner;
    }

    void ScenarioChanged (int oldValue, int newValue) {
        // Update all Client-Side Playerprefabs
        var players = GameObject.FindGameObjectsWithTag ("Player");
        foreach (GameObject p in players) {
            var playerComp = p.GetComponent<Player> ();
            playerComp.HideUnrelatedPlayers ();
        }
    }
    void OwnershipChanged (bool oldValue, bool newValue) {
        // Update all Client-Side Playerprefabs
        var players = GameObject.FindGameObjectsWithTag ("Player");
        foreach (GameObject p in players) {
            var playerComp = p.GetComponent<Player> ();
            playerComp.HideUnrelatedPlayers ();
        }
    }
}
