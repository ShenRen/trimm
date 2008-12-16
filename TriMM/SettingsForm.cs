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
using System.IO;
using System.Windows.Forms;

namespace TriMM {

    /// <summary>
    /// A Form to change the Settings for visualization.
    /// </summary>
    public partial class SettingsForm : Form {

        #region Constructors

        /// <summary>
        /// Initializes the Form with the current values.
        /// </summary>
        public SettingsForm() {
            InitializeComponent();

            // Colors
            backColorTextBox.Text = TriMM.Settings.BackColor.ToString();
            textColorTextBox.Text = TriMM.Settings.TextColor.ToString();
            plainColorTextBox.Text = TriMM.Settings.PlainColor.ToString();
            meshColorTextBox.Text = TriMM.Settings.MeshColor.ToString();
            vertexColorTextBox.Text = TriMM.Settings.VertexColor.ToString();
            normalColorTextBox.Text = TriMM.Settings.NormalColor.ToString();
            xAxisColorTextBox.Text = TriMM.Settings.XAxisColor.ToString();
            yAxisColorTextBox.Text = TriMM.Settings.YAxisColor.ToString();
            zAxisColorTextBox.Text = TriMM.Settings.ZAxisColor.ToString();
            observedVertexColorTextBox.Text = TriMM.Settings.ObservedVertexColor.ToString();
            observedTriangleColorTextBox.Text = TriMM.Settings.ObservedTriangleColor.ToString();

            // Display
            smoothCheckBox.Checked = TriMM.Settings.Smooth;
            solidCheckBox.Checked = TriMM.Settings.Solid;
            meshCheckBox.Checked = TriMM.Settings.Mesh;
            verticesCheckBox.Checked = TriMM.Settings.Vertices;
            axesCheckBox.Checked = TriMM.Settings.Axes;
            triangleNormalsCheckBox.Checked = TriMM.Settings.TriangleNormalVectors;
            vertexNormalsCheckBox.Checked = TriMM.Settings.VertexNormalVectors;
            pickingModeComboBox.SelectedIndex = TriMM.Settings.PickingMode;

            TriMM.Settings.SettingsChanged += new SettingsChangedEventHandler(Settings_SettingsChanged);
        }

        #endregion

        #region Event Handling Stuff

        #region Menu

        /// <summary>
        /// Imports the Settings from a .set file.
        /// </summary>
        /// <param name="sender">loadSettingsToolStripMenuItem</param>
        /// <param name="e">Standard EventArgs</param>
        private void LoadSettingsToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.DefaultExt = "set";
            ofd.Filter = "TriMM Settings (*.set)|*.set";
            ofd.Multiselect = false;
            ofd.Title = "Open File";
            if (ofd.ShowDialog() == DialogResult.OK) { TriMM.Settings.Parse(ofd.FileName); }
            ofd.Dispose();
        }

        /// <summary>
        /// Exports the Settings to a .set file.
        /// </summary>
        /// <param name="sender">saveSettingsToolStripMenuItem</param>
        /// <param name="e">Standard EventArgs</param>
        private void SaveSettingsToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.OverwritePrompt = true;
            sfd.DefaultExt = "set";
            sfd.Filter = "TriMM Settings (*.set)|*.set";
            sfd.Title = "Save File";
            if (sfd.ShowDialog() == DialogResult.OK) { TriMM.Settings.WriteSET(sfd.FileName); }
            sfd.Dispose();
        }

        /// <summary>
        /// Resets the settings to the default values.
        /// </summary>
        /// <param name="sender">loadDefaultToolStripMenuItem</param>
        /// <param name="e">Standard EventArgs</param>
        private void LoadDefaultToolStripMenuItem_Click(object sender, EventArgs e) { TriMM.Settings.SetToDefault(); }

        /// <summary>
        /// Resets the settings to the standard values.
        /// </summary>
        /// <param name="sender">loadStandardToolStripMenuItem</param>
        /// <param name="e">Standard EventArgs</param>
        private void LoadStandardToolStripMenuItem_Click(object sender, EventArgs e) { TriMM.Settings.SetToStandard(); }

        /// <summary>
        /// Accepts the current settings as the standard values.
        /// </summary>
        /// <param name="sender">makeStandardToolStripMenuItem</param>
        /// <param name="e">Standard EventArgs</param>
        private void MakeStandardToolStripMenuItem_Click(object sender, EventArgs e) { TriMM.Settings.MakeStandard(); }

        #endregion

        #region Colors

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
                        TriMM.Settings.BackColor = newColor;
                        backColorTextBox.Text = TriMM.Settings.BackColor.ToString();
                        break;
                    case "1":
                        TriMM.Settings.TextColor = newColor;
                        textColorTextBox.Text = TriMM.Settings.TextColor.ToString();
                        break;
                    case "2":
                        TriMM.Settings.PlainColor = newColor;
                        plainColorTextBox.Text = TriMM.Settings.PlainColor.ToString();
                        break;
                    case "3":
                        TriMM.Settings.MeshColor = newColor;
                        meshColorTextBox.Text = TriMM.Settings.MeshColor.ToString();
                        break;
                    case "4":
                        TriMM.Settings.VertexColor = newColor;
                        vertexColorTextBox.Text = TriMM.Settings.VertexColor.ToString();
                        break;
                    case "5":
                        TriMM.Settings.NormalColor = newColor;
                        normalColorTextBox.Text = TriMM.Settings.NormalColor.ToString();
                        break;
                    case "6":
                        TriMM.Settings.XAxisColor = newColor;
                        xAxisColorTextBox.Text = TriMM.Settings.XAxisColor.ToString();
                        break;
                    case "7":
                        TriMM.Settings.YAxisColor = newColor;
                        yAxisColorTextBox.Text = TriMM.Settings.YAxisColor.ToString();
                        break;
                    case "8":
                        TriMM.Settings.ZAxisColor = newColor;
                        zAxisColorTextBox.Text = TriMM.Settings.ZAxisColor.ToString();
                        break;
                    case "9":
                        TriMM.Settings.ObservedVertexColor = newColor;
                        observedVertexColorTextBox.Text = TriMM.Settings.ObservedVertexColor.ToString();
                        break;
                    case "10":
                        TriMM.Settings.ObservedTriangleColor = newColor;
                        observedTriangleColorTextBox.Text = TriMM.Settings.ObservedTriangleColor.ToString();
                        break;
                }
            }
        }

        /// <summary>
        /// Resets the colors to the default values.
        /// </summary>
        /// <param name="sender">defaultColorsButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void DefaultColorsButton_Click(object sender, EventArgs e) { TriMM.Settings.SetToDefaultColors(); }

        /// <summary>
        /// Resets the colors to the standard values.
        /// </summary>
        /// <param name="sender">standardColorsButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void StandardColorsButton_Click(object sender, EventArgs e) { TriMM.Settings.SetToStandardColors(); }

        /// <summary>
        /// Accepts the current colors as the standard colors.
        /// </summary>
        /// <param name="sender">makeStandardColorsButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void MakeStandardColorsButton_Click(object sender, EventArgs e) { TriMM.Settings.MakeStandardColors(); }

        #endregion

        #region Display

        /// <summary>
        /// Changes the setting for drawing the modell with or without the vertex normals.
        /// </summary>
        /// <param name="sender">smoothCheckBox</param>
        /// <param name="e">Standard EventArgs</param>
        private void SmoothCheckBox_CheckedChanged(object sender, EventArgs e) { TriMM.Settings.Smooth = smoothCheckBox.Checked; }

        /// <summary>
        /// Changes the setting for drawing the modell as a solid object.
        /// </summary>
        /// <param name="sender">solidCheckBox</param>
        /// <param name="e">Standard EventArgs</param>
        private void SolidCheckBox_CheckedChanged(object sender, EventArgs e) { TriMM.Settings.Solid = solidCheckBox.Checked; }

        /// <summary>
        /// Changes the setting for drawing the modells mesh.
        /// </summary>
        /// <param name="sender">meshCheckBox</param>
        /// <param name="e">Standard EventArgs</param>
        private void MeshCheckBox_CheckedChanged(object sender, EventArgs e) { TriMM.Settings.Mesh = meshCheckBox.Checked; }

        /// <summary>
        /// Changes the setting for drawing the modells vertices.
        /// </summary>
        /// <param name="sender">verticesCheckBox</param>
        /// <param name="e">Standard EventArgs</param>
        private void VerticesCheckBox_CheckedChanged(object sender, EventArgs e) { TriMM.Settings.Vertices = verticesCheckBox.Checked; }

        /// <summary>
        /// Changes the setting for drawing the triangle normals.
        /// </summary>
        /// <param name="sender">triangleNormalsCheckBox</param>
        /// <param name="e">Standard EventArgs</param>
        private void TriangleNormalsCheckBox_CheckedChanged(object sender, EventArgs e) { TriMM.Settings.TriangleNormalVectors = triangleNormalsCheckBox.Checked; }

        /// <summary>
        /// Changes the setting for drawing the Vertex normals.
        /// </summary>
        /// <param name="sender">vertexNormalsCheckBox</param>
        /// <param name="e">Standard EventArgs</param>
        private void VertexNormalsCheckBox_CheckedChanged(object sender, EventArgs e) { TriMM.Settings.VertexNormalVectors = vertexNormalsCheckBox.Checked; }

        /// <summary>
        /// Changes the setting for drawing the coordinate axes.
        /// </summary>
        /// <param name="sender">axesCheckBox</param>
        /// <param name="e">Standard EventArgs</param>
        private void AxesCheckBox_CheckedChanged(object sender, EventArgs e) { TriMM.Settings.Axes = axesCheckBox.Checked; }

        /// <summary>
        /// Changes the picking mode, possible values: None, Vertex, Edge, Triangle,  and makes the necessary adjustments.
        /// </summary>
        /// <param name="sender">pickingModeComboBox</param>
        /// <param name="e">Standard EventArgs</param>
        private void PickingModeComboBox_SelectedIndexChanged(object sender, EventArgs e) { TriMM.Settings.PickingMode = pickingModeComboBox.SelectedIndex; }

        /// <summary>
        /// Resets the display settings to the default values.
        /// </summary>
        /// <param name="sender">defaultDisplayButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void DefaultDisplayButton_Click(object sender, EventArgs e) { TriMM.Settings.SetToDefaultDisplay(); }

        /// <summary>
        /// Resets the display settings to the standard values.
        /// </summary>
        /// <param name="sender">standardDisplayButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void StandardDisplayButton_Click(object sender, EventArgs e) { TriMM.Settings.SetToStandardDisplay(); }

        /// <summary>
        /// Accepts the current display settings as the standard values.
        /// </summary>
        /// <param name="sender">makeStandardDisplayButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void MakeStandardDisplayButton_Click(object sender, EventArgs e) { TriMM.Settings.MakeStandardDisplay(); }

        #endregion

        /// <summary>
        /// Adjusts the values in the SettingsForm, when the Settings were changed.
        /// </summary>
        private void Settings_SettingsChanged() {
            // Colors
            backColorTextBox.Text = TriMM.Settings.BackColor.ToString();
            textColorTextBox.Text = TriMM.Settings.TextColor.ToString();
            plainColorTextBox.Text = TriMM.Settings.PlainColor.ToString();
            meshColorTextBox.Text = TriMM.Settings.MeshColor.ToString();
            vertexColorTextBox.Text = TriMM.Settings.VertexColor.ToString();
            normalColorTextBox.Text = TriMM.Settings.NormalColor.ToString();
            xAxisColorTextBox.Text = TriMM.Settings.XAxisColor.ToString();
            yAxisColorTextBox.Text = TriMM.Settings.YAxisColor.ToString();
            zAxisColorTextBox.Text = TriMM.Settings.ZAxisColor.ToString();
            observedVertexColorTextBox.Text = TriMM.Settings.ObservedVertexColor.ToString();
            observedTriangleColorTextBox.Text = TriMM.Settings.ObservedTriangleColor.ToString();

            // Display
            smoothCheckBox.Checked = TriMM.Settings.Smooth;
            solidCheckBox.Checked = TriMM.Settings.Solid;
            meshCheckBox.Checked = TriMM.Settings.Mesh;
            verticesCheckBox.Checked = TriMM.Settings.Vertices;
            axesCheckBox.Checked = TriMM.Settings.Axes;
            triangleNormalsCheckBox.Checked = TriMM.Settings.TriangleNormalVectors;
            vertexNormalsCheckBox.Checked = TriMM.Settings.VertexNormalVectors;
            pickingModeComboBox.SelectedIndex = TriMM.Settings.PickingMode;
        }

        #endregion

    }
}
