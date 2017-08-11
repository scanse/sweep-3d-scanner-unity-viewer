using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;

/// <summary>
/// Requests the user select a `.csv` file.
/// Reads the points from the file.
/// Splits the points into multiple children each of which is a PointCloud object with a portion of the points.
/// </summary>
public class PointCloudGenerator : MonoBehaviour
{
    public Material pointCloudMaterial;
    // controls how quickly the sensitivity when adjusting the vertical position of the point cloud
    public float verticalAdjustSpeed = 0.01f;
    public float scaleAdjustSpeed = 0.01f;
    // maintained reference to the generated children (point cloud objects)
    private GameObject[] pointClouds;
    // the points read from the data file
    private List<Vector4> data;
    // the number of points read from the data file
    int numPoints = 0;
    // the number of point clouds that will be generated from the collection of points
    int numDivisions = 0;
    // the nubmer of points in each generated point cloud
    int numPointsPerCloud = 0;
    // The maximum number of vertices unity will allow per single mesh
    const int MAX_NUMBER_OF_POINTS_PER_MESH = 65000;

    void Awake()
    {
        // Open a native file dialog, so the user can specify the file location
        SFB.ExtensionFilter[] extensions = new[] { new ExtensionFilter("Point Cloud Files", "csv") };
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
        if (paths.Length < 1)
        {
            print("Error: Please specify a CSV file.");
            Application.Quit();
            return;
        }

        // Read the points from the file
        data = CSVReader.ReadPoints(paths[0]);
        numPoints = data.Count;
        if (numPoints <= 0)
        {
            print("Error: failed to read points from " + paths[0]);
            Application.Quit();
            return;
        }

        // Calculate the appropriate division of points
        numDivisions = Mathf.CeilToInt(1.0f * numPoints / MAX_NUMBER_OF_POINTS_PER_MESH);

        // For simplicity, only use a number of points that splits evenly among numDivisions pointclouds
        numPoints -= numPoints % numDivisions;
        numPointsPerCloud = numPoints / numDivisions;

        print("" + numPoints + " points, split into " + numDivisions + " clouds of " + numPointsPerCloud + " points each.");

        pointClouds = new GameObject[numDivisions];

        // generate point cloud objects, each with the same number of points
        for (int i = 0; i < numDivisions; i++)
        {
            int offset = i * numPointsPerCloud;
            // generate a subset of data for this point cloud
            Vector3[] positions = new Vector3[numPointsPerCloud];
            float[] normalizedSignalStrength = new float[numPointsPerCloud];
            for (int j = 0; j < numPointsPerCloud; j++)
            {
                // normalzied signal strength stored in the 4th component of the vector
                normalizedSignalStrength[j] = data[offset + j].w;

                // position stored in the first 3 elements of the vector (conversion handled by implicit cast)
                positions[j] = data[offset + j];
            }

            // Create the point cloud using the subset of data
            GameObject obj = new GameObject("Empty");
            obj.transform.SetParent(transform, false);
            obj.AddComponent<PointCloud>().CreateMesh(positions, normalizedSignalStrength);
            obj.GetComponent<MeshRenderer>().material = pointCloudMaterial;
            pointClouds[i] = obj;
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Check for positive vertical adjustment
        if (Input.GetKey(KeyCode.E))
        {
            transform.Translate(new Vector3(0.0f, verticalAdjustSpeed, 0.0f));
        }
        // Check for negative vertical adjustment
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(new Vector3(0.0f, -verticalAdjustSpeed, 0.0f));
        }
        // Check for positive scale adjustment
        if (Input.GetKey(KeyCode.Z))
        {
            transform.localScale += new Vector3(scaleAdjustSpeed, scaleAdjustSpeed, scaleAdjustSpeed);
        }
        // Check for negative scale adjustment
        if (Input.GetKey(KeyCode.C))
        {
            if (transform.localScale.x > scaleAdjustSpeed * 2)
                transform.localScale -= new Vector3(scaleAdjustSpeed, scaleAdjustSpeed, scaleAdjustSpeed);
        }
    }
}
