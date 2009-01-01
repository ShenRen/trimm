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
    /// This algorithm is based on the weighting used by Szymon Rusinkiewicz in his paper
    /// "Estimating Curvatures and Their Derivatives on Triangle Meshes"
    /// published 2004 In "Symposium on 3D Data Processing, Visualization, and Transmission".
    /// Rusinkiewicz does not not use these weights for estimating Vertex normals, only for weighting
    /// the face curvatures. For the Vertex normals he uses the Max-algorithm.
    /// </summary>
    public class Rusinkiewicz : IVertexNormalAlgorithm {

        #region IVertexNormalAlgorithm Member

        #region Properties

        /// <value>Gets the name of this algorithm.</value>
        public string Name { get { return "Rusinkiewicz"; } }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the Vertex normals as an average of the normals of the adjacent Triangles,
        /// weighted by the area nearest to the Vertex.
        /// </summary>
        public void GetVertexNormals() {
            for (int i = 0; i < TriMMApp.Mesh.Vertices.Count; i++) {
                Vertex vertex = TriMMApp.Mesh.Vertices[i];
                vertex.Normal = new Vector(0, 0, 0);

                for (int j = 0; j < vertex.Triangles.Count; j++) {
                    int adj = vertex.Triangles[j];
                    Triangle triangle = TriMMApp.Mesh[adj];
                    vertex.Normal += triangle.Normal * TriMMApp.Mesh[adj].GetCornerAreaAt(i);
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
