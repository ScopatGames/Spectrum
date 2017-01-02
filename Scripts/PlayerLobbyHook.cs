using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class PlayerLobbyHook : LobbyHook {

	public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        Debug.Log("OnLobbyServerSceneLoadedForPlayer");
        if (lobbyPlayer == null)
            return;
        LobbyPlayer lp = lobbyPlayer.GetComponent<LobbyPlayer>();

        if (lp != null)
            GameData.AddMultiplayer(gamePlayer, lp.slot, lp.GetPlayerColorIndex(), lp.nameInput.text, GenerateRandomTerrainSeed());
    }

    private int GenerateRandomTerrainSeed()
    {
        return Random.Range(0, 1024);
    }

}
