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
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using TriMM.VertexNormalAlgorithms;


namespace TriMM {
    [Serializable()]

    #region EventHandler

    /// <summary>
    /// An EventHandler for the PickCleared Event. Alerts anyone who wants to know of the clearing of the selection.
    /// </summary>
    public delegate void PickClearedEventHandler();

    /// <summary>
    /// An EventHandler for the ScaleChanged Event. Alerts anyone who wants to know of changes to the scale.
    /// </summary>
    public delegate void ScaleChangedEventHandler();

    /// <summary>
    /// An EventHandler for the MinEdgeLengthChanged Event. Alerts anyone who wants to know of changes to the length of the shortest Edge.
    /// </summary>
    public delegate void MinEdgeLengthChangedEventHandler(double newLength);

    #endregion

    /// <summary>
    /// The main datastructure containing all informations about the triangle mesh.
    /// It is a list of Triangles, with an additional list of Vertices and a SortedList of Edges.
    /// Some values needed for drawing the mesh are also stored.
    /// </summary>
    public class TriangleMesh : List<Triangle> {

        #region Fields

        private List<Vertex> vertices = new List<Vertex>();
        private SortedList<decimal, Edge> edges = new SortedList<decimal, Edge>();

        private float scale;
        private double[] center = new double[3] { 0, 0, 0 };

        private int vertexColorDist;
        private int edgeColorDist;
        private int triangleColorDist;
        private double minColorScale = 1;
        private double maxColorScale = 1;

        private int observedVertex = -1;
        private int observedEdge = -1;
        private int observedTriangle = -1;

        private IVertexNormalAlgorithm vertexNormalAlgorithm;

        #region Drawing Arrays

        private float[] vertexPickingColors;
        private float[] edgePickingColors;
        private double[] edgePickingArray;
        private float[] trianglePickingColors;
        private float[] colorArray;
        private double[] triangleArray;
        private double[] edgeArray;
        private double[] vertexArray;
        private double[] normalArray;
        private double[] smoothNormalArray;
        private double[] facetNormalVectorArray;
        private double[] vertexNormalVectorArray;

        #endregion

        #region Events

        /// <value>The event thrown when all Vertices are unpicked.</value>
        public event PickClearedEventHandler PickCleared;

        /// <value>The event thrown when the scale is changed.</value>
        public event ScaleChangedEventHandler ScaleChanged;

        /// <value>The event thrown when the length of the shortest Edge is changed.</value>
        public event MinEdgeLengthChangedEventHandler MinEdgeLengthChanged;

        #endregion

        #endregion

        #region Properties

        /// <value>
        /// Gets the Vertex at Index <paramref name="index2"/> in the Triangle of Index  <paramref name="index1"/> .
        /// </value>
        /// <param name="index1">Triangle index</param>
        /// <param name="index2">Corner index</param>
        /// <returns><paramref name="index2"/> Vertex in Triangle <paramref name="index1"/>.</returns>
        public Vertex this[int index1, int index2] { get { return vertices[this[index1][index2]]; } }

        /// <value>Gets the list of all Vertices.</value>
        public List<Vertex> Vertices { get { return vertices; } set { vertices = value; } }

        /// <value>Gets the list of all Edges.</value>
        public SortedList<decimal, Edge> Edges { get { return edges; } }

        /// <value>
        /// Gets the length of the shortest Edge in this TriangleMesh.
        /// The Edges are sorted by length, so the first Edge always belongs to the set of shortest Edges.
        /// </value>
        public double MinEdgeLength { get { return edges.Values[0].Length; } }

        /// <value>Gets the scale used for drawing.</value>
        public float Scale { get { return scale; } }

        /// <value>Gets the centroid of the object as a float array.</value>
        public double[] Center { get { return center; } }

        /// <value>Gets the limits of the minCurvature color scale displayed in the TriMMControl.</value>
        public double MinColorScale { get { return minColorScale; } }

        /// <value>Gets the limits of the maxCurvature color scale displayed in the TriMMControl.</value>
        public double MaxColorScale { get { return maxColorScale; } }

        /// <value>Gets the distance between two picking colors for the Vertices.</value>
        public int VertexColorDist { get { return vertexColorDist; } }

        /// <value>Gets the distance between two picking colors for the Edges.</value>
        public int EdgeColorDist { get { return edgeColorDist; } }

        /// <value>Gets the distance between two picking colors for the Triangles.</value>
        public int TriangleColorDist { get { return triangleColorDist; } }

        /// <value>Gets the index of the observed Vertex or sets it.</value>
        public int ObservedVertex {
            get { return observedVertex; }
            set {
                observedVertex = value;
                if ((observedVertex == -1) && (PickCleared != null)) { PickCleared(); }
            }
        }

        /// <value>Gets the index of the observed Triangle or sets it.</value>
        public int ObservedTriangle { get { return observedTriangle; } set { observedTriangle = value; } }

        /// <value>Gets the index of the observed Edge or sets it.</value>
        public int ObservedEdge {
            get { return observedEdge; }
            set {
                observedEdge = value;
                if ((observedEdge == -1) && (PickCleared != null)) { PickCleared(); }
            }
        }

        /// <value>Sets the VertexNormalAlgorithm to be used.</value>
        public IVertexNormalAlgorithm VertexNormalAlgorithm { set { vertexNormalAlgorithm = value; } }

        #region Drawing Arrays

        /// <value>Gets the vertexPickingColors.</value>
        public float[] VertexPickingColors { get { return vertexPickingColors; } }

        /// <value>Gets the edgePickingColors.</value>
        public float[] EdgePickingColors { get { return edgePickingColors; } }

        /// <value>Gets the edgePickingArray.</value>
        public double[] EdgePickingArray { get { return edgePickingArray; } }

        /// <value>Gets the trianglePickingColors.</value>
        public float[] TrianglePickingColors { get { return trianglePickingColors; } }

        /// <value>Gets the colorArray.</value>
        public float[] ColorArray { get { return colorArray; } }

        /// <value>Gets the array of all Vertices of all Triangles.</value>
        public double[] TriangleArray { get { return triangleArray; } }

        /// <value>Gets the array of Edges</value>
        public double[] EdgeArray { get { return edgeArray; } }

        /// <value>Gets the array of Vertices.</value>
        public double[] VertexArray { get { return vertexArray; } }

        /// <value>Gets the array of normal vectors of all triangles expanded to all corners.</value>
        public double[] NormalArray { get { return normalArray; } }

        /// <value>Gets the array of normal vectors of all Vertices of all Triangles.</value>
        public double[] SmoothNormalArray { get { return smoothNormalArray; } }

        /// <value>Gets the array of normal vectors of all Triangles as lines.</value>
        public double[] FacetNormalVectorArray { get { return facetNormalVectorArray; } }

        /// <value>Gets the array of normal vectors of all Triangles as lines.</value>
        public double[] VertexNormalVectorArray { get { return vertexNormalVectorArray; } }

        #endregion

        #endregion

        #region Methods

        #region Initialization

        /// <summary>
        /// Clears Neighborhoods etc. for reinitializing a TriangleMesh.
        /// </summary>
        public void ClearRelations() {
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
        /// <param name="verNormals">If true, the Vertex normals are calculated with the chosen VertexNormalAlgorithm.</param>
        public void Finish(bool triNormals, bool verNormals) {
            double oldMinLength = 0;
            if (edges.Count != 0) { oldMinLength = edges.Values[0].Length; }
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
            if (edges.Count != 0) {
                if ((oldMinLength != edges.Values[0].Length) && (MinEdgeLengthChanged != null)) { MinEdgeLengthChanged(edges.Values[0].Length); }
            } else {
                MinEdgeLengthChanged(0);
            }

            // The center is calculated.
            for (int i = 0; i < 3; i++) { center[i] = 0.5 * (max[i] + min[i]); }

            // The scale is the maximum size in one coordinate direction. It is used for drawing.
            float oldScale = scale;
            scale = max[0] - min[0];
            if (max[1] - min[1] > scale) { scale = max[1] - min[1]; }
            if (max[2] - min[2] > scale) { scale = max[2] - min[2]; }
            if ((oldScale != scale) && (ScaleChanged != null)) { ScaleChanged(); }

            // The colorDist is used to space the picking colors.
            int temp = 256 * 256 * 256;
            vertexColorDist = temp / (vertices.Count + 2);
            edgeColorDist = temp / (edges.Count + 2);
            triangleColorDist = temp / (this.Count + 2);

            if (verNormals) { vertexNormalAlgorithm.GetVertexNormals(); }

            SetArrays();
        }

        #endregion

        #region Array Setters

        /// <summary>
        /// Sets all arrays used for drawing the modell.
        /// </summary>
        public void SetArrays() {
            SetTriangleArray();
            SetNormalArray();
            SetSmoothNormalArray();
            SetEdgeArray();
            SetVertexArray();
            SetFacetNormalVectorArray();
            SetVertexNormalVectorArray();
            SetVertexPickingColors();
            SetEdgePickingColors();
            SetEdgePickingArray();
            SetTrianglePickingColors();
        }

        /// <summary>
        /// Sets the Triangles in an array.
        /// </summary>
        public void SetTriangleArray() {
            List<double> triangleList = new List<double>(9 * this.Count);

            for (int i = 0; i < this.Count; i++) { for (int j = 0; j < 3; j++) { triangleList.AddRange(this[i, j]); } }

            triangleArray = triangleList.ToArray();
        }

        /// <summary>
        /// Sets the facet (Triangle) normals as an array expanded to the Vertices of the mesh.
        /// </summary>
        public void SetNormalArray() {
            List<double> normalList = new List<double>(9 * this.Count);

            for (int i = 0; i < this.Count; i++) { for (int j = 0; j < 3; j++) { normalList.AddRange(this[i].Normal); } }

            normalArray = normalList.ToArray();
        }

        /// <summary>
        /// Sets the Vertex normals as an array.
        /// </summary>
        public void SetSmoothNormalArray() {
            List<double> smoothNormalList = new List<double>(9 * this.Count);

            for (int i = 0; i < this.Count; i++) { for (int j = 0; j < 3; j++) { smoothNormalList.AddRange(this[i, j].Normal); } }

            smoothNormalArray = smoothNormalList.ToArray();
        }

        /// <summary>
        /// Gets the EdgeArray for drawing the mesh in the TriMMControl.
        /// The components of the two Vertices of each Edge are stored consecutively
        /// and used to draw a line between them in the TriMMControl.
        /// </summary>
        public void SetEdgeArray() {
            List<double> edgeList = new List<double>(6 * edges.Count);

            for (int i = 0; i < edges.Count; i++) {
                edgeList.AddRange(vertices[edges.Values[i].Vertices[0]]);
                edgeList.AddRange(vertices[edges.Values[i].Vertices[1]]);
            }

            edgeArray = edgeList.ToArray();
        }


        /// <summary>
        /// Sets the array for drawing the Vertices.
        /// </summary>
        public void SetVertexArray() {
            List<double> vertexList = new List<double>(3 * vertices.Count);

            for (int i = 0; i < vertices.Count; i++) { vertexList.AddRange(vertices[i]); }

            vertexArray = vertexList.ToArray();
        }

        /// <summary>
        /// Sets the array for drawing the facet (Triangle) normals as lines.
        /// </summary>
        public void SetFacetNormalVectorArray() {
            List<double> faceNormalVectorList = new List<double>(this.Count * 6);
            VectorND temp;

            for (int i = 0; i < this.Count; i++) {
                temp = this[i].Centroid + (this.Scale / 50) * this[i].Normal;
                faceNormalVectorList.AddRange(this[i].Centroid);
                faceNormalVectorList.AddRange(temp);
            }

            facetNormalVectorArray = faceNormalVectorList.ToArray();
        }

        /// <summary>
        /// Sets the array for drawing the Vertex normals as lines.
        /// </summary>
        public void SetVertexNormalVectorArray() {
            List<double> vertexNormalVectorList = new List<double>(vertices.Count * 6);
            VectorND temp;

            for (int i = 0; i < vertices.Count; i++) {
                temp = vertices[i] + (this.Scale / 50) * vertices[i].Normal;
                vertexNormalVectorList.AddRange(vertices[i]);
                vertexNormalVectorList.AddRange(temp);
            }

            vertexNormalVectorArray = vertexNormalVectorList.ToArray();
        }

        /// <summary>
        /// Sets an array to draw the mesh with one Triangle marked in a different color.
        /// </summary>
        /// <param name="index">Index of the Triangle to be colored as selected.</param>
        public void SetMarkedTriangleColorArray(int index) {
            colorArray = new float[this.Count * 9];
            for (int i = 0; i < this.Count; i++) {
                if (i == index) {
                    colorArray[i * 9] = colorArray[i * 9 + 3] = colorArray[i * 9 + 6] = TriMM.Settings.ObservedTriangleColor.R;
                    colorArray[i * 9 + 1] = colorArray[i * 9 + 4] = colorArray[i * 9 + 7] = TriMM.Settings.ObservedTriangleColor.G;
                    colorArray[i * 9 + 2] = colorArray[i * 9 + 5] = colorArray[i * 9 + 8] = TriMM.Settings.ObservedTriangleColor.B;
                } else {
                    colorArray[i * 9] = colorArray[i * 9 + 3] = colorArray[i * 9 + 6] = TriMM.Settings.PlainColor.R;
                    colorArray[i * 9 + 1] = colorArray[i * 9 + 4] = colorArray[i * 9 + 7] = TriMM.Settings.PlainColor.G;
                    colorArray[i * 9 + 2] = colorArray[i * 9 + 5] = colorArray[i * 9 + 8] = TriMM.Settings.PlainColor.B;
                }
            }
        }

        /// <summary>
        /// Sets an array containing a different color for each Vertex for picking.
        /// </summary>
        public void SetVertexPickingColors() {
            ColorOGL color;
            List<float> pickingColors = new List<float>(vertices.Count * 3);

            for (int i = 0; i < vertices.Count; i++) {
                color = ColorOGL.GetColorFromInt(i * vertexColorDist);
                pickingColors.AddRange(color.RGB);
            }

            vertexPickingColors = pickingColors.ToArray();
        }

        /// <summary>
        /// Sets an array containing a different color for each Edge for picking.
        /// </summary>
        public void SetEdgePickingColors() {
            ColorOGL color;
            List<float> pickingColors = new List<float>(edges.Count * 3);

            for (int i = 0; i < edges.Count; i++) {
                color = ColorOGL.GetColorFromInt(i * edgeColorDist);
                pickingColors.AddRange(color.RGB);
            }

            edgePickingColors = pickingColors.ToArray();
        }

        /// <summary>
        /// Sets an array containing, in this order, the rotation angle, the x value of the rotation axis,
        /// the y value of the roation axis and the length of the Edge to be drawn.
        /// </summary>
        public void SetEdgePickingArray() {
            List<double> edgePickingList = new List<double>(edges.Count * 4);
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
                edgePickingList.Add(angle);

                // Rotation Axis
                edgePickingList.Add(-axis[1] * axis[2]);
                edgePickingList.Add(axis[0] * axis[2]);

                // Length of the Edge to be drawn.
                edgePickingList.Add(length);
            }

            edgePickingArray = edgePickingList.ToArray();
        }

        /// <summary>
        /// Sets an array containing a different color for each Vertex for picking.
        /// </summary>
        public void SetTrianglePickingColors() {
            ColorOGL color;
            List<float> pickingColors = new List<float>(this.Count * 3);

            for (int i = 0; i < this.Count; i++) {
                color = ColorOGL.GetColorFromInt(i * triangleColorDist);
                pickingColors.AddRange(color.RGB);
                pickingColors.AddRange(color.RGB);
                pickingColors.AddRange(color.RGB);
            }

            trianglePickingColors = pickingColors.ToArray();
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
            Finish(true, true);
        }

        /// <summary>
        /// Changes the orientation of all Triangles and refreshes the TriangleMesh.
        /// </summary>
        public void FlipAllTriangles() {
            for (int i = 0; i < this.Count; i++) { this[i] = new Triangle(this[i][2], this[i][1], this[i][0]); }
            Finish(true, true);
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

            this.Finish(true, true);
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
            newMesh.Finish(true, true);
            return newMesh;
        }

        #endregion

        #endregion

    }
}
