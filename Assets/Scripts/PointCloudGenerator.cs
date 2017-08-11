using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCloudGenerator : MonoBehaviour {
    public string fileName = "test";
    public Material pointCloudMaterial;
    public float adjustmentMovementSpeed = 0.05f;
    private GameObject[] pointClouds;
    private List<Vector4> data;
    int numPoints = 0;
    int numDivisions = 0;
    int numPointsPerCloud = 0;


    void Awake()
    {
        data = CSVReader.ReadPoints(fileName);
        numPoints = data.Count;

        numDivisions = Mathf.CeilToInt(numPoints / 65000.0f);

        // only use a number of points that splits evenly among numDivisions pointclouds
        numPoints -= numPoints % numDivisions;

        numPointsPerCloud = numPoints / numDivisions;

        print("" + numPoints + " points, split into " + numDivisions + " clouds of " + numPointsPerCloud + " points each.");

        pointClouds = new GameObject[numDivisions];

        // do all the divisions with full size
        for (int i=0; i < numDivisions; i++)
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

        // try to move everything such that lowest point is on the ground plane
        //transform.Translate(new Vector3(0.0f, -0.01f*lowestHeight, 0.0f));
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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
