using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class GetMeshData : MonoBehaviour {
    
    private Mesh mesh;
    

    
    void Start() {
        //GetVertices();    
        //GetTriangles();
    }

    void GetVertices()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        StringBuilder output = new StringBuilder();
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                output.Append(mesh.vertices[i][j] + ",");
            }
            output.Append("\n");
        }
#if UNITY_STANDALONE
        File.WriteAllText(@"K:\Projects\Spectrum\MeshData\meshData.txt", output.ToString());
#endif
    }

    void GetTriangles()
    {
        int[] triangles = new int[980 * 3];  //know number of triangles is 980... use a list in the future for unknown amount
        int triangleIndex = 0; 
        int numberStrakes = 14; //Number of triangles per strake-section = strakeIndex*2 + 1; number triangles per strake = 5*(strakeIndex*2 + 1)
        int numberSections = 5; 

        //loop through inner triangles (strake = 0)
        for (int i =0; i < 4; i++)
        {
            triangles[3*i] = 0;
            triangles[3 * i+1] = i + 1;
            triangles[3 * i+2] = i + 2;
        }
        triangles[12] = 0;
        triangles[13] = 5;
        triangles[14] = 1;

        int lowerLeftVertexIndex = 1;
        int upperLeftVertexIndex = 6;
        

        triangleIndex = 15;

        //loop by strake
        for(int strakeIndex = 1; strakeIndex < numberStrakes; strakeIndex++)
        {
            int lastTriangleIndex = triangleIndex + ((strakeIndex*2+1)*5 - 1)*3;
            int secondToLastTriangleIndex = lastTriangleIndex - 3;
            int startingLowerLeft = lowerLeftVertexIndex;
            int startingUpperLeft = upperLeftVertexIndex;

            //loop by section
            for(int sectionIndex = 0; sectionIndex < numberSections; sectionIndex++)
            {
                //iterate through the lower vertices per strake-section
                for(int j = 0; j<(strakeIndex + 1); j++)
                {
                    //assign left triangle of "quad"
                    //Test if this is the last strake triangle...
                    if (triangleIndex == lastTriangleIndex)
                    {
                        triangles[triangleIndex] = startingLowerLeft;
                        triangles[triangleIndex + 1] = upperLeftVertexIndex + j;
                        triangles[triangleIndex + 2] = startingUpperLeft;
                    }
                    else
                    {
                        triangles[triangleIndex] = lowerLeftVertexIndex + j;
                        triangles[triangleIndex + 1] = upperLeftVertexIndex + j;
                        triangles[triangleIndex + 2] = upperLeftVertexIndex + j + 1;
                    }
                    if(j != strakeIndex)
                    {
                        //assign right triangle of "quad"
                        triangleIndex += 3;
                        //Test if this is the second to last strake triangle...
                        if (triangleIndex == secondToLastTriangleIndex)
                        {
                            triangles[triangleIndex] = lowerLeftVertexIndex + j;
                            triangles[triangleIndex + 1] = upperLeftVertexIndex + j + 1;
                            triangles[triangleIndex + 2] = startingLowerLeft;
                        }
                        else
                        {
                            triangles[triangleIndex] = lowerLeftVertexIndex + j;
                            triangles[triangleIndex + 1] = upperLeftVertexIndex + j + 1;
                            triangles[triangleIndex + 2] = lowerLeftVertexIndex + j + 1;
                        }
                    }
                    triangleIndex += 3;
                }
                lowerLeftVertexIndex += strakeIndex;
                upperLeftVertexIndex += strakeIndex + 1;
            }
        }
        StringBuilder output = new StringBuilder();
        for(int k = 0; k < triangles.Length; k++)
        {
            output.Append(triangles[k] + "\n");
        }
#if UNITY_STANDALONE
        File.WriteAllText(@"K:\Projects\Spectrum\MeshData\trianglesData.txt", output.ToString());
#endif    
      }


}
