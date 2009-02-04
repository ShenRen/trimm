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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace TriMM {

    /// <summary>
    /// A Window to create a new TriangleMesh representing a polyhedron.
    /// </summary>
    public partial class PolyhedronWindow : Window {

        #region Fields

        private NumericUpDown widthNumericUpDown = new NumericUpDown();
        private NumericUpDown heightNumericUpDown = new NumericUpDown();
        private NumericUpDown depthNumericUpDown = new NumericUpDown();
        private NumericUpDown stepsNumericUpDown = new NumericUpDown();

        #endregion

        #region Constructors

        /// <summary>
        /// The Window is initialized and the NumericUpDowns are included.
        /// </summary>
        public PolyhedronWindow() {
            InitializeComponent();
            this.Icon = TriMMApp.Image;

            widthNumericUpDown.DecimalPlaces = heightNumericUpDown.DecimalPlaces = depthNumericUpDown.DecimalPlaces = 15;
            widthNumericUpDown.Minimum = heightNumericUpDown.Minimum = depthNumericUpDown.Minimum = 0.00001m;
            widthNumericUpDown.Maximum = heightNumericUpDown.Maximum = depthNumericUpDown.Maximum = 100000;
            widthNumericUpDown.Increment = heightNumericUpDown.Increment = depthNumericUpDown.Increment = 0.001m;
            widthNumericUpDown.Value = heightNumericUpDown.Value = depthNumericUpDown.Value = 1;
            widthNumericUpDown.TextAlign = heightNumericUpDown.TextAlign = depthNumericUpDown.TextAlign = stepsNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            stepsNumericUpDown.Minimum = 0;
            stepsNumericUpDown.Maximum = 6;

            widthWFHost.Child = widthNumericUpDown;
            heightWFHost.Child = heightNumericUpDown;
            depthWFHost.Child = depthNumericUpDown;
            stepsWFHost.Child = stepsNumericUpDown;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the base tetrahedron.
        /// </summary>
        private TriangleMesh CreateBaseTetrahedron() {
            TriangleMesh baseMesh = new TriangleMesh();

            baseMesh.Vertices.Add(new Vertex(0.5, 0.5, 0.5));
            baseMesh.Vertices.Add(new Vertex(-0.5, -0.5, 0.5));
            baseMesh.Vertices.Add(new Vertex(-0.5, 0.5, -0.5));
            baseMesh.Vertices.Add(new Vertex(0.5, -0.5, -0.5));

            baseMesh.Add(new Triangle(0, 1, 3));
            baseMesh.Add(new Triangle(1, 0, 2));
            baseMesh.Add(new Triangle(2, 0, 3));
            baseMesh.Add(new Triangle(2, 3, 1));

            return baseMesh;
        }

        /// <summary>
        /// Creates the base pyramid.
        /// </summary>
        private TriangleMesh CreateBasePyramid() {
            TriangleMesh baseMesh = new TriangleMesh();

            baseMesh.Vertices.Add(new Vertex(0, 0, 0.5));
            baseMesh.Vertices.Add(new Vertex(0.5, 0.5, -0.5));
            baseMesh.Vertices.Add(new Vertex(-0.5, 0.5, -0.5));
            baseMesh.Vertices.Add(new Vertex(-0.5, -0.5, -0.5));
            baseMesh.Vertices.Add(new Vertex(0.5, -0.5, -0.5));

            baseMesh.Add(new Triangle(0, 1, 2));
            baseMesh.Add(new Triangle(0, 2, 3));
            baseMesh.Add(new Triangle(0, 3, 4));
            baseMesh.Add(new Triangle(0, 4, 1));
            baseMesh.Add(new Triangle(1, 2, 3));
            baseMesh.Add(new Triangle(3, 4, 1));

            return baseMesh;
        }

        /// <summary>
        /// Creates the base octahedron.
        /// </summary>
        private TriangleMesh CreateBaseOctahedron() {
            TriangleMesh baseMesh = new TriangleMesh();

            baseMesh.Vertices.Add(new Vertex(0, 0, 1));
            baseMesh.Vertices.Add(new Vertex(1, 0, 0));
            baseMesh.Vertices.Add(new Vertex(0, 1, 0));
            baseMesh.Vertices.Add(new Vertex(-1, 0, 0));
            baseMesh.Vertices.Add(new Vertex(0, -1, 0));
            baseMesh.Vertices.Add(new Vertex(0, 0, -1));

            baseMesh.Add(new Triangle(0, 1, 2));
            baseMesh.Add(new Triangle(0, 2, 3));
            baseMesh.Add(new Triangle(0, 3, 4));
            baseMesh.Add(new Triangle(0, 4, 1));
            baseMesh.Add(new Triangle(5, 2, 1));
            baseMesh.Add(new Triangle(5, 3, 2));
            baseMesh.Add(new Triangle(5, 4, 3));
            baseMesh.Add(new Triangle(5, 1, 4));

            return baseMesh;
        }

        /// <summary>
        /// Creates the base dodecahedron.
        /// </summary>
        private TriangleMesh CreateBaseDodecahedron() {
            TriangleMesh baseMesh = new TriangleMesh();

            double phi = (1 + Math.Sqrt(5)) / 2;
            double phiInv = 2 / (1 + Math.Sqrt(5));

            baseMesh.Vertices.Add(new Vertex(1, 1, 1));             // 0
            baseMesh.Vertices.Add(new Vertex(-1, 1, 1));            // 1
            baseMesh.Vertices.Add(new Vertex(1, -1, 1));            // 2
            baseMesh.Vertices.Add(new Vertex(1, 1, -1));            // 3
            baseMesh.Vertices.Add(new Vertex(-1, -1, 1));           // 4
            baseMesh.Vertices.Add(new Vertex(-1, 1, -1));           // 5
            baseMesh.Vertices.Add(new Vertex(1, -1, -1));           // 6
            baseMesh.Vertices.Add(new Vertex(-1, -1, -1));          // 7
            baseMesh.Vertices.Add(new Vertex(0, phiInv, phi));      // 8
            baseMesh.Vertices.Add(new Vertex(0, -phiInv, phi));     // 9
            baseMesh.Vertices.Add(new Vertex(0, phiInv, -phi));     // 10
            baseMesh.Vertices.Add(new Vertex(0, -phiInv, -phi));    // 11
            baseMesh.Vertices.Add(new Vertex(phiInv, phi, 0));      // 12
            baseMesh.Vertices.Add(new Vertex(-phiInv, phi, 0));     // 13
            baseMesh.Vertices.Add(new Vertex(phiInv, -phi, 0));     // 14
            baseMesh.Vertices.Add(new Vertex(-phiInv, -phi, 0));    // 15
            baseMesh.Vertices.Add(new Vertex(phi, 0, phiInv));      // 16
            baseMesh.Vertices.Add(new Vertex(-phi, 0, phiInv));     // 17
            baseMesh.Vertices.Add(new Vertex(phi, 0, -phiInv));     // 18
            baseMesh.Vertices.Add(new Vertex(-phi, 0, -phiInv));    // 19

            baseMesh.Vertices.Add(new Vertex(0, (2 * (1 + phi) + phiInv) / 5, (2 + phi) / 5));      // 20
            baseMesh.Vertices.Add(new Vertex((2 + phi) / 5, 0, (2 * (1 + phi) + phiInv) / 5));      // 21
            baseMesh.Vertices.Add(new Vertex((2 * (1 + phi) + phiInv) / 5, (2 + phi) / 5, 0));      // 22
            baseMesh.Vertices.Add(new Vertex(-(2 * (1 + phi) + phiInv) / 5, (2 + phi) / 5, 0));     // 23
            baseMesh.Vertices.Add(new Vertex(-(2 + phi) / 5, 0, -(2 * (1 + phi) + phiInv) / 5));    // 24
            baseMesh.Vertices.Add(new Vertex(-(2 * (1 + phi) + phiInv) / 5, -(2 + phi) / 5, 0));    // 25
            baseMesh.Vertices.Add(new Vertex(-(2 + phi) / 5, 0, (2 * (1 + phi) + phiInv) / 5));     // 26
            baseMesh.Vertices.Add(new Vertex(0, -(2 * (1 + phi) + phiInv) / 5, (2 + phi) / 5));     // 27
            baseMesh.Vertices.Add(new Vertex((2 * (1 + phi) + phiInv) / 5, -(2 + phi) / 5, 0));     // 28
            baseMesh.Vertices.Add(new Vertex((2 + phi) / 5, 0, -(2 * (1 + phi) + phiInv) / 5));     // 29
            baseMesh.Vertices.Add(new Vertex(0, -(2 * (1 + phi) + phiInv) / 5 , -(2 + phi) / 5));   // 30
            baseMesh.Vertices.Add(new Vertex(0, (2 * (1 + phi) + phiInv) / 5, -(2 + phi) / 5));     // 31

            baseMesh.Add(new Triangle(0, 12, 20));
            baseMesh.Add(new Triangle(12, 13, 20));
            baseMesh.Add(new Triangle(13, 1, 20));
            baseMesh.Add(new Triangle(1, 8, 20));
            baseMesh.Add(new Triangle(8, 0, 20));
            baseMesh.Add(new Triangle(0, 8, 21));
            baseMesh.Add(new Triangle(8, 9, 21));
            baseMesh.Add(new Triangle(9, 2, 21));
            baseMesh.Add(new Triangle(2, 16, 21));
            baseMesh.Add(new Triangle(16, 0, 21));
            baseMesh.Add(new Triangle(0, 16, 22));
            baseMesh.Add(new Triangle(16, 18, 22));
            baseMesh.Add(new Triangle(18, 3, 22));
            baseMesh.Add(new Triangle(3, 12, 22));
            baseMesh.Add(new Triangle(12, 0, 22));
            baseMesh.Add(new Triangle(19, 17, 23));
            baseMesh.Add(new Triangle(17, 1, 23));
            baseMesh.Add(new Triangle(1, 13, 23));
            baseMesh.Add(new Triangle(13, 5, 23));
            baseMesh.Add(new Triangle(5, 19, 23));
            baseMesh.Add(new Triangle(19, 5, 24));
            baseMesh.Add(new Triangle(5, 10, 24));
            baseMesh.Add(new Triangle(10, 11, 24));
            baseMesh.Add(new Triangle(11, 7, 24));
            baseMesh.Add(new Triangle(7, 19, 24));
            baseMesh.Add(new Triangle(19, 7, 25));
            baseMesh.Add(new Triangle(7, 15, 25));
            baseMesh.Add(new Triangle(15, 4, 25));
            baseMesh.Add(new Triangle(4, 17, 25));
            baseMesh.Add(new Triangle(17, 19, 25));
            baseMesh.Add(new Triangle(1, 17, 26));
            baseMesh.Add(new Triangle(17, 4, 26));
            baseMesh.Add(new Triangle(4, 9, 26));
            baseMesh.Add(new Triangle(9, 8, 26));
            baseMesh.Add(new Triangle(8, 1, 26));
            baseMesh.Add(new Triangle(2, 9, 27));
            baseMesh.Add(new Triangle(9, 4, 27));
            baseMesh.Add(new Triangle(4, 15, 27));
            baseMesh.Add(new Triangle(15, 14, 27));
            baseMesh.Add(new Triangle(14, 2, 27));
            baseMesh.Add(new Triangle(2, 14, 28));
            baseMesh.Add(new Triangle(14, 6, 28));
            baseMesh.Add(new Triangle(6, 18, 28));
            baseMesh.Add(new Triangle(18, 16, 28));
            baseMesh.Add(new Triangle(16, 2, 28));
            baseMesh.Add(new Triangle(3, 18, 29));
            baseMesh.Add(new Triangle(18, 6, 29));
            baseMesh.Add(new Triangle(6, 11, 29));
            baseMesh.Add(new Triangle(11, 10, 29));
            baseMesh.Add(new Triangle(10, 3, 29));
            baseMesh.Add(new Triangle(6, 14, 30));
            baseMesh.Add(new Triangle(14, 15, 30));
            baseMesh.Add(new Triangle(15, 7, 30));
            baseMesh.Add(new Triangle(7, 11, 30));
            baseMesh.Add(new Triangle(11, 6, 30));
            baseMesh.Add(new Triangle(3, 10, 31));
            baseMesh.Add(new Triangle(10, 5, 31));
            baseMesh.Add(new Triangle(5, 13, 31));
            baseMesh.Add(new Triangle(13, 12, 31));
            baseMesh.Add(new Triangle(12, 3, 31));

            return baseMesh;
        }

        /// <summary>
        /// Creates the base icosahedron.
        /// </summary>
        private TriangleMesh CreateBaseIcosahedron() {
            TriangleMesh baseMesh = new TriangleMesh();

            double phi = Math.PI * 26.56505 / 180.0;
            double theta = 0;
            double theta2 = Math.PI * 36.0 / 180.0;
            double theta72 = Math.PI * 72.0 / 180;

            baseMesh.Vertices.Add(new Vertex(0, 0, 1));

            for (int i = 1; i < 6; i++) {
                Vertex newVertex = new Vertex(Math.Cos(theta) * Math.Cos(phi), Math.Sin(theta) * Math.Cos(phi), Math.Sin(phi));
                newVertex.Normalize();
                baseMesh.Vertices.Add(newVertex);
                theta += theta72;
            }

            for (int j = 6; j < 11; j++) {
                Vertex newVertex = new Vertex(Math.Cos(theta2) * Math.Cos(-phi), Math.Sin(theta2) * Math.Cos(-phi), Math.Sin(-phi));
                newVertex.Normalize();
                baseMesh.Vertices.Add(newVertex);
                theta2 += theta72;
            }

            baseMesh.Vertices.Add(new Vertex(0, 0, -1));

            baseMesh.Add(new Triangle(0, 1, 2));
            baseMesh.Add(new Triangle(0, 2, 3));
            baseMesh.Add(new Triangle(0, 3, 4));
            baseMesh.Add(new Triangle(0, 4, 5));
            baseMesh.Add(new Triangle(0, 5, 1));
            baseMesh.Add(new Triangle(11, 7, 6));
            baseMesh.Add(new Triangle(11, 8, 7));
            baseMesh.Add(new Triangle(11, 9, 8));
            baseMesh.Add(new Triangle(11, 10, 9));
            baseMesh.Add(new Triangle(11, 6, 10));
            baseMesh.Add(new Triangle(1, 6, 2));
            baseMesh.Add(new Triangle(2, 7, 3));
            baseMesh.Add(new Triangle(3, 8, 4));
            baseMesh.Add(new Triangle(4, 9, 5));
            baseMesh.Add(new Triangle(5, 10, 1));
            baseMesh.Add(new Triangle(6, 7, 2));
            baseMesh.Add(new Triangle(7, 8, 3));
            baseMesh.Add(new Triangle(8, 9, 4));
            baseMesh.Add(new Triangle(9, 10, 5));
            baseMesh.Add(new Triangle(10, 6, 1));

            return baseMesh;
        }

        #endregion

        #region Event Handling Stuff

        /// <summary>
        /// Creates the polyhedron of the chosen type with the chosen parameters.
        /// </summary>
        /// <param name="sender">okButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void OKButton_Click(object sender, RoutedEventArgs e) {
            Cursor = System.Windows.Input.Cursors.Wait;
#if !DEBUG
            try {
#endif
                switch (typeComboBox.SelectedIndex) {
                    case 0:
                        TriMMApp.Mesh = CreateBaseTetrahedron();
                        break;
                    case 1:
                        TriMMApp.Mesh = CreateBasePyramid();
                        break;
                    case 2:
                        TriMMApp.Mesh = CreateBaseOctahedron();
                        break;
                    case 3:
                        TriMMApp.Mesh = CreateBaseDodecahedron();
                        break;
                    case 4:
                        TriMMApp.Mesh = CreateBaseIcosahedron();
                        break;
                }
                TriMMApp.Mesh.ScaleMesh(new Vector((double)widthNumericUpDown.Value, (double)heightNumericUpDown.Value, (double)depthNumericUpDown.Value));
                TriMMApp.Mesh.Subdivide((int)stepsNumericUpDown.Value);

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

        #endregion

    }
}