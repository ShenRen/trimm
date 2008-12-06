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
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using TriMM.VertexNormalAlgorithms;


namespace TriMM {
    [Serializable()]

    /// <summary>
    /// The main datastructure containing all informations about the triangle mesh.
    /// It is a list of Triangles, with an additional list of Vertices and a SortedList of Edges.
    /// Some values needed for drawing the mesh are also stored.
    /// </summary>
    public class TriangleMesh : List<Triangle> {

        #region Fields

        private List<Vertex> vertices = new List<Vertex>();
        private SortedList<decimal, Edge> edges = new SortedList<decimal, Edge>();
        private float minEdgeLength = float.PositiveInfinity;

        private float scale;
        private double[] center = new double[3] { 0, 0, 0 };

        private int vertexColorDist;
        private int edgeColorDist;
        private int triangleColorDist;

        private double minColorScale = 1;
        private double maxColorScale = 1;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Vertex at Index <paramref name="index2"/> in the Triangle of Index  <paramref name="index1"/> .
        /// </summary>
        /// <param name="index1">Triangle index</param>
        /// <param name="index2">Corner index</param>
        /// <returns><paramref name="index2"/> Vertex in Triangle <paramref name="index1"/>.</returns>
        public Vertex this[int index1, int index2] { get { return vertices[this[index1][index2]]; } }

        /// <value>Gets the list of all Vertices.</value>
        public List<Vertex> Vertices { get { return vertices; } set { vertices = value; } }

        /// <value>Gets the list of all Edges.</value>
        public SortedList<decimal, Edge> Edges { get { return edges; } }

        /// <value>Gets the length of the shortest Edge in this TriangleMesh.</value>
        public float MinEdgeLength { get { return minEdgeLength; } }

        /// <value>Gets the scale used for drawing.</value>
        public float Scale { get { return scale; } }

        /// <value>Gets the centroid of the object as a float array.</value>
        public double[] Center { get { return center; } }

        /// <value>Gets the limits of the minCurvature color scale displayed in the OGLControl.</value>
        public double MinColorScale { get { return minColorScale; } }

        /// <value>Gets the limits of the maxCurvature color scale displayed in the OGLControl.</value>
        public double MaxColorScale { get { return maxColorScale; } }

        /// <value>Gets the distance between two picking colors for the Vertices.</value>
        public int VertexColorDist { get { return vertexColorDist; } }

        /// <value>Gets the distance between two picking colors for the Edges.</value>
        public int EdgeColorDist { get { return edgeColorDist; } }

        /// <value>Gets the distance between two picking colors for the Triangles.</value>
        public int TriangleColorDist { get { return triangleColorDist; } }

        #endregion

        #region Methods

        #region Initialization

        /// <summary>
        /// Clears Neighborhoods etc. for reinitializing a TriangleMesh.
        /// </summary>
        public void ClearRelations() {
            minEdgeLength = float.PositiveInfinity;
            edges.Clear();
            for (int i = 0; i < this.Count; i++) { this[i].Edges.Clear(); }
            for (int i = 0; i < this.vertices.Count; i++) {
                vertices[i].Neighborhood.Clear();
                vertices[i].Triangles.Clear();
            }
        }

        /// <summary>
        /// Fills the intial arrays, sets the extrema, the center and the scale.
        /// Sets the adjacent Triangles and the Neighborhood of each Vertex.
        /// Fills the Edge list. Calculates the Triangle normals.
        /// Sets the local coordinate system and the rotation matrix for each Triangle.
        /// Determines the area nearest each Vertex of a Triangle and the area of the entire Triangle.
        /// Calculates the angles of the Triangles.
        /// </summary>
        /// <param name="triNormals">If true, the Triangle normals and the centroid are calculated.
        /// If False, they are not calculated.</param>
        public void Finish(bool triNormals) {
            ClearRelations();

            Edge edge;
            double edgeLength;
            float[] min = new float[3] { 0.0f, 0.0f, 0.0f };
            float[] max = new float[3] { 0.0f, 0.0f, 0.0f };

            // Calculates the minimum and maximum coordinates of every Triangle.
            for (int i = 0; i < this.Count; i++) {
                for (int j = 0; j < 3; j++) {
                    if (this[i, 1][j] <= this[i, 0][j]) {
                        this[i].Min[j] = (float)this[i, 1][j];
                        this[i].Max[j] = (float)this[i, 0][j];
                    } else {
                        this[i].Min[j] = (float)this[i, 0][j];
                        this[i].Max[j] = (float)this[i, 1][j];
                    }

                    if (this[i, 2][j] < this[i].Min[j]) { this[i].Min[j] = (float)this[i, 2][j]; }
                    if (this[i, 2][j] > this[i].Max[j]) { this[i].Max[j] = (float)this[i, 2][j]; }
                }
            }

            // Initializes maximum and minimum with the values of the first Triangle.
            for (int h = 0; h < 3; h++) {
                if (this.Count > 0) {
                    min[h] = this[0].Min[h];
                    max[h] = this[0].Max[h];
                } else {
                    min[h] = 0;
                    max[h] = 1;
                }
            }

            for (int i = 0; i < this.Count; i++) {
                if (triNormals) {
                    // Sets the Centroid for each Triangle.
                    this[i].Centroid = ((vertices[this[i][0]] + vertices[this[i][1]] + vertices[this[i][2]]) / 3).ToVertex();
                    // Sets the Normal for each Triangle.
                    this[i].Centroid.Normal = Triangle.GetNormalOf(this[i, 0], this[i, 1], this[i, 2]);
                }

                for (int j = 0; j < 3; j++) {
                    if (this[i].Min[j] < min[j]) { min[j] = this[i].Min[j]; }
                    if (this[i].Max[j] > max[j]) { max[j] = this[i].Max[j]; }

                    // Sets the adjacent Triangles.
                    this[i, j].AddAdjacentTriangle(i);
                    // Sets the Neighborhood.
                    this[i, j].Neighborhood.AddNeighbors(this[i][(j + 1) % 3], this[i][(j + 2) % 3]);
                    // Fills the Edge list.
                    edgeLength = Vertex.Distance(this[i, j], this[i, (j + 1) % 3]);
                    edge = new Edge(this[i][j], this[i][(j + 1) % 3], edgeLength);
                    this[i].Edges.Add(edge);
                    if (!edges.ContainsKey(edge.Key)) {
                        edge.Triangles.Add(i);
                        edges.Add(edge.Key, edge);
                        minEdgeLength = (float)(edgeLength < minEdgeLength ? edgeLength : minEdgeLength);
                    } else {
                        edges[edge.Key].Triangles.Add(i);
                    }
                }

                // Sets the areas closest to each corner of a Triangle.
                // This was adapted from Szymon Rusinkiewicz C++ library trimesh2 (http://www.cs.princeton.edu/gfx/proj/trimesh2/).
                VectorND[] e = new VectorND[3] { this[i, 2] - this[i, 1], this[i, 0] - this[i, 2], this[i, 1] - this[i, 0] };
                double area = 0.5 * (e[0] % e[1]).Length();
                double[] l2 = new double[3] { e[0].SquaredLength(), e[1].SquaredLength(), e[2].SquaredLength() };
                double[] ew = new double[3] { l2[0] * (l2[1] + l2[2] - l2[0]), l2[1] * (l2[2] + l2[0] - l2[1]), l2[2] * (l2[0] + l2[1] - l2[2]) };

                if (ew[0] <= 0.0f) {
                    this[i].CornerAreas[1] = -0.25 * l2[2] * area / (e[0].Dot(e[2]));
                    this[i].CornerAreas[2] = -0.25 * l2[1] * area / (e[0].Dot(e[1]));
                    this[i].CornerAreas[0] = area - this[i].CornerAreas[1] - this[i].CornerAreas[2];
                } else if (ew[1] <= 0.0f) {
                    this[i].CornerAreas[2] = -0.25 * l2[0] * area / (e[1].Dot(e[0]));
                    this[i].CornerAreas[0] = -0.25 * l2[2] * area / (e[1].Dot(e[2]));
                    this[i].CornerAreas[1] = area - this[i].CornerAreas[2] - this[i].CornerAreas[0];
                } else if (ew[2] <= 0.0f) {
                    this[i].CornerAreas[0] = -0.25 * l2[1] * area / (e[2].Dot(e[1]));
                    this[i].CornerAreas[1] = -0.25 * l2[0] * area / (e[2].Dot(e[0]));
                    this[i].CornerAreas[2] = area - this[i].CornerAreas[0] - this[i].CornerAreas[1];
                } else {
                    double ewscale = 0.5 * area / (ew[0] + ew[1] + ew[2]);
                    for (int j = 0; j < 3; j++) {
                        this[i].CornerAreas[j] = ewscale * (ew[(j + 1) % 3] + ew[(j + 2) % 3]);
                    }
                }
                // The Voronoi area of a Vertex is caclulated by summing up the calculated corner areas.
                this[i, 0].Area += this[i].CornerAreas[0];
                this[i, 1].Area += this[i].CornerAreas[1];
                this[i, 2].Area += this[i].CornerAreas[2];

                // Calculates the area for each Triangle.
                double a = this[i, 0].DistanceFrom(this[i, 1]);
                double b = this[i, 1].DistanceFrom(this[i, 2]);
                double c = this[i, 2].DistanceFrom(this[i, 0]);
                double s = 0.5 * (a + b + c);
                this[i].Area = Math.Sqrt(s * (s - a) * (s - b) * (s - c));

                // Calculates the angles of each Triangle.
                VectorND ab = this[i, 1] - this[i, 0];
                VectorND ac = this[i, 2] - this[i, 0];
                VectorND ba = this[i, 0] - this[i, 1];
                VectorND bc = this[i, 2] - this[i, 1];
                VectorND cb = this[i, 1] - this[i, 2];
                VectorND ca = this[i, 0] - this[i, 2];

                this[i].Angles[0] = Vertex.Angle(ab, ac);
                this[i].Angles[1] = Vertex.Angle(ba, bc);
                this[i].Angles[2] = Vertex.Angle(cb, ca);
            }

            // The center is calculated.
            for (int i = 0; i < 3; i++) { center[i] = 0.5 * (max[i] + min[i]); }

            // The scale is the maximum size in one coordinate direction. It is used for drawing.
            scale = max[0] - min[0];
            if (max[1] - min[1] > scale) { scale = max[1] - min[1]; }
            if (max[2] - min[2] > scale) { scale = max[2] - min[2]; }

            // The colorDist is used to space the picking colors.
            int temp = 256 * 256 * 256;
            vertexColorDist = temp / (vertices.Count + 2);
            edgeColorDist = temp / (edges.Count + 2);
            triangleColorDist = temp / (this.Count + 2);

            if (this.Count == 0) { minEdgeLength = 1; }
        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Checks whether the Vertices really form a Triangle or are alligned.
        /// </summary>
        /// <param name="tri">Index of the Triangle to be checked.</param>
        /// <returns>True, if the Vertices form a Triangle.</returns>
        public bool IsTriangle(int tri) {
            VectorND ab = this[tri, 1] - this[tri, 0];
            VectorND ac = this[tri, 2] - this[tri, 0];
            double angle = VectorND.Angle(ab, ac);

            if ((ab.Length() == 0) || (ac.Length() == 0) || (angle < Math.PI / 360) || (angle > 359 * Math.PI / 360)) {
                return false;
            } else { return true; }
        }

        /// <summary>
        /// Checks whether the Vertices really form a Triangle or are alligned.
        /// </summary>
        /// <param name="v0">Index of the first Vertex.</param>
        /// <param name="v1">Index of the second Vertex.</param>
        /// <param name="v2">Index of the third Vertex.</param>
        /// <returns>True, if the vertices form a triangle</returns>
        public bool IsTriangle(int v0, int v1, int v2) {
            VectorND ab = this.vertices[v1] - this.vertices[v0];
            VectorND ac = this.vertices[v2] - this.vertices[v0];
            double angle = VectorND.Angle(ab, ac);

            if ((ab.Length() == 0) || (ac.Length() == 0) || (angle < Math.PI / 360) || (angle > 359 * Math.PI / 360)) {
                return false;
            } else { return true; }
        }

        /// <summary>
        /// Checks whether the Vertices really form a Triangle or are alligned.
        /// </summary>
        /// <param name="v0">The first Vertex.</param>
        /// <param name="v1">The second Vertex.</param>
        /// <param name="v2">Third Vertex.</param>
        /// <returns>True, if the vertices form a triangle</returns>
        public bool IsTriangle(Vertex v0, Vertex v1, Vertex v2) {
            VectorND ab = v1 - v0;
            VectorND ac = v2 - v0;
            double angle = VectorND.Angle(ab, ac);

            if ((ab.Length() == 0) || (ac.Length() == 0) || (angle < Math.PI / 360) || (angle > 359 * Math.PI / 360)) {
                return false;
            } else { return true; }
        }

        /// <summary>
        /// Changes the orientation of the Triangle given by the index <paramref name="triangle"/>
        /// and refreshes the TriangleMesh.
        /// </summary>
        /// <param name="triangle">Index of the Triangle to be flipped.</param>
        public void FlipTriangle(int triangle) {
            this[triangle] = new Triangle(this[triangle][2], this[triangle][1], this[triangle][0]);
            Finish(true);
        }

        /// <summary>
        /// Changes the orientation of all Triangles and refreshes the TriangleMesh.
        /// </summary>
        public void FlipAllTriangles() {
            for (int i = 0; i < this.Count; i++) { this[i] = new Triangle(this[i][2], this[i][1], this[i][0]); }
            Finish(true);
        }

        /// <summary>
        /// Subdivides the Triangle given by the index <paramref name="triangle"/>
        /// by connecting the midedge Vertices and refreshes the TriangleMesh.
        /// </summary>
        /// <param name="triangle"></param>
        public void SubdivideTriangle(int triangle) {
            Triangle tri = this[triangle];
            this.RemoveAt(triangle);

            int[] indices = new int[6];
            indices[0] = tri[0];
            vertices.Add(((vertices[tri[0]] + vertices[tri[1]]) / 2).ToVertex());
            indices[1] = vertices.Count - 1;
            indices[2] = tri[1];
            vertices.Add(((vertices[tri[1]] + vertices[tri[2]]) / 2).ToVertex());
            indices[3] = vertices.Count - 1;
            indices[4] = tri[2];
            vertices.Add(((vertices[tri[2]] + vertices[tri[0]]) / 2).ToVertex());
            indices[5] = vertices.Count - 1;

            this.Add(new Triangle(indices[0], indices[1], indices[5]));
            this.Add(new Triangle(indices[1], indices[2], indices[3]));
            this.Add(new Triangle(indices[1], indices[3], indices[5]));
            this.Add(new Triangle(indices[3], indices[4], indices[5]));

            this.Finish(true);
        }

        /// <summary>
        /// Creates a copy of the TriangleMesh and returns it.
        /// </summary>
        /// <returns>A Copy of this TriangleMesh</returns>
        public TriangleMesh Copy() {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, this);
            stream.Position = 0;
            object clone = formatter.Deserialize(stream);
            return clone as TriangleMesh;
        }

        #endregion

        #region Array Getters

        /// <summary>
        /// Gets the Triangles in an array.
        /// </summary>
        /// <returns>The Triangles in an array.</returns>
        public double[] GetTriangleArray() {
            List<double> triangleList = new List<double>(9 * this.Count);

            for (int i = 0; i < this.Count; i++) { for (int j = 0; j < 3; j++) { triangleList.AddRange(this[i, j]); } }

            return triangleList.ToArray();
        }

        /// <summary>
        /// Gets the facet (Triangle) normals as an array expanded to the Vertices of the mesh.
        /// </summary>
        /// <returns>The facet normals as an array.</returns>
        public double[] GetNormalArray() {
            List<double> normalList = new List<double>(9 * this.Count);

            for (int i = 0; i < this.Count; i++) { for (int j = 0; j < 3; j++) { normalList.AddRange(this[i].Normal); } }

            return normalList.ToArray();
        }

        /// <summary>
        /// Gets the Vertex normals as an array.
        /// </summary>
        /// <returns>The Vertex normals as an array.</returns>
        public double[] GetSmoothNormalArray() {
            List<double> smoothNormalList = new List<double>(9 * this.Count);

            for (int i = 0; i < this.Count; i++) { for (int j = 0; j < 3; j++) { smoothNormalList.AddRange(this[i, j].Normal); } }

            return smoothNormalList.ToArray();
        }

        /// <summary>
        /// Gets the EdgeArray for drawing the mesh in the OGLControl.
        /// The components of the two Vertices of each Edge are stored consecutively
        /// and used to draw a line between them in the OGLControl.
        /// </summary>
        /// <returns>The array of the Edges</returns>
        public double[] GetEdgeArray() {
            List<double> asList = new List<double>(3 * edges.Count);

            for (int i = 0; i < edges.Count; i++) {
                asList.AddRange(vertices[edges.Values[i].Vertices[0]]);
                asList.AddRange(vertices[edges.Values[i].Vertices[1]]);
            }

            return asList.ToArray();
        }


        /// <summary>
        /// Gets the array for drawing the Vertices.
        /// </summary>
        /// <returns>The array of the Vertices</returns>
        public double[] GetVertexArray() {
            List<double> asList = new List<double>(3 * vertices.Count);

            for (int i = 0; i < vertices.Count; i++) { asList.AddRange(vertices[i]); }

            return asList.ToArray();
        }

        /// <summary>
        /// Gets the array for drawing the facet (Triangle) normals as lines.
        /// </summary>
        /// <returns>The array of the endpoints of the facet (Triangle) normals</returns>
        public double[] GetFacetNormalVectorArray() {
            List<double> faceNormalVectorList = new List<double>(this.Count * 6);
            VectorND temp;

            for (int i = 0; i < this.Count; i++) {
                temp = this[i].Centroid + (this.Scale / 50) * this[i].Normal;
                faceNormalVectorList.AddRange(this[i].Centroid);
                faceNormalVectorList.AddRange(temp);
            }

            return faceNormalVectorList.ToArray();
        }

        /// <summary>
        /// Gets the array for drawing the Vertex normals as lines.
        /// </summary>
        /// <returns>The array of the endpoints of the Vertex normals</returns>
        public double[] GetVertexNormalVectorArray() {
            List<double> vertexNormalVectorList = new List<double>(vertices.Count * 6);
            VectorND temp;

            for (int i = 0; i < vertices.Count; i++) {
                temp = vertices[i] + (this.Scale / 50) * vertices[i].Normal;
                vertexNormalVectorList.AddRange(vertices[i]);
                vertexNormalVectorList.AddRange(temp);
            }

            return vertexNormalVectorList.ToArray();
        }

        /// <summary>
        /// Gets an array to draw the mesh with one Triangle marked in a different color.
        /// </summary>
        /// <param name="index">Index of the Triangle to be colored as selected.</param>
        /// <param name="all">Color of the unselected Triangles.</param>
        /// <param name="selected">Color of the selected Triangles.</param>
        /// <returns>A color array to draw the TriangleMesh with one marked Triangle.</returns>
        public float[] GetMarkedTriangleColorArray(int index, ColorOGL all, ColorOGL selected) {
            float[] colors = new float[this.Count * 9];
            for (int i = 0; i < this.Count; i++) {
                if (i == index) {
                    colors[i * 9] = colors[i * 9 + 3] = colors[i * 9 + 6] = selected.R;
                    colors[i * 9 + 1] = colors[i * 9 + 4] = colors[i * 9 + 7] = selected.G;
                    colors[i * 9 + 2] = colors[i * 9 + 5] = colors[i * 9 + 8] = selected.B;
                } else {
                    colors[i * 9] = colors[i * 9 + 3] = colors[i * 9 + 6] = all.R;
                    colors[i * 9 + 1] = colors[i * 9 + 4] = colors[i * 9 + 7] = all.G;
                    colors[i * 9 + 2] = colors[i * 9 + 5] = colors[i * 9 + 8] = all.B;
                }
            }

            return colors;
        }

        /// <summary>
        /// Gets an array containing a different color for each Vertex for picking.
        /// </summary>
        /// <returns>Picking colors array.</returns>
        public float[] GetVertexPickingColors() {
            ColorOGL color;
            List<float> pickingColors = new List<float>(vertices.Count * 3);

            for (int i = 0; i < vertices.Count; i++) {
                color = ColorOGL.GetColorFromInt(i * vertexColorDist);
                pickingColors.AddRange(color.RGB);
            }

            return pickingColors.ToArray();
        }

        /// <summary>
        /// Gets an array containing a different color for each Edge for picking.
        /// </summary>
        /// <returns>Picking colors array.</returns>
        public float[] GetEdgePickingColors() {
            ColorOGL color;
            List<float> pickingColors = new List<float>(edges.Count * 3);

            for (int i = 0; i < edges.Count; i++) {
                color = ColorOGL.GetColorFromInt(i * edgeColorDist);
                pickingColors.AddRange(color.RGB);
            }

            return pickingColors.ToArray();
        }

        /// <summary>
        /// Gets an array containing, in this order, the rotation angle, the x value of the rotation axis,
        /// the y value of the roation axis and the length of the Edge to be drawn.
        /// </summary>
        /// <returns></returns>
        public double[] GetEdgePickingArray() {
            List<double> list = new List<double>(edges.Count * 4);
            VectorND axis;
            double angle, length;

            for (int i = 0; i < edges.Count; i++) {
                axis = vertices[edges.Values[i].Vertices[1]] - vertices[edges.Values[i].Vertices[0]];
                length = axis.Length();

                // Rotation Angle
                if (Math.Abs(axis[2]) < 0.000000001) {
                    angle = 180 / Math.PI * Math.Acos(axis[0] / length);
                    if (axis[1] <= 0) { angle = -angle; }

                } else {
                    angle = 180 / Math.PI * Math.Acos(axis[2] / length);
                    if (axis[2] <= 0) { angle = -angle; }
                }
                list.Add(angle);

                // Rotation Axis
                list.Add(-axis[1] * axis[2]);
                list.Add(axis[0] * axis[2]);

                // Length of the Edge to be drawn.
                list.Add(length);
            }

            return list.ToArray();
        }

        /// <summary>
        /// Gets an array containing a different color for each Vertex for picking.
        /// </summary>
        /// <returns>Picking colors array.</returns>
        public float[] GetTrianglePickingColors() {
            ColorOGL color;
            List<float> pickingColors = new List<float>(this.Count * 3);

            for (int i = 0; i < this.Count; i++) {
                color = ColorOGL.GetColorFromInt(i * triangleColorDist);
                pickingColors.AddRange(color.RGB);
                pickingColors.AddRange(color.RGB);
                pickingColors.AddRange(color.RGB);
            }

            return pickingColors.ToArray();
        }


        #endregion

        #region Static

        /// <summary>
        /// Subdivides the TriangleMesh given by <paramref name="mesh"/> <paramref name="steps"/> times.
        /// Each Triangle is subdivided into four Triangles using the midedge Vertices.
        /// </summary>
        /// <param name="mesh">The TriangleMesh to be subdivided.</param>
        /// <param name="steps">The number of subdivision steps.</param>
        /// <returns>The subdivided TriangleMesh</returns>
        public static TriangleMesh Subdivide(TriangleMesh mesh, int steps) {
            TriangleMesh newMesh = new TriangleMesh();
            List<Triangle> oldTriangles = new List<Triangle>(mesh.ToArray());
            List<Vertex> oldVertices = new List<Vertex>(mesh.Vertices.ToArray());
            List<Triangle> newTriangles = new List<Triangle>(oldTriangles.Count * 4);
            List<Vertex> newVertices = new List<Vertex>();

            for (int i = 0; i < steps; i++) {
                for (int j = 0; j < oldTriangles.Count; j++) {
                    int[] indices = new int[6];
                    List<Vertex> triVertices = new List<Vertex>(6);
                    triVertices.Add(oldVertices[oldTriangles[j][0]].Copy().ToVertex());
                    triVertices.Add(((oldVertices[oldTriangles[j][0]] + oldVertices[oldTriangles[j][1]]) / 2).ToVertex());
                    triVertices.Add(oldVertices[oldTriangles[j][1]].Copy().ToVertex());
                    triVertices.Add(((oldVertices[oldTriangles[j][1]] + oldVertices[oldTriangles[j][2]]) / 2).ToVertex());
                    triVertices.Add(oldVertices[oldTriangles[j][2]].Copy().ToVertex());
                    triVertices.Add(((oldVertices[oldTriangles[j][2]] + oldVertices[oldTriangles[j][0]]) / 2).ToVertex());

                    for (int k = 0; k < 6; k++) {
                        int index = newVertices.IndexOf(triVertices[k]);
                        if (index != -1) {
                            indices[k] = index;
                        } else {
                            indices[k] = newVertices.Count;
                            newVertices.Add(triVertices[k]);
                        }
                    }

                    newTriangles.Add(new Triangle(indices[0], indices[1], indices[5]));
                    newTriangles.Add(new Triangle(indices[1], indices[2], indices[3]));
                    newTriangles.Add(new Triangle(indices[1], indices[3], indices[5]));
                    newTriangles.Add(new Triangle(indices[3], indices[4], indices[5]));
                }

                oldTriangles = new List<Triangle>(newTriangles.ToArray());
                oldVertices = new List<Vertex>(newVertices.ToArray());
                newTriangles = new List<Triangle>(oldTriangles.Count * 4);
                newVertices = new List<Vertex>();
            }

            newMesh.AddRange(oldTriangles.ToArray());
            newMesh.Vertices.AddRange(oldVertices.ToArray());
            newMesh.Finish(true);
            return newMesh;
        }

        #endregion

        #endregion

    }
}
