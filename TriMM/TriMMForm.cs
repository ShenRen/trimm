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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using TriMM.VertexNormalAlgorithms;

namespace TriMM {
    /// <summary>
    /// The main Form of this program.
    /// </summary>
    public partial class TriMMForm : Form {

        #region Fields

        private TriMMView view;
        private TriMMControl control;

        private IVertexNormalAlgorithm[] vertexNormalAlgorithms = new IVertexNormalAlgorithm[] { new Gouraud(), new Max(), new Taubin(), new InverseTaubin(),
            new ThuermerAndWuethrich(), new ExtendedThuermerAndWuethrich(), new ChenAndWu(), new ExtendedChenAndWu(), new Rusinkiewicz(),  
            new AdjacentEdgesWeights(), new InverseAdjacentEdgesWeights(), new EdgeNormals(), new InverseEdgeNormals()};

        #endregion

        #region Constructors

        /// <summary> Initializes the TriMMForm. </summary>
        public TriMMForm() {
            InitializeComponent();
            normalComboBox.Items.AddRange(vertexNormalAlgorithms);
            normalComboBox.SelectedIndex = 0;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the control with the drawing arrays that are always available.
        /// </summary>
        private void InitializeControl() {
            control = new TriMMControl();

            aNumericUpDown.Maximum = TriMM.Mesh.Vertices.Count - 1;
            bNumericUpDown.Maximum = TriMM.Mesh.Vertices.Count - 1;
            cNumericUpDown.Maximum = TriMM.Mesh.Vertices.Count - 1;
            e1NumericUpDown.Maximum = TriMM.Mesh.Vertices.Count - 1;
            e2NumericUpDown.Maximum = TriMM.Mesh.Vertices.Count - 1;
            if (TriMM.Mesh.Vertices.Count > 0) {
                aNumericUpDown.Minimum = aNumericUpDown.Value = 0;
                bNumericUpDown.Minimum = bNumericUpDown.Value = 0;
                cNumericUpDown.Minimum = cNumericUpDown.Value = 0;
            } else {
                aNumericUpDown.Minimum = aNumericUpDown.Value = -1;
                bNumericUpDown.Minimum = bNumericUpDown.Value = -1;
                cNumericUpDown.Minimum = cNumericUpDown.Value = -1;
            }
            if (TriMM.Mesh.Edges.Count > 0) {
                e1NumericUpDown.Minimum = e1NumericUpDown.Value = 0;
                e2NumericUpDown.Minimum = e2NumericUpDown.Value = 0;
            } else {
                e1NumericUpDown.Minimum = e1NumericUpDown.Value = -1;
                e2NumericUpDown.Minimum = e2NumericUpDown.Value = -1;
            }

            vertexLabel.Text = TriMM.Mesh.Vertices.Count.ToString();
            triangleLabel.Text = TriMM.Mesh.Count.ToString();
            edgeLabel.Text = TriMM.Mesh.Edges.Count.ToString();

            control.VertexPicked += new VertexPickedEventHandler(Control_VertexPicked);
            control.EdgePicked += new EdgePickedEventHandler(Control_EdgePicked);
            control.TrianglePicked += new TrianglePickedEventHandler(Control_TrianglePicked);
        }


        /// <summary>
        /// Refreshes the values of the control.
        /// </summary>
        private void RefreshControl() {
            aNumericUpDown.Maximum = TriMM.Mesh.Vertices.Count - 1;
            bNumericUpDown.Maximum = TriMM.Mesh.Vertices.Count - 1;
            cNumericUpDown.Maximum = TriMM.Mesh.Vertices.Count - 1;
            e1NumericUpDown.Maximum = TriMM.Mesh.Vertices.Count - 1;
            e2NumericUpDown.Maximum = TriMM.Mesh.Vertices.Count - 1;
            vertexLabel.Text = TriMM.Mesh.Vertices.Count.ToString();
            triangleLabel.Text = TriMM.Mesh.Count.ToString();
            edgeLabel.Text = TriMM.Mesh.Edges.Count.ToString();

            control.Refresh();
            if (view != null) { view.RefreshView(); }
        }


        /// <summary>
        /// Clears the information about the observed Vertex and Triangle.
        /// </summary>
        private void ClearObserved() {
            control.Info.Clear();
            control.UseColorArray = false;
            TriMM.Mesh.ObservedVertex = -1;
            TriMM.Mesh.ObservedEdge = -1;
            TriMM.Mesh.ObservedTriangle = -1;
        }


        /// <summary>
        /// Opens the given file.
        /// </summary>
        /// <param name="file">Path of the *.OFF or *.STL file to be opened</param>
        private void OpenFile(string file) {
            StreamReader reader = new StreamReader(file);
#if !DEBUG
            try {
#endif
                Cursor.Current = Cursors.WaitCursor;

                // Parses the file.
                if (file.EndsWith(".off")) {
                    OffParser.Parse(reader, vertexNormalAlgorithms[normalComboBox.SelectedIndex]);
                } else if (file.EndsWith(".stl")) {
                    STLParser.Parse(reader, vertexNormalAlgorithms[normalComboBox.SelectedIndex]);
                } else if (file.EndsWith(".ply")) {
                    PlyParser.Parse(reader, vertexNormalAlgorithms[normalComboBox.SelectedIndex]);
                } else if (file.EndsWith(".obj")) {
                    ObjParser.Parse(reader, vertexNormalAlgorithms[normalComboBox.SelectedIndex]);
                }

                InitializeControl();
                meshGroupBox.Visible = saveToolStripMenuItem.Enabled = closeToolStripMenuItem.Enabled
                    = showViewToolStripMenuItem.Enabled = tabControl1.Visible = true;

#if !DEBUG
            } catch (Exception exception) {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
#endif
                reader.Close();
                Cursor.Current = Cursors.Default;
#if !DEBUG
            }
#endif
        }

        #endregion

        #region Event Handling Stuff

        #region Menu

        /// <summary>
        /// Closes a previously opened file and shows the dialog to open a new one.
        /// </summary>
        /// <param name="sender">openToolStripMenuItem</param>
        /// <param name="e">Standard EventArgs</param>
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
#if !DEBUG
            try {
#endif
                ofd.CheckFileExists = true;
                ofd.DefaultExt = "off";
                ofd.Filter = "OOGL Files (*.off)|*.off|STL Files (*.stl)|*.stl|PLY Files (ascii) (*.ply)|*.ply|Wavefront OBJ Files (*.obj)|*.obj";
                ofd.Multiselect = false;
                ofd.Title = "Open File";
                if (ofd.ShowDialog() == DialogResult.OK) {
                    CloseFile(sender, e);
                    OpenFile(ofd.FileName);
                }
#if !DEBUG
            } catch {
                MessageBox.Show("The file you picked is broken!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
#endif
                ofd.Dispose();
#if !DEBUG
            }
#endif
        }

        /// <summary>
        /// Saves the TriangleMesh to an *.OFF file.
        /// </summary>
        /// <param name="sender">saveToolStripMenuItem</param>
        /// <param name="e">Standard EventArgs</param>
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveFileDialog sfd = new SaveFileDialog();
#if !DEBUG
            try {
#endif
                sfd.AddExtension = true;
                sfd.OverwritePrompt = true;
                sfd.DefaultExt = "off";
                sfd.Filter = "OOGL Files (*.off)|*.off|STL ASCII-Files (*.stl)|*.stl|STL Binary-Files (*.stl)|*.stl|PLY Files (ascii) (*.ply)|*.ply|Wavefront OBJ Files (*.obj)|*.obj";
                sfd.Title = "Save File";
                if (sfd.ShowDialog() == DialogResult.OK) {
                    if (sfd.FilterIndex == 1) {
                        OffParser.WriteOFF(sfd.FileName);
                    } else if (sfd.FilterIndex == 2) {
                        STLParser.WriteToASCII(sfd.FileName);
                    } else if (sfd.FilterIndex == 3) {
                        STLParser.WriteToBinary(sfd.FileName);
                    } else if (sfd.FilterIndex == 4) {
                        PlyParser.WritePLY(sfd.FileName);
                    } else if (sfd.FilterIndex == 5) {
                        ObjParser.WriteOBJ(sfd.FileName);
                    }
                }
#if !DEBUG
            } catch (Exception exception) {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
#endif
                sfd.Dispose();
#if !DEBUG
            }
#endif
        }

        /// <summary>
        /// Destroys the TriMMControl and resets the GUI-elements.
        /// </summary>
        /// <param name="sender">closeToolStripMenuItem</param>
        /// <param name="e">Standard EventArgs</param>
        private void CloseFile(object sender, EventArgs e) {
            if (view != null) { view.Close(); }
            meshGroupBox.Visible = saveToolStripMenuItem.Enabled = closeToolStripMenuItem.Enabled
                = showViewToolStripMenuItem.Enabled = tabControl1.Visible = false;
            normalComboBox.SelectedIndex = 0;
            TriMM.Mesh = null;
            if (control != null) {
                control.Dispose();
                control = null;
            }
        }

        /// <summary>
        /// Closes the Form.
        /// </summary>
        /// <param name="sender">exitToolStripMenuItem</param>
        /// <param name="e">Standard EventArgs</param>
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) { this.Close(); }

        /// <summary>
        /// Opens a new TriMMView, if none exists, or focuses the existing TriMMView.
        /// When the TriMMView is opened the visualGroupBox is shown and new options for interaction are available.
        /// The TriMMView is located next to this Form. An EventHandler for the FormClosed event is bound.
        /// </summary>
        /// <param name="sender">showViewToolStripMenuItem</param>
        /// <param name="e">Standard EventArgs</param>
        private void ShowViewToolStripMenuItem_Click(object sender, EventArgs e) {
            if (view == null) {
                view = new TriMMView(control);
                view.RefreshView();
                view.Location = new Point(this.Location.X + this.Width, this.Location.Y);
                view.FormClosed += new FormClosedEventHandler(View_FormClosed);
            } else { view.Focus(); }
        }

        /// <summary>
        /// When the TriMMView is closed, several CheckBoxes are reset and view is set to null.
        /// The EventHandler for the FormClosed event is also unbound.
        /// </summary>
        /// <param name="sender">View</param>
        /// <param name="e">Standard FormClosedEventArgs</param>
        private void View_FormClosed(object sender, FormClosedEventArgs e) {
            view.FormClosed -= View_FormClosed;
            view = null;
        }

        /// <summary>
        /// Opens the SettingsForm to change the standard Settings.
        /// </summary>
        /// <param name="sender">settingsToolStripMenuItem</param>
        /// <param name="e">Standard EventArgs</param>
        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e) {
            SettingsForm sform = new SettingsForm();
            sform.ShowDialog();
        }

        /// <summary>
        /// Shows the About dialog.
        /// </summary>
        /// <param name="sender">infoToolStripMenuItem</param>
        /// <param name="e">Standard EventArgs</param>
        private void InfoToolStripMenuItem_Click(object sender, EventArgs e) {
            About about = new About();
            about.ShowDialog();
        }

        /// <summary>
        /// Changes the size of the Form, when a different Tab is selected.
        /// </summary>
        /// <param name="sender">tabControl1</param>
        /// <param name="e">Standard EventArgs</param>
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e) {
            switch (tabControl1.SelectedIndex) {
                case 0:
                    tabControl1.Height = 308;
                    break;
                case 1:
                    tabControl1.Height = 390;
                    break;
                case 2:
                    tabControl1.Height = 159;
                    break;
            }
            this.Height = 182 + tabControl1.Height;
        }

        #endregion

        #region Visualization

        /// <summary>
        /// Shows information about the picked Vertex.
        /// </summary>
        /// <param name="picked">The index of the picked Vertex, or an empty list.</param>
        private void Control_VertexPicked(List<int> picked) {
            ClearObserved();
            if (picked.Count != 0) {
                control.Info.Add("Vertex " + picked[0] + " = " + TriMM.Mesh.Vertices[picked[0]].ToString());
                TriMM.Mesh.ObservedVertex = picked[0];
                xNumericUpDown.Value = (decimal)TriMM.Mesh.Vertices[picked[0]][0];
                yNumericUpDown.Value = (decimal)TriMM.Mesh.Vertices[picked[0]][1];
                zNumericUpDown.Value = (decimal)TriMM.Mesh.Vertices[picked[0]][2];
            }
            control.Refresh();
        }

        /// <summary>
        /// Shows information about the picked Edge.
        /// </summary>
        /// <param name="picked">The index of the picked Edge, or an empty list.</param>
        private void Control_EdgePicked(List<int> picked) {
            ClearObserved();
            if (picked.Count != 0) {
                control.Info.Add("Edge " + picked[0] + " = " + TriMM.Mesh.Edges.Values[picked[0]].ToString());
                control.Info.Add("Vertex " + TriMM.Mesh.Edges.Values[picked[0]][0] + " = " + TriMM.Mesh.Vertices[TriMM.Mesh.Edges.Values[picked[0]][0]].ToString());
                control.Info.Add("Vertex " + TriMM.Mesh.Edges.Values[picked[0]][1] + " = " + TriMM.Mesh.Vertices[TriMM.Mesh.Edges.Values[picked[0]][1]].ToString());
                TriMM.Mesh.ObservedEdge = picked[0];
                e1NumericUpDown.Value = (decimal)TriMM.Mesh.Edges.Values[picked[0]][0];
                e2NumericUpDown.Value = (decimal)TriMM.Mesh.Edges.Values[picked[0]][1];
            }
            control.Refresh();
        }

        /// <summary>
        /// Shows information about the picked Triangle.
        /// </summary>
        /// <param name="picked">The index of the picked Triangle, or an empty list.</param>
        private void Control_TrianglePicked(List<int> picked) {
            ClearObserved();
            if (picked.Count != 0) {
                control.Info.Add("Triangle " + picked[0] + " = (" + TriMM.Mesh[picked[0]][0] + ", " + TriMM.Mesh[picked[0]][1] + ", " + TriMM.Mesh[picked[0]][2] + ")");
                control.Info.Add("Vertex " + TriMM.Mesh[picked[0]][0] + " = " + TriMM.Mesh[picked[0], 0].ToString());
                control.Info.Add("Vertex " + TriMM.Mesh[picked[0]][1] + " = " + TriMM.Mesh[picked[0], 1].ToString());
                control.Info.Add("Vertex " + TriMM.Mesh[picked[0]][2] + " = " + TriMM.Mesh[picked[0], 2].ToString());
                TriMM.Mesh.ObservedTriangle = picked[0];
                TriMM.Mesh.SetMarkedTriangleColorArray(picked[0]);
                control.UseColorArray = true;
                aNumericUpDown.Value = (decimal)TriMM.Mesh[picked[0]][0];
                bNumericUpDown.Value = (decimal)TriMM.Mesh[picked[0]][1];
                cNumericUpDown.Value = (decimal)TriMM.Mesh[picked[0]][2];
            }
            control.Refresh();

        }

        #endregion

        #region Manipulation

        #region Triangles

        /// <summary>
        /// Removes Triangles, that are not really Triangles, because they have three colinear Vertices.
        /// </summary>
        /// <param name="sender">removeColinButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void RemoveColinButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;

            ClearObserved();

            for (int i = TriMM.Mesh.Count - 1; i >= 0; i--) { if (!TriMM.Mesh.IsTriangle(i)) { TriMM.Mesh.RemoveAt(i); } }

            TriMM.Mesh.Finish(true,true);
            RemoveSinglesButton_Click(sender, e);
        }

        /// <summary>
        /// Removes double Triangles regardless of their orientation.
        /// </summary>
        /// <param name="sender">removeDoubleButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void RemoveDoubleButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;

            ClearObserved();

            for (int i = TriMM.Mesh.Count - 1; i >= 0; i--) {
                List<Triangle> equals = TriMM.Mesh.Where(t => t.Equals(TriMM.Mesh[i])).ToList();
                if (equals.Count > 1) { TriMM.Mesh.RemoveAt(i); }
            }
            TriMM.Mesh.Finish(true, true);
            RemoveSinglesButton_Click(sender, e);
        }

        /// <summary>
        /// Changes the orientation of the Triangle currently observed.
        /// </summary>
        /// <param name="sender">flipAllTrianglesButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void FlipObservedTriangleButton_Click(object sender, EventArgs e) {
            if (TriMM.Mesh.ObservedTriangle != -1) {
                TriMM.Mesh.FlipTriangle(TriMM.Mesh.ObservedTriangle);
                RefreshControl();
                aNumericUpDown.Value = (decimal)TriMM.Mesh[TriMM.Mesh.ObservedTriangle][0];
                bNumericUpDown.Value = (decimal)TriMM.Mesh[TriMM.Mesh.ObservedTriangle][1];
                cNumericUpDown.Value = (decimal)TriMM.Mesh[TriMM.Mesh.ObservedTriangle][2];
            }
        }

        /// <summary>
        /// Changes the orientation of all Triangles.
        /// </summary>
        /// <param name="sender">flipAllTrianglesButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void FlipAllTrianglesButton_Click(object sender, EventArgs e) {
            TriMM.Mesh.FlipAllTriangles();
            RefreshControl();
            if (TriMM.Mesh.ObservedTriangle != -1) {
                aNumericUpDown.Value = (decimal)TriMM.Mesh[TriMM.Mesh.ObservedTriangle][0];
                bNumericUpDown.Value = (decimal)TriMM.Mesh[TriMM.Mesh.ObservedTriangle][1];
                cNumericUpDown.Value = (decimal)TriMM.Mesh[TriMM.Mesh.ObservedTriangle][2];
            }
        }

        /// <summary>
        /// Subdivides the Triangle currently observed.
        /// </summary>
        /// <param name="sender">subdivideTriangleButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void SubdivideTriangleButton_Click(object sender, EventArgs e) {
            if (TriMM.Mesh.ObservedTriangle != -1) {
                TriMM.Mesh.SubdivideTriangle(TriMM.Mesh.ObservedTriangle);
                TriMM.Mesh.ObservedTriangle = -1;
                RefreshControl();
            }
        }

        /// <summary>
        /// Subdivides all Triangles, one subdivision-step at a time.
        /// </summary>
        /// <param name="sender">subdivideButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void SubdivideButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;

            ClearObserved();
            TriMM.Mesh = TriangleMesh.Subdivide(TriMM.Mesh, 1);
            RefreshControl();

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Removes a Triangle given by the three Vertices selected in the NumericUpDowns (all permutations), if it exists.
        /// </summary>
        /// <param name="sender">removeTriangleButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void RemoveTriangleButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;

            ClearObserved();

            Triangle remove = new Triangle((int)aNumericUpDown.Value, (int)bNumericUpDown.Value, (int)cNumericUpDown.Value);
            TriMM.Mesh.Remove(remove);
            remove = new Triangle((int)aNumericUpDown.Value, (int)cNumericUpDown.Value, (int)bNumericUpDown.Value);
            TriMM.Mesh.Remove(remove);
            remove = new Triangle((int)bNumericUpDown.Value, (int)aNumericUpDown.Value, (int)cNumericUpDown.Value);
            TriMM.Mesh.Remove(remove);
            remove = new Triangle((int)bNumericUpDown.Value, (int)cNumericUpDown.Value, (int)aNumericUpDown.Value);
            TriMM.Mesh.Remove(remove);
            remove = new Triangle((int)cNumericUpDown.Value, (int)aNumericUpDown.Value, (int)bNumericUpDown.Value);
            TriMM.Mesh.Remove(remove);
            remove = new Triangle((int)cNumericUpDown.Value, (int)bNumericUpDown.Value, (int)aNumericUpDown.Value);
            TriMM.Mesh.Remove(remove);

            TriMM.Mesh.Finish(true, true);
            RemoveSinglesButton_Click(sender, e);
        }

        /// <summary>
        /// Adds a Triangle given by the three Vertices selected in the NumericUpDowns.
        /// </summary>
        /// <param name="sender">addTriangleButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void AddTriangleButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;

            ClearObserved();

            if (((int)aNumericUpDown.Value != -1) && ((int)bNumericUpDown.Value != -1) && ((int)cNumericUpDown.Value != -1)) {
                Triangle newTriangle = new Triangle((int)aNumericUpDown.Value, (int)bNumericUpDown.Value, (int)cNumericUpDown.Value);
                TriMM.Mesh.Add(newTriangle);
                TriMM.Mesh.Finish(true, true);

                RefreshControl();
            }

            Cursor.Current = Cursors.Default;
        }

        #endregion

        #region Vertices

        /// <summary>
        /// Removes single Vertices that are not part of any triangles.
        /// </summary>
        /// <param name="sender">removeSinglesButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void RemoveSinglesButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;

            ClearObserved();

            List<int> markedVertices = new List<int>();

            for (int i = 0; i < TriMM.Mesh.Vertices.Count; i++) { if (TriMM.Mesh.Vertices[i].Neighborhood.Count == 0) { markedVertices.Add(i); } }
            markedVertices.Sort();

            for (int i = markedVertices.Count - 1; i >= 0; i--) {
                // Remove Vertex
                TriMM.Mesh.Vertices.RemoveAt(markedVertices[i]);

                // Adjust Triangles
                for (int j = 0; j < TriMM.Mesh.Count; j++) {
                    List<int> ordered = new List<int>();
                    for (int k = 0; k < 3; k++) { ordered.Add(TriMM.Mesh[j][k]); }
                    ordered.Sort();
                    for (int k = 0; k < 3; k++) { if (ordered[k] >= markedVertices[i]) { TriMM.Mesh[j].Replace(ordered[k], ordered[k] - 1); } }
                }
            }

            TriMM.Mesh.Finish(true,true);

            RefreshControl();
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Removes Vertices that are in the mesh more than once and adjusts the attached Triangles.
        /// </summary>
        /// <param name="sender">removeDoubleVertButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void RemoveDoubleVertButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;

            ClearObserved();

            List<List<int>> equivalent = new List<List<int>>();
            List<int> remove = new List<int>();

            List<int> first = new List<int>();
            first.Add(0);
            equivalent.Add(first);
            bool added;

            // Find equivalent Vertices.
            for (int i = 1; i < TriMM.Mesh.Vertices.Count; i++) {
                added = false;
                for (int j = 0; j < equivalent.Count; j++) {
                    if (TriMM.Mesh.Vertices[i].Equals(TriMM.Mesh.Vertices[equivalent[j][0]])) {
                        equivalent[j].Add(i);
                        added = true;
                        break;
                    }
                }
                if (!added) {
                    List<int> next = new List<int>();
                    next.Add(i);
                    equivalent.Add(next);
                }
            }

            // Remove those Vertices that are contained more than once.
            equivalent = equivalent.Where(l => l.Count > 1).ToList();
            for (int i = 0; i < equivalent.Count; i++) {
                equivalent[i].Sort();

                for (int j = equivalent[i].Count - 1; j > 0; j--) {
                    remove.Add(equivalent[i][j]);

                    // Adjust Triangles
                    for (int k = 0; k < TriMM.Mesh.Count; k++) { if (TriMM.Mesh[k].Contains(equivalent[i][j])) { TriMM.Mesh[k].Replace(equivalent[i][j], equivalent[i][0]); } }
                }
            }

            remove.Sort();
            // Remove Vertices
            for (int i = remove.Count - 1; i >= 0; i--) { TriMM.Mesh.Vertices.RemoveAt(remove[i]); }

            TriMM.Mesh.Finish(true, true);
            RefreshControl();

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Removes Vertices and the attached Triangles, that have only two neighbors.
        /// </summary>
        /// <param name="sender">remove2NVerticesButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void Remove2NVerticesButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;

            ClearObserved();

            List<int> markedVertices;

            do {
                markedVertices = new List<int>();

                for (int i = 0; i < TriMM.Mesh.Vertices.Count; i++) { if (TriMM.Mesh.Vertices[i].Neighborhood.Count == 2) { markedVertices.Add(i); } }
                markedVertices.Sort();

                for (int i = markedVertices.Count - 1; i >= 0; i--) {
                    List<int> adjacent = TriMM.Mesh.Vertices[markedVertices[i]].Triangles;
                    adjacent.Sort();

                    // Remove Vertex
                    TriMM.Mesh.Vertices.RemoveAt(markedVertices[i]);

                    // Remove Triangles attached to this Vertex.
                    for (int j = adjacent.Count - 1; j >= 0; j--) { TriMM.Mesh.RemoveAt(adjacent[j]); }

                    // Adjust Triangles
                    for (int j = 0; j < TriMM.Mesh.Count; j++) {
                        List<int> ordered = new List<int>();
                        for (int k = 0; k < 3; k++) { ordered.Add(TriMM.Mesh[j][k]); }
                        ordered.Sort();
                        for (int k = 0; k < 3; k++) { if (ordered[k] >= markedVertices[i]) { TriMM.Mesh[j].Replace(ordered[k], ordered[k] - 1); } }
                    }

                    TriMM.Mesh.Finish(true, true);
                }

            } while (markedVertices.Count != 0);

            RefreshControl();
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Removes the observed Vertex and all the Triangles attached to it.
        /// </summary>
        /// <param name="sender">removeObservedButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void RemoveObservedButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;

            if (TriMM.Mesh.ObservedVertex != -1) {
                int remove = TriMM.Mesh.ObservedVertex;
                ClearObserved();

                List<int> adjacent = TriMM.Mesh.Vertices[remove].Triangles;
                adjacent.Sort();

                // Remove Vertex
                TriMM.Mesh.Vertices.RemoveAt(remove);

                // Remove Triangles attached to this Vertex.
                for (int j = adjacent.Count - 1; j >= 0; j--) { TriMM.Mesh.RemoveAt(adjacent[j]); }

                // Adjust Triangles
                for (int j = 0; j < TriMM.Mesh.Count; j++) {
                    List<int> ordered = new List<int>();
                    for (int k = 0; k < 3; k++) { ordered.Add(TriMM.Mesh[j][k]); }
                    ordered.Sort();
                    for (int k = 0; k < 3; k++) { if (ordered[k] >= remove) { TriMM.Mesh[j].Replace(ordered[k], ordered[k] - 1); } }
                }

                TriMM.Mesh.Finish(true, true);
                RefreshControl();
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Moves the observed Vertex to the values in the corresponding NumericUpDowns.
        /// </summary>
        /// <param name="sender">moveObservedButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void MoveObservedButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;

            if (TriMM.Mesh.ObservedVertex != -1) {
                TriMM.Mesh.Vertices[TriMM.Mesh.ObservedVertex] = new Vertex((double)xNumericUpDown.Value, (double)yNumericUpDown.Value, (double)zNumericUpDown.Value);

                ClearObserved();
                TriMM.Mesh.Finish(true, true);
                RefreshControl();
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Transposes the observed Vertex by the values in the x-, y- and zNumericUpDown.
        /// </summary>
        /// <param name="sender">transposeVertexButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void TransposeVertexButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;

            if (TriMM.Mesh.ObservedVertex != -1) {
                TriMM.Mesh.Vertices[TriMM.Mesh.ObservedVertex] = (TriMM.Mesh.Vertices[TriMM.Mesh.ObservedVertex] + new Vertex((double)xNumericUpDown.Value, (double)yNumericUpDown.Value, (double)zNumericUpDown.Value)).ToVertex();

                ClearObserved();
                TriMM.Mesh.Finish(true, true);
                RefreshControl();
            }

            Cursor.Current = Cursors.Default;
        }


        /// <summary>
        /// Moves the observed Vertex along its normalvector by the distance selected in the distanceNumericUpDown.
        /// </summary>
        /// <param name="sender">moveAlongNormalButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void MoveAlongNormalButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;

            if (TriMM.Mesh.ObservedVertex != -1) {
                TriMM.Mesh.Vertices[TriMM.Mesh.ObservedVertex] = (TriMM.Mesh.Vertices[TriMM.Mesh.ObservedVertex] + (double)distanceNumericUpDown.Value * TriMM.Mesh.Vertices[TriMM.Mesh.ObservedVertex].Normal).ToVertex();

                ClearObserved();
                TriMM.Mesh.Finish(true, true);
                RefreshControl();
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Adds the Vertex given by the values in the corresponding NumericUpDowns.
        /// </summary>
        /// <param name="sender">addVertexButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void AddVertexButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;

            ClearObserved();

            Vertex newVertex = new Vertex((double)xNumericUpDown.Value, (double)yNumericUpDown.Value, (double)zNumericUpDown.Value);
            TriMM.Mesh.Vertices.Add(newVertex);

            TriMM.Mesh.Finish(true, true);
            RefreshControl();

            Cursor.Current = Cursors.Default;
        }


        #endregion

        #region Edges

        /// <summary>
        /// Flips the Edge given by the two selected Vertices.
        /// </summary>
        /// <param name="sender">flipButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void FlipButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;

            ClearObserved();

            int ind1 = (int)e1NumericUpDown.Value;
            int ind2 = (int)e2NumericUpDown.Value;

            if ((ind1 != -1) && (ind2 != -1)) {
                double length = VectorND.Distance(TriMM.Mesh.Vertices[ind1], TriMM.Mesh.Vertices[ind2]);
                Edge theEdge = new Edge(ind1, ind2, length);

                if (TriMM.Mesh.Edges.ContainsKey(theEdge.Key)) {
                    List<int> triangles = TriMM.Mesh.Edges[theEdge.Key].Triangles;
                    triangles.Sort();

                    // Get the Triangles that Edge belongs to, connects the Vertices opposite the Edge with a new Edge and removes the old Edge.
                    if (triangles.Count == 2) {
                        int o1 = TriMM.Mesh[triangles[0]].GetOppositeCorner(theEdge);
                        int o2 = TriMM.Mesh[triangles[1]].GetOppositeCorner(theEdge);

                        int triInd1 = TriMM.Mesh[triangles[0]].IndexOf(o1);
                        int triInd2 = TriMM.Mesh[triangles[1]].IndexOf(o2);

                        Triangle tri1 = new Triangle(o1, TriMM.Mesh[triangles[0]][(triInd1 + 1) % 3], o2);
                        Triangle tri1per1 = new Triangle(tri1[0], tri1[2], tri1[1]);
                        Triangle tri1per2 = new Triangle(tri1[1], tri1[0], tri1[2]);
                        Triangle tri1per3 = new Triangle(tri1[1], tri1[2], tri1[0]);
                        Triangle tri1per4 = new Triangle(tri1[2], tri1[0], tri1[1]);
                        Triangle tri1per5 = new Triangle(tri1[2], tri1[1], tri1[0]);

                        Triangle tri2 = new Triangle(o2, TriMM.Mesh[triangles[1]][(triInd2 + 1) % 3], o1);
                        Triangle tri2per1 = new Triangle(tri2[0], tri2[2], tri2[1]);
                        Triangle tri2per2 = new Triangle(tri2[1], tri2[0], tri2[2]);
                        Triangle tri2per3 = new Triangle(tri2[1], tri2[2], tri2[0]);
                        Triangle tri2per4 = new Triangle(tri2[2], tri2[0], tri2[1]);
                        Triangle tri2per5 = new Triangle(tri2[2], tri2[1], tri2[0]);

                        bool isNotIn1 = true;
                        bool isNotIn2 = true;

                        if (TriMM.Mesh.Contains(tri1) || TriMM.Mesh.Contains(tri1per1) || TriMM.Mesh.Contains(tri1per2) || TriMM.Mesh.Contains(tri1per3) || TriMM.Mesh.Contains(tri1per4) || TriMM.Mesh.Contains(tri1per5)) { isNotIn1 = false; }
                        if (TriMM.Mesh.Contains(tri2) || TriMM.Mesh.Contains(tri2per1) || TriMM.Mesh.Contains(tri2per2) || TriMM.Mesh.Contains(tri2per3) || TriMM.Mesh.Contains(tri2per4) || TriMM.Mesh.Contains(tri2per5)) { isNotIn2 = false; }

                        if (isNotIn1 && (TriMM.Mesh.IsTriangle(o1, TriMM.Mesh[triangles[0]][(triInd1 + 1) % 3], o2))) { TriMM.Mesh.Add(tri1); }
                        if (isNotIn2 && (TriMM.Mesh.IsTriangle(o2, TriMM.Mesh[triangles[1]][(triInd2 + 1) % 3], o1))) { TriMM.Mesh.Add(tri2); }

                        TriMM.Mesh.RemoveAt(triangles[1]);
                        TriMM.Mesh.RemoveAt(triangles[0]);

                        TriMM.Mesh.Finish(true, true);
                        RefreshControl();
                    }
                }
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Subdivides the selected Edge by adding the midedge Vertex
        /// and subdividing the incident Triangles with the new Vertex.
        /// </summary>
        /// <param name="sender">subdivideEdgeButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void SubdivideEdgeButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;

            ClearObserved();

            int ind1 = (int)e1NumericUpDown.Value;
            int ind2 = (int)e2NumericUpDown.Value;

            if ((ind1 != -1) && (ind2 != -1)) {
                double length = VectorND.Distance(TriMM.Mesh.Vertices[ind1], TriMM.Mesh.Vertices[ind2]);
                Edge theEdge = new Edge(ind1, ind2, length);

                if (TriMM.Mesh.Edges.ContainsKey(theEdge.Key)) {
                    List<int> triangles = TriMM.Mesh.Edges[theEdge.Key].Triangles;
                    triangles.Sort();
                    Vertex midedge = (0.5 * (TriMM.Mesh.Vertices[ind1] + TriMM.Mesh.Vertices[ind2])).ToVertex();
                    TriMM.Mesh.Vertices.Add(midedge);

                    for (int i = triangles.Count - 1; i >= 0; i--) {
                        Triangle replace = new Triangle(TriMM.Mesh[triangles[i]][0], TriMM.Mesh[triangles[i]][1], TriMM.Mesh[triangles[i]][2]);
                        replace.Replace(ind1, TriMM.Mesh.Vertices.Count - 1);
                        TriMM.Mesh.Add(replace);
                        replace = new Triangle(TriMM.Mesh[triangles[i]][0], TriMM.Mesh[triangles[i]][1], TriMM.Mesh[triangles[i]][2]);
                        replace.Replace(ind2, TriMM.Mesh.Vertices.Count - 1);
                        TriMM.Mesh.Add(replace);
                        TriMM.Mesh.RemoveAt(triangles[i]);
                    }

                    TriMM.Mesh.Finish(true, true);
                    RemoveSinglesButton_Click(sender, e);
                }
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Removes the Edge given by the two selected Vertices.
        /// Also removes the incident Triangles.
        /// </summary>
        /// <param name="sender">removeEdgeButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void RemoveEdgeButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;

            ClearObserved();

            int ind1 = (int)e1NumericUpDown.Value;
            int ind2 = (int)e2NumericUpDown.Value;

            if ((ind1 != -1) && (ind2 != -1)) {
                double length = VectorND.Distance(TriMM.Mesh.Vertices[ind1], TriMM.Mesh.Vertices[ind2]);
                Edge theEdge = new Edge(ind1, ind2, length);

                if (TriMM.Mesh.Edges.ContainsKey(theEdge.Key)) {
                    List<int> triangles = TriMM.Mesh.Edges[theEdge.Key].Triangles;
                    triangles.Sort();
                    for (int i = triangles.Count - 1; i >= 0; i--) { TriMM.Mesh.RemoveAt(triangles[i]); }

                    TriMM.Mesh.Finish(true, true);
                    RemoveSinglesButton_Click(sender, e);
                }
            }

            Cursor.Current = Cursors.Default;
        }

        #endregion

        #endregion

        /// <summary>
        /// When a new VertexNormalAlgorithm is selected the normals are calculated and the TriMMControl is refreshed.
        /// </summary>
        /// <param name="sender">normalComboBox</param>
        /// <param name="e">Standard EventArgs</param>
        private void NormalComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (TriMM.Mesh != null) {
                TriMM.Mesh.VertexNormalAlgorithm = vertexNormalAlgorithms[normalComboBox.SelectedIndex];
                TriMM.Mesh.Finish(true, true);
                RefreshControl();
            }
        }

        #endregion

    }
}
