using UnityEngine;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour {

    public int spaceBoundaryRadius = 45;
    public int planetBoundaryRadius = 80;

    private PlayerControllerSpace playerControllerSpace;
    private PlayerControllerPlanet playerControllerPlanet;
    private PlayerBarrier playerBarrier;

    void Awake()
    {
        playerControllerSpace = GetComponent<PlayerControllerSpace>();
        playerControllerPlanet = GetComponent<PlayerControllerPlanet>();
        playerBarrier = GetComponent<PlayerBarrier>();
    }

    void Start()
    {
        EnableSpaceControl();
    }

    public void EnableSpaceControl()
    {
        playerBarrier.boundaryRadius = spaceBoundaryRadius;

        if (isLocalPlayer)
        {
            playerControllerPlanet.enabled = false;
            playerControllerSpace.enabled = true;
        }
        else
        {
            DisableAllControl();
        }
    }

    public void EnablePlanetControl()
    {
        playerBarrier.boundaryRadius = planetBoundaryRadius;
        if (isLocalPlayer)
        {
            playerControllerSpace.enabled = false;
            playerControllerPlanet.enabled = true;
        }
        else
        {
            DisableAllControl();
        }
        
    }

    public void DisableAllControl()
    {
        playerControllerSpace.enabled = false;
        playerControllerPlanet.enabled = false;
    }
}
