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

using System;
using System.Collections.Generic;


namespace TriMM {
    [Serializable()]

    /// <summary>
    /// The ring neighborhood (sometimes also called star) of a Vertex.
    /// </summary>
    public class Neighborhood : List<int> {

        #region Constructors

        /// <summary>
        /// Creates an empty neighborhood.
        /// </summary>
        public Neighborhood() { }

        /// <summary>
        /// Creates a neighborhood and fills it with the given <paramref name="neighbors"/>.
        /// </summary>
        /// <param name="neighbors">Initial neighbors.</param>
        public Neighborhood(IEnumerable<int> neighbors) { this.AddRange(neighbors); }

        #endregion

        #region Methods

        /// <summary>
        /// Allows adding a list of neighbors, ensuring that no Neighbor is contained twice.
        /// </summary>
        /// <param name="neighbors">A list of Neighbors</param>
        public void AddNeighbors(List<int> neighbors) {
            for (int i = 0; i < neighbors.Count; i++) { if (!this.Contains(neighbors[i])) { this.Add(neighbors[i]); } }
        }

        /// <summary>
        /// Allows adding an arbitrary amount of individual Neighors, ensuring that no Neighbor is contained twice.
        /// </summary>
        /// <param name="neighbors">An unspecified amount of individual Neighbors</param>
        public void AddNeighbors(params int[] neighbors) {
            for (int i = 0; i < neighbors.Length; i++) { if (!this.Contains(neighbors[i])) { this.Add(neighbors[i]); } }
        }

        /// <summary>
        /// Replaces the index in the chosen Neighbor with a given new index.
        /// </summary>
        /// <param name="oldIndex">The old index.</param>
        /// <param name="newIndex">The new index.</param>
        public void Replace(int oldIndex, int newIndex) {
            int oldNIndex = this.IndexOf(oldIndex);
            if (oldNIndex != -1) { this[oldNIndex] = newIndex; }
        }

        #endregion
    }
}
