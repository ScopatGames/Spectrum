using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class LevelController : MonoBehaviour {
    public _Levels currentLevel;
    public GameObject mainCamera;

    private GameObject gameController;
    private TerrainData terrainData;
    private PlayerData playerData;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag(_Tags.gameController);
        terrainData = gameController.GetComponent<TerrainData>();
        playerData = gameController.GetComponent<PlayerData>();

        //Initialize the current level
        switch (currentLevel)
        {
            case _Levels.MainMenu:
                //Regenerate terrain
                terrainData.RegenerateTerrain();
                break;
            case _Levels.Neutral:
                playerData.SpawnPlayerSpace(_Levels.PlayerOne, new Vector3(-3.0f, 0.0f, 0.0f), Quaternion.identity);
                playerData.SpawnPlayerSpace(_Levels.PlayerTwo, new Vector3(3.0f, 0.0f, 0.0f), Quaternion.identity);
                terrainData.ActivateTerrain(terrainData.activeTerrain);
                mainCamera.GetComponent<SmoothCameraSpace>().player = playerData.players[0].transform;
                break;
            case _Levels.PlayerOne:
                _Levels playerType = (terrainData.activeTerrain == _Levels.PlayerOne) ? _Levels.PlayerTwo : _Levels.PlayerOne;
                int playerIndex = (playerType == _Levels.PlayerOne) ? 0 : 1;
                playerData.SpawnPlayerPlanet(playerType, new Vector3(0.0f, 49.9f, 0.0f), Quaternion.identity);
                terrainData.ActivateTerrain(terrainData.activeTerrain);
                mainCamera.GetComponent<SmoothCameraPlanet>().player = playerData.players[playerIndex].transform;
                break;
        }
    }

    public void PlayerOne()
    {
        CrossPlatformInputManager.SetButtonUp("LevelOne");
        terrainData.activeTerrain = _Levels.PlayerOne;
        SceneManager.LoadScene(_Scenes.sceneBattlePlanet);
    }

    public void PlayerTwo()
    {
        CrossPlatformInputManager.SetButtonUp("LevelTwo");
        terrainData.activeTerrain = _Levels.PlayerTwo;
        SceneManager.LoadScene(_Scenes.sceneBattlePlanet);
    }

    public void Neutral()
    {
        CrossPlatformInputManager.SetButtonUp("Neutral");
        terrainData.activeTerrain = _Levels.Neutral;
        SceneManager.LoadScene(_Scenes.sceneBattleSpace);
    }

    public void MainMenu()
    {
        CrossPlatformInputManager.SetButtonUp("MainMenu");
        terrainData.activeTerrain = _Levels.Neutral;
        SceneManager.LoadScene(_Scenes.sceneMainMenu);
    }
}
