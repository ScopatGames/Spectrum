using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class LevelController : MonoBehaviour {
    public _Levels currentLevel;
    //public GameObject mainCamera;

    private GameController gameController;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag(_Tags.gameController).GetComponent<GameController>();

        //Initialize the current level
        switch (currentLevel)
        {
            case _Levels.MainMenu:
                break;
            case _Levels.Lobby:
                //Regenerate terrain
                //terrainData.RegenerateTerrain();
                break;
            case _Levels.Neutral:
                /*
                playerData.SpawnPlayerSpace(_Levels.PlayerOne, new Vector3(-3.0f, 0.0f, 0.0f), Quaternion.identity);
                playerData.SpawnPlayerSpace(_Levels.PlayerTwo, new Vector3(3.0f, 0.0f, 0.0f), Quaternion.identity);
                */
                gameController.ActivateTerrain(gameController.activeTerrain);
                //mainCamera.GetComponent<SmoothCameraSpace>().player = playerData.players[0].transform;
                break;
            case _Levels.PlayerOne:
                _Levels playerType = (gameController.activeTerrain == _Levels.PlayerOne) ? _Levels.PlayerTwo : _Levels.PlayerOne;
                int playerIndex = (playerType == _Levels.PlayerOne) ? 0 : 1;
                //playerData.SpawnPlayerPlanet(playerType, new Vector3(0.0f, 49.9f, 0.0f), Quaternion.identity);
                gameController.ActivateTerrain(gameController.activeTerrain);
                //mainCamera.GetComponent<SmoothCameraPlanet>().player = playerData.players[playerIndex].transform;
                break;
        }
    }

    public void PlayerOne()
    {
        CrossPlatformInputManager.SetButtonUp("LevelOne");
        gameController.activeTerrain = _Levels.PlayerOne;
        SceneManager.LoadScene(_Scenes.sceneBattlePlanet);
    }

    public void PlayerTwo()
    {
        CrossPlatformInputManager.SetButtonUp("LevelTwo");
        gameController.activeTerrain = _Levels.PlayerTwo;
        SceneManager.LoadScene(_Scenes.sceneBattlePlanet);
    }

    public void Neutral()
    {
        CrossPlatformInputManager.SetButtonUp("Neutral");
        gameController.activeTerrain = _Levels.Neutral;
        SceneManager.LoadScene(_Scenes.sceneBattleSpace);
    }

    public void MainMenu()
    {
        CrossPlatformInputManager.SetButtonUp("MainMenu");
        gameController.activeTerrain = _Levels.Neutral;
        SceneManager.LoadScene(_Scenes.sceneMainMenu);
    }

    public void Lobby()
    {
        CrossPlatformInputManager.SetButtonUp("Lobby");
        gameController.activeTerrain = _Levels.Neutral;
        SceneManager.LoadScene(_Scenes.sceneLobby);
    }
}
