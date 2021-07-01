using Mirror;

public struct CreatePlayerMessage : NetworkMessage {
    public string email;
    public int scenario;
    public PlayerRole role;
}
