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
        // sends the change of this Player to all other clients
        belongsToScenario = _scenarioId;
        isOwner = _isOwner;
    }

    void ScenarioChanged (int oldValue, int newValue) {
        HideUnrelatedPlayersOnLocalPlayer ();
    }
    void OwnershipChanged (bool oldValue, bool newValue) {
        HideUnrelatedPlayersOnLocalPlayer ();
    }

    void HideUnrelatedPlayersOnLocalPlayer () {
        var player = GameObject.FindGameObjectsWithTag ("LocalPlayer") [0];
        if (player != null) {
            var playerComp = player.GetComponent<Player> ();
            playerComp.HideUnrelatedPlayers ();
        } else {
            Debug.LogWarning ("[PlayerNetworking ScenarioChanged] Should find LocalPlayer to Hide unrelated Players, but no LocalPlayer was found! Going to Hide all Players.");
            var players = GameObject.FindGameObjectsWithTag ("Player");
            foreach (GameObject p in players) {
                var playerComp = p.GetComponent<Player> ();
                playerComp.playerVisuals.Hide ();
            }
        }
    }
}
