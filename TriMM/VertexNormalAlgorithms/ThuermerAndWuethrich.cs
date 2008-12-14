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
    /// The algorithm introduced by G. Thürmer and C. Wüthrich in their article
    /// "Computing Vertex Normals from Polygonal Facets".
    /// The facet normals are weighted by the angle of the adjacent Triangle.
    /// </summary>
    public class ThuermerAndWuethrich : IVertexNormalAlgorithm {

        #region IVertexNormalAlgorithm Member

        #region Properties

        /// <value>Gets the name of this algorithm.</value>
        public string Name { get { return "Thuermer and Wuethrich"; } }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the Vertex normals as an average of the normals of the adjacent Triangles,
        /// weighted by the angle between the neighboring Vertices.
        /// </summary>
        public void GetVertexNormals() {
            for (int i = 0; i < TriMM.Mesh.Vertices.Count; i++) {
                Vertex vertex = TriMM.Mesh.Vertices[i];
                vertex.Normal = new VectorND(0, 0, 0);

                for (int j = 0; j < vertex.Triangles.Count; j++) {
                    Triangle triangle = TriMM.Mesh[vertex.Triangles[j]];
                    vertex.Normal += triangle.GetAngleAt(i) * triangle.Normal;
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
