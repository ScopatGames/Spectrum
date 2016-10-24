using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class SceneControllerSinglePlayerLobby : MonoBehaviour {

	public void StartGame()
    {
        CrossPlatformInputManager.SetButtonUp("StartGame");
        SceneManager.LoadScene(_Scenes.sceneSinglePlayer);
    }
}
