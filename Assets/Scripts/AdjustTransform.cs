using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustTransform : MonoBehaviour
{
    // the initial scale
    public float initialScale = 1.0f;

    // toggle and sensitivity for controlling the point cloud transform
    public bool userCanAdjustHeight = true;
    public float heightAdjustSensitivity = 0.01f;
    public bool userCanAdjustScale = true;
    public float scaleAdjustSensitivity = 0.005f;
    public bool userCanAdjustYaw = true;
    public float yawAdjustSensitivity = 0.15f;

    // Use this for initialization
    void Start()
    {
        transform.localScale = new Vector3(initialScale, initialScale, initialScale);
    }

    // Update is called once per frame
    void Update()
    {
        // Check for vertical adjustment
        if (userCanAdjustHeight)
        {
            if (Input.GetKey(KeyCode.E))
            {
                transform.Translate(new Vector3(0.0f, heightAdjustSensitivity, 0.0f));
            }
            if (Input.GetKey(KeyCode.Q))
            {
                transform.Translate(new Vector3(0.0f, -heightAdjustSensitivity, 0.0f));
            }
        }
        // Check for yaw adjustment
        if (userCanAdjustYaw)
        {
            if (Input.GetKey(KeyCode.R))
            {
                transform.Rotate(0.0f, -yawAdjustSensitivity, 0.0f);
            }
            if (Input.GetKey(KeyCode.T))
            {
                transform.Rotate(0.0f, yawAdjustSensitivity, 0.0f);
            }
        }
        // Check for scale adjustment
        if (userCanAdjustScale)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                transform.localScale += new Vector3(scaleAdjustSensitivity, scaleAdjustSensitivity, scaleAdjustSensitivity);
            }
            if (Input.GetKey(KeyCode.C))
            {
                if (transform.localScale.x > scaleAdjustSensitivity * 4)
                    transform.localScale -= new Vector3(scaleAdjustSensitivity, scaleAdjustSensitivity, scaleAdjustSensitivity);
            }
        }
    }
}
