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
using System.Collections.Generic;

namespace TriMM {
    [Serializable()]

    /// <summary>
    /// An Edge stores the indices of the two Vertices it connects, as well as a key.
    /// The indices of the adjacent Triangles and the length of the Edge are also stored.
    /// The key allows comparing two Edges and recognizing (a,b) as equal to (b,a).
    /// </summary>
    public class Edge {

        #region Fields

        private int[] vertices;
        private List<int> triangles = new List<int>();
        private decimal key;
        private double length;

        #endregion

        #region Properties

        /// <value>Gets the index of the Vertex at the given position of the Edge.</value>
        public int this[int index]{get { return vertices[index]; } }

        /// <value>Gets the two Vertices connected by this Edge.</value>
        public int[] Vertices { get { return vertices; } }

        /// <value>Gets the list of Triangles adjacent to this Edge or sets it.</value>
        public List<int> Triangles { get { return triangles; } set { triangles = value; } }

        /// <value>Gets the key of this Edge or sets it.</value>
        public decimal Key { get { return key; } set { key = value; } }

        /// <value>Gets the length of the Edge or sets it.</value>
        public double Length { get { return length; } set { length = value; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Sets the Array of Vertex-indices with the smaller index in the first position
        /// and sets the key as well. The length must also be passed to the constructor.
        /// The key is created from the length rounded to 12 decimals and the Vertices, ordered by their index,
        /// with a length set to seven digits.
        /// Will not work for meshes with more than seven digits, but meshes that size are problematic anyway.
        /// (Picking will not work for meshes too large for example).
        /// </summary>
        /// <param name="a">Index of the first Vertex</param>
        /// <param name="b">Index of the second Vertex</param>
        /// <param name="length">Length of the Edge</param>
        public Edge(int a, int b, double length) {
            this.length = length;
            if (a < b) { vertices = new int[2] { a, b }; } else { vertices = new int[2] { b, a }; }
            string one = "0.000000000000";
            for (int i = 0; i < 7 - vertices[0].ToString().Length; i++) { one += "0"; }
            one += vertices[0].ToString();
            string two = "0.0000000000000000000";
            for (int i = 0; i < 7 - vertices[1].ToString().Length; i++) { two += "0"; }
            two += vertices[1].ToString();
            key = decimal.Round((decimal)length, 12) + decimal.Parse(one) + decimal.Parse(two);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the index of one Vertex in this Edge when given the index of the other Vertex.
        /// </summary>
        /// <param name="vertex1">Index of one Vertex</param>
        /// <returns>Index of the other Vertex, -1 if <paramref name="vertex1"/> is not in this Edge.</returns>
        public int GetOtherVertex(int vertex1) {
            int vertex2 = -1;
            if (this.vertices[0] == vertex1) { return vertices[1]; } else if (this.vertices[1] == vertex1) { return vertices[0]; }
            return vertex2;
        }

        /// <summary>
        /// Checks if this Edge is connected to the Edge given by <paramref name="other"/>.
        /// </summary>
        /// <param name="other">Another Edge.</param>
        /// <returns>True, if the two Edges have a common Vertex.</returns>
        /// <remarks>Too slow.</remarks>
        public bool IncidentTo(Edge other) {
            if ((this.vertices[0] == other.Vertices[0]) || (this.vertices[0] == other.Vertices[1])
                || (this.vertices[1] == other.Vertices[0]) || (this.vertices[1] == other.Vertices[1])) { 
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Determines the key for the Edge given by two indices of Vertices.
        /// </summary>
        /// <param name="a">Index of the first Vertex.</param>
        /// <param name="b">Index of the second Vertex.</param>
        /// <param name="length">The distance between the Vertices.</param>
        /// <returns>Key that Edge would have.</returns>
        public static decimal GetKey(int a, int b, double length) {
            string one = "0.000000000000";
            string two = "0.0000000000000000000";
            if (a <= b) {
                for (int i = 0; i < 7 - a.ToString().Length; i++) { one += "0"; }
                one += a.ToString();
                for (int i = 0; i < 7 - b.ToString().Length; i++) { two += "0"; }
                two += b.ToString();
            } else {
                for (int i = 0; i < 7 - b.ToString().Length; i++) { one += "0"; }
                one += b.ToString();
                for (int i = 0; i < 7 - a.ToString().Length; i++) { two += "0"; }
                two += a.ToString();
            }
            return decimal.Round((decimal)length, 12) + decimal.Parse(one) + decimal.Parse(two);
        }

        /// <summary>
        /// Returns the Edge as a string.
        /// </summary>
        /// <returns>Edge as a string</returns>
        public override string ToString() { return "[" + vertices[0] + ", " + vertices[1] + "]"; }

        /// <summary>
        /// Returns true, if the Keys of this Edge and the Edge compared to it are equal.
        /// </summary>
        /// <param name="other">Another Edge</param>
        /// <returns>True, if the Edges are equal.</returns>
        public override bool Equals(object other) {
            if (other is Edge) {
                if (this.key == (other as Edge).key) { return true; } else { return false; }
            } else {
                return base.Equals(other);
            }
        }

        /// <summary>
        /// Just, so Visual Studio doesn't throw a warning.
        /// </summary>
        /// <returns>base.GetHashCode()</returns>
        public override int GetHashCode() { return base.GetHashCode(); }

        #endregion
    }
}
