using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

    private SetupManager setupManager;
    
    void Start()
    {
        //setupManager = GameObject.FindGameObjectWithTag(_Tags.lobbyManager).GetComponent<SetupManager>();
        setupManager = null;
    }

    public override void OnStartServer()
    {

        setupManager.RegenerateTerrain();

    }

    public override void OnStartClient(NetworkClient client)
    {
        setupManager.AssignPlayerColors();
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        //base.OnServerAddPlayer(conn, playerControllerId);
        if(numPlayers >= 2)
        {
            if (LogFilter.logError) { Debug.LogError("Only 2 players allowed in the game."); }
            return;
        }
        if (playerPrefab == null)
        {
            if (LogFilter.logError) { Debug.LogError("The PlayerPrefab is empty on the NetworkManager. Please setup a PlayerPrefab object."); }
            return;
        }

        if (playerPrefab.GetComponent<NetworkIdentity>() == null)
        {
            if (LogFilter.logError) { Debug.LogError("The PlayerPrefab does not have a NetworkIdentity. Please add a NetworkIdentity to the player prefab."); }
            return;
        }

        if (playerControllerId < conn.playerControllers.Count && conn.playerControllers[playerControllerId].IsValid && conn.playerControllers[playerControllerId].gameObject != null)
        {
            if (LogFilter.logError) { Debug.LogError("There is already a player at that playerControllerId for this connections."); }
            return;
        }

        GameObject player;
        Transform startPos = GetStartPosition();
        if (startPos != null)
        {
            player = (GameObject)Instantiate(playerPrefab, startPos.position, startPos.rotation);
        }
        else
        {
            player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }

        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        //Assign color to player
        player.GetComponentInChildren<MeshRenderer>().material.color = setupManager.playerColorDictionaries[numPlayers-1][_ColorType.PlayerShipSpace.ToString()];

        //Assign player to gameController
        setupManager.players[numPlayers-1] = player;
    }

}
