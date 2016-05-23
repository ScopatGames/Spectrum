using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class LevelController : MonoBehaviour {

    private TerrainData terrainData;

    void Start()
    {
        terrainData = GameObject.FindGameObjectWithTag(_Tags.gameController).GetComponent<TerrainData>();
        if (terrainData.terrainGenerated)
        {
            terrainData.ActivateTerrain(terrainData.activeTerrain);
        }
    }

    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("LevelOne"))
        {
            CrossPlatformInputManager.SetButtonUp("LevelOne");
            terrainData.activeTerrain = _Levels.PlayerOne;
            SceneManager.LoadScene(_Scenes.sceneAtmosphereTest);
        }

        if (CrossPlatformInputManager.GetButtonDown("LevelTwo"))
        {
            CrossPlatformInputManager.SetButtonUp("LevelTwo");
            terrainData.activeTerrain = _Levels.PlayerTwo;
            SceneManager.LoadScene(_Scenes.sceneAtmosphereTest);
        }

        if (CrossPlatformInputManager.GetButtonDown("Neutral"))
        {
            CrossPlatformInputManager.SetButtonUp("Neutral");
            terrainData.activeTerrain = _Levels.Neutral;
            SceneManager.LoadScene(_Scenes.sceneSpaceTest);
        }
    }
	
}
