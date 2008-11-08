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
    /// A variation of the algorithm introduced by G. Taubin in his article
    /// "Estimating The Tensor Of Curvature Of A Surface From A Polyhedral Approximation".
    /// This variation of the algorithm uses the inverse of the area of a Triangle as weight for the normal vector.
    /// </summary>
    internal class InverseTaubin : IVertexNormalAlgorithm {

        #region IVertexNormalAlgorithm Member

        #region Properties

        /// <value>Gets the name of this algorithm.</value>
        public string Name { get { return "Inverse Taubin"; } }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the Vertex normals as an average of the normals of the adjacent Triangles
        /// with weights proportional to the inverse of the area of the Triangle.
        /// </summary>
        /// <param name="mesh">The TriangleMesh to calculate the vertex normals for.</param>
        public void GetVertexNormals(ref TriangleMesh mesh) {
            for (int i = 0; i < mesh.Vertices.Count; i++) {
                Vertex vertex = mesh.Vertices[i];
                vertex.Normal = new VectorND(0, 0, 0);

                for (int j = 0; j < vertex.Triangles.Count; j++) { vertex.Normal += mesh[vertex.Triangles[j]].Normal / mesh[vertex.Triangles[j]].Area; }
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
