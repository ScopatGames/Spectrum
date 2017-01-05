using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class PlayerLobbyHook : LobbyHook {

    public GameObject gameManagerPrefab;

	public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
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
