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
    /// The algorithm suggested by H. Gouraud in his article
    /// "Continuous Shading of Curved Surfaces".
    /// The facet normals of the incident Triangles are all weighted equally.
    /// </summary>
    public class Gouraud : IVertexNormalAlgorithm{

        #region IVertexNormalAlgorithm Member

        #region Properties

        /// <value>Gets the name of this algorithm.</value>
        public string Name { get { return TriMMApp.Lang.GetElementsByTagName("G")[0].InnerText; } }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the Vertex normals as an average of the normals of the incident Triangles with equal weights.
        /// </summary>
        /// <param name="mesh">Reference to the TriangleMesh to calculate the vertex normals for.</param>
        public void GetVertexNormals(ref TriangleMesh mesh) {
            for (int i = 0; i < mesh.Vertices.Count; i++) {
                Vertex vertex = mesh.Vertices[i];
                vertex.Normal = new Vector(0, 0, 0);

                for (int j = 0; j < vertex.Triangles.Count; j++) { vertex.Normal += mesh[vertex.Triangles[j]].Normal; }
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
