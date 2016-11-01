using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSetupSP : MonoBehaviour {

    public int colorIndex;
    public string playerName;
    public int playerNumber;
    public bool isReady = false;
    public int randomTerrainSeed;


    public TextAsset colorList;
    private Dictionary<string, Color> playerColors;

    //Reference gameobjects for space and planet settings
    public GameObject playerSpaceTree;
    public GameObject playerPlanetTree;
    private MeshRenderer meshRendererSpace;
    private MeshRenderer meshRendererPlanet;
    private ParticleSystem particleSystemSpace;
    private ParticleSystem particleSystemPlanet;
    private CircleCollider2D spaceCollider;
    private BoxCollider2D planetCollider;

    void Start()
    {
        

        //get player colors
        playerColors = (new ColorDictionary(colorList)).GetColorDictionary(((_Colors)colorIndex).ToString());

        //get components
        meshRendererPlanet = playerPlanetTree.GetComponentInChildren<MeshRenderer>();
        meshRendererSpace = playerSpaceTree.GetComponentInChildren<MeshRenderer>();
        particleSystemPlanet = playerPlanetTree.GetComponentInChildren<ParticleSystem>();
        particleSystemSpace = playerSpaceTree.GetComponentInChildren<ParticleSystem>();
        planetCollider = GetComponent<BoxCollider2D>();
        spaceCollider = GetComponent<CircleCollider2D>();


        //set colors
        meshRendererPlanet.material.color = playerColors[_ColorType.PlayerShipPlanet.ToString()];
        meshRendererSpace.material.color = playerColors[_ColorType.PlayerShipSpace.ToString()];

    }

    public void EnableSpaceGraphics()
    {
        meshRendererPlanet.enabled = false;
        meshRendererSpace.enabled = true;
        particleSystemPlanet.Clear();
        particleSystemPlanet.Stop();
        particleSystemSpace.Play();
        planetCollider.enabled = false;
        spaceCollider.enabled = true;
    }

    public void EnablePlanetGraphics()
    {
        meshRendererPlanet.enabled = true;
        meshRendererSpace.enabled = false;
        particleSystemPlanet.Play();
        particleSystemSpace.Clear();
        particleSystemSpace.Stop();
        planetCollider.enabled = true;
        spaceCollider.enabled = false;
    }

    public void DisableAllGraphics()
    {
        meshRendererPlanet.enabled = false;
        meshRendererSpace.enabled = false;
        particleSystemPlanet.Clear();
        particleSystemPlanet.Stop();
        particleSystemSpace.Clear();
        particleSystemSpace.Stop();
        planetCollider.enabled = false;
        spaceCollider.enabled = false;
    }
}
