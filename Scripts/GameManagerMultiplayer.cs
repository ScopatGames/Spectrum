using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;

public class GameManagerMultiplayer : NetworkBehaviour {

    static public GameManagerMultiplayer instance;
    [HideInInspector]
    public ItemController itemController;

    private GameData gameData;
    public bool opponentIsReady = false;
    
    void Awake()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        gameData = GameData.instance;
        itemController = GetComponent<ItemController>();
    }
    
    void Start()
    {
        gameData.Setup();
        StartCoroutine("InitiateGameStateMultiNeutral");
        
        if (isServer)
        {
            StartCoroutine("InitiateItems");
            StartCoroutine("GameLoop");
        }
    }
    
    //PUBLIC METHODS -- Currently called from GUI buttons
    //------------------------------------------------

    public void ChangeGameStateMultiPlayerOnePlanet()
    {
        RpcGameStateSetup(_GameState.MultiPlayerOnePlanet);
    }

    public void ChangeGameStateMultiPlayerTwoPlanet()
    {
        RpcGameStateSetup(_GameState.MultiPlayerTwoPlanet);
    }

    public void ChangeGameStateMultiNeutral()
    {
        RpcGameStateSetup(_GameState.MultiNeutral);
    }
    
    [Command]
    public void CmdDestroyTerrainTile(int tileIndex)
    {
        RpcDestroyTerrainTile(tileIndex);
    }

    //PRIVATE METHODS
    //--------------------------------------------------------------
    [Command]
    private void CmdClientCheckin()
    {
        opponentIsReady = true;
    }

    private IEnumerator GameLoop()
    {
        while(GameData.playerManagers.Count < 2)
            yield return null;

    }

    private void GameStateMultiNeutral()
    {
        if (GameData.playerTerrains.Count == 2)
        {
            GameData.playerTerrains[0].SetActive(false);
            GameData.playerTerrains[1].SetActive(false);
        }
        gameData.dynamicLight.intensity = gameData.spaceDynamicLightingIntensity;
        gameData.dynamicLight.transform.rotation = Quaternion.Euler(gameData.spaceDynamicLightingRotation);
        gameData.backgroundMeshRenderer.material = gameData.spaceBackgroundMaterial;
        gameData.starsParticleSystem.Play();
        foreach (PlayerManager pm in GameData.playerManagers)
        {
            pm.PlayerStateChange(_GameState.MultiNeutral);
        }
    }

    private void GameStateMultiPlayerOnePlanet()
    {
        if (GameData.playerTerrains.Count == 2)
        {
            GameData.playerTerrains[0].SetActive(true);
            GameData.playerTerrains[1].SetActive(false);
        }
        gameData.dynamicLight.intensity = gameData.planetDynamicLightingIntensity;
        gameData.dynamicLight.transform.rotation = Quaternion.Euler(gameData.planetDynamicLightingRotation);
        gameData.backgroundMeshRenderer.material = gameData.planetBackgroundMaterial;
        gameData.starsParticleSystem.Stop();
        gameData.starsParticleSystem.Clear();
        foreach (PlayerManager pm in GameData.playerManagers)
        {
            pm.PlayerStateChange(_GameState.MultiPlayerOnePlanet);
        }
    }

    private void GameStateMultiPlayerTwoPlanet()
    {
        if (GameData.playerTerrains.Count == 2)
        {
            GameData.playerTerrains[0].SetActive(false);
            GameData.playerTerrains[1].SetActive(true);
        }
        gameData.dynamicLight.intensity = gameData.planetDynamicLightingIntensity;
        gameData.dynamicLight.transform.rotation = Quaternion.Euler(gameData.planetDynamicLightingRotation);
        gameData.backgroundMeshRenderer.material = gameData.planetBackgroundMaterial;
        gameData.starsParticleSystem.Stop();
        gameData.starsParticleSystem.Clear();
        foreach (PlayerManager pm in GameData.playerManagers)
        {
            pm.PlayerStateChange(_GameState.MultiPlayerTwoPlanet);
        }
    }
    
    private IEnumerator InitiateGameStateMultiNeutral()
    {
        while (GameData.playerManagers.Count < 2)
        {
            yield return null;
        }

        //wait a frame to let all the player components initialize
        yield return null;

        GameStateMultiNeutral();

        if (!isServer)
        {
            CmdClientCheckin();
        }
    }

    private IEnumerator InitiateItems()
    {
        /*while (!opponentIsReady)
        {
            yield return null;
        }*/
        itemController.PoolSetup();
        itemController.DeployBombDrops(0, 1);
        itemController.DeployNeutralPickups(0, 9);

        return null;

    }

    [ClientRpc]
    private void RpcDestroyTerrainTile(int tileIndex)
    {
        gameData.terrainTileList[tileIndex].GetComponent<TerrainTileInfo>().DestroyTile();
    }

    [ClientRpc]
    private void RpcGameStateSetup(_GameState gameState)
    {
        switch (gameState)
        {
            case _GameState.MultiNeutral:
                GameStateMultiNeutral();
                break;
            case _GameState.MultiPlayerOnePlanet:
                GameStateMultiPlayerOnePlanet();
                break;
            case _GameState.MultiPlayerTwoPlanet:
                GameStateMultiPlayerTwoPlanet();
                break;
        }
    }
}
