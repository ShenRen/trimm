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
using System.Linq;
using System.Collections.Generic;


namespace TriMM {
    [Serializable()]

    /// <summary>
    /// A Triangle stores the indices of its Vertices, its Edges, the angles at the Vertices,
    /// its centroid (containing the normal vector), its area, 
    /// the Voronoi-areas of the Vertices within the Triangle and its extrema.
    /// </summary>
    public class Triangle {

        #region Fields

        private List<int> vertices = new List<int>(3);
        private List<Edge> edges = new List<Edge>(3);
        private Vertex centroid;
        private double area;
        private double[] cornerAreas = new double[3];
        private double[] angles = new double[3];
        private float[] max = new float[3] { 0.0f, 0.0f, 0.0f };
        private float[] min = new float[3] { 0.0f, 0.0f, 0.0f };

        #endregion

        #region Properties

        /// <value>
        /// Gets the index of the Vertex at the corner of the Triangle given by <paramref name="index"/>.
        /// </value>
        /// <param name="index">The corner of the Triangle.</param>
        /// <returns>The index of the desired Vertex.</returns>
        public int this[int index] { get { return vertices[index]; } }

        /// <value>Gets the list of indices of the Vertices.</value>
        public List<int> Vertices { get { return vertices; } }

        /// <value>Gets the list of Edges or set it.</value>
        public List<Edge> Edges { get { return edges; } set { edges = value; } }

        /// <value>Gets the normal vector or set it.</value>
        public VectorND Normal { get { return centroid.Normal; } set { centroid.Normal = value; } }

        /// <value>Gets the centroid or sets it.</value>
        public Vertex Centroid { get { return centroid; } set { centroid = value; } }

        /// <value>Gets the area or sets it.</value>
        public double Area { get { return area; } set { area = value; } }

        /// <value>Gets the areas closest to each corner or sets them.</value>
        public double[] CornerAreas { get { return cornerAreas; } set { cornerAreas = value; } }

        /// <value>Gets the angles at each corner or sets them.</value>
        public double[] Angles { get { return angles; } set { angles = value; } }

        /// <value>Gets the maximum coordinates.</value>
        public float[] Max { get { return max; } set { min = value; } }

        /// <value>Gets the minimum coordinates.</value>
        public float[] Min { get { return min; } set { max = value; } }

        #endregion

        #region Constructors

        /// <summary>
        /// This constructor creates a new Triangle from the given Vertices.
        /// </summary>
        /// <param name="a">The index of the first Vertex</param>
        /// <param name="b">The index of the second Vertex</param>
        /// <param name="c">The index of the third Vertex</param>
        public Triangle(int a, int b, int c) {
            this.vertices.Add(a);
            this.vertices.Add(b);
            this.vertices.Add(c);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the index of the Vertex given by <paramref name="index"/>.
        /// </summary>
        /// <param name="index">Index of the Vertex in the VertexList of the TriangleMesh.</param>
        /// <returns>Index of the Vertex in the Triangle.</returns>
        public int IndexOf(int index) { return vertices.IndexOf(index); }

        /// <summary>
        /// Returns true, if this Triangle contains the Vertex with index <paramref name="index"/>.
        /// </summary>
        /// <param name="index">Index of the Vertex</param>
        /// <returns>True, if the Vertex is in this Triangle</returns>
        public bool Contains(int index) { if (vertices.Contains(index)) { return true; } else { return false; } }

        /// <summary>
        /// Replaces the index of the corner given by <paramref name="oldIndex"/> by <paramref name="newIndex"/>.
        /// </summary>
        /// <param name="oldIndex">old index</param>
        /// <param name="newIndex">new index</param>
        public void Replace(int oldIndex, int newIndex) { vertices[vertices.IndexOf(oldIndex)] = newIndex; }

        /// <summary>
        /// Returns an array containing the neighbors of the Vertex given by <paramref name="vertex"/>
        /// in counter-clockwise order.
        /// </summary>
        /// <param name="vertex">Index of the Vertex.</param>
        /// <returns>The neighbors.</returns>
        public int[] GetNeighborsOf(int vertex) {
            return new int[] { vertices[(vertices.IndexOf(vertex) + 1) % 3], vertices[(vertices.IndexOf(vertex) + 2) % 3] };
        }

        /// <summary>
        /// Returns the angle at the Vertex given by the index <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">Index of the Vertex.</param>
        /// <returns>The angle at that Vertex</returns>
        public double GetAngleAt(int vertex) { return angles[vertices.IndexOf(vertex)]; }

        /// <summary>
        /// Returns the angle at the Vertex opposite the Edge defined
        /// by <paramref name="vertex1"/> and <paramref name="vertex2"/>.
        /// </summary>
        /// <param name="vertex1">Index of the first Vertex of the Edge</param>
        /// <param name="vertex2">Index of the second Vertex of the Edge</param>
        /// <returns>The angle opposite that Edge</returns>
        public double GetOppositeAngle(int vertex1, int vertex2) {
            for (int i = 0; i < 3; i++) {
                if ((vertices[i] != vertex1) && (vertices[i] != vertex2)) {
                    return GetAngleAt(vertices[i]);
                }
            }
            return 0;
        }

        /// <summary>
        /// Returns an array containing the angles opposite the Vertex given by <paramref name="vertex"/>
        /// in counter-clockwise order.
        /// </summary>
        /// <param name="vertex">Index of the Vertex.</param>
        /// <returns>The opposite angles.</returns>
        public double[] GetOppositeAngles(int vertex) {
            return new double[2] { angles[(vertices.IndexOf(vertex) + 1) % 3], angles[(vertices.IndexOf(vertex) + 2) % 3] };
        }

        /// <summary>
        /// Returns the area nearest to the Vertex given by the index <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">Index of the Vertex.</param>
        /// <returns>The angle at that Vertex</returns>
        public double GetCornerAreaAt(int vertex) { return cornerAreas[vertices.IndexOf(vertex)]; }

        /// <summary>
        /// Returns the index of the Vertex opposite the Edge defined
        /// by <paramref name="vertex1"/> and <paramref name="vertex2"/>.
        /// </summary>
        /// <param name="vertex1">Index of the first Vertex of the Edge</param>
        /// <param name="vertex2">Index of the second Vertex of the Edge</param>
        /// <returns>The index of the Vertex opposite that Edge</returns>
        public int GetOppositeCorner(int vertex1, int vertex2) {
            for (int i = 0; i < 3; i++) {
                if ((vertices[i] != vertex1) && (vertices[i] != vertex2)) {
                    return vertices[i];
                }
            }
            return -1;
        }

        /// <summary>
        /// Returns the index of the Vertex opposite the given Edge.
        /// </summary>
        /// <param name="e">The Edge whose opposite corner is wanted.</param>
        /// <returns>The index of the Vertex opposite that Edge.</returns>
        public int GetOppositeCorner(Edge e) {
            for (int i = 0; i < 3; i++) {
                if ((vertices[i] != e.Vertices[0]) && (vertices[i] != e.Vertices[1])) {
                    return vertices[i];
                }
            }
            return -1;
        }

        /// <summary>
        /// Compares two Triangles regardless of orientation.
        /// </summary>
        /// <param name="obj">A second Triangle</param>
        /// <returns>True, if the Triangles consist of the same Vertices.</returns>
        public override bool Equals(object obj) {
            if (obj is Triangle) {
                Triangle other = obj as Triangle;
                if (this.vertices.Contains(other.vertices[0]) && this.vertices.Contains(other.vertices[1]) && this.vertices.Contains(other.vertices[2])) {
                    return true;
                } else{ return false;}
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Just so Visual Studio doesn't complain.
        /// </summary>
        /// <returns>base.GetHashCode()</returns>
        public override int GetHashCode() { return base.GetHashCode(); }

        #region Static

        /// <summary>
        /// Calculates the area of the Triangle formed by the three given points.
        /// The points shouldn't be colinear.
        /// </summary>
        /// <param name="A">Corner of the Triangle</param>
        /// <param name="B">Corner of the Triangle</param>
        /// <param name="C">Corner of the Triangle</param>
        /// <returns>Area of the Triangle</returns>
        public static double GetAreaOf(VectorND A, VectorND B, VectorND C) {
            VectorND cb = B - C;
            VectorND ca = A - C;

            double a = cb.Length();
            double b = ca.Length();

            return 0.5 * a * b * Math.Sin(Vertex.Angle(cb, ca));
        }

        /// <summary>
        /// Calculates the normal vector of the Triangle given by the three VectorNDs
        /// <paramref name="a"/>, <paramref name="b"/> and <paramref name="c"/>
        /// </summary>
        /// <param name="a">VectorND one.</param>
        /// <param name="b">VectorND two.</param>
        /// <param name="c">VectorND three.</param>
        /// <returns>The normal vector of the given Triangle</returns>
        public static VectorND GetNormalOf(VectorND a, VectorND b, VectorND c) {
            VectorND v1 = b - a;
            VectorND v2 = c - a;

            VectorND normal = v1 % v2;
            normal.Normalize();

            return normal;
        }

        /// <summary>
        /// Calculates the centroid of the Triangle formed by the three given points.
        /// The points shouldn't be colinear.
        /// </summary>
        /// <param name="A">Corner of the Triangle</param>
        /// <param name="B">Corner of the Triangle</param>
        /// <param name="C">Corner of the Triangle</param>
        /// <returns>Centroid of the Triangle</returns>
        public static Vertex GetCentroidOf(VectorND A, VectorND B, VectorND C) { return ((A + B + C) / 3).ToVertex(); }

        #endregion

        #endregion
    }
}
