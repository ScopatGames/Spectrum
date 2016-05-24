using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateTriangleTile : MonoBehaviour {
    //CLASS IS OBSOLETE <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

    //This class generates mesh and collider geometry for each tile gameobject.
    private List<int> tileTriangles = new List<int>();
    private List<Vector3> tileVerticesList = new List<Vector3>();
    private Vector3 newVertex;
    private PolygonCollider2D polygonCollider2D;
    //private Vector2[] tileUVs;
    
    public void GenerateMesh(Vector3[] tileVertices, float tileDepth)
    {
        //Configure PolygonCollider2D
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        Vector2[] polygonColliderVertices = new Vector2[3];
        for(int i =0; i < 3; i++)
        {
            polygonColliderVertices[i] = new Vector2(tileVertices[i].x, tileVertices[i].y);
        }
        polygonCollider2D.SetPath(0, polygonColliderVertices);


        //If tile has an inset, calculate the new inset vertices
        if (tileDepth != 0.0f)
        {
            //Create vertex list
            tileVerticesList.Add(tileVertices[0]);                   // Vertex 0
            tileVerticesList.Add(new Vector3(0, 0, tileDepth));     // Vertex 1
            tileVerticesList.Add(tileVertices[2]);                  // Vertex 2
            tileVerticesList.Add(tileVertices[0]);                  // Vertex 3
            tileVerticesList.Add(tileVertices[1]);                  // Vertex 4
            tileVerticesList.Add(tileVerticesList[1]);              // Vertex 5
            tileVerticesList.Add(tileVertices[1]);                  // Vertex 6
            tileVerticesList.Add(tileVertices[2]);                  // Vertex 7
            tileVerticesList.Add(tileVerticesList[1]);              // Vertex 8
            
            tileVertices = tileVerticesList.ToArray();

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
        }
        else
        {
            for(int i = 0; i<3; i++)
            {
                tileTriangles.Add(i);
            }

            /*tileUVs = new Vector2[3];
            tileUVs[0] = new Vector2(0.5f, 1.0f);
            tileUVs[1] = new Vector2(1.0f, 0.0f);
            tileUVs[2] = new Vector2(0.0f, 0.0f);
            */
        }

        //Assign mesh
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = tileVertices;
        mesh.triangles = tileTriangles.ToArray();
        //mesh.uv = tileUVs;
        mesh.RecalculateNormals();

        


    }

    
	
	
}
