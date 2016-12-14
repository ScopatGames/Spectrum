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
        GameData.playerManagers.Clear();
        AddPlayer();
        AddOpponent();
        CrossPlatformInputManager.SetButtonUp("StartGame");
        SceneManager.LoadScene(_Scenes.sceneSinglePlayer);
    }

    public void HexTerrain(bool hexTerrain)
    {
        GameData.terrainType = (hexTerrain) ? _TerrainType.Hexagonal : _TerrainType.QuadsTris;
    }

    //PRIVATE METHODS
    private void AddPlayer()
    {
        GameData.AddSinglePlayer(0, playerColorIndex, "player", Random.Range(0, 1024));
    }

    private void AddOpponent()
    {
        opponentColorIndex = playerColorIndex;
        while(opponentColorIndex == playerColorIndex)
        {
            opponentColorIndex = Random.Range(0, colorDictionary.GetColorCount());
        }
        GameData.AddSinglePlayer(1, opponentColorIndex, "opponent",Random.Range(0, 1024));
    }
}
