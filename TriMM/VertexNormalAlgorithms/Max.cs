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

    ///<summary>The algorithm introduced by Nelson Max in his article
    /// "Weights for Computing Vertex Normals from Facet Normals".
    /// The normal vector at a Vertex is calculated using the angle between the neighbors
    /// in counter-clockwise order and the corresponding facet normal vector.
    /// The facet normal vectors can be calculated from the neighbors and thus be
    /// eliminated in this calculation.
    /// </summary> 
    public class Max : IVertexNormalAlgorithm {

        #region IVertexNormalAlgorithm Member

        #region Properties

        /// <value>Gets the name of this algorithm.</value>
        public string Name { get { return TriMMApp.Lang.GetElementsByTagName("M")[0].InnerText; } }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the Vertex normals as an average of the normals of the incident Triangles,
        /// weighted by the angle between the neighboring Vertices.
        /// As the Triangle normal can be calculated from the neighboring Vertices,
        /// the actual Triangle normals and the angles are not needed.
        /// (More exact, since the calculation of the angle requires calculating an arcus cosine.)
        /// The cross product of the neighbors is calculated and divided by the product of their length instead.
        /// </summary>
        /// <param name="mesh">Reference to the TriangleMesh to calculate the vertex normals for.</param>
        public void GetVertexNormals(ref TriangleMesh mesh) {
            Vector pro1, pro2;
            double factor;
            int[] neighbors;

            for (int i = 0; i < mesh.Vertices.Count; i++) {
                Vertex vertex = mesh.Vertices[i];
                vertex.Normal = new Vector(0, 0, 0);

                for (int j = 0; j < vertex.Triangles.Count; j++) {
                    neighbors = mesh[vertex.Triangles[j]].GetNeighborsOf(i);
                    pro1 = mesh.Vertices[neighbors[0]] - vertex;
                    pro2 = mesh.Vertices[neighbors[1]] - vertex;
                    factor = pro1.SquaredLength() * pro2.SquaredLength();
                    vertex.Normal += (pro1 % pro2) / factor;
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
