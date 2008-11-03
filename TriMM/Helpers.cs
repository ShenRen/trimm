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

    /// <summary>
    /// This class is just a collection of all those methods used by this program,
    /// that do not clearly belong to a certain class.
    /// It is a static class, containing only static methods.
    /// </summary>
    public static class Helpers {

        #region Methods

        /// <summary>
        /// Converts an Integer into a unique ColorOGL.
        /// </summary>
        /// <param name="index">Integer to be converted</param>
        /// <returns>Color</returns>
        public static ColorOGL GetColorFromInt(int index) {
            ColorOGL color = new ColorOGL();
            color.R = (float)(255 - index % 256) / 255;
            color.G = (float)(255 - (int)((index - 256 * 256 * (int)(index / (256 * 256))) / 256)) / 255;
            color.B = (float)(255 - (int)(index / (256 * 256))) / 255;

            return color;
        }

        /// <summary>
        /// Converts a float RGB color into a unique Integer.
        /// </summary>
        /// <param name="color">Color</param>
        /// <returns>Corresponding Integer</returns>
        public static int GetIntFromColor(ColorOGL color) {
            return (int)(Math.Ceiling((1.0f - color.B) * 255) * 256 * 256 +
                Math.Ceiling((1.0f - color.G) * 255) * 256 + Math.Ceiling((1.0f - color.R) * 255));
        }

        /// <summary>
        /// Takes a float-array of RGB-colors and returns a list of the corresponding Vertices.
        /// Every picked Vertices is added to the list only once.
        /// </summary>
        /// <param name="selected">Array of the picked Vertices</param>
        /// <param name="colorDist">Distance between the colors</param>
        /// <param name="max">Maximum possible index</param>
        /// <returns>Unique list of selected triangles</returns>
        public static List<int> UniqueSelection(float[] selected, int colorDist, int max) {
            ColorOGL color = new ColorOGL();
            List<int> unique = new List<int>();
            int index;
            for (int i = 0; i < selected.Length / 3; i++) {
                color = new ColorOGL(selected[i * 3], selected[i * 3 + 1], selected[i * 3 + 2]);
                index = GetIntFromColor(color) / colorDist;
                if ((index < max) && (!unique.Contains(index))) {
                    unique.Add(index);
                }
            }

            return unique;
        }

        /// <summary>
        /// Calculates the bottom left corner and the width and height of a rectangle
        /// given by two opposing corners (<paramref name="x1"/>, <paramref name="y1"/>)
        /// and (<paramref name="x2"/>, <paramref name="y2"/>).
        /// </summary>
        /// <param name="x1">X-coordinate of the first corner</param>
        /// <param name="y1">Y-coordinate of the first corner</param>
        /// <param name="x2">X-coordinate of the second corner</param>
        /// <param name="y2">Y-coordinate of the second corner</param>
        /// <returns>[0]: X-coordinate of bottom left corner
        /// [1]: Y-coordinate of bottom left corner
        /// [2]: Width of picking rectangle
        /// [3]: Height of picking rectangle</returns>
        public static int[] GetPickingRectangle(int x1, int y1, int x2, int y2) {
            int[] rect = new int[4];

            if (x1 <= x2) { rect[0] = x1; } else { rect[0] = x2; }
            if (y1 <= y2) { rect[1] = y1; } else { rect[1] = y2; }
            rect[2] = Math.Abs(x1 - x2) + 1; //width
            rect[3] = Math.Abs(y1 - y2) + 1; //height

            return rect;
        }

        #endregion
    }
}
