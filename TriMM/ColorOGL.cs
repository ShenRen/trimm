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
// For more information and contact details look at TriMMs website:
// http://trimm.sourceforge.net/
//
//
// This class is based on the class of the same name
// from PAVEl: PAVEl (Paretoset Analysis Visualization and Evaluation) is a tool for
// interactively displaying and evaluating large sets of highdimensional data.
// Its main intended use is the analysis of result sets from multi-objective evolutionary algorithms.
//
// Copyright (C) 2007  PG500, ISF, University of Dortmund
//      PG500 are: Christoph Begau, Christoph Heuel, Raffael Joliet, Jan Kolanski,
//                 Mandy Kr�ller, Christian Moritz, Daniel Niggemann, Mathias St�ber,
//                 Timo St�nner, Jan Varwig, Dafan Zhai
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
// For more information and contact details visit http://pavel.googlecode.com

using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace TriMM {

    /// <summary>
    /// This class covers the System.Drawing.Color-struct.
    /// It caches the float value of the color, so it doesn't have to be calculated.
    /// </summary>
    public class ColorOGL {

        #region Fields

        private Color color;
        private float[] rgbFloat;

        #endregion

        #region Properties

        /// <value>Gets the red value of the ColorOGL as a float or sets it</value>
        public float R {
            get { return rgbFloat[0]; }
            set { rgbFloat[0] = value; color = ColorFromFloat(rgbFloat); }
        }

        /// <value>Gets the green value of the ColorOGL as a float or sets it</value>
        public float G {
            get { return rgbFloat[1]; }
            set { rgbFloat[1] = value; color = ColorFromFloat(rgbFloat); }
        }

        /// <value>Gets the blue value of the ColorOGL as a float or sets it</value>
        public float B {
            get { return rgbFloat[2]; }
            set { rgbFloat[2] = value; color = ColorFromFloat(rgbFloat); }
        }

        /// <value>Gets a float-array of the RGB values of the ColorOGL</value>
        public float[] RGB {
            get { return new float[] { rgbFloat[0], rgbFloat[1], rgbFloat[2] }; }
        }

        /// <value>Gets the ColorOGL as Color or sets it</value>
        public Color Color {
            get { return color; }
            set {
                color = value;
                rgbFloat = FloatFromColor(color);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Sets the color to white.
        /// </summary>
        public ColorOGL() { Color = Color.FromRgb(255, 255, 255); }

        /// <summary>
        /// Sets the color to <paramref name="color"/>.
        /// </summary>
        /// <param name="color">Color to be set</param>
        public ColorOGL(Color color) { Color = color; }

        /// <summary>
        /// Sets the color to the values given for the RGB components as floats.
        /// </summary>
        /// <param name="r">Value for the R component.</param>
        /// <param name="g">Value for the G component.</param>
        /// <param name="b">Value for the B component.</param>
        public ColorOGL(float r, float g, float b) {
            this.rgbFloat = new float[3];
            this.R = r;
            this.G = g;
            this.B = b;
        }

        #endregion

        #region Methods

        #region Static

        /// <summary>
        /// Calculates the float-values of the given Color.
        /// </summary>
        /// <param name="color">Given Color</param>
        /// <returns>Float-array of the transformed RGBA-values</returns>
        private static float[] FloatFromColor(Color color) {
            float[] rgbFloat = new float[3];
            rgbFloat[0] = color.R / 255.0f;
            rgbFloat[1] = color.G / 255.0f;
            rgbFloat[2] = color.B / 255.0f;
            return rgbFloat;
        }

        /// <summary>
        /// Calculates the Color from the given float-values.
        /// </summary>
        /// <param name="colors">Float-array of RGBA-values</param>
        /// <returns>The transformed Color</returns>
        private static Color ColorFromFloat(float[] colors) { return Color.FromRgb((byte)(colors[0] * 255), (byte)(colors[1] * 255), (byte)(colors[2] * 255)); }

        /// <summary>
        /// Calculates a color table with values interpolated between the given colors.
        /// </summary>
        /// <param name="colors">An arbitrary number of System.Drawing.Colors</param>
        /// <returns>An array of interpolated ColorOGLs</returns>
        public static ColorOGL[] InterpolationArray(params Color[] colors) {
            ColorOGL[] colorTable = new ColorOGL[short.MaxValue + 1];
            int steps = colors.Length - 1;

            ColorOGL[] oglColors = new ColorOGL[colors.Length];
            for (int i = 0; i < colors.Length; i++) {
                oglColors[i] = new ColorOGL(colors[i]);
            }

            for (int j = 0; j < steps; j++) {
                for (int k = j * short.MaxValue / steps; k <= (j + 1) * short.MaxValue / steps; k++) {
                    colorTable[k] = oglColors[j].Interpolate(oglColors[j + 1], (float)(steps * k - j * short.MaxValue) / short.MaxValue);
                }
            }

            return colorTable;
        }

        /// <summary>
        /// Calculates a color table with values interpolated between the given colors.
        /// </summary>
        /// <param name="colors">An arbitrary number of ColorOGLs</param>
        /// <returns>An array of interpolated ColorOGLs</returns>
        public static ColorOGL[] InterpolationArray(params ColorOGL[] colors) {
            ColorOGL[] colorTable = new ColorOGL[short.MaxValue + 1];
            int steps = colors.Length - 1;

            for (int j = 0; j < steps; j++) {
                for (int k = j * short.MaxValue / steps; k <= (j + 1) * short.MaxValue / steps; k++) {
                    colorTable[k] = colors[j].Interpolate(colors[j + 1], (float)(steps * k - j * short.MaxValue) / short.MaxValue);
                }
            }

            return colorTable;
        }

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
                if ((index < max) && (!unique.Contains(index))) { unique.Add(index); }
            }

            return unique;
        }

        #endregion

        /// <summary>
        /// Returns the color converted to System.Windows.Media.Color.
        /// </summary>
        /// <returns>The color as System.Drawing.Color</returns>
        public Color ToColor() { return Color.FromRgb((byte)(rgbFloat[0] * 255), (byte)(rgbFloat[1] * 255), (byte)(rgbFloat[2] * 255)); }

        /// <summary>
        /// Calculates the linear interpolation between two Colors,
        /// this color being dist=0, the other being dist=1
        /// </summary>
        /// <param name="other">Other Color</param>
        /// <param name="dist">Keep this between 0 and 1 please</param>
        /// <returns>Interpolated Color</returns>
        public ColorOGL Interpolate(ColorOGL other, float dist) {
            ColorOGL c = new ColorOGL();
            for (int i = 0; i < 3; i++) {
                c.rgbFloat[i] = this.rgbFloat[i] + (other.rgbFloat[i] - this.rgbFloat[i]) * dist;
                if (c.rgbFloat[i] > 1.0f) { c.rgbFloat[i] = 1.0f; } else if (c.rgbFloat[i] < 0.0f) { c.rgbFloat[i] = 0.0f; }
            }
            c.color = ColorFromFloat(c.rgbFloat);
            return c;
        }

        /// <summary>
        /// Returns a string representing the color (R;G;B).
        /// </summary>
        /// <returns>(R;G;B)</returns>
        public override string ToString() { return "(" + rgbFloat[0] + ";" + rgbFloat[1] + ";" + rgbFloat[2] + ")"; }

        #endregion
    }
}
