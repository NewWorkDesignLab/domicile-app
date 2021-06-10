using Mirror;

public struct CreatePlayerMessage : NetworkMessage {
    public string email;
    public int scenario;
    public PlayerRole role;
}

public enum PlayerRole {
    None,
    Player,
    Spectator
}
