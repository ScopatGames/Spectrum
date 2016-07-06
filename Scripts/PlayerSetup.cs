using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

    [Header("Network")]
    [Space]
    [SyncVar]
    public int colorIndex;

    [SyncVar]
    public string playerName;

    [SyncVar]
    public int playerNumber;

    [SyncVar]
    public bool isReady = false;

    //Reference gameobjects for space and planet settings
    public GameObject playerSpaceTree;
    public GameObject playerPlanetTree;
    private MeshRenderer meshRendererSpace;
    private MeshRenderer meshRendererPlanet;
    private ParticleSystem particleSystemSpace;
    private ParticleSystem particleSystemPlanet;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!isServer)
        {
            GameManager.AddPlayer(gameObject, playerNumber, colorIndex, playerName);
        }

        meshRendererPlanet = playerPlanetTree.GetComponentInChildren<MeshRenderer>();
        meshRendererSpace = playerSpaceTree.GetComponentInChildren<MeshRenderer>();
        particleSystemPlanet = playerPlanetTree.GetComponentInChildren<ParticleSystem>();
        particleSystemSpace = playerSpaceTree.GetComponentInChildren<ParticleSystem>(); 

    }

}
