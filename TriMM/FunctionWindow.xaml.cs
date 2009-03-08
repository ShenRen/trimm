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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Globalization;

namespace TriMM {

    /// <summary>
    /// A Window to create a new TriangleMesh representing a function over x and y.
    /// </summary>
    public partial class FunctionWindow : Window {

        #region Fields

        // Index of the factor to calculate in the correct angle-mode
        // [0]: DEG, [1]: RAD, [2]: GRAD
        private byte factorIndex = 1;
        private int lastSelectionStart = 0;
        private int meshType = 0;

        private NumericUpDown lengthNumericUpDown = new NumericUpDown();
        private NumericUpDown stepsNumericUpDown = new NumericUpDown();
        private NumericUpDown xLengthNumericUpDown = new NumericUpDown();
        private NumericUpDown xStepsNumericUpDown = new NumericUpDown();
        private NumericUpDown yLengthNumericUpDown = new NumericUpDown();
        private NumericUpDown yStepsNumericUpDown = new NumericUpDown();

        #endregion

        #region Constructors

        /// <summary>
        /// The FunctionWindow is initialized. Images are loaded from the Resouces.
        /// The NumericUpDowns are initialized.
        /// </summary>
        public FunctionWindow() {
            InitializeComponent();
            this.Icon = TriMMApp.Image;

            BitmapSource bitmapSource1 = Imaging.CreateBitmapSourceFromHBitmap(
                    TriMM.Properties.Resources.tri6.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            image1.Source = bitmapSource1;

            BitmapSource bitmapSource2 = Imaging.CreateBitmapSourceFromHBitmap(
                    TriMM.Properties.Resources.tri8.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            image2.Source = bitmapSource2;

            BitmapSource bitmapSource3 = Imaging.CreateBitmapSourceFromHBitmap(
                    TriMM.Properties.Resources.xroot.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            rootImage.Source = bitmapSource3;

            BitmapSource bitmapSource4 = Imaging.CreateBitmapSourceFromHBitmap(
                    TriMM.Properties.Resources.Pi.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            piImage.Source = bitmapSource4;

            stepsNumericUpDown.TextAlign = lengthNumericUpDown.TextAlign = xLengthNumericUpDown.TextAlign = xStepsNumericUpDown.TextAlign
                = yLengthNumericUpDown.TextAlign = yStepsNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            lengthNumericUpDown.DecimalPlaces = xLengthNumericUpDown.DecimalPlaces = yLengthNumericUpDown.DecimalPlaces = 14;
            lengthNumericUpDown.Minimum = xLengthNumericUpDown.Minimum = yLengthNumericUpDown.Minimum = 0.00001m;
            lengthNumericUpDown.Maximum = xLengthNumericUpDown.Maximum = yLengthNumericUpDown.Maximum = 10000;
            lengthNumericUpDown.Increment = xLengthNumericUpDown.Increment = yLengthNumericUpDown.Increment = 0.001m;
            lengthNumericUpDown.Value = xLengthNumericUpDown.Value = yLengthNumericUpDown.Value = 1;
            stepsNumericUpDown.Minimum = 0;
            stepsNumericUpDown.Maximum = 7;
            xStepsNumericUpDown.Minimum = yStepsNumericUpDown.Minimum = 1;
            xStepsNumericUpDown.Maximum = yStepsNumericUpDown.Maximum = 100;

            stepsWFHost.Child = stepsNumericUpDown;
            lengthWFHost.Child = lengthNumericUpDown;
            xLengthWFHost.Child = xLengthNumericUpDown;
            xStepsWFHost.Child = xStepsNumericUpDown;
            yLengthWFHost.Child = yLengthNumericUpDown;
            yStepsWFHost.Child = yStepsNumericUpDown;

            // The decimal separator is set according to the current culture.
            dotButton.Content = dotButton.Tag = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a hexagonal base mesh.
        /// </summary>
        /// <returns>The hexagonal base mesh</returns>
        private TriangleMesh CreateHexBase() {
            TriangleMesh baseMesh = new TriangleMesh();

            Cursor = System.Windows.Input.Cursors.Wait;

            double baseLength = (double)lengthNumericUpDown.Value;
            baseMesh.Vertices.Add(new Vertex(0, 0, 0));
            baseMesh.Vertices.Add(new Vertex(baseLength, 0, 0));
            baseMesh.Vertices.Add(new Vertex(baseLength / 2, baseLength * Math.Sqrt(3) / 2, 0));
            baseMesh.Vertices.Add(new Vertex(-baseLength / 2, baseLength * Math.Sqrt(3) / 2, 0));
            baseMesh.Vertices.Add(new Vertex(-baseLength, 0, 0));
            baseMesh.Vertices.Add(new Vertex(-baseLength / 2, -baseLength * Math.Sqrt(3) / 2, 0));
            baseMesh.Vertices.Add(new Vertex(baseLength / 2, -baseLength * Math.Sqrt(3) / 2, 0));

            baseMesh.Add(new Triangle(0, 1, 2));
            baseMesh.Add(new Triangle(0, 2, 3));
            baseMesh.Add(new Triangle(0, 3, 4));
            baseMesh.Add(new Triangle(0, 4, 5));
            baseMesh.Add(new Triangle(0, 5, 6));
            baseMesh.Add(new Triangle(0, 6, 1));

            baseMesh.Subdivide((int)stepsNumericUpDown.Value);

            Cursor = System.Windows.Input.Cursors.Arrow;

            return baseMesh;
        }

        /// <summary>
        /// Creates a rectangular base mesh.
        /// </summary>
        /// <returns>The rectangular base mesh</returns>
        private TriangleMesh CreateSquareBase() {
            TriangleMesh baseMesh = new TriangleMesh();

            int xSteps = (int)xStepsNumericUpDown.Value * 2;
            int ySteps = (int)yStepsNumericUpDown.Value * 2;
            double xLength = (double)xLengthNumericUpDown.Value;
            double yLength = (double)yLengthNumericUpDown.Value;
            double xMin = -(xLength * (int)xStepsNumericUpDown.Value) / 2;
            double yMin = -(yLength * (int)yStepsNumericUpDown.Value) / 2;

            baseMesh.Vertices.Add(new Vertex(xMin, yMin, 0));

            for (int i = 0; i <= ySteps; i++) {
                for (int j = 0; j <= xSteps; j++) {
                    if (i != 0) {
                        baseMesh.Vertices.Add((baseMesh.Vertices[0] + new Vector(j * xLength * 0.5, i * yLength * 0.5, 0)).ToVertex());
                    } else {
                        if (j != 0) {
                            baseMesh.Vertices.Add((baseMesh.Vertices[0] + new Vector(j * xLength * 0.5, i * yLength * 0.5, 0)).ToVertex());
                        }
                    }
                }
            }

            int point = 0;
            do {
                baseMesh.Add(new Triangle(point, point + 1, point + xSteps + 2));
                baseMesh.Add(new Triangle(point + 1, point + 2, point + xSteps + 2));
                baseMesh.Add(new Triangle(point + 2, point + xSteps + 3, point + xSteps + 2));
                baseMesh.Add(new Triangle(point + xSteps + 3, point + 2 * xSteps + 4, point + xSteps + 2));
                baseMesh.Add(new Triangle(point + 2 * xSteps + 4, point + 2 * xSteps + 3, point + xSteps + 2));
                baseMesh.Add(new Triangle(point + 2 * xSteps + 3, point + 2 * xSteps + 2, point + xSteps + 2));
                baseMesh.Add(new Triangle(point + 2 * xSteps + 2, point + xSteps + 1, point + xSteps + 2));
                baseMesh.Add(new Triangle(point + xSteps + 1, point, point + xSteps + 2));

                if (point % (2 * xSteps + 2) < xSteps - 2) { point += 2; } else { point += xSteps + 4; }
            } while (point <= baseMesh.Vertices.Count - (5 + 2 * xSteps));

            return baseMesh;
        }

        #endregion

        #region Event Handling Stuff

        #region Operators

        /// <summary>
        /// Adds the Tag of the pressed button in the operatorsGroupBox at the cursor-position
        /// in the functionTextBox, or replaces selected text with it.
        /// </summary>
        /// <param name="sender">Button in the operatorsGroupBox, with the exception of the backButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void OperatorButton_Click(object sender, RoutedEventArgs e) {
            if (functionTextBox.SelectedText != "") {
                functionTextBox.SelectedText = ((System.Windows.Controls.Button)sender).Tag.ToString();
                lastSelectionStart = functionTextBox.SelectionStart;
            } else {
                functionTextBox.Text = functionTextBox.Text.Insert(lastSelectionStart, ((System.Windows.Controls.Button)sender).Tag.ToString());
                functionTextBox.SelectionStart = lastSelectionStart = lastSelectionStart + ((System.Windows.Controls.Button)sender).Tag.ToString().Length;
            }

            functionTextBox.Focus();
        }

        /// <summary>
        /// Erases the symbol before the cursor-position or the selected text in the formulaTextBox.
        /// </summary>
        /// <param name="sender">eraseButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void EraseButton_Click(object sender, RoutedEventArgs e) {
            lastSelectionStart = functionTextBox.SelectionStart;
            if (functionTextBox.SelectedText != "") {
                functionTextBox.SelectedText = "";
            } else if (lastSelectionStart > 0) {
                String text = functionTextBox.Text.Substring(0, lastSelectionStart - 1) + functionTextBox.Text.Substring(lastSelectionStart, functionTextBox.Text.Length - lastSelectionStart);
                functionTextBox.Text = text;
                functionTextBox.SelectionStart = lastSelectionStart = lastSelectionStart - 1;
            }

            functionTextBox.Focus();
        }

        /// <summary>
        /// Sets the factorIndex to 0 for DEG.
        /// </summary>
        /// <param name="sender">degRadioButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void DEGRadioButton_Checked(object sender, RoutedEventArgs e) { factorIndex = 0; }

        /// <summary>
        /// Sets the factorIndex to 1 for RAD.
        /// </summary>
        /// <param name="sender">radRadioButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void RADRadioButton_Checked(object sender, RoutedEventArgs e) { factorIndex = 1; }

        /// <summary>
        /// Sets the factorIndex to 2 for GRAD.
        /// </summary>
        /// <param name="sender">gradRadioButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void GRADRadioButton_Checked(object sender, RoutedEventArgs e) { factorIndex = 2; }

        #endregion

        /// <summary>
        /// Creates the TriangleMesh and closes this Window.
        /// </summary>
        /// <param name="sender">okButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void OKButton_Click(object sender, RoutedEventArgs e) {
            Cursor = System.Windows.Input.Cursors.Wait;
#if !DEBUG
            try {
#endif
                if (meshType == 0) {
                    TriMMApp.Mesh = CreateHexBase();
                } else {
                    TriMMApp.Mesh = CreateSquareBase();
                }

                string function = (functionTextBox.Text == "") ? "0" : functionTextBox.Text;
                Calculator.factor = Calculator.factors[factorIndex];
                for (int i = 0; i < TriMMApp.Mesh.Vertices.Count; i++) {
                    String editedFormula = function;
                    editedFormula = editedFormula.Replace("x", "(" + TriMMApp.Mesh.Vertices[i][0].ToString() + ")");
                    editedFormula = editedFormula.Replace("y", "(" + TriMMApp.Mesh.Vertices[i][1].ToString() + ")");
                    TriMMApp.Mesh.Vertices[i][2] = Calculator.Evaluate(editedFormula);
                }

                TriMMApp.Mesh.Finish(true);
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
        /// Sets the lastSelectionStart, when the user presses a key in the functionTextBox manually.
        /// Also creates the mesh, when the enter-button was pressed.
        /// </summary>
        /// <param name="sender">functionTextBox</param>
        /// <param name="e">Standard KeyEventArgs</param>
        private void FunctionTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e) {
            lastSelectionStart = functionTextBox.SelectionStart;
            if (e.Key.Equals(Key.Enter)) { OKButton_Click(sender, new RoutedEventArgs()); }
        }

        /// <summary>
        /// Adjusts the FunctionWindow to create a hexagonal base mesh.
        /// </summary>
        /// <param name="sender">hexRadioButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void HexRadioButton_Checked(object sender, RoutedEventArgs e) {
            meshType = 0;
            if (stepsLabel != null) {
                stepsLabel.Visibility = stepsDockPanel.Visibility = lengthLabel.Visibility = lengthDockPanel.Visibility = Visibility.Visible;
                xLengthLabel.Visibility = xLengthDockPanel.Visibility = xStepsLabel.Visibility = xStepsDockPanel.Visibility
                    = yLengthLabel.Visibility = yLengthDockPanel.Visibility = yStepsLabel.Visibility = yStepsDockPanel.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Adjusts the FunctionWindow to create a rectangular base mesh.
        /// </summary>
        /// <param name="sender">squareRadioButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void SquareRadioButton_Checked(object sender, RoutedEventArgs e) {
            meshType = 1;
            if (stepsLabel != null) {
                stepsLabel.Visibility = stepsDockPanel.Visibility = lengthLabel.Visibility = lengthDockPanel.Visibility = Visibility.Collapsed;
                xLengthLabel.Visibility = xLengthDockPanel.Visibility = xStepsLabel.Visibility = xStepsDockPanel.Visibility
                    = yLengthLabel.Visibility = yLengthDockPanel.Visibility = yStepsLabel.Visibility = yStepsDockPanel.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Sets the MaxWidth of the functionTextBox to prevent the functionTextBox from widening the FunctionWindow.
        /// </summary>
        /// <param name="sender">this</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void Window_Loaded(object sender, RoutedEventArgs e) { functionTextBox.MaxWidth = functionTextBox.ActualWidth; }

        #endregion

    }
}