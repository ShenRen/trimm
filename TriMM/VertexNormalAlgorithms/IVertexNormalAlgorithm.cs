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

/// <summary>
/// This namespace contains all the vertex normal algorithms.
/// </summary>
namespace TriMM.VertexNormalAlgorithms {

    /// <summary>
    /// The Interface for all Vertex normal algorithms.
    /// </summary>
    public interface IVertexNormalAlgorithm {

        #region Properties

        /// <value>Gets a string serving as an identifier for this algorithm. </value>
        string Name { get; }

        #endregion

        #region Methods

        /// <summary>
        /// The main method of the algorithm. It takes a reference to a TriangleMesh and
        /// calculates the normal vector for every Vertex in the given TriangleMesh.
        /// </summary>
        void GetVertexNormals();

        #endregion
    }
}
