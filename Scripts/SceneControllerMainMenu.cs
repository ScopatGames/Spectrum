using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class SceneControllerMainMenu : MonoBehaviour {
    	
    public void Lobby()
    {
        CrossPlatformInputManager.SetButtonUp("Lobby");
        SceneManager.LoadScene(_Scenes.sceneLobby);
    }

    public void Battle()
    {
        CrossPlatformInputManager.SetButtonUp("Lobby");
        SceneManager.LoadScene(_Scenes.sceneBattle);
    }

}
