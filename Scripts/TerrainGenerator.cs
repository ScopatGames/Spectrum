using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;


public class TerrainGenerator : MonoBehaviour {
    //CLASS IS OBSOLETE<<<<<<<<<<<<<<<<<<<<<<<
    public TextAsset verticesTextFile;
    public TextAsset trianglesTextFile;
    public Vector3 terrainOffset = new Vector3(0, 0, 0);
    public float terrainScale = 1f;
    public GameObject terrainTile;

    private List<Vector3> terrainVertices = new List<Vector3>();
    private List<int> terrainTriangles = new List<int>();
    private int numTiles = 0;
    private bool skipTile = false;
    	
	void Start () {
        //Parse terrain vertex data
        string[] fileRows = verticesTextFile.text.Split('\n');
        for (int i = 0; i < (fileRows.Length-1); i++)
        {
            string[] values = fileRows[i].Split(',');
            terrainVertices.Add(new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]))*terrainScale);
        }
        //Parse triangle data
        fileRows = trianglesTextFile.text.Split('\n');
        for (int i = 0; i < (fileRows.Length-1); i++)
        {
            terrainTriangles.Add(int.Parse(fileRows[i]));
        }

        //Determine number of tiles
        if (terrainTriangles.Count % 3 == 0)
        {
            numTiles = terrainTriangles.Count / 3;
        }
        else
        {
            Debug.Log("Error: terrainTriangles array needs to be a multiple of 3");
        }
        
        //Instantiate tiles
        for(int i = 0; i < numTiles; i++)
        {
            if (i > 845 && Random.Range(0.0f, 1.0f) < .33f) //This give a 67% chance that the outer tiles will be filled
            {
                skipTile = true;
            }
            else
            {
                skipTile = false;
            }
            if (!skipTile) { 
                Vector3[] tileVertices = new Vector3[3];

                //Find global tile vertex coordinates
                for (int j = 0; j < 3; j++)
                {
                    tileVertices[j] = terrainVertices[terrainTriangles[i * 3 + j]];
                }
                Vector3 tilePosition = CalculateTriangleCentroid(tileVertices[0], tileVertices[1], tileVertices[2]);
                //Transform tileVertices to local coordinates relative to tile position
                for (int j = 0; j < 3; j++)
                {
                    tileVertices[j] -= tilePosition;
                }

                float tileDepth = Random.Range(0.4f, 0.8f);
                if (Random.Range(0f, 1f) < 0.5f)
                {
                    tileDepth *= -1f;
                }

                GameObject newTile = Instantiate(terrainTile, tilePosition + terrainOffset, Quaternion.identity) as GameObject;
                newTile.transform.parent = transform;
                newTile.GetComponent<GenerateTriangleTile>().GenerateMesh(tileVertices, tileDepth);
            }
        }



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

}
