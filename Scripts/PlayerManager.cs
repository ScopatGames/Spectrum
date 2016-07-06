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

    public void Setup()
    {
        //Get references to the components
        playerSetup = instance.GetComponent<PlayerSetup>();
    }

    
  
}
