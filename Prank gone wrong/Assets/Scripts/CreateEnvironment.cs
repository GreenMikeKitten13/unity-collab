using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using System;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class CreateEnvironment : MonoBehaviour
{
    Mesh meshObject;
    Vector3[] vertices;
    int[] triangles;
    int sizeX = 250; // Reduce size for stability
    int sizeZ = 250; // Reduce size for stability
    MeshCollider meshCollider;

    void Start()
    {
        meshObject = new Mesh();
        GetComponent<MeshFilter>().mesh = meshObject;
        meshCollider = GetComponent<MeshCollider>();

        CreateMesh();
        UpdateMesh();

        InvokeRepeating("CreateTrees", 15, 15);
    }

    void CreateMesh()
    {
        vertices = new Vector3[(sizeX + 1) * (sizeZ + 1)];
        triangles = new int[sizeX * sizeZ * 6];

        // Generate vertices
        for (int z = 0, i = 0; z <= sizeZ; z++)
        {
            for (int x = 0; x <= sizeX; x++)
            {
                float y = Mathf.PerlinNoise(x * 0.025f, z * 0.025f) * 6f; // Adjust Perlin scale & height
                vertices[i++] = new Vector3(x, y, z);
            }
        }

        // Generate triangles
        int vert = 0, tris = 0;
        for (int z = 0; z < sizeZ; z++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                int topLeft = vert;
                int topRight = vert + 1;
                int bottomLeft = vert + (sizeX + 1);
                int bottomRight = vert + (sizeX + 1) + 1;

                // First triangle
                triangles[tris++] = topLeft;
                triangles[tris++] = bottomLeft;
                triangles[tris++] = topRight;

                // Second triangle
                triangles[tris++] = topRight;
                triangles[tris++] = bottomLeft;
                triangles[tris++] = bottomRight;

                vert++;
            }
            vert++;
        }
    }

    void UpdateMesh()
    {
        meshObject.Clear();
        meshObject.vertices = vertices;
        meshObject.triangles = triangles;
        meshObject.RecalculateNormals();

        // Update the MeshCollider
        meshCollider.sharedMesh = null; // Reset first
        meshCollider.sharedMesh = meshObject;
        meshCollider.convex = false; // Keep false for large meshes
    }


    public GameObject Log;
    
    void CreateTrees()
    {
        //for (int RandomTree = UnityEngine.Random.Range(70, 220), Max = 0; Max == RandomTree; Max++)
        //{
            
            GameObject newLog = Instantiate(Log);
            newLog.transform.Translate(new(0 + UnityEngine.Random.Range(0, 20), 0, 0 + UnityEngine.Random.Range(0, 20)));
            newLog.transform.SetParent(transform);
            newLog.transform.localScale = new(1, UnityEngine.Random.Range(1.5f, 3),1);
            Debug.Log(newLog);
            Debug.Log(newLog.transform.position);
        

    
       //}
        
    }
}
