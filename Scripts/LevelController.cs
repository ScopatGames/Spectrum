using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class LevelController : MonoBehaviour {

    private TerrainData terrainData;

    void Start()
    {
        terrainData = GameObject.FindGameObjectWithTag(_Tags.gameController).GetComponent<TerrainData>();

        //Activate the terrain that is stored in the state variable on TerrainData
        if (terrainData.terrainGenerated)
        {
            terrainData.ActivateTerrain(terrainData.activeTerrain);
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
	
}
