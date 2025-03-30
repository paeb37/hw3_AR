#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class TriangleCreator : MonoBehaviour
{
    void Start()
    {
        // Create a new mesh
        Mesh mesh = new Mesh();

        Vector3[] vertices = {
                    new Vector3(-0.25f, -0.5f, 0.1f), // Bottom-left corner
                    new Vector3(0.25f, -0.5f, 0.1f),  // Bottom-right corner
                    new Vector3(0f, 0.5f, 0.1f)      // Top corner
        };

        // Define the triangle indices (clockwise order)
        int[] triangles = { 0, 1, 2 };

        // Assign vertices and triangles to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        // Attach the mesh to a GameObject
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

        meshFilter.mesh = mesh;
        
        // Create and configure the material
        Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        material.color = Color.blue; // Set a visible color
        material.SetFloat("_Metallic", 0.0f); // Reduce metallic effect
        material.SetFloat("_Glossiness", 0.5f); // Set glossiness
        meshRenderer.material = material;

#if UNITY_EDITOR
        // Save the mesh as an asset in the project folder
        AssetDatabase.CreateAsset(mesh, "Assets/TriangleMesh.asset");
        AssetDatabase.SaveAssets();
#endif
    }
}
