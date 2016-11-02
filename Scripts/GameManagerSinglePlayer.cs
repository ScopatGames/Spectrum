using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameManagerSinglePlayer : MonoBehaviour {

    public GameObject playerPrefab;
    private GameData gameData;
    private GameObject player;

    void Awake()
    {
        gameData = GetComponent<GameData>();
        InstantiatePlayer();
    }

    void Start()
    {
        gameData.Setup();
        StartCoroutine("InitiateGameStateSingleNeutral");
        StartCoroutine("GameLoop");
    }
    
    //PUBLIC METHODS -- Currently called from GUI buttons
    //------------------------------------------------
    public void ChangeGameStateSinglePlanetAttack()
    {
        GameStateSetup(_GameState.SinglePlanetAttack);
    }

    public void ChangeGameStateSinglePlanetDefend()
    {
        GameStateSetup(_GameState.SinglePlanetDefend);
    }

    public void ChangeGameStateSingleNeutral()
    {
        GameStateSetup(_GameState.SingleNeutral);
    }

    //PRIVATE METHODS
    //--------------------------------------------------------------
    private IEnumerator GameLoop()
    {
        while (GameData.playerManagers.Count < 1)
        {
            yield return null;
        }

    }

    private void InstantiatePlayer()
    {
        System.Random rnd = new System.Random();
        player = (GameObject)Instantiate(playerPrefab);
        player.transform.parent = null;
        //associate this gameobject to the playermanager
        foreach (PlayerManager pm in GameData.playerManagers)
        {
            if (pm.playerNumber == 0)
            {
                pm.SetupSP(player);
                break;
            }
        }
    }
    /**** USE THESE AS REFERENCE TO BUILD SINGLE PLAYER STATE CHANGES
    private void GameStateMultiNeutral()
    {
        if (playerTerrains.Count == 2)
        {
            playerTerrains[0].SetActive(false);
            playerTerrains[1].SetActive(false);
        }
        dynamicLight.intensity = spaceDynamicLightingIntensity;
        dynamicLight.transform.rotation = Quaternion.Euler(spaceDynamicLightingRotation);
        backgroundMeshRenderer.material = spaceBackgroundMaterial;
        starsParticleSystem.Play();
        foreach (PlayerManager pm in playerManagers)
        {
            pm.PlayerStateChange(_GameState.MultiNeutral);
        }
    }

    private void GameStateMultiPlayerOnePlanet()
    {
        if (playerTerrains.Count == 2)
        {
            playerTerrains[0].SetActive(true);
            playerTerrains[1].SetActive(false);
        }
        dynamicLight.intensity = planetDynamicLightingIntensity;
        dynamicLight.transform.rotation = Quaternion.Euler(planetDynamicLightingRotation);
        backgroundMeshRenderer.material = planetBackgroundMaterial;
        starsParticleSystem.Stop();
        starsParticleSystem.Clear();
        foreach (PlayerManager pm in playerManagers)
        {
            pm.PlayerStateChange(_GameState.MultiPlayerOnePlanet);
        }
    }

    private void GameStateMultiPlayerTwoPlanet()
    {
        if (playerTerrains.Count == 2)
        {
            playerTerrains[0].SetActive(false);
            playerTerrains[1].SetActive(true);
        }
        dynamicLight.intensity = planetDynamicLightingIntensity;
        dynamicLight.transform.rotation = Quaternion.Euler(planetDynamicLightingRotation);
        backgroundMeshRenderer.material = planetBackgroundMaterial;
        starsParticleSystem.Stop();
        starsParticleSystem.Clear();
        foreach (PlayerManager pm in playerManagers)
        {
            pm.PlayerStateChange(_GameState.MultiPlayerTwoPlanet);
        }
    }
    */
    private void GameStateSingleNeutral() {
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
            if (pm.playerNumber == 0)
            {
                pm.PlayerStateChange(_GameState.SingleNeutral);
                break;
            }
        }
    }

    private void GameStateSinglePlanetAttack() { }

    private void GameStateSinglePlanetDefend() { }

    private void GameStateSetup(_GameState gameState)
    {
        switch (gameState)
        {
            case _GameState.SingleNeutral:
                GameStateSingleNeutral();
                break;
            case _GameState.SinglePlanetAttack:
                GameStateSinglePlanetAttack();
                break;
            case _GameState.SinglePlanetDefend:
                GameStateSinglePlanetDefend();
                break;
        }
    }

    private IEnumerator InitiateGameStateSingleNeutral()
    {
        while (GameData.playerManagers.Count < 1)
        {
            yield return null;
        }

        //wait a frame to let all the player components initialize
        yield return null;

        GameStateSingleNeutral();
    }

    
}
