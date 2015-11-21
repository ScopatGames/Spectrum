using UnityEngine;
using System.Collections;

public class GenerateTriangleTile : MonoBehaviour {
    //This class generates mesh and collider geometry for each tile gameobject.
    	

    public void GenerateMesh(Vector3[] tileVertices, float tileInset)
    {
        //Initialize tile triangles array
        int[] tileTriangles = new int[3];

        //If tile has an inset, calculate the new inset vertices
        if(tileInset != 0.0f)
        {

        }
        else
        {
            for(int i = 0; i<3; i++)
            {
                tileTriangles[i] = i;
            }
        }

        //Assign mesh
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = tileVertices;
        mesh.triangles = tileTriangles;
   }

    
	
	
}
