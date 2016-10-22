using System;
using UnityEngine;

[Serializable]
public class PlayerManager {

    public int playerColorIndex;
    public Transform spawnPoint;
    public int playerNumber;
    public GameObject instance;
    public string playerName;
    public int randomTerrainSeed;

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
        playerSetup.randomTerrainSeed = randomTerrainSeed;

    }

    //Generate Random Terrain Seed currently inside PlayerLobbyHook.cs
    /*public int GenerateRandomTerrainSeed()
    {
        return (new System.Random()).Next(0, 1024);
    }*/

    public void PlayerStateChange(_GameState gameState)
    {
        switch (gameState)
        {
            case _GameState.Neutral:
                playerSetup.EnableSpaceGraphics();
                instance.transform.position = GameManager.instance.spaceSpawnPoints[playerSetup.playerNumber].position;
                instance.transform.rotation = Quaternion.Euler(Vector3.zero);
                playerControl.EnableSpaceControl();
                playerCamera.EnableSpaceCamera();
                break;

            case _GameState.PlayerOnePlanet:
                if (playerNumber == 1)
                {
                    playerSetup.EnablePlanetGraphics();
                    instance.transform.position = GameManager.instance.planetSpawnPoints[0].position;
                    instance.transform.rotation = Quaternion.Euler(Vector3.zero);
                    playerControl.EnablePlanetControl();
                    playerCamera.EnablePlanetCameraAttacker();
                }
                else
                {
                    playerSetup.DisableAllGraphics();
                    playerControl.DisableAllControl(); 
                    playerCamera.EnablePlanetCameraDefender();
                    
                }
                break;

            case _GameState.PlayerTwoPlanet:
                if (playerNumber == 0)
                {
                    playerSetup.EnablePlanetGraphics();
                    instance.transform.position = GameManager.instance.planetSpawnPoints[0].position;
                    instance.transform.rotation = Quaternion.Euler(Vector3.zero);
                    playerControl.EnablePlanetControl();
                    playerCamera.EnablePlanetCameraAttacker();
                }
                else
                {
                    playerSetup.DisableAllGraphics();
                    playerControl.DisableAllControl(); 
                    playerCamera.EnablePlanetCameraDefender();
                }
                break;
        }
    }



}
