using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameManagerSinglePlayer : MonoBehaviour {

    static public GameManagerSinglePlayer instance;
    static public List<PlayerManager> playerManagers = new List<PlayerManager>();
    static public System.Random randomSeedGenerator = new System.Random();

    [Header("Number of players:")]
    public int numberOfPlayers;

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

        StartCoroutine("InitiateGameStateSingleNeutral");
        
        StartCoroutine("RegenerateTerrain");

        StartCoroutine("GameLoop");
    }

    //STATIC METHODS
    //------------------------------------------------
    static public void AddPlayer(GameObject player, int playerNum, int playerColorIndex, string name, int randomTerrainSeed)
    {
        PlayerManager tempPlayer = new PlayerManager();
        tempPlayer.instance = player;
        tempPlayer.playerNumber = playerNum;
        tempPlayer.playerColorIndex = playerColorIndex;
        tempPlayer.playerName = name;
        tempPlayer.randomTerrainSeed = randomTerrainSeed;
        tempPlayer.Setup();

        playerManagers.Add(tempPlayer);
    }

    //PUBLIC METHODS -- Currently called from GUI buttons
    //------------------------------------------------
    public void ChangeGameStateSinglePlanetAttack()
    {
        GameStateSetup(_GameState.SinglePlanetAttack);
    }

    public void ChangeGameStateSinglePlanetDefend()
    {
        GameStateSetup(_GameState.SinglePlanetDefend);
    }

    public void ChangeGameStateSingleNeutral()
    {
        GameStateSetup(_GameState.SingleNeutral);
    }





    //PRIVATE METHODS
    //--------------------------------------------------------------
    private void AssignColorToTerrainTiles()
    {
        MeshRenderer mR;
        for (int i = 0; i < numberOfPlayers; i++)
        {
            mR = terrainTilePrefab[i].GetComponent<MeshRenderer>();
            mR.sharedMaterial.SetColor("_Color", playerColorDictionaries[i][_ColorType.BaseMain.ToString()]);
            mR.sharedMaterial.SetColor("_RimColor", playerColorDictionaries[i][_ColorType.BaseRim.ToString()]);
        }
    }

    private void AssignPlayerColors()
    {
        //Assign color sub-dictionaries to each player. This is still applicable to single player, hence looping through 2
        for (int i = 0; i < 2; i++)
        {
            playerColorDictionaries[i] = colorDictionary.GetColorDictionary(((_Colors)playerManagers[i].playerColorIndex).ToString());
        }
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
        while (playerManagers.Count < numberOfPlayers)
            yield return null;

    }
    /**** USE THESE AS REFERENCE TO BUILD SINGLE PLAYER STATE CHANGES
    private void GameStateMultiNeutral()
    {
        if (playerTerrains.Count == 2)
        {
            playerTerrains[0].SetActive(false);
            playerTerrains[1].SetActive(false);
        }
        dynamicLight.intensity = spaceDynamicLightingIntensity;
        dynamicLight.transform.rotation = Quaternion.Euler(spaceDynamicLightingRotation);
        backgroundMeshRenderer.material = spaceBackgroundMaterial;
        starsParticleSystem.Play();
        foreach (PlayerManager pm in playerManagers)
        {
            pm.PlayerStateChange(_GameState.MultiNeutral);
        }
    }

    private void GameStateMultiPlayerOnePlanet()
    {
        if (playerTerrains.Count == 2)
        {
            playerTerrains[0].SetActive(true);
            playerTerrains[1].SetActive(false);
        }
        dynamicLight.intensity = planetDynamicLightingIntensity;
        dynamicLight.transform.rotation = Quaternion.Euler(planetDynamicLightingRotation);
        backgroundMeshRenderer.material = planetBackgroundMaterial;
        starsParticleSystem.Stop();
        starsParticleSystem.Clear();
        foreach (PlayerManager pm in playerManagers)
        {
            pm.PlayerStateChange(_GameState.MultiPlayerOnePlanet);
        }
    }

    private void GameStateMultiPlayerTwoPlanet()
    {
        if (playerTerrains.Count == 2)
        {
            playerTerrains[0].SetActive(false);
            playerTerrains[1].SetActive(true);
        }
        dynamicLight.intensity = planetDynamicLightingIntensity;
        dynamicLight.transform.rotation = Quaternion.Euler(planetDynamicLightingRotation);
        backgroundMeshRenderer.material = planetBackgroundMaterial;
        starsParticleSystem.Stop();
        starsParticleSystem.Clear();
        foreach (PlayerManager pm in playerManagers)
        {
            pm.PlayerStateChange(_GameState.MultiPlayerTwoPlanet);
        }
    }
    */
    private void GameStateSingleNeutral() { }

    private void GameStateSinglePlanetAttack() { }

    private void GameStateSinglePlanetDefend() { }

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
            tileDepth = (numVertices == 3) ? (perlinNoiseFactor * 0.4f + 0.4f) * terrainScale / 10f : (perlinNoiseFactor * 0.4f + 1f) * terrainScale / 10f;
            if (Mathf.PerlinNoise(i * 9.99f, perlinNoiseFactor) < 0.5f)
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

    private IEnumerator InitiateGameStateSingleNeutral()
    {
        while (playerManagers.Count < numberOfPlayers)
        {
            yield return null;
        }

        //wait a frame to let all the player components initialize
        yield return null;

        GameStateSingleNeutral();
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
        for (int i = 0; i < numberOfPlayers; i++)
        {
            playerTerrainVertices[i] = new List<Vector3>();
            for (int j = 0; j < parsedTerrainVertices.Count; j++)
            {
                perlinNoiseFactor = 1f + 0.11f * Mathf.PerlinNoise(0.99f * j, playerManagers[i].randomTerrainSeed * 0.99f);
                playerTerrainVertices[i].Add(parsedTerrainVertices[j] * terrainScale * perlinNoiseFactor);
            }
        }
        return playerTerrainVertices;
    }

    private IEnumerator RegenerateTerrain()
    {
        while (playerManagers.Count < numberOfPlayers)
            yield return null;

        //This method (re)generates randomized terrain for both players

        //Clear player terrain list

        if (playerTerrains.Count > 0)
        {
            for (int i = 0; i < playerTerrains.Count; i++)
            {
                Destroy(playerTerrains[i]);
            }
            playerTerrains.Clear();
        }

        AssignPlayerColors();

        AssignColorToTerrainTiles();

        //parse vertices data from input file, scale, and randomize for each player
        playerTerrainVertices = PseudoRandomizeVertices(parsedTerrainVertices);

        //Generate player one and player two terrains
        for (int i = 0; i < numberOfPlayers; i++)
        {
            //Create the current player terrain tile container
            playerTerrains.Add(new GameObject());
            //Rename the terrain container to match current player
            playerTerrains[i].name = "Terrain" + (i + 1).ToString();
            //Set parent to the SetupManager GameObject and disable it
            playerTerrains[i].transform.parent = transform;
            //Create terrain tiles for the current player
            GenerateTerrain(playerTerrains[i], playerTerrainVertices[i], i);
            playerTerrains[i].SetActive(false);
        }

        
    }

    private void GameStateSetup(_GameState gameState)
    {
        switch (gameState)
        {
            case _GameState.SingleNeutral:
                GameStateSingleNeutral();
                break;
            case _GameState.SinglePlanetAttack:
                GameStateSinglePlanetAttack();
                break;
            case _GameState.SinglePlanetDefend:
                GameStateSinglePlanetDefend();
                break;
        }
    }
}
