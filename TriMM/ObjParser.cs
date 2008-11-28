﻿// Part of TriMM, the TriangleMesh Manipulator.
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
// For more information and contact details look at STLNormalSwitchers website:
// http://trimm.sourceforge.net/

using System;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using TriMM.VertexNormalAlgorithms;


namespace TriMM {

    /// <summary>
    /// The ObjParser allows parsing files in Wavefronts *.OBJ format.
    /// Only triangle meshes are supported.
    /// Information other than that about the Vertices, the vertex normals and the faces is lost.
    /// 
    /// </summary>
    public static class ObjParser {

        #region Methods

        /// <summary>
        /// The given StreamReader <paramref name="file"/> is parsed and the TriangleMesh returned.
        /// </summary>
        /// <param name="file">The *.OBJ file to be parsed.</param>
        /// <param name="normalAlgo">The algorithm to calculate the Vertex normals with.</param>
        /// <returns>The parsed TriangleMesh</returns>
        public static TriangleMesh Parse(StreamReader file, IVertexNormalAlgorithm normalAlgo) {
            TriangleMesh triangleMesh = new TriangleMesh();

            // Temporary variables.
            Vertex vertex;
            VectorND normal;
            String input = null;
            String[] inputList;
            int vertices = 0;
            int normals = 0;
            int a, b, c;

            // The numbers in the file must have the decimal separator ".".
            NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
            numberFormatInfo.NumberDecimalSeparator = ".";

#if !DEBUG
            try {
#endif

                input = file.ReadLine();

                while (input != null) {
                    input.Trim();

                    // RemoveEmptyEntities removes empty entities, resulting from more than one whitespace.
                    inputList = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (inputList[0] == "v") {
                        // Vertices must start with the letter "v" and contain three coordinates.
                        vertex = new Vertex(double.Parse(inputList[1], NumberStyles.Float, numberFormatInfo),
                           double.Parse(inputList[2], NumberStyles.Float, numberFormatInfo), double.Parse(inputList[3], NumberStyles.Float, numberFormatInfo));
                        triangleMesh.Vertices.Add(vertex);
                        vertices++;
                    } else if (inputList[0] == "vn") {
                        // Vertex normals must start with the letters "vn" and contain three coordinates.
                        // There must be one Vertex normal for every Vertex.
                        normal = new VectorND(double.Parse(inputList[1], NumberStyles.Float, numberFormatInfo),
                           double.Parse(inputList[2], NumberStyles.Float, numberFormatInfo), double.Parse(inputList[3], NumberStyles.Float, numberFormatInfo));
                        triangleMesh.Vertices[normals].Normal = normal;
                        normals++;
                    } else if (inputList[0] == "f") {
                        // Triangles must start with the letter "f" and contain three indices of Vertices.
                        if (inputList[1].Contains("/")) {
                            // The OBJ format allows entering information about the Vertices (v), the texture (vt) and the normal vectors at the Vertices (vn) in the form v/vt/vn.
                            // Only the Vertex informations are used, as no texture is supported by this program and the normal vectors are attached to the Vertices directly.
                            a = int.Parse(inputList[1].Substring(0, inputList[1].IndexOf('/')), numberFormatInfo) - 1;
                            b = int.Parse(inputList[2].Substring(0, inputList[2].IndexOf('/')), numberFormatInfo) - 1;
                            c = int.Parse(inputList[3].Substring(0, inputList[3].IndexOf('/')), numberFormatInfo) - 1;
                        } else {
                            // The simple version only gives information about the Vertices.
                            a = int.Parse(inputList[1], numberFormatInfo) - 1;
                            b = int.Parse(inputList[2], numberFormatInfo) - 1;
                            c = int.Parse(inputList[3], numberFormatInfo) - 1;
                        }

                        // The OBJ format allows refering to the indices of the Vertices with a negative number indicating the position of the Vertex above the face.
                        // This notation must be used consistently, so if (a < 0) so are b and c.
                        if (a < 0) {
                            a += triangleMesh.Vertices.Count;
                            b += triangleMesh.Vertices.Count;
                            c += triangleMesh.Vertices.Count;
                        }
                        triangleMesh.Add(new Triangle(a, b, c));
                    }

                    input = file.ReadLine();
                }

                // The TriangleMesh is complete and can be finalized.
                triangleMesh.Finish(true);
                // If there are no Vertex normals in the file, they are calculated with the chosen algorithm.
                // If there were Vertex normals missing for some Vertices, all are calculated.
                if (vertices != normals) { normalAlgo.GetVertexNormals(ref triangleMesh); }

#if !DEBUG
            } catch { throw new Exception("The OBJ file is broken!"); }
#endif

            return triangleMesh;
        }

        /// <summary>
        /// Exports the data from the given TriangleMesh to the OBJ format..
        /// If Vertex normals exist, they are written to that file as well.
        /// </summary>
        /// <param name="filename">Path to the file to be written.</param>
        /// <param name="triangleMesh">The TriangleMesh to be exported.</param>
        public static void WriteOBJ(string filename, TriangleMesh triangleMesh) {
            bool normals = false;
            int a, b, c;

            // The TriangleMesh contains Vertex normals.
            if (triangleMesh.Vertices[0].Normal != null) { normals = true; }

            StreamWriter sw = new StreamWriter(filename);
#if !DEBUG
            try {
#endif
                // The Header.
                sw.WriteLine("# Written by the TriMM ObjParser (by Christian Moritz)");

                // The Vertices
                for (int i = 0; i < triangleMesh.Vertices.Count; i++) {
                    sw.WriteLine("v " + triangleMesh.Vertices[i][0] + " " + triangleMesh.Vertices[i][1] + " " + triangleMesh.Vertices[i][2]);
                }

                // The Vertex Normals
                if (normals) {
                    for (int i = 0; i < triangleMesh.Vertices.Count; i++) {
                        sw.WriteLine("vn " + triangleMesh.Vertices[i].Normal[0] + " " + triangleMesh.Vertices[i].Normal[1] + " " + triangleMesh.Vertices[i].Normal[2]);

                    }
                }

                // The Triangles.
                if (normals) {
                    for (int j = 0; j < triangleMesh.Count; j++) {
                        a = triangleMesh[j][0] + 1;
                        b = triangleMesh[j][1] + 1;
                        c = triangleMesh[j][2] + 1;
                        sw.WriteLine("f " + a + "//" + a + " " + b + "//" + b + " " + c + "//" + c);
                    }
                } else {
                    for (int j = 0; j < triangleMesh.Count; j++) {
                        a = triangleMesh[j][0] + 1;
                        b = triangleMesh[j][1] + 1;
                        c = triangleMesh[j][2] + 1;
                        sw.WriteLine("f " + a +  " " + b + " " + c);
                    }
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
