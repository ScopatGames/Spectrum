using UnityEngine;
using System.Collections.Generic;

public class PlayerData : MonoBehaviour
{

    public TextAsset colorList;

    public GameObject playerPrefabSpace;
    public GameObject playerPrefabPlanet;

    public bool randomColors = true;

    public _Colors playerOneColor = _Colors.Black;
    public _Colors playerTwoColor = _Colors.Black;

    [HideInInspector]
    public Dictionary<string, Color>[] playerColorDictionaries = new Dictionary<string, Color>[2];
    [HideInInspector]
    public GameObject[] players = new GameObject[2];



    void Awake()
    {
        PlayerColors();
    }

    public void PlayerColors()
    {
        ColorDictionary colorDictionary = new ColorDictionary(colorList);
        //Pick colors
        if (randomColors)
        {
            System.Random rnd = new System.Random();
            playerOneColor = (_Colors)rnd.Next(0, 8);
            playerTwoColor = playerOneColor;
            while (playerTwoColor == playerOneColor)
            {
                playerTwoColor = (_Colors)rnd.Next(0, 8);
            }
        }

        playerColorDictionaries[0] = colorDictionary.GetColorDictionary(playerOneColor.ToString());
        playerColorDictionaries[1] = colorDictionary.GetColorDictionary(playerTwoColor.ToString());
    }

    public void SpawnPlayerSpace(_Levels playerNumber, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        int playerIndex = (playerNumber == _Levels.PlayerOne)? 0 : 1;
        players[playerIndex] = Instantiate(playerPrefabSpace, spawnPosition, spawnRotation) as GameObject;
        players[playerIndex].name = playerNumber.ToString();
        players[playerIndex].GetComponentInChildren<MeshRenderer>().material.color = playerColorDictionaries[playerIndex][_ColorType.PlayerShipSpace.ToString()];
        if (playerIndex == 0)
        {
            players[playerIndex].tag = _Tags.playerOne;
        }
        else
        {
            players[playerIndex].tag = _Tags.playerTwo;
        }

    }

    public void SpawnPlayerPlanet(_Levels playerNumber, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        int playerIndex = (playerNumber == _Levels.PlayerOne) ? 0 : 1;
        players[playerIndex] = Instantiate(playerPrefabPlanet, spawnPosition, spawnRotation) as GameObject;
        players[playerIndex].name = playerNumber.ToString();
        players[playerIndex].GetComponentInChildren<MeshRenderer>().material.color = playerColorDictionaries[playerIndex][_ColorType.PlayerShipPlanet.ToString()];
        players[playerIndex].tag = _Tags.player;
    }
}