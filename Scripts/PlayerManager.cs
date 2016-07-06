using System;
using UnityEngine;

[Serializable]
public class PlayerManager {

    public int playerColorIndex;
    public Transform spawnPoint;
    public int playerNumber;
    public GameObject instance;
    public string playerName;

    public PlayerSetup playerSetup;
    public PlayerControl playerControl;
    public PlayerCamera playerCamera;

    public void Setup()
    {
        //Get references to the components
        playerSetup = instance.GetComponent<PlayerSetup>();
        playerControl = instance.GetComponent<PlayerControl>();
        playerCamera = instance.GetComponent<PlayerCamera>();


        playerSetup.colorIndex = playerColorIndex;
        playerSetup.playerName = playerName;
        playerSetup.playerNumber = playerNumber;

    }

    public void PlayerStateChange(_GameState gameState)
    {
        switch (gameState)
        {
            case _GameState.Lobby:
            case _GameState.MainMenu:
                playerSetup.DisableAllGraphics();
                break;

            case _GameState.Neutral:
                playerSetup.EnableSpaceGraphics();
                break;

            case _GameState.PlayerOne:
                if (playerNumber == 0)
                {
                    playerSetup.EnablePlanetGraphics();
                }
                else
                {
                    playerSetup.DisableAllGraphics();
                }
                break;

            case _GameState.PlayerTwo:
                if (playerNumber == 1)
                {
                    playerSetup.EnablePlanetGraphics();
                }
                else
                {
                    playerSetup.DisableAllGraphics();
                }
                break;
        }
    }



}
