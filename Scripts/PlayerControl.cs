using UnityEngine;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour {

    private PlayerControllerSpace playerControllerSpace;
    private PlayerControllerPlanet playerControllerPlanet;

    void Awake()
    {
        playerControllerSpace = GetComponent<PlayerControllerSpace>();
        playerControllerPlanet = GetComponent<PlayerControllerPlanet>();
    }

    void Start()
    {
        EnableSpaceControl();
    }

    public void EnableSpaceControl()
    {
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
