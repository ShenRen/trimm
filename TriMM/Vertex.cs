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
using System.Collections.Generic;


namespace TriMM {
    [Serializable()]

    /// <summary>
    /// A Vertex of the mesh. Can store the normal vector, the Voronoi area, a list of adjacent Triangles and a Neighborhood.
    /// </summary>
    public class Vertex : VectorND {

        #region Fields

        private VectorND normal;
        private double area;

        private List<int> triangles = new List<int>();
        private Neighborhood neighborhood = new Neighborhood();

        #endregion

        #region Properties

        /// <value>Gets the normal vector of this Vertex or sets it.</value>
        public VectorND Normal { get { return normal; } set { normal = value; } }

        /// <value>Gets the Voronoi area of this Vertex or sets it.</value>
        public double Area { get { return area; } set { area = value; } }

        /// <value>Gets the List of Triangles adjacent to this Vertex or sets it.</value>
        public List<int> Triangles { get { return triangles; } set { triangles = value; } }

        /// <value>Gets the 1-Neighborhood (could be any Neighborhood, but is used as 1-Neighborhood) of this Vertex or sets it.</value>
        public Neighborhood Neighborhood { get { return neighborhood; } set { neighborhood = value; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Vertex of the dimension given by the number of values passed to the constructor.
        /// </summary>
        /// <param name="values">Elements of the Vertex</param>
        public Vertex(params double[] values) : base(values) { }

        /// <summary>
        /// Creates a new Vertex of the dimension given by the number of values passed to the constructor.
        /// </summary>
        /// <param name="values">An enumeration of elements of the Vertex</param>
        public Vertex(IEnumerable<double> values) : base(values) { }

        #endregion

        #region Methods

        /// <summary>
        /// If the given Triangle is not already in the list of adjacent Triangle, it is added.
        /// </summary>
        /// <param name="triangle">New adjacent Triangle.</param>
        public void AddAdjacentTriangle(int triangle) { if (!triangles.Contains(triangle)) { triangles.Add(triangle); } }

        #endregion

    }
}
