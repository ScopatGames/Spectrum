using UnityEngine;
using System.Collections.Generic;

public class TerrainData : MonoBehaviour {
    public TextAsset baselineVertices;
    public TextAsset baselineFaces;
    public float terrainScale = 40.0f;
    public Vector3 terrainOffset = new Vector3(0, 0, 0);
    public GameObject terrainTilePrefab;
    public GameObject playerTerrainPrefab;
    [HideInInspector]
    public _Levels activeTerrain;
    [HideInInspector]
    public bool terrainGenerated = false;
    private List<GameObject> playerTerrains = new List<GameObject>();
    private List<Vector3>[] playerTerrainVertices = new List<Vector3>[2];
    private List<Vector4> terrainFaces = new List<Vector4>();

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
        }
    }

	void Start () {
        //If the terrain has not be generated, do so:
        if (!terrainGenerated)
        {
            //parse face data from input file; never needs to be re-run
            ParseFaces();

            //Generate player one and player two terrains
            RegenerateTerrain();

            terrainGenerated = true;
        }
	}

    public void RegenerateTerrain()
    {
        //This method (re)generates randomized terrain for both players

        //Clear player terrain list
        playerTerrains.Clear();

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
            playerTerrains[i].SetActive(false);
            //Create terrain tiles for the current player
            GenerateTerrain(playerTerrains[i], playerTerrainVertices[i]);
        }
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
            randomScaleFactorOne = Random.Range(1.0f, 1.05f);
            randomScaleFactorTwo = Random.Range(1.0f, 1.05f);
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

    private void GenerateTerrain(GameObject playerTerrain, List<Vector3> playerTerrainVertices)
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
            GameObject newTile = Instantiate(terrainTilePrefab, tilePosition + terrainOffset, Quaternion.identity) as GameObject;
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
