using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class SceneControllerMainMenu : MonoBehaviour {
    	
    public void Multiplayer()
    {
        CrossPlatformInputManager.SetButtonUp("Multiplayer");
        SceneManager.LoadScene(_Scenes.sceneMultiplayerLobby);
    }

    public void SinglePlayer()
    {
        CrossPlatformInputManager.SetButtonUp("SinglePlayer");
        SceneManager.LoadScene(_Scenes.sceneSinglePlayerLobby);
    }

}
