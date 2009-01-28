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

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using System.Windows.Interop;

namespace TriMM {

    /// <summary>
    /// A Window to create a new TriangleMesh representing a box.
    /// </summary>
    public partial class BoxWindow : Window {

        #region Fields

        private NumericUpDown lengthNumericUpDown = new NumericUpDown();
        private NumericUpDown widthElementsNumericUpDown = new NumericUpDown();
        private NumericUpDown heightElementsNumericUpDown = new NumericUpDown();
        private NumericUpDown depthElementsNumericUpDown = new NumericUpDown();

        private double length;
        private int width, height, depth;
        private int meshType = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// The Window is initialized and the NumericUpDowns are included.
        /// </summary>
        public BoxWindow() {
            InitializeComponent();

            BitmapSource bitmapSource1 = Imaging.CreateBitmapSourceFromHBitmap(
                    TriMM.Properties.Resources.tri8.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

            image1.Source = bitmapSource1;
            BitmapSource bitmapSource2 = Imaging.CreateBitmapSourceFromHBitmap(
                    TriMM.Properties.Resources.tri2.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

            image2.Source = bitmapSource2;


            lengthNumericUpDown.DecimalPlaces = 15;
            lengthNumericUpDown.Minimum = 0.00001m;
            lengthNumericUpDown.Maximum = 100000;
            lengthNumericUpDown.Increment = 0.001m;
            lengthNumericUpDown.Value = 1;
            lengthNumericUpDown.TextAlign = widthElementsNumericUpDown.TextAlign = heightElementsNumericUpDown.TextAlign = depthElementsNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            widthElementsNumericUpDown.Minimum = heightElementsNumericUpDown.Minimum = depthElementsNumericUpDown.Minimum = 1;
            widthElementsNumericUpDown.Maximum = heightElementsNumericUpDown.Maximum = depthElementsNumericUpDown.Maximum = 100;

            lengthWFHost.Child = lengthNumericUpDown;
            widthElementsWFHost.Child = widthElementsNumericUpDown;
            heightElementsWFHost.Child = heightElementsNumericUpDown;
            depthElementsWFHost.Child = depthElementsNumericUpDown;

            this.Icon = TriMMApp.Image;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a basic two-dimensional mesh of the appropriate size.
        /// </summary>
        /// <param name="x">The number of elements in x-direction.</param>
        /// <param name="y">The number of elements in y-direction.</param>
        private List<Vector> CreateBaseMesh(int x, int y) {
            List<Vector> bm;

            if (meshType == 0) {
                bm = new List<Vector>((2 * x + 1) * (2 * y + 1));
                bm.Add(new Vector(-length * x * 0.5, -length * y * 0.5));

                for (int i = 0; i <= 2 * y; i++) {
                    for (int j = 0; j <= 2 * x; j++) {
                        if (i != 0) {
                            bm.Add(bm[0] + new Vector(j * length * 0.5, i * length * 0.5));
                        } else {
                            if (j != 0) {
                                bm.Add(bm[0] + new Vector(j * length * 0.5, i * length * 0.5));
                            }
                        }
                    }
                }
            } else {
                bm = new List<Vector>();
            }

            return bm;
        }

        /// <summary>
        /// Connects the Vertices to the correct triangles for the chosen mesh type.
        /// </summary>
        /// <param name="mesh">The new TriangleMesh containing only the Vertices.</param>
        /// <param name="steps">The number of steps to be taken.</param>
        /// <returns>The completed TriangleMesh.</returns>
        private TriangleMesh CreateTriangles(TriangleMesh mesh, int steps) {
            int point = 0;
            if (meshType == 0) {
                do {
                    mesh.Add(new Triangle(point, point + 1, point + steps + 2));
                    mesh.Add(new Triangle(point + 1, point + 2, point + steps + 2));
                    mesh.Add(new Triangle(point + 2, point + steps + 3, point + steps + 2));
                    mesh.Add(new Triangle(point + steps + 3, point + 2 * steps + 4, point + steps + 2));
                    mesh.Add(new Triangle(point + 2 * steps + 4, point + 2 * steps + 3, point + steps + 2));
                    mesh.Add(new Triangle(point + 2 * steps + 3, point + 2 * steps + 2, point + steps + 2));
                    mesh.Add(new Triangle(point + 2 * steps + 2, point + steps + 1, point + steps + 2));
                    mesh.Add(new Triangle(point + steps + 1, point, point + steps + 2));

                    if (point % (2 * steps + 2) < steps - 2) { point += 2; } else { point += steps + 4; }
                } while (point <= mesh.Vertices.Count - (5 + 2 * steps));
            } else {
            }

            mesh.Finish(true);
            return mesh;
        }

        /// <summary>
        /// Creates the top mesh from the given base mesh.
        /// </summary>
        /// <param name="bm">The base mesh</param>
        /// <returns>The top mesh</returns>
        private TriangleMesh CreateTopMesh(List<Vector> bm) {
            TriangleMesh top = new TriangleMesh();

            for (int i = 0; i < bm.Count; i++) { top.Vertices.Add(new Vertex(bm[i][0], bm[i][1], depth * length * 0.5)); }
            return CreateTriangles(top, 2 * width);
        }

        /// <summary>
        /// Creates the bottom mesh from the given base mesh.
        /// </summary>
        /// <param name="bm">The base mesh</param>
        /// <returns>The bottom mesh</returns>
        private TriangleMesh CreateBottomMesh(List<Vector> bm) {
            TriangleMesh bottom = new TriangleMesh();

            for (int i = 0; i < bm.Count; i++) { bottom.Vertices.Add(new Vertex(bm[i][0], -bm[i][1], -depth * length * 0.5)); }
            return CreateTriangles(bottom, 2 * width);
        }

        /// <summary>
        /// Creates the left mesh from the given base mesh.
        /// </summary>
        /// <param name="bm">The base mesh</param>
        /// <returns>The left mesh</returns>
        private TriangleMesh CreateLeftMesh(List<Vector> bm) {
            TriangleMesh left = new TriangleMesh();

            for (int i = 0; i < bm.Count; i++) {left.Vertices.Add(new Vertex(-width * length * 0.5, -bm[i][0], bm[i][1])); }
            return CreateTriangles(left, 2 * height);
        }

        /// <summary>
        /// Creates the right mesh from the given base mesh.
        /// </summary>
        /// <param name="bm">The base mesh</param>
        /// <returns>The right mesh</returns>
        private TriangleMesh CreateRightMesh(List<Vector> bm) {
            TriangleMesh right = new TriangleMesh();

            for (int i = 0; i < bm.Count; i++) { right.Vertices.Add(new Vertex(width * length * 0.5, bm[i][0], bm[i][1])); }
            return CreateTriangles(right, 2 * height);
        }

        /// <summary>
        /// Creates the back mesh from the given base mesh.
        /// </summary>
        /// <param name="bm">The base mesh</param>
        /// <returns>The back mesh</returns>
        private TriangleMesh CreateBackMesh(List<Vector> bm) {
            TriangleMesh back = new TriangleMesh();

            for (int i = 0; i < bm.Count; i++) { back.Vertices.Add(new Vertex(bm[i][0], -height * length * 0.5, bm[i][1])); }
            return CreateTriangles(back, 2 * width);
        }

        /// <summary>
        /// Creates the front mesh from the given base mesh.
        /// </summary>
        /// <param name="bm">The base mesh</param>
        /// <returns>The front mesh</returns>
        private TriangleMesh CreateFrontMesh(List<Vector> bm) {
            TriangleMesh front = new TriangleMesh();

            for (int i = 0; i < bm.Count; i++) { front.Vertices.Add(new Vertex(-bm[i][0], height * length * 0.5, bm[i][1])); }
            return CreateTriangles(front, 2 * width);
        }

        #endregion

        #region Event Handling Stuff

        /// <summary>
        /// Creates a new Cube with the chosen length of each element and amount of elements per dimension.
        /// </summary>
        /// <param name="sender">okButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void OKButton_Click(object sender, RoutedEventArgs e) {
            Cursor = System.Windows.Input.Cursors.Wait;
#if !DEBUG
            try {
#endif
            TriMMApp.Mesh = new TriangleMesh();

            // Gets the parameters.
            length = (double)lengthNumericUpDown.Value;
            width = (int)widthElementsNumericUpDown.Value;
            height = (int)heightElementsNumericUpDown.Value;
            depth = (int)depthElementsNumericUpDown.Value;

            // The side meshes are created.
            List<Vector> bm = CreateBaseMesh(width, height);
            TriangleMesh top = CreateTopMesh(bm);
            TriangleMesh bottom = CreateBottomMesh(bm);

            bm = CreateBaseMesh(height, depth);
            TriangleMesh left = CreateLeftMesh(bm);
            TriangleMesh right = CreateRightMesh(bm);

            bm = CreateBaseMesh(width, depth);
            TriangleMesh front = CreateFrontMesh(bm);
            TriangleMesh back = CreateBackMesh(bm);

            // The side meshes are united to create one complete mesh.
            TriMMApp.Mesh = TriangleMesh.Union(top, bottom);
            TriMMApp.Mesh = TriangleMesh.Union(TriMMApp.Mesh, left);
            TriMMApp.Mesh = TriangleMesh.Union(TriMMApp.Mesh, right);
            TriMMApp.Mesh = TriangleMesh.Union(TriMMApp.Mesh, back);
            TriMMApp.Mesh = TriangleMesh.Union(TriMMApp.Mesh, front);

            TriangleMesh mesh = TriMMApp.Mesh;
            TriMMApp.VertexNormalAlgorithm.GetVertexNormals(ref mesh);
            TriMMApp.Mesh.SetArrays();

            this.Close();

#if !DEBUG
            } catch (Exception exception) {
                System.Windows.MessageBox.Show(exception.Message, TriMMApp.Lang.GetElementsByTagName("ErrorTitle")[0].InnerText, MessageBoxButton.OK, MessageBoxImage.Error);
                TriMMApp.Mesh = null;
            } finally {
#endif
            Cursor = System.Windows.Input.Cursors.Arrow;
#if !DEBUG
            }
#endif
        }

        /// <summary>
        /// Sets the type of element to be used to the one separating a square into 8 triangles.
        /// </summary>
        /// <param name="sender">radioButton1</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void RadioButton1_Checked(object sender, RoutedEventArgs e) { meshType = 0; }

        /// <summary>
        /// Sets the type of element to be used to the one separating a square into 2 triangles.
        /// </summary>
        /// <param name="sender">radioButton2</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void RadioButton2_Checked(object sender, RoutedEventArgs e) { meshType = 0; }

        #endregion

    }
}
