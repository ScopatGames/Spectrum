using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class GameManagerSinglePlayer : MonoBehaviour {

    public GameObject playerPrefab;
    public GameObject opponentPrefab;
    public GameObject bombPrefab;

    private GameData gameData;
    private GameObject opponent;
    private GameObject player;

    void Awake()
    {
        gameData = GetComponent<GameData>();
        InstantiatePlayer();
        InstantiateOpponent();
    }

    void Start()
    {
        gameData.Setup();
        StartCoroutine("InitiateGameStateSingleNeutral");
        StartCoroutine("GameLoop");
    }
    
    //PUBLIC METHODS -- Currently called from GUI buttons
    //------------------------------------------------
    public void Bomb()
    {
        GameObject bomb = (GameObject)Instantiate(bombPrefab, player.transform.position, new Quaternion());
        bomb.GetComponent<Rigidbody2D>().velocity = player.GetComponent<Rigidbody2D>().velocity;
    }

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

    private void GameStateSingleNeutral()
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
            pm.PlayerStateChange(_GameState.SingleNeutral);
        }
    }

    private void GameStateSinglePlanetAttack()
    {
        if (GameData.playerTerrains.Count == 2)
        {
            GameData.playerTerrains[0].SetActive(false);
            GameData.playerTerrains[1].SetActive(true);
            gameData.SetupDefenses();
        }
        gameData.dynamicLight.intensity = gameData.planetDynamicLightingIntensity;
        gameData.dynamicLight.transform.rotation = Quaternion.Euler(gameData.planetDynamicLightingRotation);
        gameData.backgroundMeshRenderer.material = gameData.planetBackgroundMaterial;
        gameData.starsParticleSystem.Stop();
        gameData.starsParticleSystem.Clear();
        foreach (PlayerManager pm in GameData.playerManagers)
        {
            pm.PlayerStateChange(_GameState.SinglePlanetAttack);
        }
    }

    private void GameStateSinglePlanetDefend()
    {
        if (GameData.playerTerrains.Count == 2)
        {
            GameData.playerTerrains[0].SetActive(true);
            GameData.playerTerrains[1].SetActive(false);
            gameData.SetupDefenses();
        }
        gameData.dynamicLight.intensity = gameData.planetDynamicLightingIntensity;
        gameData.dynamicLight.transform.rotation = Quaternion.Euler(gameData.planetDynamicLightingRotation);
        gameData.backgroundMeshRenderer.material = gameData.planetBackgroundMaterial;
        gameData.starsParticleSystem.Stop();
        gameData.starsParticleSystem.Clear();
        foreach (PlayerManager pm in GameData.playerManagers)
        {
            pm.PlayerStateChange(_GameState.SinglePlanetDefend);
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

    private void InstantiateOpponent()
    {
        opponent = (GameObject)Instantiate(opponentPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        opponent.transform.parent = null;
        //associate this gameobject to the opponent playermanager
        foreach (PlayerManager pm in GameData.playerManagers)
        {
            if (pm.playerNumber == 1)
            {
                pm.SetupSP(opponent);
                break;
            }
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
    
}
