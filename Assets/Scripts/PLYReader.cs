using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// Naively parses a ply file of an expected structure into an array of Vector4 objects.
/// Expected structure is vertices with following attributes ONLY: float x, float y, float z, u_char signal_strength
/// Each Vector4 represents a point, where the x,y,z components are the position and the w component holds normalized signal strength.
/// Handles the conversion from centimeters to meters.
/// Handles the conversion from right handed coordinates where z is up, to unity's left handed coordinates where y is up.
/// </summary>
public class PLYReader
{
    public static List<Vector4> ReadPoints(string file)
    {
        // Check file exists
        if (!File.Exists(file))
        {
            return new List<Vector4>();
        }

        try
        {
            // Interpret File
            using (BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open)))
            {
                int fileLength = (int)reader.BaseStream.Length;
                string buildingLine = "";
                int vertexCount = 0;
                int charSize = sizeof(char);

                // read the header
                int numRead = 0;
                while ((numRead += charSize) < fileLength)
                {
                    char nextChar = reader.ReadChar();
                    if (nextChar == '\n')
                    {
                        if (buildingLine.Contains("end_header"))
                        {
                            break;
                        }
                        else if (buildingLine.Contains("element vertex"))
                        {
                            string[] array = Regex.Split(buildingLine, @"\s+");
                            if (array.Length - 2 > 0)
                            {
                                int target = Array.IndexOf(array, "vertex") + 1;
                                vertexCount = Convert.ToInt32(array[target]);
                                buildingLine = "";
                            }
                            else
                            {
                                return new List<Vector4>();
                            }
                        }
                    }
                    else
                    {
                        buildingLine += nextChar;
                    }
                }
                
                // Read the vertices
                List<Vector4> points = new List<Vector4>();
                float x, y, z, normalizedSignalStrength;
                for (int i = 0; i < vertexCount; i++)
                {
                    // Read the position
                    // convert from the centimeters to meters, and flip z and y (RHS to LHS)
                    x = 0.01f * reader.ReadSingle();
                    z = 0.01f * reader.ReadSingle();
                    y = 0.01f * reader.ReadSingle();

                    // Read the signal strength and normalize it. ie: [0 : 254] => [0.0f : 1.0f]
                    normalizedSignalStrength = 1.0f * reader.ReadByte() / 254.0f;

                    // Package the position and signal strength into a single 4 element vector
                    points.Add(new Vector4(x, y, z, normalizedSignalStrength));
                }
                return points;
            }
        }
        catch (Exception e)
        {
            return new List<Vector4>();
        }
    }
}
