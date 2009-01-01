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
using System.Windows;
using System.Globalization;

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
        /// The given StreamReader <paramref name="file"/> is parsed and the TriangleMesh built.
        /// </summary>
        /// <param name="file">The *.OBJ file to be parsed.</param>
        public static void Parse(StreamReader file) {
            TriMMApp.Mesh = new TriangleMesh();

            // Temporary variables.
            Vertex vertex;
            Vector normal;
            String input = null;
            String[] inputList;
            int vertices = 0;
            int normals = 0;
            int a, b, c;

            // The numbers in the file must have the decimal separator ".".
            NumberFormatInfo nFI = new NumberFormatInfo();
            nFI.NumberDecimalSeparator = ".";

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
                        vertex = new Vertex(double.Parse(inputList[1], NumberStyles.Float, nFI),
                           double.Parse(inputList[2], NumberStyles.Float, nFI), double.Parse(inputList[3], NumberStyles.Float, nFI));
                        TriMMApp.Mesh.Vertices.Add(vertex);
                        vertices++;
                    } else if (inputList[0] == "vn") {
                        // Vertex normals must start with the letters "vn" and contain three coordinates.
                        // There must be one Vertex normal for every Vertex.
                        normal = new Vector(double.Parse(inputList[1], NumberStyles.Float, nFI),
                           double.Parse(inputList[2], NumberStyles.Float, nFI), double.Parse(inputList[3], NumberStyles.Float, nFI));
                        TriMMApp.Mesh.Vertices[normals].Normal = normal;
                        normals++;
                    } else if (inputList[0] == "f") {
                        // Triangles must start with the letter "f" and contain three indices of Vertices.
                        if (inputList[1].Contains("/")) {
                            // The OBJ format allows entering information about the Vertices (v), the texture (vt) and the normal vectors at the Vertices (vn) in the form v/vt/vn.
                            // Only the Vertex informations are used, as no texture is supported by this program and the normal vectors are attached to the Vertices directly.
                            a = int.Parse(inputList[1].Substring(0, inputList[1].IndexOf('/')), nFI) - 1;
                            b = int.Parse(inputList[2].Substring(0, inputList[2].IndexOf('/')), nFI) - 1;
                            c = int.Parse(inputList[3].Substring(0, inputList[3].IndexOf('/')), nFI) - 1;
                        } else {
                            // The simple version only gives information about the Vertices.
                            a = int.Parse(inputList[1], nFI) - 1;
                            b = int.Parse(inputList[2], nFI) - 1;
                            c = int.Parse(inputList[3], nFI) - 1;
                        }

                        // The OBJ format allows refering to the indices of the Vertices with a negative number indicating the position of the Vertex above the face.
                        // This notation must be used consistently, so if (a < 0) so are b and c.
                        if (a < 0) {
                            a += TriMMApp.Mesh.Vertices.Count;
                            b += TriMMApp.Mesh.Vertices.Count;
                            c += TriMMApp.Mesh.Vertices.Count;
                        }
                        TriMMApp.Mesh.Add(new Triangle(a, b, c));
                    }

                    input = file.ReadLine();
                }

                // The TriangleMesh is complete and can be finalized.
                // If there are no Vertex normals in the file, they are calculated with the chosen algorithm.
                // If there were Vertex normals missing for some Vertices, all are calculated.
                if (vertices != normals) {
                    TriMMApp.Mesh.Finish(true, true);
                } else {
                    TriMMApp.Mesh.Finish(true, false);
                }

#if !DEBUG
            } catch {
                throw new Exception(TriMMApp.Lang.GetElementsByTagName("ObjBrokenFileError")[0].InnerText); }
#endif

        }

        /// <summary>
        /// Exports the data from the given TriangleMesh to the OBJ format..
        /// If Vertex normals exist, they are written to that file as well.
        /// </summary>
        /// <param name="filename">Path to the file to be written.</param>
        public static void WriteOBJ(string filename) {
            bool normals = false;
            int a, b, c;

            // The numbers in the file must have the decimal separator ".".
            NumberFormatInfo nFI = new NumberFormatInfo();
            nFI.NumberDecimalSeparator = ".";

            // The TriangleMesh contains Vertex normals.
            if (TriMMApp.Mesh.Vertices[0].Normal != null) { normals = true; }

            StreamWriter sw = new StreamWriter(filename);
#if !DEBUG
            try {
#endif
                // The Header.
                sw.WriteLine(TriMMApp.Lang.GetElementsByTagName("ObjHeader")[0].InnerText);

                // The Vertices
                for (int i = 0; i < TriMMApp.Mesh.Vertices.Count; i++) {
                    sw.WriteLine("v " + TriMMApp.Mesh.Vertices[i][0].ToString(nFI) + " " + TriMMApp.Mesh.Vertices[i][1].ToString(nFI) + " " + TriMMApp.Mesh.Vertices[i][2].ToString(nFI));
                }

                // The Vertex Normals
                if (normals) {
                    for (int i = 0; i < TriMMApp.Mesh.Vertices.Count; i++) {
                        sw.WriteLine("vn " + TriMMApp.Mesh.Vertices[i].Normal[0].ToString(nFI) + " " + TriMMApp.Mesh.Vertices[i].Normal[1].ToString(nFI) + " " + TriMMApp.Mesh.Vertices[i].Normal[2].ToString(nFI));
                    }
                }

                // The Triangles.
                if (normals) {
                    for (int j = 0; j < TriMMApp.Mesh.Count; j++) {
                        a = TriMMApp.Mesh[j][0] + 1;
                        b = TriMMApp.Mesh[j][1] + 1;
                        c = TriMMApp.Mesh[j][2] + 1;
                        sw.WriteLine("f " + a + "//" + a + " " + b + "//" + b + " " + c + "//" + c);
                    }
                } else {
                    for (int j = 0; j < TriMMApp.Mesh.Count; j++) {
                        a = TriMMApp.Mesh[j][0] + 1;
                        b = TriMMApp.Mesh[j][1] + 1;
                        c = TriMMApp.Mesh[j][2] + 1;
                        sw.WriteLine("f " + a +  " " + b + " " + c);
                    }
                }

#if !DEBUG
            } catch (Exception exception) {
                MessageBox.Show(exception.Message, TriMMApp.Lang.GetElementsByTagName("ErrorTitle")[0].InnerText, MessageBoxButton.OK, MessageBoxImage.Error);
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
