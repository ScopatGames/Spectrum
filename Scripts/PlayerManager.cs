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
    public PlayerSetupSP playerSetupSP;
    public PlayerControl playerControl;
    public PlayerCamera playerCamera;
    public PlayerCameraSP playerCameraSP;

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

    public void SetupSP(GameObject playerGameObject)
    {
        instance = playerGameObject;

        //Get references to the components
        playerSetupSP = instance.GetComponent<PlayerSetupSP>();
        playerControl = instance.GetComponent<PlayerControl>();
        playerCameraSP = instance.GetComponent<PlayerCameraSP>();


        playerSetupSP.colorIndex = playerColorIndex;
        playerSetupSP.playerName = playerName;
        playerSetupSP.playerNumber = playerNumber;
        playerSetupSP.randomTerrainSeed = randomTerrainSeed;

    }

    public void PlayerStateChange(_GameState gameState)
    {
        switch (gameState)
        {
            case _GameState.SingleNeutral:
                playerSetupSP.EnableSpaceGraphics();
                instance.transform.position = GameData.instance.spaceSpawnPoints[playerSetupSP.playerNumber].position;
                instance.transform.rotation = Quaternion.Euler(Vector3.zero);
                playerControl.EnableSpaceControl();
                playerCameraSP.EnableSpaceCamera();
                break;

            case _GameState.MultiNeutral:
                playerSetup.EnableSpaceGraphics();
                instance.transform.position = GameData.instance.spaceSpawnPoints[playerSetup.playerNumber].position;
                instance.transform.rotation = Quaternion.Euler(Vector3.zero);
                playerControl.EnableSpaceControl();
                playerCamera.EnableSpaceCamera();
                break;

            case _GameState.MultiPlayerOnePlanet:
                if (playerNumber == 1)
                {
                    playerSetup.EnablePlanetGraphics();
                    instance.transform.position = GameData.instance.planetSpawnPoints[0].position;
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

            case _GameState.MultiPlayerTwoPlanet:
                if (playerNumber == 0)
                {
                    playerSetup.EnablePlanetGraphics();
                    instance.transform.position = GameData.instance.planetSpawnPoints[0].position;
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
