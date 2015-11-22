using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateTriangleTile : MonoBehaviour {
    //This class generates mesh and collider geometry for each tile gameobject.
    private List<int> tileTriangles = new List<int>();
    private List<Vector3> tileVerticesList = new List<Vector3>();
    private Vector3 newVertex;
    //private Vector2[] tileUVs;
    
    public void GenerateMesh(Vector3[] tileVertices, float tileInset, float tileInsetBorder)
    {
        //If tile has an inset, calculate the new inset vertices
        if(tileInset != 0.0f && Mathf.Abs(tileInsetBorder) > 0.0f && Mathf.Abs(tileInsetBorder) < 1.0f)
        {
            //Create vertex list
            tileVerticesList.Add(tileVertices[0] * tileInsetBorder + new Vector3(0,0,tileInset));    //0
            tileVerticesList.Add(tileVertices[1] * tileInsetBorder + new Vector3(0, 0, tileInset));    //1
            tileVerticesList.Add(tileVertices[2] * tileInsetBorder + new Vector3(0, 0, tileInset));    //2
            tileVerticesList.Add(tileVertices[0]);                      //3
            tileVerticesList.Add(tileVertices[1]);                      //4
            tileVerticesList.Add(tileVerticesList[1]);                  //5
            tileVerticesList.Add(tileVerticesList[0]);                  //6
            tileVerticesList.Add(tileVertices[1]);                      //7
            tileVerticesList.Add(tileVertices[2]);                      //8
            tileVerticesList.Add(tileVerticesList[2]);                  //9
            tileVerticesList.Add(tileVerticesList[1]);                  //10
            tileVerticesList.Add(tileVertices[2]);                      //11
            tileVerticesList.Add(tileVertices[0]);                      //12
            tileVerticesList.Add(tileVerticesList[0]);                  //13
            tileVerticesList.Add(tileVerticesList[2]);                  //14

            
            

            /*for (int i = 0; i < tileVertices.Length; i++)
            {
                tileVerticesList.Add(tileVertices[i]);
            }
            
            //Calculate three new vertices
            for (int i = 0; i < ; i++)
            {
                newVertex = tileVertices[i] * tileInsetBorder;
                newVertex.z = tileInset;
                tileVerticesList.Add(newVertex);
            }
            */

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
            tileTriangles.Add(6);

            tileTriangles.Add(6);
            tileTriangles.Add(4);
            tileTriangles.Add(5);

            tileTriangles.Add(7);
            tileTriangles.Add(8);
            tileTriangles.Add(10);

            tileTriangles.Add(10);
            tileTriangles.Add(8);
            tileTriangles.Add(9);

            tileTriangles.Add(11);
            tileTriangles.Add(12);
            tileTriangles.Add(14);

            tileTriangles.Add(14);
            tileTriangles.Add(12);
            tileTriangles.Add(13);

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
