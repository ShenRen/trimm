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
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace TriMM {

    /// <summary>
    /// A Window to create a new TriangleMesh representing an ellipsoid.
    /// </summary>
    public partial class EllipsoidWindow : Window {

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
        public EllipsoidWindow() {
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
        /// Creates the base mesh as an Icosahedron.
        /// </summary>
        private TriangleMesh CreateBaseMesh() {
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
        /// Creates an Ellipsoid with the chosen parameters.
        /// </summary>
        /// <param name="sender">okButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void OKButton_Click(object sender, RoutedEventArgs e) {
            Cursor = System.Windows.Input.Cursors.Wait;

#if !DEBUG
            try {
#endif
                TriMMApp.Mesh = CreateBaseMesh();
                TriMMApp.Mesh.Subdivide((int)stepsNumericUpDown.Value);
                for (int i = 0; i < TriMMApp.Mesh.Vertices.Count; i++) { TriMMApp.Mesh.Vertices[i].Normalize(); }
                TriMMApp.Mesh.ScaleMesh(new Vector((double)widthNumericUpDown.Value, (double)heightNumericUpDown.Value, (double)depthNumericUpDown.Value));
                
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