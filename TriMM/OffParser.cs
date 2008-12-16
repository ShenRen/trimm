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
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using TriMM.VertexNormalAlgorithms;


namespace TriMM {

    /// <summary>
    /// The OffParser allows parsing files in the Geomview OOGL format *.OFF.
    /// Only triangle meshes are supported.
    /// </summary>
    public static class OffParser {

        #region Methods

        /// <summary>
        /// The given StreamReader <paramref name="file"/> is parsed and the TriangleMesh built.
        /// </summary>
        /// <param name="file">The *.OFF file to be parsed.</param>
        /// <param name="normalAlgo">The algorithm to calculate the Vertex normals with.</param>
        public static void Parse(StreamReader file, IVertexNormalAlgorithm normalAlgo) {
            TriMM.Mesh = new TriangleMesh();
            TriMM.Mesh.VertexNormalAlgorithm = normalAlgo;

            // Temporary variables.
            Vertex vertex;
            VectorND normal;
            String input = null;
            int count = 0;
            int vertices = 0;
            int faces = 0;
            bool vN = false;

            // The numbers in the file must have the decimal separator ".".
            NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
            numberFormatInfo.NumberDecimalSeparator = ".";

            // The header is processed here.
            while (count < 2) {
                input = file.ReadLine();
                input.Trim();

                // Empty lines and comment-lines are skipped.
                if ((input != "") && (!input.StartsWith("#"))) {

                    // A header exists.
                    if (input.Contains("OFF")) {

                        // Vertex normal vectors are contained.
                        if (input.Contains("N")) { vN = true; }

                        count++;
                    } else if (count == 1) {
                        // Cuts comments at the end of the last line of the header.
                        if (input.Contains("#")) { input = input.Substring(0, input.IndexOf("#")); }

                        // RemoveEmptyEntities removes empty entities, resulting from more than one whitespace.
                        String[] v = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        // The last line of the header contains the numbers of Vertices, Faces (Triangles) and Edges (not needed).
                        if (v.Length != 3) {
                            throw new Exception("Wrong number of counters!");
                        } else {
                            vertices = int.Parse(v[0]);
                            faces = int.Parse(v[1]);
                        }
                        count++;
                    } else {
                        // There is only the last line of the header with the numbers of Vertices, Faces and Edges present.
                        count++;
                    }
                }
            }

            // The following lines in the file contain the Vertices.
            count = 0;
            while (count < vertices) {
                input = file.ReadLine();
                input.Trim();
                if ((input != "") && (!input.StartsWith("#"))) {
                    if (input.Contains("#")) { input = input.Substring(0, input.IndexOf("#")); }

                    // RemoveEmptyEntities removes empty entities, resulting from more than one whitespace.
                    String[] v = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    // Only the Vertex is read and added to the VertexList of the owners TriangleMesh, everything else in the line is ignored.
                    vertex = new Vertex(double.Parse(v[0], NumberStyles.Float, numberFormatInfo),
                        double.Parse(v[1], NumberStyles.Float, numberFormatInfo), double.Parse(v[2], NumberStyles.Float, numberFormatInfo));

                    // Vertex normals can follow the Vertices.
                    if (vN) {
                        normal = new VectorND(double.Parse(v[3], NumberStyles.Float, numberFormatInfo),
                            double.Parse(v[4], NumberStyles.Float, numberFormatInfo), double.Parse(v[5], NumberStyles.Float, numberFormatInfo));
                        vertex.Normal = normal;
                    }
                    TriMM.Mesh.Vertices.Add(vertex);

                    count++;
                }
            }

            // The following lines in the file contain the Faces as combinations of the indices of the Vertices parsed above.
            count = 0;
            while (count < faces) {
                input = file.ReadLine();
                input.Trim();
                if ((input != "") && (!input.StartsWith("#"))) {
                    if (input.Contains("#")) { input = input.Substring(0, input.IndexOf("#")); }

                    // RemoveEmptyEntities removes empty entities, resulting from more than one whitespace
                    String[] v = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    // Only the Triangle is read and added to the owners TriangleMesh, everything else in the line is ignored.
                    TriMM.Mesh.Add(new Triangle(int.Parse(v[1], numberFormatInfo), int.Parse(v[2], numberFormatInfo), int.Parse(v[3], numberFormatInfo)));

                    count++;
                }
            }

            // The TriangleMesh is complete and can be finalized.
            // If there are no Vertex normals in the file, they are calculated with the chosen algorithm.
            if (!vN) {
                TriMM.Mesh.Finish(true, true);
            } else {
                TriMM.Mesh.Finish(true, false);
            }
        }

        /// <summary>
        /// Exports the data from the given TriangleMesh to the Geomview OOGL format *.OFF (ASCII).
        /// If Vertex normals exist, they are written to that file as well.
        /// </summary>
        /// <param name="filename">Path to the file to be written.</param>
        public static void WriteOFF(string filename) {
            bool normals = false;

            // The TriangleMesh contains Vertex normals.
            if (TriMM.Mesh.Vertices[0].Normal != null) { normals = true; }

            StreamWriter sw = new StreamWriter(filename);
#if !DEBUG
            try {
#endif
                // The Header.
                sw.WriteLine("# Written by the TriMM OffParser (by Christian Moritz)");
                if (normals) { sw.WriteLine("NOFF"); } else { sw.WriteLine("OFF"); }
                sw.WriteLine(TriMM.Mesh.Vertices.Count + " " + TriMM.Mesh.Count + " " + TriMM.Mesh.Edges.Count);

                // The Vertices
                for (int i = 0; i < TriMM.Mesh.Vertices.Count; i++) {
                    if (normals) {
                        sw.WriteLine(TriMM.Mesh.Vertices[i][0] + " " + TriMM.Mesh.Vertices[i][1] + " " + TriMM.Mesh.Vertices[i][2] + " "
                            + TriMM.Mesh.Vertices[i].Normal[0] + " " + TriMM.Mesh.Vertices[i].Normal[1] + " " + TriMM.Mesh.Vertices[i].Normal[2]);
                    } else {
                        sw.WriteLine(TriMM.Mesh.Vertices[i][0] + " " + TriMM.Mesh.Vertices[i][1] + " " + TriMM.Mesh.Vertices[i][2]);
                    }
                }

                // The Triangles.
                for (int j = 0; j < TriMM.Mesh.Count; j++) {
                    sw.WriteLine(3 + " " + TriMM.Mesh[j][0] + " " + TriMM.Mesh[j][1] + " " + TriMM.Mesh[j][2]);
                }
#if !DEBUG
            } catch (Exception exception) {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
#endif
                sw.Close();
#if !DEBUG
            }
#endif
        }

        #endregion
    }
}
