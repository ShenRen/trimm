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
    /// The algorithm introduced by S.-G. Chen and J.-Y. Wu in their article
    /// "Estimating normal vectors and curvatures by centroid weights".
    /// The weights are proportional to the inverse of the squared distance between
    /// the centroid of the adjacent Triangle and the current Vertex.
    /// This extended version uses the triangle area as an additional weight.
    /// </summary>
    public class ExtendedChenAndWu : IVertexNormalAlgorithm {

        #region IVertexNormalAlgorithm Member

        #region Properties

        /// <value>Gets the name of this algorithm.</value>
        public string Name { get { return "Extended Chen and Wu"; } }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the Vertex normals as an average of the normals of the adjacent Triangles,
        /// weighted by the inverse of the squared distance between the Vertex and the Centroid of the adjacent Triangle
        /// and the triangle area.
        /// </summary>
        public void GetVertexNormals() {
            for (int i = 0; i < TriMM.Mesh.Vertices.Count; i++) {
                Vertex vertex = TriMM.Mesh.Vertices[i];
                vertex.Normal = new VectorND(0, 0, 0);

                for (int j = 0; j < vertex.Triangles.Count; j++) {
                    Triangle triangle = TriMM.Mesh[vertex.Triangles[j]];
                    vertex.Normal += triangle.Normal * triangle.Area / VectorND.SquaredDistance(triangle.Centroid, vertex);
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
