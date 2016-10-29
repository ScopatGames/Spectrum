using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;

public class GameManagerMultiplayer :NetworkBehaviour {

    private GameData gameData;
    
    void Awake()
    {
        gameData = GetComponent<GameData>();
    }
    
    void Start()
    {
        gameData.Setup();
        StartCoroutine("InitiateGameStateMultiNeutral");
        
        if (isServer)
        {
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

    //PRIVATE METHODS
    //--------------------------------------------------------------
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
