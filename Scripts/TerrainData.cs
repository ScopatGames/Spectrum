using UnityEngine;
using System.Collections.Generic;

public class TerrainData : MonoBehaviour {
    public TextAsset baselineVertices;
    public TextAsset baselineFaces;
    public GameObject playerOneTerrain;
    public GameObject playerTwoTerrain;
    public float terrainScale = 1.0f;
    public Vector3 terrainOffset = new Vector3(0, 0, 0);
    public GameObject terrainTilePrefab;

    private bool terrainGenerated = false;
    private List<Vector3> playerOneTerrainVertices = new List<Vector3>();
    private List<Vector3> playerTwoTerrainVertices = new List<Vector3>();
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
            //parse face data from input file
            ParseFaces();
            //parse vertices data from input file and scale for each player
            ParseAndScaleVertices(playerOneTerrainVertices, playerTwoTerrainVertices);

            //Generate player one terrain
            GenerateTerrain(playerOneTerrain, playerOneTerrainVertices);
            //Generate player two terrain
            GenerateTerrain(playerTwoTerrain, playerTwoTerrainVertices);

            terrainGenerated = true;
        }
	}

    private void ParseAndScaleVertices(List<Vector3> pOneTerrainVertices, List<Vector3> pTwoTerrainVertices)
    {
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
            pOneTerrainVertices.Add(new Vector3(value0, value1, 0.0f) * terrainScale * randomScaleFactorOne);
            pTwoTerrainVertices.Add(new Vector3(value0, value1, 0.0f) * terrainScale * randomScaleFactorTwo);
        }
        
    } 

    private void ParseFaces()
    {
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
        Vector3[] newTriangleVertices = new Vector3[3];
        Vector3[] newQuadVertices = new Vector3[4];
        Vector3 tilePosition;
        float tileDepth;
        for (int i = 0; i < terrainFaces.Count; i++)
        {
            //Check if face is a triangle or quad
            if (terrainFaces[i][3] == -1)
            {
                //Calculate a random tile depth
                tileDepth = Random.Range(0.4f, 0.8f) * terrainScale / 10f ;
                if (Random.Range(0f, 1f) < 0.5f)
                {
                    tileDepth *= -1f;
                }
                //Generate Triangle
                for (int j = 0; j < 3; j++)
                {
                    newTriangleVertices[j] = playerTerrainVertices[(int)terrainFaces[i][j]];
                }
                tilePosition = CalculateTriangleCentroid(newTriangleVertices[0], newTriangleVertices[1], newTriangleVertices[2]);
                //Transform tileVertices to local coordinates relative to tile position
                for (int j = 0; j < 3; j++)
                {
                    newTriangleVertices[j] -= tilePosition;
                }
                GameObject newTile = Instantiate(terrainTilePrefab, tilePosition + terrainOffset, Quaternion.identity) as GameObject;
                newTile.transform.parent = playerTerrain.transform;
                GenerateTriangleTile(newTile, newTriangleVertices, tileDepth);
            }
            else
            {
                //Calculate a random tile depth
                tileDepth = Random.Range(1.0f, 1.4f)*terrainScale/10f;
                if (Random.Range(0f, 1f) < 0.5f)
                {
                    tileDepth *= -1f;
                }
                //Generate Quad
                for (int j = 0; j < 4; j++)
                {
                    newQuadVertices[j] = playerTerrainVertices[(int)terrainFaces[i][j]];
                }
                tilePosition = CalculateQuadCentroid(newQuadVertices[0], newQuadVertices[1], newQuadVertices[2], newQuadVertices[3]);
                //Transform tileVertices to local coordinates relative to tile position
                for (int j = 0; j < 4; j++)
                {
                    newQuadVertices[j] -= tilePosition;
                }
                GameObject newTile = Instantiate(terrainTilePrefab, tilePosition + terrainOffset, Quaternion.identity) as GameObject;
                newTile.transform.parent = playerTerrain.transform;
                GenerateQuadTile(newTile, newQuadVertices, tileDepth);
            }
        }
    }

    private void GenerateTriangleTile(GameObject newTile, Vector3[] triangleVertices, float tileDepth)
    {
        //This class generates mesh and collider geometry for each tile gameobject.
        List<int> tileTriangles = new List<int>();
        List<Vector3> triangleVerticesList = new List<Vector3>();
        PolygonCollider2D polygonCollider2D;

        //Configure PolygonCollider2D
        polygonCollider2D = newTile.GetComponent<PolygonCollider2D>();
        Vector2[] polygonColliderVertices = new Vector2[3];
        for (int i = 0; i < 3; i++)
        {
            polygonColliderVertices[i] = new Vector2(triangleVertices[i].x, triangleVertices[i].y);
        }
        polygonCollider2D.SetPath(0, polygonColliderVertices);

        //Create vertex list
        triangleVerticesList.Add(triangleVertices[0]);                  // Vertex 0
        triangleVerticesList.Add(new Vector3(0, 0, tileDepth));         // Vertex 1
        triangleVerticesList.Add(triangleVertices[2]);                  // Vertex 2
        triangleVerticesList.Add(triangleVertices[0]);                  // Vertex 3
        triangleVerticesList.Add(triangleVertices[1]);                  // Vertex 4
        triangleVerticesList.Add(triangleVerticesList[1]);              // Vertex 5
        triangleVerticesList.Add(triangleVertices[1]);                  // Vertex 6
        triangleVerticesList.Add(triangleVertices[2]);                  // Vertex 7
        triangleVerticesList.Add(triangleVerticesList[1]);              // Vertex 8

        triangleVertices = triangleVerticesList.ToArray();

        /*tileUVs = new Vector2[6];
        tileUVs[0] = new Vector2(0.5f, 1.0f);
        tileUVs[1] = new Vector2(1.0f, 0.0f);
        tileUVs[2] = new Vector2(0.0f, 0.0f);
        tileUVs[3] = new Vector2(0.5f, tileInsetBorder);
        tileUVs[4] = new Vector2(tileInsetBorder, 1 - tileInsetBorder);
        tileUVs[5] = new Vector2(1 - tileInsetBorder, 1 - tileInsetBorder);
        */

        //Outer tris
        tileTriangles.Add(0);
        tileTriangles.Add(1);
        tileTriangles.Add(2);

        tileTriangles.Add(3);
        tileTriangles.Add(4);
        tileTriangles.Add(5);

        tileTriangles.Add(6);
        tileTriangles.Add(7);
        tileTriangles.Add(8);

        //Assign mesh
        Mesh mesh = new Mesh();
        newTile.GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = triangleVertices;
        mesh.triangles = tileTriangles.ToArray();
        //mesh.uv = tileUVs;
        mesh.RecalculateNormals();
    }

    private Vector3 CalculateTriangleCentroid(Vector3 vertexA, Vector3 vertexB, Vector3 vertexC)
    {
        //This script calculates the centroid (transform.position) of the triangle tile
        Vector3 centroid = Vector3.zero;
        for (int i = 0; i < 3; i++)
        {
            centroid[i] = (vertexA[i] + vertexB[i] + vertexC[i]) / 3.0f;
        }
        return centroid;
    }

    private void GenerateQuadTile(GameObject newTile, Vector3[] quadVertices, float tileDepth)
    {
        //This class generates mesh and collider geometry for each tile gameobject.
        List<int> tileTriangles = new List<int>();
        List<Vector3> quadVerticesList = new List<Vector3>();
        PolygonCollider2D polygonCollider2D;

        //Configure PolygonCollider2D
        polygonCollider2D = newTile.GetComponent<PolygonCollider2D>();
        Vector2[] polygonColliderVertices = new Vector2[4];
        for (int i = 0; i < 4; i++)
        {
            polygonColliderVertices[i] = new Vector2(quadVertices[i].x, quadVertices[i].y);
        }
        polygonCollider2D.SetPath(0, polygonColliderVertices);


        //Create vertex list
        quadVerticesList.Add(quadVertices[0]);                  // Vertex 0
        quadVerticesList.Add(new Vector3(0, 0, tileDepth));     // Vertex 1
        quadVerticesList.Add(quadVertices[3]);                  // Vertex 2

        quadVerticesList.Add(quadVertices[0]);                  // Vertex 3
        quadVerticesList.Add(quadVertices[1]);                  // Vertex 4
        quadVerticesList.Add(quadVerticesList[1]);              // Vertex 5

        quadVerticesList.Add(quadVertices[1]);                  // Vertex 6
        quadVerticesList.Add(quadVertices[2]);                  // Vertex 7
        quadVerticesList.Add(quadVerticesList[1]);              // Vertex 8

        quadVerticesList.Add(quadVertices[2]);                  // Vertex 9
        quadVerticesList.Add(quadVertices[3]);                  // Vertex 10
        quadVerticesList.Add(quadVerticesList[1]);              // Vertex 11

        quadVertices = quadVerticesList.ToArray();

        /*tileUVs = new Vector2[6];
        tileUVs[0] = new Vector2(0.5f, 1.0f);
        tileUVs[1] = new Vector2(1.0f, 0.0f);
        tileUVs[2] = new Vector2(0.0f, 0.0f);
        tileUVs[3] = new Vector2(0.5f, tileInsetBorder);
        tileUVs[4] = new Vector2(tileInsetBorder, 1 - tileInsetBorder);
        tileUVs[5] = new Vector2(1 - tileInsetBorder, 1 - tileInsetBorder);
        */

        //Outer tris
        tileTriangles.Add(0);
        tileTriangles.Add(1);
        tileTriangles.Add(2);

        tileTriangles.Add(3);
        tileTriangles.Add(4);
        tileTriangles.Add(5);

        tileTriangles.Add(6);
        tileTriangles.Add(7);
        tileTriangles.Add(8);

        tileTriangles.Add(9);
        tileTriangles.Add(10);
        tileTriangles.Add(11);

        //Assign mesh
        Mesh mesh = new Mesh();
        newTile.GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = quadVertices;
        mesh.triangles = tileTriangles.ToArray();
        //mesh.uv = tileUVs;
        mesh.RecalculateNormals();
    }

    private Vector3 CalculateQuadCentroid(Vector3 vertexA, Vector3 vertexB, Vector3 vertexC, Vector3 vertexD)
    {
        //This script calculates the centroid (transform.position) of the quad tile
        Vector3 centroid = Vector3.zero;
        for (int i = 0; i < 3; i++)
        {
            centroid[i] = (vertexA[i] + vertexB[i] + vertexC[i] + vertexD[i]) / 4.0f;
        }
        return centroid;
    }

}
