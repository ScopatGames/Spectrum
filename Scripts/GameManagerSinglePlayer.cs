using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

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

    public void ReturnToSinglePlayerLobby()
    {
        CrossPlatformInputManager.SetButtonUp("Lobby");
        SceneManager.LoadScene(_Scenes.sceneSinglePlayerLobby);
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
        player = (GameObject)Instantiate(playerPrefab, new Vector3(1000, 1000, 1000), Quaternion.identity);
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

    private void GameStateSinglePlanetAttack() {
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
            if (pm.playerNumber == 0)
            {
                pm.PlayerStateChange(_GameState.SinglePlanetAttack);
                break;
            }
        }
    }

    private void GameStateSinglePlanetDefend() {
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
            if(pm.playerNumber == 0)
            {
                pm.PlayerStateChange(_GameState.SinglePlanetDefend);
                break;
            }
        }
    }

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
