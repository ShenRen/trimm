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
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;


namespace TriMM {

    /// <summary>
    /// A Form displaying a TriMMControl and two ScrollBars to move the image,
    /// as well as button to center the image again.
    /// It also gives a choice of what to display and allows changing colors as well as making screenshots.
    /// </summary>
    public partial class TriMMView : Form {

        #region Fields

        private TriMMControl control;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the View with the OGLControl.
        /// </summary>
        /// <param name="control">Reference to the OGLControl to be displayed</param>
        public TriMMView(TriMMControl control) {
            InitializeComponent();
            this.control = control;
            panel.Controls.Add(this.control, 1, 0);
            ColorPanelButton_Click(new object(), new EventArgs());
            pickingModeComboBox.SelectedIndex = 0;

            backColorTextBox.Text = control.ClearColor.ToString();
            textColorTextBox.Text = control.TextColor.ToString();
            plainColorTextBox.Text = control.PlainColor.ToString();
            meshColorTextBox.Text = control.MeshColor.ToString();
            vertexColorTextBox.Text = control.VertexColor.ToString();
            normalColorTextBox.Text = control.NormalColor.ToString();
            xAxisColorTextBox.Text = control.XAxisColor.ToString();
            yAxisColorTextBox.Text = control.YAxisColor.ToString();
            zAxisColorTextBox.Text = control.ZAxisColor.ToString();
            observedVertexColorTextBox.Text = control.ObservedVertexColor.ToString();
            observedTriangleColorTextBox.Text = control.ObservedTriangleColor.ToString();

            this.control.VertexPicked += new VertexPickedEventHandler(Control_VertexPicked);
            this.control.EdgePicked += new EdgePickedEventHandler(Control_EdgePicked);
            this.control.TrianglePicked += new TrianglePickedEventHandler(Control_TrianglePicked);
            TriMM.Mesh.PickCleared += new PickClearedEventHandler(PickCleared);

            this.Show();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refreshes the maximum value for the observedNumericUpDown.
        /// </summary>
        public void RefreshView() {
            if (pickingModeComboBox.SelectedIndex == 1) {
                observedNumericUpDown.Maximum = (decimal)(TriMM.Mesh.Vertices.Count) - 1;
            } else if (pickingModeComboBox.SelectedIndex == 2) {
                observedNumericUpDown.Maximum = (decimal)(TriMM.Mesh.Edges.Count) - 1;
            } else if (pickingModeComboBox.SelectedIndex == 3) {
                observedNumericUpDown.Maximum = (decimal)(TriMM.Mesh.Count) - 1;
            }
            radiusNumericUpDown.Value = (decimal)control.ObservedRadius;
            observedNumericUpDown.Value = -1;
        }

        #endregion

        #region Event Handling Stuff


        /// <summary>
        /// Refreshes the TriMMControl, drawing the modell as a full, shaded object, if checked.
        /// </summary>
        /// <param name="sender">solidCheckBox</param>
        /// <param name="e">Standard EventArgs</param>
        private void SolidCheckBox_CheckedChanged(object sender, EventArgs e) {
            control.ShowModell = solidCheckBox.Checked;
            control.Refresh();
        }

        /// <summary>
        /// Refreshes the TriMMControl, drawing the modell as a mesh, if checked.
        /// </summary>
        /// <param name="sender">meshCheckBox</param>
        /// <param name="e">Standard EventArgs</param>
        private void MeshCheckBox_CheckedChanged(object sender, EventArgs e) {
            control.ShowMesh = meshCheckBox.Checked;
            control.Refresh();
        }

        /// <summary>
        /// Refreshes the TriMMControl, drawing the Vertices of the modell, if checked.
        /// </summary>
        /// <param name="sender">verticesCheckBox</param>
        /// <param name="e">Standard EventArgs</param>
        private void VerticesCheckBox_CheckedChanged(object sender, EventArgs e) {
            control.ShowVertices = verticesCheckBox.Checked;
            control.Refresh();
        }

        /// <summary>
        /// Refreshes the TriMMControl, drawing the Triangle normals, if checked.
        /// </summary>
        /// <param name="sender">facetNormalsCheckBox</param>
        /// <param name="e">Standard EventArgs</param>
        private void FacetNormalsCheckBox_CheckedChanged(object sender, EventArgs e) {
            control.ShowFacetNormalVectors = facetNormalsCheckBox.Checked;
            control.Refresh();
        }

        /// <summary>
        /// Refreshes the TriMMControl, drawing the Vertex normals, if checked.
        /// </summary>
        /// <param name="sender">vertexNormalsCheckBox</param>
        /// <param name="e">Standard EventArgs</param>
        private void VertexNormalsCheckBox_CheckedChanged(object sender, EventArgs e) {
            control.ShowVertexNormalVectors = vertexNormalsCheckBox.Checked;
            control.Refresh();
        }

        /// <summary>
        /// Refreshes the TriMMControl, drawing the coordinate axes, if checked.
        /// </summary>
        /// <param name="sender">axesCheckBox</param>
        /// <param name="e">Standard EventArgs</param>
        private void AxesCheckBox_CheckedChanged(object sender, EventArgs e) {
            control.ShowAxes = axesCheckBox.Checked;
            control.Refresh();
        }

        /// <summary>
        /// Refreshes the TriMMControl, drawing it smooth using the Vertex normals, if checked.
        /// </summary>
        /// <param name="sender">smoothCheckBox</param>
        /// <param name="e">Standard EventArgs</param>
        private void SmoothCheckBox_CheckedChanged(object sender, EventArgs e) {
            control.Smooth = smoothCheckBox.Checked;
            control.Refresh();
        }

        /// <summary>
        /// The image is translated into the opposite direction of the ScrollBar movement,
        /// or the camera is moved in the direction of the camera movement (equivalent).
        /// </summary>
        /// <param name="sender">xScrollBar</param>
        /// <param name="e">Standard ScrollEventArgs</param>
        private void XScrollBar_Scroll(object sender, ScrollEventArgs e) {
            control.Origin[0] = -TriMM.Mesh.Scale * xScrollBar.Value / 10000;
            control.Refresh();
        }

        /// <summary>
        /// The image is translated into the opposite direction of the ScrollBar movement,
        /// or the camera is moved in the direction of the camera movement (equivalent).
        /// </summary>
        /// <param name="sender">yScrollBar</param>
        /// <param name="e">Standard ScrollEventArgs</param>
        private void YScrollBar_Scroll(object sender, ScrollEventArgs e) {
            control.Origin[1] = TriMM.Mesh.Scale * yScrollBar.Value / 10000;
            control.Refresh();
        }

        /// <summary>
        /// The image is centered again.
        /// </summary>
        /// <param name="sender">centerButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void CenterButton_Click(object sender, EventArgs e) {
            xScrollBar.Value = yScrollBar.Value = 0;
            control.Origin[0] = control.Origin[1] = 0;
            control.Refresh();
        }

        /// <summary>
        /// Changes the picking mode, possible values: None, Vertex, Edge, Triangle,
        /// and makes the necessary adjustments.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PickingModeComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            control.PickingMode = pickingModeComboBox.SelectedIndex;
            observedNumericUpDown.Value = -1;
            if (pickingModeComboBox.SelectedIndex == 0) {
                observedLabel.Visible = observedNumericUpDown.Visible = radiusLabel.Visible = radiusNumericUpDown.Visible = clearObservedButton.Visible = false;
            } else if (pickingModeComboBox.SelectedIndex == 1) {
                observedNumericUpDown.Maximum = (decimal)(TriMM.Mesh.Vertices.Count) - 1;
                observedLabel.Visible = observedNumericUpDown.Visible = radiusLabel.Visible = radiusNumericUpDown.Visible = clearObservedButton.Visible = true;
            } else if (pickingModeComboBox.SelectedIndex == 2) {
                observedNumericUpDown.Maximum = (decimal)(TriMM.Mesh.Edges.Count) - 1;
                observedLabel.Visible = observedNumericUpDown.Visible = radiusLabel.Visible = radiusNumericUpDown.Visible = clearObservedButton.Visible = true;
            } else {
                observedNumericUpDown.Maximum = (decimal)(TriMM.Mesh.Count) - 1;
                observedLabel.Visible = observedNumericUpDown.Visible = clearObservedButton.Visible = true;
                radiusLabel.Visible = radiusNumericUpDown.Visible = false;
            }
        }

        /// <summary>
        /// Changes the observed Vertex.
        /// </summary>
        /// <param name="sender">observedNumericUpDown</param>
        /// <param name="e">Standard EventArgs</param>
        private void ObservedNumericUpDown_ValueChanged(object sender, EventArgs e) {
            if (observedNumericUpDown.Value == -1) {
                TriMM.Mesh.ObservedVertex = -1;
                control.Info.Clear();
                control.UseColorArray = false;
            } else {
                if (pickingModeComboBox.SelectedIndex == 1) {
                    control.PickVertex((int)observedNumericUpDown.Value);
                } else if (pickingModeComboBox.SelectedIndex == 2) {
                    control.PickEdge((int)observedNumericUpDown.Value);
                } else if (pickingModeComboBox.SelectedIndex == 3) {
                    control.PickTriangle((int)observedNumericUpDown.Value);
                }
            }
            control.Refresh();
        }

        /// <summary>
        /// Changes the radius of the sphere around the observed Vertex.
        /// </summary>
        /// <param name="sender">radiusNumericUpDown</param>
        /// <param name="e">Standard EventArgs</param>
        private void RadiusNumericUpDown_ValueChanged(object sender, EventArgs e) {
            control.ObservedRadius = (float)radiusNumericUpDown.Value;
            control.Refresh();
        }

        /// <summary>
        /// Deselects the observed Vertex.
        /// </summary>
        /// <param name="sender">clearObservedButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void ClearObservedButton_Click(object sender, EventArgs e) { observedNumericUpDown.Value = -1; }

        /// <summary>
        /// Sets the z-axis clipping plane to the selected value.
        /// </summary>
        /// <param name="sender">clippingPlaneNumericUpDown</param>
        /// <param name="e">Standard EventArgs</param>
        private void ClippingPlaneNumericUpDown_ValueChanged(object sender, EventArgs e) {
            control.ClippingPlane = (float)clippingPlaneNumericUpDown.Value;
            control.Refresh();
        }

        /// <summary>
        /// Resets rotation, translation, scaling and zoom values in the OGLControl.
        /// </summary>
        /// <param name="sender">resetViewButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void ResetViewButton_Click(object sender, EventArgs e) {
            xScrollBar.Value = yScrollBar.Value = 0;
            control.ResetView();
            radiusNumericUpDown.ValueChanged -= RadiusNumericUpDown_ValueChanged;
            radiusNumericUpDown.Value = (decimal)control.ObservedRadius;
            radiusNumericUpDown.ValueChanged += RadiusNumericUpDown_ValueChanged;
            clippingPlaneNumericUpDown.ValueChanged -= ClippingPlaneNumericUpDown_ValueChanged;
            clippingPlaneNumericUpDown.Value = (decimal)1.1;
            clippingPlaneNumericUpDown.ValueChanged += ClippingPlaneNumericUpDown_ValueChanged;
            solidCheckBox.Checked = meshCheckBox.Checked = true;
            verticesCheckBox.Checked = facetNormalsCheckBox.Checked = vertexNormalsCheckBox.Checked = smoothCheckBox.Checked = axesCheckBox.Checked = false;
            control.Refresh();
        }

        /// <summary>
        /// Makes a screenshot of the image displayed in the OGLControl and
        /// saves it as a *.png, *.bmp or *.jpg file the user chooses.
        /// </summary>
        /// <param name="sender">screenshotbutton</param>
        /// <param name="e">Standard EventArgs</param>
        private void ScreenshotButton_Click(object sender, EventArgs e) {
            System.Drawing.Imaging.ImageFormat imagef;
            System.Drawing.Bitmap image = control.Screenshot();
            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "PNG (*.png)|*.png|Bitmap (*.bmp)|*.bmp|JPEG (*.jpg)|*.jpg";
            sd.AddExtension = true;
            sd.OverwritePrompt = true;
            sd.DefaultExt = "png";
            if (sd.ShowDialog() == DialogResult.OK) {
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
        /// Opens the color panel or closes it.
        /// </summary>
        /// <param name="sender">colorPanelButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void ColorPanelButton_Click(object sender, EventArgs e) {
            if (splitContainer1.Panel2Collapsed) {
                colorPanelButton.Text = "<";
                this.Width += 140;
                this.MinimumSize = new Size(this.MinimumSize.Width + 140, this.MinimumSize.Height);
                splitContainer1.Panel2Collapsed = false;
            } else {
                colorPanelButton.Text = ">";
                splitContainer1.Panel2Collapsed = true;
                this.MinimumSize = new Size(this.MinimumSize.Width - 140, this.MinimumSize.Height);
                this.Width -= 140;
            }
        }

        /// <summary>
        /// Opens a ColorDialog allowing the user to choose a color to draw
        /// whatever is specified by the chosen button.
        /// </summary>
        /// <param name="sender">One of the color buttons</param>
        /// <param name="e">Standard EventArgs</param>
        private void ColorButton_Click(object sender, EventArgs e) {
            ColorDialog cd = new ColorDialog();

            if (cd.ShowDialog() == DialogResult.OK) {

                ColorOGL newColor = new ColorOGL(cd.Color);

                switch (((Button)sender).Tag.ToString()) {
                    case "0":
                        control.ClearColor = newColor;
                        backColorTextBox.Text = control.ClearColor.ToString();
                        break;
                    case "1":
                        control.TextColor = newColor;
                        textColorTextBox.Text = control.TextColor.ToString();
                        break;
                    case "2":
                        control.PlainColor = newColor;
                        plainColorTextBox.Text = control.PlainColor.ToString();
                        break;
                    case "3":
                        control.MeshColor = newColor;
                        meshColorTextBox.Text = control.MeshColor.ToString();
                        break;
                    case "4":
                        control.VertexColor = newColor;
                        vertexColorTextBox.Text = control.VertexColor.ToString();
                        break;
                    case "5":
                        control.NormalColor = newColor;
                        normalColorTextBox.Text = control.NormalColor.ToString();
                        break;
                    case "6":
                        control.XAxisColor = newColor;
                        xAxisColorTextBox.Text = control.XAxisColor.ToString();
                        break;
                    case "7":
                        control.YAxisColor = newColor;
                        yAxisColorTextBox.Text = control.YAxisColor.ToString();
                        break;
                    case "8":
                        control.ZAxisColor = newColor;
                        zAxisColorTextBox.Text = control.ZAxisColor.ToString();
                        break;
                    case "9":
                        control.ObservedVertexColor = newColor;
                        observedVertexColorTextBox.Text = control.ObservedVertexColor.ToString();
                        break;
                    case "10":
                        control.ObservedTriangleColor = newColor;
                        observedTriangleColorTextBox.Text = control.ObservedTriangleColor.ToString();
                        break;
                }

                control.Refresh();
            }
        }

        /// <summary>
        /// Resets the colors to the standard values.
        /// </summary>
        /// <param name="sender">standardColorsButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void StandardColorsButton_Click(object sender, EventArgs e) {
            control.ResetColors();
            backColorTextBox.Text = control.ClearColor.ToString();
            textColorTextBox.Text = control.TextColor.ToString();
            plainColorTextBox.Text = control.PlainColor.ToString();
            meshColorTextBox.Text = control.MeshColor.ToString();
            vertexColorTextBox.Text = control.VertexColor.ToString();
            normalColorTextBox.Text = control.NormalColor.ToString();
            xAxisColorTextBox.Text = control.XAxisColor.ToString();
            yAxisColorTextBox.Text = control.YAxisColor.ToString();
            zAxisColorTextBox.Text = control.ZAxisColor.ToString();
            observedVertexColorTextBox.Text = control.ObservedVertexColor.ToString();
            observedTriangleColorTextBox.Text = control.ObservedTriangleColor.ToString();
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
            control.Refresh();
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
            control.Refresh();
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
            control.Refresh();
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
        /// The TriMMControl is set free, so that it is not destroyed with the View.
        /// </summary>
        /// <param name="sender">Whatever closes the View</param>
        /// <param name="e">Standard FormClosingEventArgs</param>
        private void View_FormClosing(object sender, FormClosingEventArgs e) {
            panel.Controls.Remove(control);
            ResetViewButton_Click(sender, e);
        }

        #endregion

    }
}