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
using System.Windows.Controls;
using System.Windows.Forms;

namespace TriMM {

    /// <summary>
    /// A Window displaying a TriMMControl and two ScrollBars to move the image,
    /// as well as button to center the image again.
    /// It also gives a choice of what to display and allows changing colors as well as making screenshots.
    /// </summary>
    public partial class TriMMView : Window {

        #region Fields

        private NumericUpDown observedNumericUpDown = new NumericUpDown();
        private NumericUpDown radiusNumericUpDown = new NumericUpDown();
        private NumericUpDown clippingPlaneNumericUpDown = new NumericUpDown();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the TriMMView at the chosen position.
        /// </summary>
        /// <param name="left">The distance of the TriMMView from the left screen border.</param>
        /// <param name="top">The distance of the TriMMView from the top screen border.</param>
        public TriMMView(double left, double top) : this() {
            Left = left;
            Top = top;
        }

        /// <summary>
        /// Initializes the TriMMView.
        /// </summary>
        public TriMMView() {
            InitializeComponent(); 
            pickingModeComboBox.SelectionChanged += PickingModeComboBox_SelectionChanged;

            this.Icon = TriMMApp.Image;
            clippingPlaneNumericUpDown.Minimum = -1.1m;
            clippingPlaneNumericUpDown.Maximum = 1.1m;
            clippingPlaneNumericUpDown.Value = 1.1m;
            clippingPlaneNumericUpDown.Increment = 0.1m;
            clippingPlaneNumericUpDown.DecimalPlaces = 7;
            clippingPlaneNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            clippingPlaneWFHost.Child = clippingPlaneNumericUpDown;
            observedNumericUpDown.Minimum = -1;
            observedNumericUpDown.Maximum = -1;
            observedNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            observedWFHost.Child = observedNumericUpDown;
            radiusNumericUpDown.DecimalPlaces = 7;
            radiusNumericUpDown.Increment = 0.01m;
            radiusNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            radiusWFHost.Child = radiusNumericUpDown;
            controlWFHost.Child = TriMMApp.Control;
            TriMMApp.Control.Width = 584;
            TriMMApp.Control.Height = 574;

            smoothCheckBox.IsChecked = TriMMApp.Settings.Smooth;
            solidCheckBox.IsChecked = TriMMApp.Settings.Solid;
            meshCheckBox.IsChecked = TriMMApp.Settings.Mesh;
            verticesCheckBox.IsChecked = TriMMApp.Settings.Vertices;
            triangleNormalsCheckBox.IsChecked = TriMMApp.Settings.TriangleNormalVectors;
            vertexNormalsCheckBox.IsChecked = TriMMApp.Settings.VertexNormalVectors;
            xAxisCheckBox.IsChecked = TriMMApp.Settings.XAxis;
            yAxisCheckBox.IsChecked = TriMMApp.Settings.YAxis;
            zAxisCheckBox.IsChecked = TriMMApp.Settings.ZAxis;
            pickingModeComboBox.SelectedIndex = TriMMApp.Settings.PickingMode;

            RefreshView();

            observedNumericUpDown.ValueChanged += new EventHandler(ObservedNumericUpDown_ValueChanged);
            radiusNumericUpDown.ValueChanged += new EventHandler(RadiusNumericUpDown_ValueChanged);
            clippingPlaneNumericUpDown.ValueChanged += new EventHandler(ClippingPlaneNumericUpDown_ValueChanged);
            TriMMApp.Control.VertexPicked += new VertexPickedEventHandler(Control_VertexPicked);
            TriMMApp.Control.EdgePicked += new EdgePickedEventHandler(Control_EdgePicked);
            TriMMApp.Control.TrianglePicked += new TrianglePickedEventHandler(Control_TrianglePicked);
            TriMMApp.Mesh.PickCleared += new PickClearedEventHandler(PickCleared);
            TriMMApp.Mesh.MinEdgeLengthChanged += new MinEdgeLengthChangedEventHandler(Mesh_MinEdgeLengthChanged);
            TriMMApp.Settings.SettingsChanged += new SettingsChangedEventHandler(Settings_SettingsChanged);

            this.Show();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refreshes the maximum value for the observedNumericUpDown.
        /// </summary>
        public void RefreshView() {
            if (pickingModeComboBox.SelectedIndex == 1) {
                observedNumericUpDown.Maximum = (decimal)(TriMMApp.Mesh.Vertices.Count) - 1;
            } else if (pickingModeComboBox.SelectedIndex == 2) {
                observedNumericUpDown.Maximum = (decimal)(TriMMApp.Mesh.Edges.Count) - 1;
            } else if (pickingModeComboBox.SelectedIndex == 3) {
                observedNumericUpDown.Maximum = (decimal)(TriMMApp.Mesh.Count) - 1;
            }
            radiusNumericUpDown.Value = (decimal)TriMMApp.Control.ObservedRadius;
            observedNumericUpDown.Value = -1;
        }

        #endregion

        #region Event Handling Stuff

        /// <summary>
        /// Changes the setting for drawing the modell with or without the vertex normals.
        /// </summary>
        /// <param name="sender">smoothCheckBox</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void SmoothCheckBox_CheckedChanged(object sender, RoutedEventArgs e) { TriMMApp.Settings.Smooth = smoothCheckBox.IsChecked.Value; }

        /// <summary>
        /// Changes the setting for drawing the modell as a solid object.
        /// </summary>
        /// <param name="sender">solidCheckBox</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void SolidCheckBox_CheckedChanged(object sender, RoutedEventArgs e) { TriMMApp.Settings.Solid = solidCheckBox.IsChecked.Value; }

        /// <summary>
        /// Changes the setting for drawing the modells mesh.
        /// </summary>
        /// <param name="sender">meshCheckBox</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void MeshCheckBox_CheckedChanged(object sender, RoutedEventArgs e) { TriMMApp.Settings.Mesh = meshCheckBox.IsChecked.Value; }

        /// <summary>
        /// Changes the setting for drawing the modells vertices.
        /// </summary>
        /// <param name="sender">verticesCheckBox</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void VerticesCheckBox_CheckedChanged(object sender, RoutedEventArgs e) { TriMMApp.Settings.Vertices = verticesCheckBox.IsChecked.Value; }

        /// <summary>
        /// Changes the setting for drawing the triangle normals.
        /// </summary>
        /// <param name="sender">facetNormalsCheckBox</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void TriangleNormalsCheckBox_CheckedChanged(object sender, RoutedEventArgs e) { TriMMApp.Settings.TriangleNormalVectors = triangleNormalsCheckBox.IsChecked.Value; }

        /// <summary>
        /// Changes the setting for drawing the Vertex normals.
        /// </summary>
        /// <param name="sender">vertexNormalsCheckBox</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void VertexNormalsCheckBox_CheckedChanged(object sender, RoutedEventArgs e) { TriMMApp.Settings.VertexNormalVectors = vertexNormalsCheckBox.IsChecked.Value; }

        /// <summary>
        /// Changes the setting for drawing the coordinate axes.
        /// </summary>
        /// <param name="sender">xAxisCheckBox</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void XAxisCheckBox_CheckedChanged(object sender, RoutedEventArgs e) { TriMMApp.Settings.XAxis = xAxisCheckBox.IsChecked.Value; }

        /// <summary>
        /// Changes the setting for drawing the coordinate axes.
        /// </summary>
        /// <param name="sender">yAxisCheckBox</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void YAxisCheckBox_CheckedChanged(object sender, RoutedEventArgs e) { TriMMApp.Settings.YAxis = yAxisCheckBox.IsChecked.Value; }

        /// <summary>
        /// Changes the setting for drawing the coordinate axes.
        /// </summary>
        /// <param name="sender">zAxisCheckBox</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void ZAxisCheckBox_CheckedChanged(object sender, RoutedEventArgs e) { TriMMApp.Settings.ZAxis = zAxisCheckBox.IsChecked.Value; }

        /// <summary>
        /// The image is translated into the opposite direction of the ScrollBar movement,
        /// or the camera is moved in the direction of the ScrollBar movement (equivalent).
        /// </summary>
        /// <param name="sender">xScrollBar</param>
        /// <param name="e">Standard ScrollEventArgs</param>
        private void XScrollBar_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e) {
            TriMMApp.Control.Origin[0] = (float)(-TriMMApp.Mesh.Scale * xScrollBar.Value / 10000);
            TriMMApp.Control.Refresh();
        }

        /// <summary>
        /// The image is translated into the opposite direction of the ScrollBar movement,
        /// or the camera is moved in the direction of the ScrollBar movement (equivalent).
        /// </summary>
        /// <param name="sender">yScrollBar</param>
        /// <param name="e">Standard ScrollEventArgs</param>
        private void YScrollBar_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e) {
            TriMMApp.Control.Origin[1] = (float)(TriMMApp.Mesh.Scale * yScrollBar.Value / 10000);
            TriMMApp.Control.Refresh();
        }

        /// <summary>
        /// The image is centered again.
        /// </summary>
        /// <param name="sender">centerButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void CenterButton_Click(object sender, RoutedEventArgs e) {
            xScrollBar.Value = yScrollBar.Value = 0;
            TriMMApp.Control.Origin[0] = TriMMApp.Control.Origin[1] = 0;
            TriMMApp.Control.Refresh();
        }

        /// <summary>
        /// Changes the picking mode, possible values: None, Vertex, Edge, Triangle,
        /// and makes the necessary adjustments.
        /// </summary>
        /// <param name="sender">pickingModeComboBox</param>
        /// <param name="e">Standard SelectionChangedEventArgs</param>
        private void PickingModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            TriMMApp.Settings.PickingMode = pickingModeComboBox.SelectedIndex;
            observedNumericUpDown.Value = -1;
            if (pickingModeComboBox.SelectedIndex == 0) {
                observedLabel.Visibility = observedWFHost.Visibility = radiusLabel.Visibility = radiusWFHost.Visibility = clearObservedButton.Visibility = Visibility.Hidden;
            } else if (pickingModeComboBox.SelectedIndex == 1) {
                observedNumericUpDown.Maximum = (decimal)(TriMMApp.Mesh.Vertices.Count) - 1;
                observedLabel.Visibility = observedWFHost.Visibility = radiusLabel.Visibility = radiusWFHost.Visibility = clearObservedButton.Visibility = Visibility.Visible;
            } else if (pickingModeComboBox.SelectedIndex == 2) {
                observedNumericUpDown.Maximum = (decimal)(TriMMApp.Mesh.Edges.Count) - 1;
                observedLabel.Visibility = observedWFHost.Visibility = radiusLabel.Visibility = radiusWFHost.Visibility = clearObservedButton.Visibility = Visibility.Visible;
            } else {
                observedNumericUpDown.Maximum = (decimal)(TriMMApp.Mesh.Count) - 1;
                observedLabel.Visibility = observedWFHost.Visibility = clearObservedButton.Visibility = Visibility.Visible;
                radiusLabel.Visibility = radiusWFHost.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Changes the observed Vertex.
        /// </summary>
        /// <param name="sender">observedNumericUpDown</param>
        /// <param name="e">Standard EventArgs</param>
        private void ObservedNumericUpDown_ValueChanged(object sender, EventArgs e) {
            if (observedNumericUpDown.Value == -1) {
                TriMMApp.Mesh.ObservedVertex = -1;
                TriMMApp.Control.Info.Clear();
                TriMMApp.Control.UseColorArray = false;
            } else {
                if (pickingModeComboBox.SelectedIndex == 1) {
                    TriMMApp.Control.PickVertex((int)observedNumericUpDown.Value);
                } else if (pickingModeComboBox.SelectedIndex == 2) {
                    TriMMApp.Control.PickEdge((int)observedNumericUpDown.Value);
                } else if (pickingModeComboBox.SelectedIndex == 3) {
                    TriMMApp.Control.PickTriangle((int)observedNumericUpDown.Value);
                }
            }
            TriMMApp.Control.Refresh();
        }

        /// <summary>
        /// Changes the radius of the sphere around the observed Vertex.
        /// </summary>
        /// <param name="sender">radiusNumericUpDown</param>
        /// <param name="e">Standard EventArgs</param>
        private void RadiusNumericUpDown_ValueChanged(object sender, EventArgs e) {
            TriMMApp.Control.ObservedRadius = (float)radiusNumericUpDown.Value;
            TriMMApp.Control.Refresh();
        }

        /// <summary>
        /// Deselects the observed Vertex.
        /// </summary>
        /// <param name="sender">clearObservedButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void ClearObservedButton_Click(object sender, RoutedEventArgs e) { observedNumericUpDown.Value = -1; }

        /// <summary>
        /// Sets the z-axis clipping plane to the selected value.
        /// </summary>
        /// <param name="sender">clippingPlaneNumericUpDown</param>
        /// <param name="e">Standard EventArgs</param>
        private void ClippingPlaneNumericUpDown_ValueChanged(object sender, EventArgs e) {
            TriMMApp.Control.ClippingPlane = (float)clippingPlaneNumericUpDown.Value;
            TriMMApp.Control.Refresh();
        }

        /// <summary>
        /// Opens the SettingsWindow to change the standard Settings.
        /// </summary>
        /// <param name="sender">settingsButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void SettingsButton_Click(object sender, RoutedEventArgs e) {
            SettingsWindow sform = new SettingsWindow();
            sform.ShowDialog();
        }

        /// <summary>
        /// Makes a screenshot of the image displayed in the TriMMControl and
        /// saves it as a *.png, *.bmp or *.jpg file the user chooses.
        /// </summary>
        /// <param name="sender">screenshotButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void ScreenshotButton_Click(object sender, RoutedEventArgs e) {
            System.Drawing.Imaging.ImageFormat imagef;
            System.Drawing.Bitmap image = TriMMApp.Control.Screenshot();
            Microsoft.Win32.SaveFileDialog sd = new Microsoft.Win32.SaveFileDialog();
            sd.Filter = "PNG (*.png)|*.png|Bitmap (*.bmp)|*.bmp|JPEG (*.jpg)|*.jpg";
            sd.AddExtension = true;
            sd.OverwritePrompt = true;
            sd.DefaultExt = "png";
            if (sd.ShowDialog() == true) {
                switch (sd.FilterIndex) {
                    case 2:
                        imagef = System.Drawing.Imaging.ImageFormat.Bmp;
                        break;
                    case 3:
                        imagef = System.Drawing.Imaging.ImageFormat.Jpeg;
                        break;
                    default:
                        imagef = System.Drawing.Imaging.ImageFormat.Png;
                        break;
                }
                image.Save(sd.FileName, imagef);
            }
        }

        /// <summary>
        /// Resets rotation, translation, scaling and zoom values in the TriMMControl.
        /// </summary>
        /// <param name="sender">resetViewButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void ResetViewButton_Click(object sender, RoutedEventArgs e) {
            xScrollBar.Value = yScrollBar.Value = 0;
            TriMMApp.Control.ResetView();
            radiusNumericUpDown.ValueChanged -= RadiusNumericUpDown_ValueChanged;
            radiusNumericUpDown.Value = (decimal)TriMMApp.Control.ObservedRadius;
            radiusNumericUpDown.ValueChanged += RadiusNumericUpDown_ValueChanged;
            clippingPlaneNumericUpDown.ValueChanged -= ClippingPlaneNumericUpDown_ValueChanged;
            clippingPlaneNumericUpDown.Value = (decimal)1.1;
            clippingPlaneNumericUpDown.ValueChanged += ClippingPlaneNumericUpDown_ValueChanged;
        }

        /// <summary>
        /// Changes the observed Vertex to the one picked in the visualisation.
        /// </summary>
        /// <param name="picked">Index of the observed Vertex</param>
        private void Control_VertexPicked(List<int> picked) {
            observedNumericUpDown.ValueChanged -= ObservedNumericUpDown_ValueChanged;
            if (picked.Count != 0) {
                observedNumericUpDown.Value = picked[0];
            } else {
                observedNumericUpDown.Value = -1;
            }
            TriMMApp.Control.Refresh();
            observedNumericUpDown.ValueChanged += ObservedNumericUpDown_ValueChanged;
        }

        /// <summary>
        /// Changes the observed Edge to the one picked in the visualisation.
        /// </summary>
        /// <param name="picked">Index of the observed Edge</param>
        void Control_EdgePicked(List<int> picked) {
            observedNumericUpDown.ValueChanged -= ObservedNumericUpDown_ValueChanged;
            if (picked.Count != 0) {
                observedNumericUpDown.Value = picked[0];
            } else {
                observedNumericUpDown.Value = -1;
            }
            TriMMApp.Control.Refresh();
            observedNumericUpDown.ValueChanged += ObservedNumericUpDown_ValueChanged;
        }

        /// <summary>
        /// Changes the observed Triangle to the one picked in the visualisation.
        /// </summary>
        /// <param name="picked">Index of the observed Triangle</param>
        private void Control_TrianglePicked(List<int> picked) {
            observedNumericUpDown.ValueChanged -= ObservedNumericUpDown_ValueChanged;
            if (picked.Count != 0) {
                observedNumericUpDown.Value = picked[0];
            } else {
                observedNumericUpDown.Value = -1;
            }
            TriMMApp.Control.Refresh();
            observedNumericUpDown.ValueChanged += ObservedNumericUpDown_ValueChanged;
        }

        /// <summary>
        /// Sets the observedNumericUpDown to -1, when the PickCleared event was thrown somewhere else.
        /// </summary>
        private void PickCleared() {
            observedNumericUpDown.ValueChanged -= ObservedNumericUpDown_ValueChanged;
            observedNumericUpDown.Value = -1;
            observedNumericUpDown.ValueChanged += ObservedNumericUpDown_ValueChanged;
        }

        /// <summary>
        /// Changes the value of the radiusNumericUpDown, when the minimum Edge length in the mesh is changed.
        /// Because of the change to the radiusNumericUpDown the TriMMControl is also notified of the change.
        /// </summary>
        /// <param name="newLength">The new minimum length.</param>
        private void Mesh_MinEdgeLengthChanged(double newLength) { radiusNumericUpDown.Value = (decimal)newLength / 2; }

        /// <summary>
        /// Refreshes the settings within the TriMMView, when the Settings are changed.
        /// </summary>
        private void Settings_SettingsChanged() {
            smoothCheckBox.IsChecked = TriMMApp.Settings.Smooth;
            solidCheckBox.IsChecked = TriMMApp.Settings.Solid;
            meshCheckBox.IsChecked = TriMMApp.Settings.Mesh;
            verticesCheckBox.IsChecked = TriMMApp.Settings.Vertices;
            triangleNormalsCheckBox.IsChecked = TriMMApp.Settings.TriangleNormalVectors;
            vertexNormalsCheckBox.IsChecked = TriMMApp.Settings.VertexNormalVectors;
            xAxisCheckBox.IsChecked = TriMMApp.Settings.XAxis;
            yAxisCheckBox.IsChecked = TriMMApp.Settings.YAxis;
            zAxisCheckBox.IsChecked = TriMMApp.Settings.ZAxis;
            pickingModeComboBox.SelectedIndex = TriMMApp.Settings.PickingMode;

            TriMMApp.Control.Refresh();
        }

        #endregion

    }
}
