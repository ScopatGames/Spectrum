﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;

public class GameManager :NetworkBehaviour {

    static public GameManager instance;
    static public List<PlayerManager> playerManagers = new List<PlayerManager>();
    static public System.Random randomSeedGenerator = new System.Random();

    [Header("------ Terrain Generation Data ------")]
    public TextAsset textInputVertices;
    public TextAsset textInputFaces;
    public float terrainScale = 40.0f;
    public List<GameObject> terrainTilePrefab = new List<GameObject>();

    [HideInInspector]
    static public List<GameObject> playerTerrains = new List<GameObject>();

    private List<Vector4> parsedTerrainFaces;
    private List<Vector3> parsedTerrainVertices;
    private List<Vector3>[] playerTerrainVertices;

    [Header("------ Player Data ------")]
    public TextAsset colorListTextAsset;
    public bool randomColors = true;
    public _Colors playerOneColor = _Colors.Black;
    public _Colors playerTwoColor = _Colors.Black;

    [HideInInspector]
    public Dictionary<string, Color>[] playerColorDictionaries = new Dictionary<string, Color>[2];

    private ColorDictionary colorDictionary;
    [Header("------ Dynamic Lighting ------")]
    public Light dynamicLight;

    [Header("------ Background Graphics ------")]
    public MeshRenderer backgroundMeshRenderer;
    public Material spaceBackgroundMaterial;
    public Material planetBackgroundMaterial;
    public ParticleSystem starsParticleSystem;

    [Header("------ Space Battle Data ------")]
    public List<Transform> spaceSpawnPoints = new List<Transform>();
    public Vector3 spaceDynamicLightingRotation;
    public float spaceDynamicLightingIntensity;

    [Header("------ Planet Battle Data ------")]
    public List<Transform> planetSpawnPoints = new List<Transform>();
    public Vector3 planetDynamicLightingRotation;
    public float planetDynamicLightingIntensity;

    
    void Awake()
    {
        //Singleton
        instance = this;
    }
    
    void Start()
    {
        //Instantiate new color dictionary
        colorDictionary = new ColorDictionary(colorListTextAsset);

        //parse face and vertex data from input files
        parsedTerrainFaces = ParseFaces(textInputFaces);
        parsedTerrainVertices = ParseVertices(textInputVertices);

        StartCoroutine("RegenerateTerrain");
    }

    //STATIC METHODS
    //------------------------------------------------
    static public void AddPlayer(GameObject player, int playerNum, int playerColorIndex, string name)
    {
        PlayerManager tempPlayer = new PlayerManager();
        tempPlayer.instance = player;
        tempPlayer.playerNumber = playerNum;
        tempPlayer.playerColorIndex = playerColorIndex;
        tempPlayer.playerName = name;
        tempPlayer.randomTerrainSeed = randomSeedGenerator.Next(0, 1024);
        tempPlayer.Setup();

        playerManagers.Add(tempPlayer);
    }

    //PUBLIC METHODS
    //------------------------------------------------
    public void ChangeGameStatePlayerOnePlanet()
    {
        RpcGameStateSetup(_GameState.PlayerOnePlanet);
    }

    public void ChangeGameStatePlayerTwoPlanet()
    {
        RpcGameStateSetup(_GameState.PlayerTwoPlanet);
    }

    public void ChangeGameStateNeutral()
    {
        RpcGameStateSetup(_GameState.Neutral);
    }




    //PRIVATE METHODS
    //--------------------------------------------------------------
    private void AssignColorToTerrainTiles()
    {
        MeshRenderer mR;
        for (int i = 0; i < 2; i++)
        {
            mR = terrainTilePrefab[i].GetComponent<MeshRenderer>();
            mR.sharedMaterial.SetColor("_Color", playerColorDictionaries[i][_ColorType.BaseMain.ToString()]);
            mR.sharedMaterial.SetColor("_RimColor", playerColorDictionaries[i][_ColorType.BaseRim.ToString()]);
        }
    }

    private void AssignPlayerColors()
    {
        //Assign color enums
        playerOneColor = (_Colors)playerManagers[0].playerColorIndex;
        playerTwoColor = (_Colors)playerManagers[1].playerColorIndex;

        //Assign color sub-dictionaries to each player
        playerColorDictionaries[0] = colorDictionary.GetColorDictionary(playerOneColor.ToString());
        playerColorDictionaries[1] = colorDictionary.GetColorDictionary(playerTwoColor.ToString());
    }

    private Vector3 CalculateCentroid(Vector3[] vertices)
    {
        //This script calculates the centroid (transform.position) of the quad tile
        Vector3 centroid = Vector3.zero;
        for (int i = 0; i < vertices.Length; i++)
        {
            centroid += vertices[i];
        }
        centroid /= vertices.Length;

        return centroid;
    }

    private IEnumerator GameLoop()
    {
        while(playerManagers.Count < 2)
            yield return null;

    }

    [ClientRpc]
    private void RpcGameStateSetup(_GameState gameState)
    {
        switch (gameState)
        {
            case _GameState.Neutral:
                playerTerrains[0].SetActive(false);
                playerTerrains[1].SetActive(false);
                dynamicLight.intensity = spaceDynamicLightingIntensity;
                dynamicLight.transform.rotation = Quaternion.Euler(spaceDynamicLightingRotation);
                backgroundMeshRenderer.material = spaceBackgroundMaterial;
                starsParticleSystem.Play();
                foreach (PlayerManager pm in playerManagers)
                {
                    pm.PlayerStateChange(gameState);
                }
                break;
            case _GameState.PlayerOnePlanet:
                playerTerrains[0].SetActive(true);
                playerTerrains[1].SetActive(false);
                dynamicLight.intensity = planetDynamicLightingIntensity;
                dynamicLight.transform.rotation = Quaternion.Euler(planetDynamicLightingRotation);
                backgroundMeshRenderer.material = planetBackgroundMaterial;
                starsParticleSystem.Stop();
                starsParticleSystem.Clear();
                foreach (PlayerManager pm in playerManagers)
                {
                    pm.PlayerStateChange(gameState);
                }
                break;
            case _GameState.PlayerTwoPlanet:
                playerTerrains[0].SetActive(false);
                playerTerrains[1].SetActive(true);
                dynamicLight.intensity = planetDynamicLightingIntensity;
                dynamicLight.transform.rotation = Quaternion.Euler(planetDynamicLightingRotation);
                backgroundMeshRenderer.material = planetBackgroundMaterial;
                starsParticleSystem.Stop();
                starsParticleSystem.Clear();
                foreach (PlayerManager pm in playerManagers)
                {
                    pm.PlayerStateChange(gameState);
                }
                break;
        }
    }

    private void GenerateTerrain(GameObject playerTerrain, List<Vector3> playerTerrainVertices, int terrainIndex)
    {
        Vector3 tilePosition;
        float tileDepth;
        float perlinNoiseFactor;
        int numVertices;

        for (int i = 0; i < parsedTerrainFaces.Count; i++)
        {
            //Check if face is a triangle or quad
            numVertices = (parsedTerrainFaces[i][3] == -1) ? 3 : 4;

            //Calculate a random tile depth
            perlinNoiseFactor = Mathf.PerlinNoise(playerManagers[terrainIndex].randomTerrainSeed * 0.99f, i * 9.99f);
            tileDepth = (numVertices == 3) ? (perlinNoiseFactor*0.4f+0.4f) * terrainScale / 10f : (perlinNoiseFactor*0.4f+1f) * terrainScale / 10f;
            if (Mathf.PerlinNoise(i*9.99f, perlinNoiseFactor) < 0.5f)
            {
                tileDepth *= -1f;
            }

            //Generate Tile
            Vector3[] newVertices = new Vector3[numVertices];
            for (int j = 0; j < numVertices; j++)
            {
                newVertices[j] = playerTerrainVertices[(int)parsedTerrainFaces[i][j]];
            }
            tilePosition = CalculateCentroid(newVertices);

            //Transform tileVertices to local coordinates relative to tile position
            for (int j = 0; j < numVertices; j++)
            {
                newVertices[j] -= tilePosition;
            }

            //Instantiate tile prefab
            GameObject newTile = (GameObject)Instantiate(terrainTilePrefab[terrainIndex], tilePosition, Quaternion.identity);
            newTile.transform.parent = playerTerrain.transform;
            //Create mesh
            GenerateTile(newTile, newVertices, tileDepth);
        }
    }

    private void GenerateTile(GameObject newTile, Vector3[] vertices, float tileDepth)
    {
        //This class generates mesh and collider geometry for each tile gameobject.
        List<Vector3> verticesList = new List<Vector3>();
        int[] triangles = null;
        PolygonCollider2D polygonCollider2D;

        int numVertices = vertices.Length;

        //Configure PolygonCollider2D
        polygonCollider2D = newTile.GetComponent<PolygonCollider2D>();
        Vector2[] polygonColliderVertices = new Vector2[numVertices];
        for (int i = 0; i < numVertices; i++)
        {
            polygonColliderVertices[i] = new Vector2(vertices[i].x, vertices[i].y);
        }
        polygonCollider2D.SetPath(0, polygonColliderVertices);

        switch (numVertices)
        {
            case 3:
                //Create vertex list
                verticesList.Add(vertices[0]);                  // Vertex 0
                verticesList.Add(new Vector3(0, 0, tileDepth));         // Vertex 1
                verticesList.Add(vertices[2]);                  // Vertex 2
                verticesList.Add(vertices[0]);                  // Vertex 3
                verticesList.Add(vertices[1]);                  // Vertex 4
                verticesList.Add(verticesList[1]);              // Vertex 5
                verticesList.Add(vertices[1]);                  // Vertex 6
                verticesList.Add(vertices[2]);                  // Vertex 7
                verticesList.Add(verticesList[1]);              // Vertex 8

                triangles = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
                break;

            case 4:
                //Create vertex list
                verticesList.Add(vertices[0]);                  // Vertex 0
                verticesList.Add(new Vector3(0, 0, tileDepth)); // Vertex 1
                verticesList.Add(vertices[3]);                  // Vertex 2

                verticesList.Add(vertices[0]);                  // Vertex 3
                verticesList.Add(vertices[1]);                  // Vertex 4
                verticesList.Add(verticesList[1]);              // Vertex 5

                verticesList.Add(vertices[1]);                  // Vertex 6
                verticesList.Add(vertices[2]);                  // Vertex 7
                verticesList.Add(verticesList[1]);              // Vertex 8

                verticesList.Add(vertices[2]);                  // Vertex 9
                verticesList.Add(vertices[3]);                  // Vertex 10
                verticesList.Add(verticesList[1]);              // Vertex 11

                triangles = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
                break;
        }

        //Assign mesh
        Mesh mesh = new Mesh();
        newTile.GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = verticesList.ToArray();
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private List<Vector4> ParseFaces(TextAsset textInputFaces)
    {
        //This method parses the faces' vertex pointers from the input file
        List<Vector4> parsedTerrainFaces = new List<Vector4>();
        //Parse face data
        string[] values;
        string[] fileRows = textInputFaces.text.Split('\n');
        for (int i = 0; i < (fileRows.Length - 1); i++)
        {
            values = fileRows[i].Split(',');
            parsedTerrainFaces.Add(new Vector4(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[3])));
        }
        return parsedTerrainFaces;
    }

    private List<Vector3> ParseVertices(TextAsset textInputVertices)
    {
        //This method parses the vertices from the input file
        List<Vector3> parsedTerrainVertices = new List<Vector3>();
        string[] values;
        string[] fileRows = textInputVertices.text.Split('\n');
        for (int i = 0; i < (fileRows.Length - 1); i++)
        {
            values = fileRows[i].Split(',');
            Vector3 value = new Vector3(float.Parse(values[0]), float.Parse(values[1]), 0.0f);
            parsedTerrainVertices.Add(value);
        }
        return parsedTerrainVertices;
    }

    private List<Vector3>[] PseudoRandomizeVertices(List<Vector3> parsedTerrainVertices)
    {
        //This method parses the vertices, scales, and randomizes them for each player
        playerTerrainVertices = new List<Vector3>[2];

        //scale and randomize vertex data
        float perlinNoiseFactor;
        for (int i = 0; i < 2; i++)
        {
            playerTerrainVertices[i] = new List<Vector3>();
            for (int j = 0; j < parsedTerrainVertices.Count; j++)
            {
                perlinNoiseFactor = 1f + 0.11f*Mathf.PerlinNoise(0.99f * j, playerManagers[i].randomTerrainSeed*0.99f);
                playerTerrainVertices[i].Add(parsedTerrainVertices[j] * terrainScale * perlinNoiseFactor);
            }
        }
        return playerTerrainVertices;
    }

    private IEnumerator RegenerateTerrain()
    {
        while (playerManagers.Count < 2)
            yield return null;

        //This method (re)generates randomized terrain for both players

        //Clear player terrain list

        if (playerTerrains.Count > 0)
        {
            Destroy(playerTerrains[0]);
            Destroy(playerTerrains[1]);
            playerTerrains.Clear();
        }

        AssignPlayerColors();

        AssignColorToTerrainTiles();

        //parse vertices data from input file, scale, and randomize for each player
        playerTerrainVertices = PseudoRandomizeVertices(parsedTerrainVertices);

        //Generate player one and player two terrains
        for (int i = 0; i < 2; i++)
        {
            //Create the current player terrain tile container
            playerTerrains.Add(new GameObject());
            //Rename the terrain container to match current player
            playerTerrains[i].name = "Terrain" + (i + 1).ToString();
            //Set parent to the SetupManager GameObject and disable it
            playerTerrains[i].transform.parent = transform;
            //Create terrain tiles for the current player
            GenerateTerrain(playerTerrains[i], playerTerrainVertices[i], i);
        }

        

        if (isServer)
        {
            RpcGameStateSetup(_GameState.Neutral);
            StartCoroutine("GameLoop");
        }
    }
}
