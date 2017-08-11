using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;

public class PointCloudGenerator : MonoBehaviour
{
    public Material pointCloudMaterial;
    public float adjustmentMovementSpeed = 0.05f;
    private GameObject[] pointClouds;
    private List<Vector4> data;
    int numPoints = 0;
    int numDivisions = 0;
    int numPointsPerCloud = 0;
    const int MAX_NUMBER_OF_POINTS_PER_MESH = 65000;

    void Awake()
    {
        // Have the user specify the file
        SFB.ExtensionFilter[] extensions = new[] {
                new ExtensionFilter("Point Cloud Files", "csv")
        };
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
        if (paths.Length < 1)
        {
            print("Error: Please specify a CSV file.");
            Application.Quit();
            return;
        }

        // Get the points from the file
        data = CSVReader.ReadPoints(paths[0]);
        numPoints = data.Count;
        if (numPoints <= 0)
        {
            print("Error: failed to read points from " + paths[0]);
            Application.Quit();
            return;
        }

        numDivisions = Mathf.CeilToInt(1.0f * numPoints / MAX_NUMBER_OF_POINTS_PER_MESH);

        // only use a number of points that splits evenly among numDivisions pointclouds
        numPoints -= numPoints % numDivisions;

        numPointsPerCloud = numPoints / numDivisions;

        print("" + numPoints + " points, split into " + numDivisions + " clouds of " + numPointsPerCloud + " points each.");

        pointClouds = new GameObject[numDivisions];

        // do all the divisions with full size
        for (int i = 0; i < numDivisions; i++)
        {
            int offset = i * numPointsPerCloud;
            Vector3[] positions = new Vector3[numPointsPerCloud];
            float[] normalizedSignalStrength = new float[numPointsPerCloud];
            for (int j = 0; j < numPointsPerCloud; j++)
            {
                normalizedSignalStrength[j] = data[offset + j].w;
                positions[j] = 0.01f * data[offset + j];
            }

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
        if (Input.GetKey(KeyCode.E))
        {
            //here you put the code of your event
            transform.Translate(new Vector3(0.0f, adjustmentMovementSpeed, 0.0f));
        }

        if (Input.GetKey(KeyCode.Q))
        {
            //here you put the code of your event
            transform.Translate(new Vector3(0.0f, -adjustmentMovementSpeed, 0.0f));
        }
    }
}
