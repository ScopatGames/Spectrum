using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class GetMeshData : MonoBehaviour {
    
    private Mesh mesh;
    

    // Use this for initialization
    void Start() {
        mesh = GetComponent<MeshFilter>().mesh;
        StringBuilder output = new StringBuilder();
        for (int i = 0; i < mesh.vertices.Length; i++) {
            for (int j = 0; j<3; j++)
            {
                output.Append(mesh.vertices[i][j] + ",");
            }
            output.Append("\n");
        }
        File.WriteAllText(@"K:\Projects\Spectrum\MeshData\meshData.dat", output.ToString());
        


    }
	
   


}
