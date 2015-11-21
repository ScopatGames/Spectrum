using UnityEngine;
using System.Collections;

public class TerrainGenerator : MonoBehaviour {
    public Vector3 terrainOffset = new Vector3(0, 0, 0);
    public Vector3[] terrainVertices;
    public int[] terrainTriangles;
    public GameObject terrainTile;
    public TextAsset verticesTextFile;
    public TextAsset trianglesTextFile;

    private int numTiles = 0;
    	
	void Start () {
        string[] verticesParsed = verticesTextFile.text.Split('\n');
       string[] trianglesParsed = trianglesTextFile.text.Split('\n');


        
        
        //Determine number of tiles
        if (terrainTriangles.Length % 3 == 0)
        {
            numTiles = terrainTriangles.Length / 3;
        }
        else
        {
            Debug.Log("Error: terrainTriangles array needs to be a multiple of 3");
        }
        
        //Instantiate tiles
        for(int i = 0; i < numTiles; i++)
        {
            Vector3[] tileVertices = new Vector3[3];
            
            //Find global tile vertex coordinates
            for (int j = 0; j<3; j++)
            {
                tileVertices[j] = terrainVertices[terrainTriangles[i*3 + j]];
            }
            Vector3 tilePosition = CalculateTriangleCentroid(tileVertices[0], tileVertices[1], tileVertices[2]);
            //Transform tileVertices to local coordinates relative to tile position
            for (int j = 0; j<3; j++)
            {
                tileVertices[j] -= tilePosition;
            }

            float tileInset = 0.0f; // TODO add some logic to this
            GameObject newTile = Instantiate(terrainTile, tilePosition+terrainOffset, Quaternion.identity) as GameObject;
            newTile.GetComponent<GenerateTriangleTile>().GenerateMesh(tileVertices, tileInset);
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
