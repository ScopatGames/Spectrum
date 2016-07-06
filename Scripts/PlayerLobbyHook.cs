using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class PlayerLobbyHook : LobbyHook {

	public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        if (lobbyPlayer == null)
            return;
        LobbyPlayer lp = lobbyPlayer.GetComponent<LobbyPlayer>();

        if (lp != null)
            GameManager.AddPlayer(gamePlayer, lp.slot, lp.GetPlayerColorIndex(), lp.nameInput.text);
    }
       
}
