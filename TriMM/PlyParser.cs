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
// For more information and contact details look at STLNormalSwitchers website:
// http://trimm.sourceforge.net/

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using TriMM.VertexNormalAlgorithms;

namespace TriMM {

    /// <summary>
    /// The PlyParser allows parsing files in the Stanford Triangle Format *.PLY.
    /// Only triangle meshes are supported.
    /// </summary>
    public static class PlyParser {

        #region Structs

        /// <summary>
        /// This struct is used for storing the information about "elements" given in the header
        /// of a *.PLY file for further processing.
        /// </summary>
        private struct Element {

            #region Fields

            public string name;
            public int count;
            public List<string> properties;

            #endregion

            #region Constructors

            /// <summary>
            /// The Constructor initializes the Element with a name and the number of such elements contained in the file.
            /// A list for the properties of the element is initialized.
            /// </summary>
            /// <param name="name">Name of the Element.</param>
            /// <param name="count">Number of such Elements in the file.</param>
            public Element(string name, int count) {
                this.name = name;
                this.count = count;
                properties = new List<string>();
            }

            #endregion
        }

        #endregion

        #region Methods

        /// <summary>
        /// The given StreamReader <paramref name="file"/> is parsed and the TriangleMesh returned.
        /// Currently only version 1.0 ASCII *.PLY files are supported, as I found no full documentation
        /// of the binary versions so far.
        /// </summary>
        /// <param name="file">The *.PLY file to be parsed.</param>
        /// <param name="normalAlgo">The algorithm to calculate the Vertex normals with.</param>
        /// <returns>The parsed TriangleMesh</returns>
        public static TriangleMesh Parse(StreamReader file, IVertexNormalAlgorithm normalAlgo) {
            TriangleMesh triangleMesh = new TriangleMesh();

            // Temporary variables.
            Vertex vertex;
            String input = null;
            int count = 0;
            int vertices = 0;
            int faces = 0;
            string format;
            string version;
            List<Element> elements = new List<Element>();
            String[] inputList;

            // The numbers in the file must have the decimal separator ".".
            NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
            numberFormatInfo.NumberDecimalSeparator = ".";

            // The file must not be empty!
            input = file.ReadLine();
            if (input == null) { throw new Exception("The file is not a PLY file, or it is broken!"); }
            input.Trim();
            inputList = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            // The first word in the first line must be "ply"!
            if (inputList[0] != "ply") { throw new Exception("The file is not a PLY file, or it is broken!"); }
            input = file.ReadLine();
            input.Trim();
            inputList = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            // There has to be more information than just "ply"!
            if (input == null) { throw new Exception("The file is not a PLY file, or it is broken!"); }

            // The header is processed here.
            do {
                // Empty lines and comment-lines are skipped.
                if ((input == "") || (input.StartsWith("comment"))) { } else if (inputList[0] == "format") {
                    if (inputList.Length != 3) { throw new Exception("The file is not a PLY file, or it is broken!"); }
                    format = inputList[1];
                    version = inputList[2];
                } else if (inputList[0] == "element") {
                    // The Elements are read and stored. There need to be Elements called "vertex" and "face".
                    if (inputList.Length != 3) { throw new Exception("The file is not a PLY file, or it is broken!"); }
                    elements.Add(new Element(inputList[1], int.Parse(inputList[2])));
                } else if (inputList[0] == "property") {
                    // The properties of the Elements are stored with the elements.
                    elements[elements.Count - 1].properties.Add(input);
                }

                // There has to be more information than just the header, which must end with "end_header"!
                input = file.ReadLine();
                if (input == null) { throw new Exception("The file is not a PLY file, or it is broken!"); }
                input.Trim();
                inputList = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            } while (input != "end_header");

            for (int i = 0; i < elements.Count; i++) {
                Element el = elements[i];
                // Vertices are needed for this program.
                if (el.name == "vertex") {
                    vertices = el.count;
                    // At least the x-, y- and z-coordinates of a Vertex are needed.
                    // There could be more informations, but as there are no standard names for them in the original 
                    // description of the PLY format, they are ignored by this program.
                    if (el.properties.Count < 3) {
                        throw new Exception("The file is not a PLY file, or it is broken!");
                    } else {
                        bool x = false, y = false, z = false;
                        for (int j = 0; j < el.properties.Count; j++) {
                            String[] l = el.properties[j].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            switch (l[2]) {
                                case "x":
                                    x = true;
                                    break;
                                case "y":
                                    y = true;
                                    break;
                                case "z":
                                    z = true;
                                    break;
                            }
                        }
                        if (!x || !y || !z) { throw new Exception("The file is not a PLY file, or it is broken!"); }
                    }
                } else if (el.name == "face") {
                    faces = el.count;
                    // The faces need to be indices of the Vertices!
                    if (el.properties[0] != "property list uchar int vertex_index") { throw new Exception("The file is not a PLY file, or it is broken!"); }
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
                    inputList = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    // Only the Vertex is read and added to the VertexList of the owners TriangleMesh, everything else in the line is ignored.
                    vertex = new Vertex(double.Parse(inputList[0], NumberStyles.Float, numberFormatInfo),
                        double.Parse(inputList[1], NumberStyles.Float, numberFormatInfo), double.Parse(inputList[2], NumberStyles.Float, numberFormatInfo));

                    triangleMesh.Vertices.Add(vertex);

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
                    inputList = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    // Only triangles are allowed for this program.
                    if (int.Parse(inputList[0]) != 3) { throw new Exception("At least one of the faces is not a triangle!"); }

                    // Only the Triangle is read and added to the owners TriangleMesh, everything else in the line is ignored.
                    triangleMesh.Add(new Triangle(int.Parse(inputList[1], numberFormatInfo), int.Parse(inputList[2], numberFormatInfo), int.Parse(inputList[3], numberFormatInfo)));

                    count++;
                }
            }

            // The TriangleMesh is complete and can be finalized.
            triangleMesh.Finish(true);
            // The Vertex normals are calculated with the chosen algorithm.
            normalAlgo.GetVertexNormals(ref triangleMesh);

            return triangleMesh;
        }

        /// <summary>
        /// Exports the data from the given TriangleMesh to the Stanford Triangle Format *.PLY.
        /// </summary>
        /// <param name="filename">Path to the file to be written.</param>
        /// <param name="triangleMesh">The TriangleMesh to be exported.</param>
        public static void WritePLY(string filename, TriangleMesh triangleMesh) {
            StreamWriter sw = new StreamWriter(filename);
#if !DEBUG
            try {
#endif
                // The Header.
                sw.WriteLine("ply");
                sw.WriteLine("comment Written by the TriMM PlyParser (by Christian Moritz)");
                sw.WriteLine("format ascii 1.0");
                sw.WriteLine("element vertex " + triangleMesh.Vertices.Count.ToString());
                sw.WriteLine("property double x");
                sw.WriteLine("property double y");
                sw.WriteLine("property double z");
                sw.WriteLine("element face " + triangleMesh.Count.ToString());
                sw.WriteLine("property list uchar int vertex_index");
                sw.WriteLine("end_header");

                // The Vertices.
                for (int i = 0; i < triangleMesh.Vertices.Count; i++) {
                    sw.WriteLine(triangleMesh.Vertices[i][0] + " " + triangleMesh.Vertices[i][1] + " " + triangleMesh.Vertices[i][2]);
                }

                // The Triangles.
                for (int j = 0; j < triangleMesh.Count; j++) {
                    sw.WriteLine(3 + " " + triangleMesh[j][0] + " " + triangleMesh[j][1] + " " + triangleMesh[j][2]);
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
