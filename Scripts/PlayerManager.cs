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
    public PlayerControlSP playerControlSP;
    public PlayerCamera playerCamera;
    public PlayerCameraSP playerCameraSP;

    public OpponentController opponentController;

    public void DestroyPlayerSP(bool debris)
    {
        playerSetupSP.DisableAllGraphics();
        playerControlSP.DisableAllControl();
        if (debris)
        {
            // TODO create debris particle system 
        }
    }

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

        //Get references to the components for player
        switch(playerNumber)
        {
            case 0: //player
                playerSetupSP = instance.GetComponent<PlayerSetupSP>();
                playerControlSP = instance.GetComponent<PlayerControlSP>();
                playerCameraSP = instance.GetComponent<PlayerCameraSP>();


                playerSetupSP.colorIndex = playerColorIndex;
                playerSetupSP.playerName = playerName;
                playerSetupSP.playerNumber = playerNumber;
                playerSetupSP.randomTerrainSeed = randomTerrainSeed;
                break;
            case 1: //computer opponent
                opponentController = instance.GetComponent<OpponentController>();
                break;
        }
    }

    public void PlayerStateChange(_GameState gameState)
    {
        switch (gameState)
        {
            case _GameState.SingleNeutral:
                switch (playerNumber)
                {
                    case 0: //player
                        playerSetupSP.EnableSpaceGraphics();
                        instance.transform.position = GameData.instance.spaceSpawnPoints[playerSetupSP.playerNumber].position;
                        instance.transform.rotation = Quaternion.Euler(Vector3.zero);
                        playerControlSP.EnableSpaceControl();
                        playerCameraSP.EnableSpaceCamera();
                        break;
                    case 1: //computer opponent
                        opponentController.EnableSpaceContainer();
                        break;
                }
                break;

            case _GameState.SinglePlanetAttack:
                switch (playerNumber)
                {
                    case 0: //player
                        playerSetupSP.EnablePlanetGraphics();
                        instance.transform.position = GameData.instance.planetSpawnPoints[playerSetupSP.playerNumber].position;
                        instance.transform.rotation = Quaternion.Euler(Vector3.zero);
                        playerControlSP.EnablePlanetControl();
                        playerCameraSP.EnablePlanetCameraAttacker();
                        break;
                    case 1: //computer opponent
                        opponentController.EnableDefensiveContainer();
                        break;
                }
                
                break;

            case _GameState.SinglePlanetDefend:
                switch (playerNumber)
                {
                    case 0: //player
                        playerSetupSP.DisableAllGraphics();
                        playerControlSP.EnablePlanetDefenseControl();
                        playerCameraSP.EnablePlanetCameraDefender();
                        break;
                    case 1: //computer opponent
                        opponentController.EnableOffensiveContainer();
                        break;
                }
                
                break;

            case _GameState.MultiNeutral:
                playerSetup.EnableSpaceGraphics();
                instance.transform.position = GameData.instance.spaceSpawnPoints[playerSetup.playerNumber].position;
                instance.transform.rotation = Quaternion.Euler(Vector3.zero);
                playerControl.EnableSpaceControl();
                playerCamera.EnableSpaceCamera();
                break;

            case _GameState.MultiPlayerOnePlanet:
                switch (playerNumber)
                {
                    case 0:
                        playerSetup.DisableAllGraphics();
                        playerControl.EnablePlanetDefenseControl();
                        playerCamera.EnablePlanetCameraDefender();
                        break;
                    case 1:
                        playerSetup.EnablePlanetGraphics();
                        instance.transform.position = GameData.instance.planetSpawnPoints[0].position;
                        instance.transform.rotation = Quaternion.Euler(Vector3.zero);
                        playerControl.EnablePlanetControl();
                        playerCamera.EnablePlanetCameraAttacker();
                        break;
                }
                break;

            case _GameState.MultiPlayerTwoPlanet:
                switch (playerNumber)
                {
                    case 0:
                        playerSetup.EnablePlanetGraphics();
                        instance.transform.position = GameData.instance.planetSpawnPoints[0].position;
                        instance.transform.rotation = Quaternion.Euler(Vector3.zero);
                        playerControl.EnablePlanetControl();
                        playerCamera.EnablePlanetCameraAttacker();
                        break;
                    case 1:
                        playerSetup.DisableAllGraphics();
                        playerControl.EnablePlanetDefenseControl();
                        playerCamera.EnablePlanetCameraDefender();
                        break;
                }
                break;
        }
    }



}
