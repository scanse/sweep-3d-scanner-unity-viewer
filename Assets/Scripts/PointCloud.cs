using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Creates a mesh where each vertex is a point, and the color for each vertex is proportional to its signal strength.
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PointCloud : MonoBehaviour
{

    private Mesh mesh;
    int numPoints = 0;

    // Creates the gameObject's mesh using the provided points and signal strength values
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
            // map the normalized signal strength onto the hue spectrum in HSV color space
            colors[i] = Color.HSVToRGB(normalizedSignalStrength[i], 1.0f, 1.0f);
        }

        mesh.vertices = points;
        mesh.colors = colors;
        mesh.SetIndices(indices, MeshTopology.Points, 0);
    }
}