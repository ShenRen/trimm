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

using System;
using System.IO;
using System.Windows.Forms;
using System.Globalization;

namespace TriMM {

    /// <summary>
    /// The PlyParser allows parsing files in the Stanford Triangle Format *.PLY.
    /// Only triangle meshes are supported.
    /// </summary>
    public static class PlyParser {

        #region Methods

        /// <summary>
        /// The given StreamReader <paramref name="file"/> is parsed and the TriangleMesh returned.
        /// </summary>
        /// <param name="file">The *.PLY file to be parsed.</param>
        /// <returns>The parsed TriangleMesh</returns>
        public static TriangleMesh Parse(StreamReader file) {
            TriangleMesh triangleMesh = new TriangleMesh();

            // Temporary variables.
            Vertex vertex;
            String input = null;
            int count = 0;
            int vertices = 0;
            int faces = 0;
            bool ascii = false;
            bool ply = false;
            bool v = false;
            bool f = false;
            bool ind = false;
            bool x = false;
            bool y = false;
            bool z = false;

            // The numbers in the file must have the decimal separator ".".
            NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
            numberFormatInfo.NumberDecimalSeparator = ".";

            // The header is processed here.
            do {
                input = file.ReadLine();
                input.Trim();

                // Empty lines and comment-lines are skipped.
                if ((input != "") && (!input.StartsWith("comment"))) {
                    if (!ply && (input == "ply")) {
                        ply = true;
                    } else if (ply) {
                        if (!ascii && (input == "format ascii 1.0")) {
                            ascii = true;
                        } else if (ascii) {
                            if (!v && input.StartsWith("element vertex")) {
                                // RemoveEmptyEntities removes empty entities, resulting from more than one whitespace.
                                String[] l = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                vertices = int.Parse(l[2]);
                                v = true;
                            } else if (v) {
                                if (!x && input.StartsWith("property") && input.EndsWith("x")) {
                                    x = true;
                                } else if (x) {
                                    if (!y && input.StartsWith("property") && input.EndsWith("y")) {
                                        y = true;
                                    } else if (y) {
                                        if (!z && input.StartsWith("property") && input.EndsWith("z")) {
                                            z = true;
                                        } else if (z) {
                                            if (!input.StartsWith("property") || input.StartsWith("property list")) {
                                                if (!f && input.StartsWith("element face")) {
                                                    // RemoveEmptyEntities removes empty entities, resulting from more than one whitespace.
                                                    String[] l = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                                    faces = int.Parse(l[2]);
                                                    f = true;
                                                } else if (f) {
                                                    if (!ind && (input == "property list uchar int vertex_index")) {
                                                        ind = true;
                                                    }
                                                } else {
                                                    throw new Exception("The file is not a PLY file, or it is broken!");
                                                }
                                            }
                                        } else {
                                            throw new Exception("The file is not a PLY file, or it is broken!");
                                        }
                                    } else {
                                        throw new Exception("The file is not a PLY file, or it is broken!");
                                    }
                                } else {
                                    throw new Exception("The file is not a PLY file, or it is broken!");
                                }
                            } else {
                                throw new Exception("The file is not a PLY file, or it is broken!");
                            }
                        } else {
                            throw new Exception("The file is not an ASCII PLY file, or it is broken!");
                        }
                    } else {
                        throw new Exception("The file is not a PLY file, or it is broken!");
                    }
                }
            } while (input != "end_header");

            // The following lines in the file contain the Vertices.
            count = 0;
            while (count < vertices) {
                input = file.ReadLine();
                input.Trim();
                if ((input != "") && (!input.StartsWith("#"))) {
                    if (input.Contains("#")) { input = input.Substring(0, input.IndexOf("#")); }

                    // RemoveEmptyEntities removes empty entities, resulting from more than one whitespace.
                    String[] l = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    // Only the Vertex is read and added to the VertexList of the owners TriangleMesh, everything else in the line is ignored.
                    vertex = new Vertex(double.Parse(l[0], NumberStyles.Float, numberFormatInfo),
                        double.Parse(l[1], NumberStyles.Float, numberFormatInfo), double.Parse(l[2], NumberStyles.Float, numberFormatInfo));

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
                    String[] l = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    // Only the Triangle is read and added to the owners TriangleMesh, everything else in the line is ignored.
                    triangleMesh.Add(new Triangle(int.Parse(l[1], numberFormatInfo), int.Parse(l[2], numberFormatInfo), int.Parse(l[3], numberFormatInfo)));

                    count++;
                }
            }

            // The TriangleMesh is complete and can be finalized.
            triangleMesh.Finish(true);

            return triangleMesh;
        }

        /// <summary>
        /// Exports the data from the given TriangleMesh to the Stanford Triangle Format *.PLY.
        /// If Vertex normals exist, they are written to that file as well.
        /// </summary>
        /// <param name="filename">Path to the file to be written.</param>
        /// <param name="triangleMesh">The TriangleMesh to be exported.</param>
        public static void WritePLY(string filename, TriangleMesh triangleMesh) {
            StreamWriter sw = new StreamWriter(filename);
#if !DEBUG
            try {
#endif
            // The Header.
            sw.WriteLine("comment Written by the TriMM PlyParser (by Christian Moritz)");
            sw.WriteLine("ply");
            sw.WriteLine("format ascii 1.0");
            sw.WriteLine("element vertex " + triangleMesh.Vertices.Count.ToString());
            sw.WriteLine("property double x");
            sw.WriteLine("property double x");
            sw.WriteLine("property double x");
            sw.WriteLine("element face " + triangleMesh.Count.ToString());
            sw.WriteLine("property list uchar int vertex_index");
            sw.WriteLine("end_header");

            // The Vertices
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
