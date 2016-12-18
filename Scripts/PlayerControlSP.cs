using UnityEngine;
using System.Collections;

public class PlayerControlSP : MonoBehaviour {

    public int spaceBoundaryRadius = 45;
    public int planetBoundaryRadius = 80;

    private PlayerControllerSpaceSP playerControllerSpace;
    private PlayerControllerPlanetSP playerControllerPlanet;
    private PlayerControllerPlanetDefenseSP playerControllerPlanetDefense;
    private PlayerBarrier playerBarrier;

    void Awake()
    {
        playerControllerSpace = GetComponent<PlayerControllerSpaceSP>();
        playerControllerPlanet = GetComponent<PlayerControllerPlanetSP>();
        playerControllerPlanetDefense = GetComponent<PlayerControllerPlanetDefenseSP>();
        playerBarrier = GetComponent<PlayerBarrier>();
    }

    public void EnableSpaceControl()
    {
        playerBarrier.enabled = true;
        playerBarrier.boundaryRadius = spaceBoundaryRadius;
        playerControllerPlanet.enabled = false;
        playerControllerPlanetDefense.enabled = false;
        playerControllerSpace.enabled = true;
    }

    public void EnablePlanetControl()
    {
        playerBarrier.enabled = true;
        playerBarrier.boundaryRadius = planetBoundaryRadius;
        playerControllerSpace.enabled = false;
        playerControllerPlanetDefense.enabled = false;
        playerControllerPlanet.enabled = true;
    }

    public void EnablePlanetDefenseControl()
    {
        playerBarrier.enabled = false;
        playerControllerSpace.enabled = false;
        playerControllerPlanet.enabled = false;
        playerControllerPlanetDefense.enabled = true;
    }

    public void DisableAllControl()
    {
        playerControllerSpace.enabled = false;
        playerControllerPlanet.enabled = false;
        playerControllerPlanetDefense.enabled = false;
    }
}
