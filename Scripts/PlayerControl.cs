using UnityEngine;
using UnityEngine.Networking;

public class PlayerControl : MonoBehaviour {

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

    public void EnableSpaceControl()
    {
        playerBarrier.boundaryRadius = spaceBoundaryRadius;
        playerControllerPlanet.enabled = false;
        playerControllerSpace.enabled = true;
    }

    public void EnablePlanetControl()
    {
        playerBarrier.boundaryRadius = planetBoundaryRadius;
        playerControllerSpace.enabled = false;
        playerControllerPlanet.enabled = true;
    }

    public void DisableAllControl()
    {
        playerControllerSpace.enabled = false;
        playerControllerPlanet.enabled = false;
    }
}
