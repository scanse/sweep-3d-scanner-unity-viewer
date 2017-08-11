using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PointCloud : MonoBehaviour
{

    private Mesh mesh;
    int numPoints = 0;

    // Use this for initialization
    void Start()
    {
    }

    public void CreateMesh(Vector3[] points, float[] normalizedSignalStrength)
    {
        numPoints = points.Length;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        int[] indices = new int[numPoints];
        Color[] colors = new Color[numPoints];
        for (int i = 0; i < numPoints; ++i)
        {
            indices[i] = i;
            //colors[i] = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            colors[i] = Color.HSVToRGB(normalizedSignalStrength[i], 1.0f, 1.0f);
        }

        mesh.vertices = points;
        mesh.colors = colors;
        mesh.SetIndices(indices, MeshTopology.Points, 0);

    }
}