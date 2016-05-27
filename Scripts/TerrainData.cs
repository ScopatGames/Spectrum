using UnityEngine;
using System.Collections.Generic;

public class TerrainData : MonoBehaviour {
    public TextAsset baselineVertices;
    public TextAsset baselineFaces;
    public float terrainScale = 40.0f;
    public Vector3 terrainOffset = new Vector3(0, 0, 0);
    public List<GameObject> terrainTilePrefab = new List<GameObject>();
    public GameObject playerTerrainPrefab;

    [HideInInspector]
    public _Levels activeTerrain;
    [HideInInspector]
    public bool terrainGenerated = false;

    public List<GameObject> playerTerrains = new List<GameObject>();
    public List<Vector3>[] playerTerrainVertices = new List<Vector3>[2];

    private List<Vector4> terrainFaces = new List<Vector4>();
    private PlayerData playerData;
    private static TerrainData instance;

    void Awake()
    {
        //Singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            playerData = GetComponent<PlayerData>();
            activeTerrain = _Levels.Neutral;
            //parse face data from input file; never needs to be re-run
            ParseFaces();
        }
    }

    public void RegenerateTerrain()
    {
        //This method (re)generates randomized terrain for both players

        //Clear player terrain list
        
        if (terrainGenerated)
        {
            Destroy(playerTerrains[0]);
            Destroy(playerTerrains[1]);
            playerTerrains.Clear();
            playerTerrainVertices[0].Clear();
            playerTerrainVertices[1].Clear();
        }

        //Pick colors
        playerData.PlayerColors();

        //Assign color to terrain tiles
        AssignColorToTerrainTiles();

        //parse vertices data from input file, scale, and randomize for each player
        ParseScaleRandomizeVertices(playerTerrainVertices);

        //Generate player one and player two terrains
        for (int i = 0; i < 2; i++)
        {
            //Create the current player terrain tile container
            playerTerrains.Add(Instantiate(playerTerrainPrefab, Vector3.zero, Quaternion.identity) as GameObject);
            //Rename the terrain container to match current player
            playerTerrains[i].name = (i + 1).ToString();
            //Set parent to the GameController GameObject and disable it
            playerTerrains[i].transform.parent = transform;
            //Create terrain tiles for the current player
            GenerateTerrain(playerTerrains[i], playerTerrainVertices[i], i);
        }
        ActivateTerrain(_Levels.Neutral);
        terrainGenerated = true;
    }

    public void ActivateTerrain(_Levels level){
        switch (level)
        {
            case _Levels.Neutral:
                playerTerrains[0].SetActive(false);
                playerTerrains[1].SetActive(false);
                break;
            case _Levels.PlayerOne:
                playerTerrains[0].SetActive(true);
                playerTerrains[1].SetActive(false);
                break;
            case _Levels.PlayerTwo:
                playerTerrains[0].SetActive(false);
                playerTerrains[1].SetActive(true);
                break;
        }
    }

    private void AssignColorToTerrainTiles()
    {
        MeshRenderer mR;
        for (int i = 0; i < 2; i++)
        {
            mR = terrainTilePrefab[i].GetComponent<MeshRenderer>();
            mR.sharedMaterial.SetColor("_Color", playerData.playerColorDictionaries[i][_ColorType.BaseMain.ToString()]);
            mR.sharedMaterial.SetColor("_RimColor", playerData.playerColorDictionaries[i][_ColorType.BaseRim.ToString()]);
        }
    }

    private void ParseScaleRandomizeVertices(List<Vector3>[] playerTerrainVertices)
    {
        //This method parses the vertices, scales, and randomizes them for each player
        playerTerrainVertices[0] = new List<Vector3>();
        playerTerrainVertices[1] = new List<Vector3>();
        //Parse terrain vertex data and scale
        float randomScaleFactorOne, randomScaleFactorTwo, value0, value1;
        string[] values;
        string[] fileRows = baselineVertices.text.Split('\n');
        for (int i = 0; i < (fileRows.Length - 1); i++)
        {
            randomScaleFactorOne = Random.Range(1.0f, 1.1f);
            randomScaleFactorTwo = Random.Range(1.0f, 1.1f);
            values = fileRows[i].Split(',');
            value0 = float.Parse(values[0]);
            value1 = float.Parse(values[1]);
            playerTerrainVertices[0].Add(new Vector3(value0, value1, 0.0f) * terrainScale * randomScaleFactorOne);
            playerTerrainVertices[1].Add(new Vector3(value0, value1, 0.0f) * terrainScale * randomScaleFactorTwo);
        }
    } 

    private void ParseFaces()
    {
        //This method parses the faces' vertex pointers from the input file

        terrainFaces.Clear();
        //Parse face data
        string[] values;
        string[] fileRows = baselineFaces.text.Split('\n');
        for (int i = 0; i < (fileRows.Length - 1); i++)
        {
            values = fileRows[i].Split(',');
            terrainFaces.Add(new Vector4(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[3])));
        }
    }

    private void GenerateTerrain(GameObject playerTerrain, List<Vector3> playerTerrainVertices, int terrainIndex)
    {
        Vector3 tilePosition;
        float tileDepth;
        int numVertices;

        for (int i = 0; i < terrainFaces.Count; i++)
        {
            //Check if face is a triangle or quad
            numVertices = (terrainFaces[i][3] == -1) ? 3 : 4;

            //Calculate a random tile depth
            tileDepth = (numVertices == 3) ? Random.Range(0.4f, 0.8f) * terrainScale / 10f : Random.Range(1.0f, 1.4f) * terrainScale / 10f;
            if (Random.Range(0f, 1f) < 0.5f)
            {
                tileDepth *= -1f;
            }

            //Generate Tile
            Vector3[] newVertices = new Vector3[numVertices];
            for (int j = 0; j < numVertices; j++)
            {
                newVertices[j] = playerTerrainVertices[(int)terrainFaces[i][j]];
            }
            tilePosition = CalculateCentroid(newVertices);

            //Transform tileVertices to local coordinates relative to tile position
            for (int j = 0; j < numVertices; j++)
            {
                newVertices[j] -= tilePosition;
            }

            //Instantiate tile prefab
            GameObject newTile = Instantiate(terrainTilePrefab[terrainIndex], tilePosition + terrainOffset, Quaternion.identity) as GameObject;
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

    private Vector3 CalculateCentroid(Vector3[] vertices)
    {
        //This script calculates the centroid (transform.position) of the quad tile
        Vector3 centroid = Vector3.zero;
        for(int i = 0; i < vertices.Length; i++)
        {
            centroid += vertices[i]; 
        }
        centroid /= vertices.Length;
       
        return centroid;
    }

}
