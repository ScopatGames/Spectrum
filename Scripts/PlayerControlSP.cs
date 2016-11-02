using UnityEngine;
using System.Collections;

public class PlayerControlSP : MonoBehaviour {

    public int spaceBoundaryRadius = 45;
    public int planetBoundaryRadius = 80;

    private PlayerControllerSpaceSP playerControllerSpace;
    private PlayerControllerPlanetSP playerControllerPlanet;
    private PlayerBarrier playerBarrier;

    void Awake()
    {
        playerControllerSpace = GetComponent<PlayerControllerSpaceSP>();
        playerControllerPlanet = GetComponent<PlayerControllerPlanetSP>();
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
