// Part of TriMM, the TriangleMesh Manipulator.
//
// Copyright (C) 2008  Christian Moritz
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along
// with this program. If not, see <http://www.gnu.org/licenses/>.
//
// For more information and contact details look at TriMMs website:
// http://trimm.sourceforge.net/

using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Globalization;

namespace TriMM {
    /// <summary>
    /// A parser for Binary or ASCII STL-files.
    /// </summary>
    public static class STLParser {

        #region Methods

        /// <summary>
        /// Parse the given Stream.
        /// </summary>
        /// <param name="file">Stream to parse</param>
        public static void Parse(StreamReader file) {
            TriMMApp.Mesh = new TriangleMesh();
            
            // Test, if the stream is ASCII or binary
            if (TestIfASCII(file)) {ParseASCII(file);} else { ParseBinary(file);}
        }

        /// <summary>
        /// Parses a binary STL file.
        /// </summary>
        /// <remarks>
        /// Because ASCII STL files can become very large, a binary version of STL exists. 
        /// A binary STL file has an 80 character header (which is generally ignored - but 
        /// which should never begin with 'solid' because that will lead most software to assume 
        /// that this is an ASCII STL file). Following the header is a 4 byte unsigned 
        /// integer indicating the number of triangular facets in the file. Following that 
        /// is data describing each triangle in turn. The file simply ends after the last triangle.
        /// Each triangle is described by twelve floating point numbers: three for the normal 
        /// and then three for the X/Y/Z coordinate of each vertex - just as with the ASCII version 
        /// of STL. After the twelve floats there is a two byte unsigned 'short' integer that 
        /// is the 'attribute byte count' - in the standard format, this should be zero because most 
        /// software does not understand anything else.( http://en.wikipedia.org/wiki/STL_(file_format) )
        /// </remarks>
        /// <param name="file">Stream to parse</param>
        private static void ParseBinary(StreamReader file) {
            BinaryReader binReader = new BinaryReader(file.BaseStream);

            // Set stream back to zero.
            binReader.BaseStream.Position = 0;
            char[] charBuf = new char[80];

            // The first 80 bytes are trash
            binReader.Read(charBuf, 0, 80);

            // Next 4 bytes contain the count of the normal/3D-vertexes record
            int count = (int)binReader.ReadUInt32();

            // Throw InvalidDataException if size does not fit
            // 84 Byte for header+count, count * (size of data = 50 Bytes)
            if (binReader.BaseStream.Length != (84 + count * 50)) { throw new InvalidDataException(); }

#if !DEBUG
            try {
#endif
            List<VectorND> normals = new List<VectorND>(count);
            List<Vertex[]> triangles = new List<Vertex[]>(count);

            // Read the records
            for (int i = 0; i < count; i++) {
                Vertex[] tmp = new Vertex[3] { new Vertex(0, 0, 0), new Vertex(0, 0, 0), new Vertex(0, 0, 0) };

                // Normal/vertices

                // First one is the normal
                normals.Add(new VectorND((double)binReader.ReadSingle(), (double)binReader.ReadSingle(), (double)binReader.ReadSingle()));

                // Next three are vertices
                for (int j = 0; j < 3; j++) { for (int k = 0; k < 3; k++) { tmp[j][k] = (double)binReader.ReadSingle(); } }
                triangles.Add(tmp);
                TriMMApp.Mesh.Vertices = TriMMApp.Mesh.Vertices.Union(tmp).ToList();

                // Last two bytes are only to fill up to 50 bytes
                binReader.Read(charBuf, 0, 2);
            }

            // Adds the Triangles with the given normals to the mesh, calculating their centroid.
            for (int i = 0; i < count; i++) {
                int ind0 = TriMMApp.Mesh.Vertices.IndexOf(triangles[i][0]);
                int ind1 = TriMMApp.Mesh.Vertices.IndexOf(triangles[i][1]);
                int ind2 = TriMMApp.Mesh.Vertices.IndexOf(triangles[i][2]);
                Vertex centroid = Triangle.GetCentroidOf(triangles[i][0], triangles[i][1], triangles[i][2]);
                centroid.Normal = normals[i];

                Triangle newTriangle = new Triangle(ind0, ind1, ind2);
                newTriangle.Centroid = centroid;
                TriMMApp.Mesh.Add(newTriangle);
            }
#if !DEBUG
            } catch {
                MessageBox.Show(TriMMApp.Lang.GetElementsByTagName("STLBrokenFileError")[0].InnerText, TriMMApp.Lang.GetElementsByTagName("ErrorTitle")[0].InnerText, MessageBoxButton.OK, MessageBoxImage.Error);
            } finally {
#endif
            binReader.Close();
#if !DEBUG
            }
#endif

            TriMMApp.Mesh.Finish(false, true);
        }

        /// <summary>
        /// Parse STL-ASCII-Format.
        /// </summary>
        /// <param name="file">Stream to parse</param>
        private static void ParseASCII(StreamReader file) {
            String input = null;
            int count = 0;

            NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
            numberFormatInfo.NumberDecimalSeparator = ".";

            file.BaseStream.Position = 0;
            StreamReader sr = new StreamReader(file.BaseStream);

            List<VectorND> normals = new List<VectorND>(count);
            List<Vertex[]> triangles = new List<Vertex[]>(count);
            Vertex[] tmp = new Vertex[3] { new Vertex(0, 0, 0), new Vertex(0, 0, 0), new Vertex(0, 0, 0) };

#if !DEBUG
            try {
#endif
            while ((input = sr.ReadLine()) != null) {
                input = input.Trim();
                if (count == 4) {
                    count = 0;
                    triangles.Add(tmp);
                    tmp = new Vertex[3] { new Vertex(0, 0, 0), new Vertex(0, 0, 0), new Vertex(0, 0, 0) };
                }

                // RemoveEmptyEntities removes empty entities, resulting from more than one whitespace
                String[] v = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (v.Length > 0) {
                    if (v[0].ToLower() == "vertex") {
                        // Parse string, NumberStyles.Float secures that different formats can be parsed
                        // such as: "-2.23454e-001" (exponential format)
                        for (int i = 0; i < 3; i++) { tmp[count - 1][i] = double.Parse(v[i + 1], NumberStyles.Float, numberFormatInfo); }
                        if (!TriMMApp.Mesh.Vertices.Contains(tmp[count - 1])) { TriMMApp.Mesh.Vertices.Add(tmp[count - 1]); }
                        count++;
                    } else if (v[0].ToLower() == "facet") {
                        normals.Add(new VectorND(double.Parse(v[2], NumberStyles.Float, numberFormatInfo),
                            double.Parse(v[3], NumberStyles.Float, numberFormatInfo), double.Parse(v[4], NumberStyles.Float, numberFormatInfo)));
                        count++;
                    }
                }
            }

            // Adds the Triangles with the given normals to the mesh, calculating their centroid.
            for (int i = 0; i < normals.Count; i++) {
                int ind0 = TriMMApp.Mesh.Vertices.IndexOf(triangles[i][0]);
                int ind1 = TriMMApp.Mesh.Vertices.IndexOf(triangles[i][1]);
                int ind2 = TriMMApp.Mesh.Vertices.IndexOf(triangles[i][2]);
                Vertex centroid = Triangle.GetCentroidOf(triangles[i][0], triangles[i][1], triangles[i][2]);
                centroid.Normal = normals[i];

                Triangle newTriangle = new Triangle(ind0, ind1, ind2);
                newTriangle.Centroid = centroid;
                TriMMApp.Mesh.Add(newTriangle);
            }
#if !DEBUG
            } catch {
                MessageBox.Show(TriMMApp.Lang.GetElementsByTagName("STLBrokenFileError")[0].InnerText, TriMMApp.Lang.GetElementsByTagName("ErrorTitle")[0].InnerText, MessageBoxButton.OK, MessageBoxImage.Error);
            } finally {
#endif
            sr.Close();
#if !DEBUG
            }
#endif
            TriMMApp.Mesh.Finish(false, true);
        }

        /// <summary>
        /// Tests whether the file is ASCII. This is true, if the file contains "vertex" and "normal".
        /// </summary>
        /// <param name="file">file to be tested</param>
        /// <returns>true, if it is ASCII. Otherwise false.</returns>
        private static bool TestIfASCII(StreamReader file) {
            bool foundVertex = false;
            bool foundNormal = false;
            String input = "";
            while ((input = file.ReadLine()) != null) {
                input = input.Trim();
                String[] v = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (0 != v.Length) {
                    if ("vertex" == v[0].ToLower()) {
                        foundVertex = true;
                    } else if ("facet" == v[0].ToLower()) {
                        foundNormal = true;
                    }
                }
                if (foundNormal && foundVertex) { return true; }
            }
            return (foundVertex && foundNormal);
        }

        /// <summary>
        /// Writes the data (normal vectors and vertices of the triangles) to the chosen file in ASCII-mode.
        /// </summary>
        /// <param name="filename">Path of the file to be saved</param>
        public static void WriteToASCII(string filename) {
            StreamWriter sw = new StreamWriter(filename);
#if !DEBUG
            try {
#endif
            sw.WriteLine("solid ");
            for (int i = 0; i < TriMMApp.Mesh.Count; i++) {
                sw.WriteLine("  facet normal " + TriMMApp.Mesh[i].Normal[0] + " " + TriMMApp.Mesh[i].Normal[1] + " " + TriMMApp.Mesh[i].Normal[2] + " ");
                sw.WriteLine("    outer loop");
                for (int j = 0; j < 3; j++) {
                    sw.WriteLine("      vertex " + TriMMApp.Mesh[i, j][0] + " " + TriMMApp.Mesh[i, j][1] + " " + TriMMApp.Mesh[i, j][2] + " ");
                }
                sw.WriteLine("    endloop");
                sw.WriteLine("  endfacet");
            }
            sw.WriteLine("endsolid ");
#if !DEBUG
            } catch (Exception exception) {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } finally {
#endif
            sw.Close();
#if !DEBUG
            }
#endif
        }

        /// <summary>
        /// Writes the data (normal vectors and vertices of the triangles) to the chosen file in binary-mode.
        /// </summary>
        /// <param name="filename">Path of the file to be saved.</param>
        public static void WriteToBinary(string filename) {
            FileStream fs = new FileStream(filename, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
#if !DEBUG
            try {
#endif
            byte abc = 0;
            byte[] headerArr = new byte[80];
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            encoding.GetBytes(TriMMApp.Lang.GetElementsByTagName("STLHeader")[0].InnerText).CopyTo(headerArr, 0);

            for (int c = 0; c < 80; c++) { bw.Write(headerArr[c]); }

            bw.Write((UInt32)(TriMMApp.Mesh.Count));

            for (int i = 0; i < TriMMApp.Mesh.Count; i++) {

                // Normal vector
                for (int j = 0; j < 3; j++) { bw.Write((float)TriMMApp.Mesh[i].Normal[j]); }

                // Next three are vertices
                for (int j = 0; j < 3; j++) { for (int k = 0; k < 3; k++) { bw.Write((float)TriMMApp.Mesh[i, j][k]); } }

                // Last two bytes are only to fill up to 50 bytes
                bw.Write(abc);
                bw.Write(abc);
            }
#if !DEBUG
            } catch (Exception exception) {
                MessageBox.Show(exception.Message, TriMMApp.Lang.GetElementsByTagName("ErrorTitle")[0].InnerText, MessageBoxButton.OK, MessageBoxImage.Error);
            } finally {
#endif
            fs.Close();
            bw.Close();
#if !DEBUG
            }
#endif
        }

        #endregion
    }
}
