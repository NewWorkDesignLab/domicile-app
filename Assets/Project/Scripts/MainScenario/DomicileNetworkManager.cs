using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class DomicileNetworkManager : NetworkManager {
    public static DomicileNetworkManager instance;

    public override void Awake () {
        instance = this;
        base.Awake ();
    }

    public override void OnStartServer () {
        base.OnStartServer ();
        NetworkServer.RegisterHandler<CreatePlayerMessage> (OnCreatePlayer);
    }

    public override void OnClientConnect (NetworkConnection conn) {
        base.OnClientConnect (conn);

        CreatePlayerMessage createPlayerMessage = new CreatePlayerMessage {
            email = DataManager.persistedData.lastKnownUid,
            scenario = SessionManager.scenario.id,
            role = SessionManager.participation.enum_role
        };

        conn.Send (createPlayerMessage);
    }

    void OnCreatePlayer (NetworkConnection conn, CreatePlayerMessage message) {
        GameObject gameobject = Instantiate (playerPrefab);
        NetworkServer.Spawn (gameobject);

        Player player = gameobject.GetComponent<Player> ();
        player.email = message.email;
        player.scenario = message.scenario;
        player.role = message.role;

        NetworkServer.AddPlayerForConnection (conn, gameobject);
    }
}
