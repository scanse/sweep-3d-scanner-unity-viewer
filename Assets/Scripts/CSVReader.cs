using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
            float x, y, z, signalStrength;
            for (int i = 0; i < numPoints; i++)
            {
                string[] row = Regex.Split(lines[i + 1], SPLIT_RE);
                if (row.Length == 0 || row[0] == "") continue;

                x = float.Parse(row[1]);
                z = float.Parse(row[2]);
                y = float.Parse(row[3]);
                signalStrength = float.Parse(row[4]);

                points.Add(new Vector4(x, y, z, signalStrength / 254.0f));
            }

            return points;
        }
        catch (Exception e)
        {
            return new List<Vector4>();
        }
    }
}