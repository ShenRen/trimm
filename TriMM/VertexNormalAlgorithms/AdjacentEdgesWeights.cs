// Part of TriMM, the TriangleMesh Manipulator
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

using System.Collections.Generic;

namespace TriMM.VertexNormalAlgorithms {

    /// <summary>
    /// This algorithm uses the average of the length of the Edges containing the current Vertex
    /// in the current adjacent Triangle as weights for the Triangle normal.
    /// </summary>
    public class AdjacentEdgesWeights : IVertexNormalAlgorithm {

        #region IVertexNormalAlgorithm Member

        #region Properties

        /// <value>Gets the name of this algorithm.</value>
        public string Name { get { return "AdjacentEdgesWeights"; } }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the Vertex normals as an average of the normals of the adjacent Triangles,
        /// weighted by the average of the length of the Edges containing the current Vertex
        /// in the current adjacent Triangle.
        /// </summary>
        public void GetVertexNormals() {
            for (int i = 0; i < TriMMApp.Mesh.Vertices.Count; i++) {
                Vertex vertex = TriMMApp.Mesh.Vertices[i];
                vertex.Normal = new VectorND(0, 0, 0);

                for (int j = 0; j < vertex.Triangles.Count; j++) {
                    Triangle triangle = TriMMApp.Mesh[vertex.Triangles[j]];
                    int[] neighbors = triangle.GetNeighborsOf(i);
                    double weight = (VectorND.Distance(TriMMApp.Mesh.Vertices[i], TriMMApp.Mesh.Vertices[neighbors[0]])
                        + VectorND.Distance(TriMMApp.Mesh.Vertices[i], TriMMApp.Mesh.Vertices[neighbors[1]])) / 2;
                    vertex.Normal += triangle.Normal * weight;
                }
                vertex.Normal.Normalize();
            }
        }

        #endregion

        #endregion

        #region Other Methods

        /// <summary>Overrides the ToString method to return the Name.</summary>
        /// <returns>The Name</returns>
        public override string ToString() { return Name; }

        #endregion
    }
}
