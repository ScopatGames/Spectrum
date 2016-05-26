using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class LevelController : MonoBehaviour {
    public GameObject mainCamera;

    private GameObject gameController;
    private TerrainData terrainData;
    private PlayerData playerData;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag(_Tags.gameController);
        terrainData = gameController.GetComponent<TerrainData>();
        playerData = gameController.GetComponent<PlayerData>();

        //Activate the terrain that is stored in the state variable on TerrainData
        if (terrainData.terrainGenerated)
        {
            terrainData.ActivateTerrain(terrainData.activeTerrain);
            if (terrainData.activeTerrain != _Levels.Neutral)
            {
                _Levels playerIndex = (terrainData.activeTerrain==_Levels.PlayerOne) ? _Levels.PlayerTwo : _Levels.PlayerOne;
                playerData.SpawnPlayerPlanet(playerIndex, new Vector3(0.0f, 49.9f, 0.0f), Quaternion.identity);
                mainCamera.GetComponent<SmoothCameraAtmosphere>().player = playerData.players[(int)playerIndex-1].transform;
            }
            else
            {
                playerData.SpawnPlayerSpace(_Levels.PlayerOne, new Vector3(-3.0f, 0.0f, 0.0f), Quaternion.identity);
                playerData.SpawnPlayerSpace(_Levels.PlayerTwo, new Vector3(3.0f, 0.0f, 0.0f), Quaternion.identity);
                mainCamera.GetComponent<SmoothCameraSpace>().player = playerData.players[0].transform;
            }
        }
    }

    public void PlayerOne()
    {
        CrossPlatformInputManager.SetButtonUp("LevelOne");
        terrainData.activeTerrain = _Levels.PlayerOne;
        SceneManager.LoadScene(_Scenes.sceneAtmosphereTest);
    }

    public void PlayerTwo()
    {
        CrossPlatformInputManager.SetButtonUp("LevelTwo");
        terrainData.activeTerrain = _Levels.PlayerTwo;
        SceneManager.LoadScene(_Scenes.sceneAtmosphereTest);
    }

    public void Neutral()
    {
        CrossPlatformInputManager.SetButtonUp("Neutral");
        terrainData.activeTerrain = _Levels.Neutral;
        SceneManager.LoadScene(_Scenes.sceneSpaceTest);
    }

    public void RegenerateTerrain()
    {
        terrainData.RegenerateTerrain();
    }
	
}
