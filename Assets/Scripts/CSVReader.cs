using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/// <summary>
/// Naively parses a csv file of the expected structure into an array of Vector4 objects.
/// Each Vector4 represents a point, where the x,y,z components are the position and the w component holds normalized signal strength.
/// Handles the conversion from centimeters to meters.
/// Handles the conversion from right handed coordinates where z is up, to unity's left handed coordinates where y is up.
/// </summary>
public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

    public static List<Vector4> ReadPoints(string file)
    {
        try
        {
            string data = System.IO.File.ReadAllText(file);
            string[] lines = Regex.Split(data, LINE_SPLIT_RE);

            int numPoints = lines.Length - 1;
            if (numPoints <= 0)
                return null;

            List<Vector4> points = new List<Vector4>();
            float x, y, z, normalizedSignalStrength;
            for (int i = 0; i < numPoints; i++)
            {
                string[] row = Regex.Split(lines[i + 1], SPLIT_RE);
                if (row.Length == 0 || row[0] == "") continue;

                // Read the position
                // convert from the centimeters to meters, and flip z and y (RHS to LHS)
                x = 0.01f * float.Parse(row[1]);
                z = 0.01f * float.Parse(row[2]);
                y = 0.01f * float.Parse(row[3]);
                // Read the signal strength and normalize it. ie: [0 : 254] => [0.0f : 1.0f]
                normalizedSignalStrength = float.Parse(row[4]) / 254.0f;

                // Package the position and signal strength into a single 4 element vector
                points.Add(new Vector4(x, y, z, normalizedSignalStrength));
            }

            return points;
        }
        catch (Exception e)
        {
            return new List<Vector4>();
        }
    }
}