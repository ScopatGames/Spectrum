using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;


public class SinglePlayerLobbyHook : MonoBehaviour {
    public TextAsset colorListTextAsset;
    public MeshRenderer avatarMesh;
    private ColorDictionary colorDictionary;
    private Dictionary<string, Color> playerColors;
    private int playerColorIndex;
    private int opponentColorIndex;
    private int prevRando = -1;

    void Awake () {
        colorDictionary = new ColorDictionary(colorListTextAsset);
        playerColorIndex = -1;
        ChangeAvatarColor();
	}
	
    //PUBLIC METHODS
	public void ChangeAvatarColor()
    {
        playerColorIndex = (playerColorIndex < (colorDictionary.GetColorCount() - 1)) ? ++playerColorIndex : 0;
        playerColors = colorDictionary.GetColorDictionary(((_Colors)playerColorIndex).ToString());
        avatarMesh.material.color = playerColors[_ColorType.PlayerShipSpace.ToString()];
    }

    public void StartGame()
    {
        if(GameData.playerManagers.Count > 0)
        {
            GameData.playerManagers.Clear();
        }
        AddPlayer();
        AddOpponent();
        CrossPlatformInputManager.SetButtonUp("StartGame");
        SceneManager.LoadScene(_Scenes.sceneSinglePlayer);
    }

    //PRIVATE METHODS
    private void AddPlayer()
    {
        GameData.AddSinglePlayer(0, playerColorIndex, "player", GenerateRandomTerrainSeed());
    }

    private void AddOpponent()
    {
        System.Random randGen = new System.Random();
        opponentColorIndex = playerColorIndex;
        while(opponentColorIndex == playerColorIndex)
        {
            opponentColorIndex = randGen.Next(0, colorDictionary.GetColorCount());
        }
        GameData.AddSinglePlayer(1, opponentColorIndex, "opponent", GenerateRandomTerrainSeed());
    }

    private int GenerateRandomTerrainSeed()
    {

        int rando = prevRando;
        while (rando == prevRando) {
            rando = (new System.Random()).Next(0, 1024);
        }
        return rando;
    }
}
