using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class SceneControllerMainMenu : MonoBehaviour {
    	
    public void Lobby()
    {
        CrossPlatformInputManager.SetButtonUp("Lobby");
        SetupManager.activeTerrain = _Levels.Neutral;
        SceneManager.LoadScene(_Scenes.sceneLobby);
    }
}
