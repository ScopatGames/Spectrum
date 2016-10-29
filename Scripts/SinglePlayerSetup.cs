using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SinglePlayerSetup : MonoBehaviour {
    public TextAsset colorListTextAsset;
    public MeshRenderer avatarMesh;
    public GameObject player;
    public GameObject opponent;
    private ColorDictionary colorDictionary;
    private Dictionary<string, Color> playerColors;
    private int playerColorIndex;
    private int opponentColorIndex;

    void Awake () {
        colorDictionary = new ColorDictionary(colorListTextAsset);
        playerColorIndex = -1;
        ChangeAvatarColor();
	}
	
	public void ChangeAvatarColor()
    {
        playerColorIndex = (playerColorIndex < (colorDictionary.GetColorCount() - 1)) ? ++playerColorIndex : 0;
        playerColors = colorDictionary.GetColorDictionary(((_Colors)playerColorIndex).ToString());
        avatarMesh.material.color = playerColors[_ColorType.PlayerShipSpace.ToString()];
    }

    public void AddPlayerAndOpponent()
    {
        System.Random randGen = new System.Random();
        opponentColorIndex = randGen.Next(0, colorDictionary.GetColorCount());

        GameData.AddPlayer(player, 0, playerColorIndex, "player", GenerateRandomTerrainSeed());
        GameData.AddOpponent(opponent, 1, opponentColorIndex, "opponent", GenerateRandomTerrainSeed());
    }

    private int GenerateRandomTerrainSeed()
    {
        return (new System.Random()).Next(0, 1024);
    }
}
