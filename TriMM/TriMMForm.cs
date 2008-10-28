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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TriMM {
    /// <summary>
    /// The main Form of this program.
    /// </summary>
    public partial class TriMMForm : Form {

        #region Fields

        private TriangleMesh mesh;

        private TriMMView view;
        private TriMMControl control;

        private int observedVertex = -1;
        private int observedTriangle = -1;

        #endregion

        #region Constructors

        /// <summary> Initializes the TriMMForm. </summary>
        public TriMMForm() { InitializeComponent(); }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the control with the drawing arrays that are always available.
        /// </summary>
        private void InitializeControl() {
            control = new TriMMControl();

            aNumericUpDown.Maximum = mesh.Vertices.Count - 1;
            bNumericUpDown.Maximum = mesh.Vertices.Count - 1;
            cNumericUpDown.Maximum = mesh.Vertices.Count - 1;
            e1NumericUpDown.Maximum = mesh.Vertices.Count - 1;
            e2NumericUpDown.Maximum = mesh.Vertices.Count - 1;
            vertexLabel.Text = mesh.Vertices.Count.ToString();
            triangleLabel.Text = mesh.Count.ToString();
            edgeLabel.Text = mesh.Edges.Count.ToString();


            control.ShowMesh = true;
            control.VertexColorDist = mesh.VertexColorDist;
            control.TriangleColorDist = mesh.TriangleColorDist;
            control.VertexPickingColors = mesh.GetVertexPickingColors();
            control.TrianglePickingColors = mesh.GetTrianglePickingColors();
            control.ObservedRadius = (float)mesh.MinEdgeLength / 2;
            control.PickingRadius = (float)mesh.MinEdgeLength / 2;
            control.Center = mesh.Center;
            control.TriangleArray = mesh.GetTriangleArray();
            control.NormalArray = mesh.GetNormalArray();
            control.MyScale = mesh.Scale;
            control.VertexArray = mesh.GetVertexArray();
            control.EdgeArray = mesh.GetEdgeArray();
            control.FacetNormalVectorArray = mesh.GetFacetNormalVectorArray();
            control.SmoothNormalArray = mesh.GetSmoothNormalArray();

            control.VertexPicked += new VertexPickedEventHandler(Control_VertexPicked);
            control.TrianglePicked += new TrianglePickedEventHandler(Control_TrianglePicked);
        }


        /// <summary>
        /// Refreshes the values of the control.
        /// </summary>
        private void RefreshControl() {
            aNumericUpDown.Maximum = mesh.Vertices.Count - 1;
            bNumericUpDown.Maximum = mesh.Vertices.Count - 1;
            cNumericUpDown.Maximum = mesh.Vertices.Count - 1;
            e1NumericUpDown.Maximum = mesh.Vertices.Count - 1;
            e2NumericUpDown.Maximum = mesh.Vertices.Count - 1;
            vertexLabel.Text = mesh.Vertices.Count.ToString();
            triangleLabel.Text = mesh.Count.ToString();
            edgeLabel.Text = mesh.Edges.Count.ToString();

            control.VertexColorDist = mesh.VertexColorDist;
            control.TriangleColorDist = mesh.TriangleColorDist;
            control.VertexPickingColors = mesh.GetVertexPickingColors();
            control.TrianglePickingColors = mesh.GetTrianglePickingColors();
            control.ObservedRadius = (float)mesh.MinEdgeLength / 2;
            control.PickingRadius = (float)mesh.MinEdgeLength / 2;
            control.Center = mesh.Center;
            control.TriangleArray = mesh.GetTriangleArray();
            control.NormalArray = mesh.GetNormalArray();
            control.MyScale = mesh.Scale;
            control.VertexArray = mesh.GetVertexArray();
            control.EdgeArray = mesh.GetEdgeArray();
            control.FacetNormalVectorArray = mesh.GetFacetNormalVectorArray();
            control.SmoothNormalArray = mesh.GetSmoothNormalArray();

            control.Refresh();
        }


        /// <summary>
        /// Clears the information about the observed Vertex.
        /// </summary>
        private void ClearObserved() {
            control.Info.Clear();
            control.ObservedVertex = null;
            control.UseColorArray = false;
            observedVertex = -1;

            xNumericUpDown.Value = 0;
            yNumericUpDown.Value = 0;
            zNumericUpDown.Value = 0;
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
                    mesh = OffParser.Parse(reader);
                } else if (file.EndsWith(".stl")) {
                    mesh = STLParser.Parse(reader);
                } else if (file.EndsWith(".ply")) {
                    mesh = PlyParser.Parse(reader);
                }

                InitializeControl();
                meshGroupBox.Visible = saveToolStripMenuItem.Enabled = closeToolStripMenuItem.Enabled
                    = showViewToolStripMenuItem.Enabled = manipulationGroupBox.Visible = true;

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
                ofd.Filter = "OOGL Files (*.off)|*.off|STL Files (*.stl)|*.stl|PLY Files (*.ply)|*.ply";
                ofd.Multiselect = false;
                ofd.Title = "Open File";
                if (ofd.ShowDialog() == DialogResult.OK) {
                    CloseFile(sender, e);
                    OpenFile(ofd.FileName);
                }
#if !DEBUG
            } catch (Exception exception) {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                sfd.Filter = "OOGL Files (*.off)|*.off|STL ASCII-Files (*.stl)|*.stl|STL Binary-Files (*.stl)|*.stl|PLY Files (*.ply)|*.ply";
                sfd.Title = "Save File";
                if (sfd.ShowDialog() == DialogResult.OK) {
                    if (sfd.FilterIndex == 1) {
                        OffParser.WriteOFF(sfd.FileName, mesh);
                    } else if (sfd.FilterIndex == 2) {
                        STLParser.WriteToASCII(sfd.FileName, mesh);
                    } else if (sfd.FilterIndex == 3) {
                        STLParser.WriteToBinary(sfd.FileName, mesh);
                    } else if (sfd.FilterIndex == 4) {
                        PlyParser.WritePLY(sfd.FileName, mesh);
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
        /// Destroys the OGLControl and resets the GUI-elements.
        /// </summary>
        /// <param name="sender">closeToolStripMenuItem</param>
        /// <param name="e">Standard EventArgs</param>
        private void CloseFile(object sender, EventArgs e) {
            if (view != null) { view.Close(); }
            meshGroupBox.Visible = saveToolStripMenuItem.Enabled = closeToolStripMenuItem.Enabled
                = showViewToolStripMenuItem.Enabled = manipulationGroupBox.Visible = false;
            aNumericUpDown.Value = bNumericUpDown.Value = cNumericUpDown.Value = e1NumericUpDown.Value = e2NumericUpDown.Value = 0;
            mesh = null;
            if (control != null) {
                control.DestroyContexts();
                control = null;
            }
        }

        /// <summary>
        /// Opens a new OGLView, if none exists, or focuses the existing TriMMView.
        /// When the OGLView is opened the visualGroupBox is shown and new options for interaction are available.
        /// The OGLView is located next to this Form. An EventHandler for the FormClosed event is bound.
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
        /// When the OGLView is closed, several CheckBoxes are reset and view is set to null.
        /// The EventHandler for the FormClosed event is also unbound.
        /// </summary>
        /// <param name="sender">View</param>
        /// <param name="e">Standard FormClosedEventArgs</param>
        private void View_FormClosed(object sender, FormClosedEventArgs e) {
            view.FormClosed -= View_FormClosed;
            view = null;
        }

        /// <summary>
        /// Closes the Form.
        /// </summary>
        /// <param name="sender">exitToolStripMenuItem</param>
        /// <param name="e">Standard EventArgs</param>
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) { this.Close(); }

        #endregion

        #region Visualization


        /// <summary>
        /// Shows information about the picked Vertex.
        /// </summary>
        /// <param name="picked">The index of the picked Vertex</param>
        private void Control_VertexPicked(List<int> picked) {
            ClearObserved();
            if (picked.Count != 0) {
                control.Info.Add("Vertex " + picked[0] + " = " + mesh.Vertices[picked[0]].ToString());
                control.ObservedVertex = mesh.Vertices[picked[0]];
                observedVertex = picked[0];
                xNumericUpDown.Value = (decimal)mesh.Vertices[picked[0]][0];
                yNumericUpDown.Value = (decimal)mesh.Vertices[picked[0]][1];
                zNumericUpDown.Value = (decimal)mesh.Vertices[picked[0]][2];
            }
            control.Refresh();
        }

        private void Control_TrianglePicked(List<int> picked) {
            ClearObserved();
            if (picked.Count != 0) {
                control.Info.Add("Triangle " + picked[0] + " = (" + mesh[picked[0]][0] + ", " + mesh[picked[0]][1] + ", " + mesh[picked[0]][2] + ")");
                control.Info.Add("Vertex " + mesh[picked[0]][0] + " = " + mesh[picked[0], 0].ToString());
                control.Info.Add("Vertex " + mesh[picked[0]][1] + " = " + mesh[picked[0], 1].ToString());
                control.Info.Add("Vertex " + mesh[picked[0]][2] + " = " + mesh[picked[0], 2].ToString());
                observedTriangle = picked[0];
                control.ColorArray = mesh.GetMarkedTriangleColorArray(picked[0], control.PlainColor, control.ObservedTriangleColor);
                control.UseColorArray = true;
                aNumericUpDown.Value = (decimal)mesh[picked[0]][0];
                bNumericUpDown.Value = (decimal)mesh[picked[0]][1];
                cNumericUpDown.Value = (decimal)mesh[picked[0]][2];
            }
            control.Refresh();

        }

        #endregion

        #region Manipulation

        /// <summary>
        /// Does all cleaning steps.
        /// </summary>
        /// <param name="sender">fullCleaningButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void FullCleaningButton_Click(object sender, EventArgs e) {
            RemoveColinButton_Click(sender, e);
            RemoveSinglesButton_Click(sender, e);
            RemoveDoubleVertButton_Click(sender, e);
            RemoveDoubleButton_Click(sender, e);
            Remove2NVerticesButton_Click(sender, e);
        }

        /// <summary>
        /// Removes Triangles, that are not really Triangles, because they have colinear corners.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveColinButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;

            ClearObserved();

            for (int i = mesh.Count - 1; i >= 0; i--) { if (!mesh.IsTriangle(i)) { mesh.RemoveAt(i); } }

            mesh.Finish(true);
            RefreshControl();
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Removes single Vertices, that are not part of any triangles.
        /// </summary>
        /// <param name="sender">removeSinglesButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void RemoveSinglesButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;

            ClearObserved();

            List<int> markedVertices = new List<int>();

            for (int i = 0; i < mesh.Vertices.Count; i++) { if (mesh.Vertices[i].Neighborhood.Count == 0) { markedVertices.Add(i); } }
            markedVertices.Sort();

            for (int i = markedVertices.Count - 1; i >= 0; i--) {
                // Remove Vertex
                mesh.Vertices.RemoveAt(markedVertices[i]);

                // Adjust Triangles
                for (int j = 0; j < mesh.Count; j++) {
                    List<int> ordered = new List<int>();
                    for (int k = 0; k < 3; k++) { ordered.Add(mesh[j][k]); }
                    ordered.Sort();
                    for (int k = 0; k < 3; k++) { if (ordered[k] >= markedVertices[i]) { mesh[j].Replace(ordered[k], ordered[k] - 1); } }
                }
            }

            mesh.Finish(true);

            RefreshControl();
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Removes Vertices, that are in the mesh more than once.
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
            for (int i = 1; i < mesh.Vertices.Count; i++) {
                added = false;
                for (int j = 0; j < equivalent.Count; j++) {
                    if (mesh.Vertices[i].Equals(mesh.Vertices[equivalent[j][0]])) {
                        equivalent[j].Add(i);
                        added = true;
                        break;
                    }
                }
                if (!added) {
                    List<int> next = new List<int>();
                    next.Add(i);
                }
            }

            // Remove those Vertices that are contained more than once.
            equivalent = equivalent.Where(l => l.Count > 1).ToList();
            for (int i = 0; i < equivalent.Count; i++) {
                equivalent[i].Sort();

                for (int j = equivalent[i].Count - 1; j > 0; j--) {
                    remove.Add(equivalent[i][j]);

                    // Adjust Triangles
                    for (int k = 0; k < mesh.Count; k++) { if (mesh[k].Contains(equivalent[i][j])) { mesh[k].Replace(equivalent[i][j], equivalent[i][0]); } }
                }
            }

            remove.Sort();
            // Remove Vertices
            for (int i = remove.Count - 1; i >= 0; i--) { mesh.Vertices.RemoveAt(remove[i]); }

            mesh.Finish(true);
            RefreshControl();

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Removes double Triangles regardless of their orientation.
        /// </summary>
        /// <param name="sender">removeDoubleButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void RemoveDoubleButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;

            ClearObserved();

            for (int i = mesh.Count - 1; i >= 0; i--) {
                List<Triangle> equals = mesh.Where(t => t.Equals(mesh[i])).ToList();
                if (equals.Count > 1) { mesh.RemoveAt(i); }
            }
            mesh.Finish(true);
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

                for (int i = 0; i < mesh.Vertices.Count; i++) { if (mesh.Vertices[i].Neighborhood.Count == 2) { markedVertices.Add(i); } }
                markedVertices.Sort();

                for (int i = markedVertices.Count - 1; i >= 0; i--) {
                    List<int> adjacent = mesh.Vertices[markedVertices[i]].AdjacentTriangles;
                    adjacent.Sort();

                    // Remove Vertex
                    mesh.Vertices.RemoveAt(markedVertices[i]);

                    // Remove Triangles attached to this Vertex.
                    for (int j = adjacent.Count - 1; j >= 0; j--) { mesh.RemoveAt(adjacent[j]); }

                    // Adjust Triangles
                    for (int j = 0; j < mesh.Count; j++) {
                        List<int> ordered = new List<int>();
                        for (int k = 0; k < 3; k++) { ordered.Add(mesh[j][k]); }
                        ordered.Sort();
                        for (int k = 0; k < 3; k++) { if (ordered[k] >= markedVertices[i]) { mesh[j].Replace(ordered[k], ordered[k] - 1); } }
                    }
                }

                if (markedVertices.Count != 0) { mesh.Finish(true); }
            } while (markedVertices.Count != 0);

            RefreshControl();
            Cursor.Current = Cursors.Default;
        }


        private void FlipAllTrianglesButton_Click(object sender, EventArgs e) {
            mesh.FlipAllTriangles();
            RefreshControl();
            if (observedTriangle != -1) {
                aNumericUpDown.Value = (decimal)mesh[observedTriangle][0];
                bNumericUpDown.Value = (decimal)mesh[observedTriangle][1];
                cNumericUpDown.Value = (decimal)mesh[observedTriangle][2];
            }
        }

        private void FlipObservedTriangleButton_Click(object sender, EventArgs e) {
            if (observedTriangle != -1) {
                mesh.FlipTriangle(observedTriangle);
                RefreshControl();
                aNumericUpDown.Value = (decimal)mesh[observedTriangle][0];
                bNumericUpDown.Value = (decimal)mesh[observedTriangle][1];
                cNumericUpDown.Value = (decimal)mesh[observedTriangle][2];
            }
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
            mesh.Remove(remove);
            remove = new Triangle((int)aNumericUpDown.Value, (int)cNumericUpDown.Value, (int)bNumericUpDown.Value);
            mesh.Remove(remove);
            remove = new Triangle((int)bNumericUpDown.Value, (int)aNumericUpDown.Value, (int)cNumericUpDown.Value);
            mesh.Remove(remove);
            remove = new Triangle((int)bNumericUpDown.Value, (int)cNumericUpDown.Value, (int)aNumericUpDown.Value);
            mesh.Remove(remove);
            remove = new Triangle((int)cNumericUpDown.Value, (int)aNumericUpDown.Value, (int)bNumericUpDown.Value);
            mesh.Remove(remove);
            remove = new Triangle((int)cNumericUpDown.Value, (int)bNumericUpDown.Value, (int)aNumericUpDown.Value);
            mesh.Remove(remove);

            mesh.Finish(true);

            RefreshControl();
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Adds a Triangle given by the three Vertices selected in the NumericUpDowns.
        /// </summary>
        /// <param name="sender">addTriangleButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void AddTriangleButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;

            ClearObserved();

            Triangle newTriangle = new Triangle((int)aNumericUpDown.Value, (int)bNumericUpDown.Value, (int)cNumericUpDown.Value);
            mesh.Add(newTriangle);
            mesh.Finish(true);

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

            if (observedVertex != -1) {
                int remove = observedVertex;
                ClearObserved();

                List<int> adjacent = mesh.Vertices[remove].AdjacentTriangles;
                adjacent.Sort();

                // Remove Vertex
                mesh.Vertices.RemoveAt(remove);

                // Remove Triangles attached to this Vertex.
                for (int j = adjacent.Count - 1; j >= 0; j--) { mesh.RemoveAt(adjacent[j]); }

                // Adjust Triangles
                for (int j = 0; j < mesh.Count; j++) {
                    List<int> ordered = new List<int>();
                    for (int k = 0; k < 3; k++) { ordered.Add(mesh[j][k]); }
                    ordered.Sort();
                    for (int k = 0; k < 3; k++) { if (ordered[k] >= remove) { mesh[j].Replace(ordered[k], ordered[k] - 1); } }
                }

                mesh.Finish(true);
                RefreshControl();
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Moves the observed Vertex to the values in the corresponding NumericUpDowns.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveObservedButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;

            if (observedVertex != -1) {
                mesh.Vertices[observedVertex] = new Vertex((double)xNumericUpDown.Value, (double)yNumericUpDown.Value, (double)zNumericUpDown.Value);

                ClearObserved();
                mesh.Finish(true);
                RefreshControl();
            }

            Cursor.Current = Cursors.Default;
        }


        private void TransposeVertexButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;

            if (observedVertex != -1) {
                mesh.Vertices[observedVertex] = (mesh.Vertices[observedVertex] + new Vertex((double)xNumericUpDown.Value, (double)yNumericUpDown.Value, (double)zNumericUpDown.Value)).ToVertex();

                ClearObserved();
                mesh.Finish(true);
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
            mesh.Vertices.Add(newVertex);

            mesh.Finish(true);
            RefreshControl();
            if (view != null) { view.RefreshView(); }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Flips the Edge given by the two selected Vertices.
        /// </summary>
        /// <param name="sender">flipButton</param>
        /// <param name="e">Standard EventArgs</param>
        private void FlipButton_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;

            int ind1 = (int)e1NumericUpDown.Value;
            int ind2 = (int)e2NumericUpDown.Value;
            double length = VectorND.Distance(mesh.Vertices[ind1], mesh.Vertices[ind2]);
            Edge theEdge = new Edge(ind1, ind2, length);

            if (mesh.Edges.ContainsKey(theEdge.Key)) {
                List<int> triangles = mesh.Edges[theEdge.Key].Triangles;
                triangles.Sort();

                // Get the Triangles that Edge belongs to, connects the Vertices opposite the Edge with a new Edge and removes the old Edge.
                if (triangles.Count == 2) {
                    int o1 = mesh[triangles[0]].GetOppositeCorner(theEdge);
                    int o2 = mesh[triangles[1]].GetOppositeCorner(theEdge);

                    int triInd1 = mesh[triangles[0]].IndexOf(o1);
                    int triInd2 = mesh[triangles[1]].IndexOf(o2);

                    Triangle tri1 = new Triangle(o1, mesh[triangles[0]][(triInd1 + 1) % 3], o2);
                    Triangle tri1per1 = new Triangle(tri1[0], tri1[2], tri1[1]);
                    Triangle tri1per2 = new Triangle(tri1[1], tri1[0], tri1[2]);
                    Triangle tri1per3 = new Triangle(tri1[1], tri1[2], tri1[0]);
                    Triangle tri1per4 = new Triangle(tri1[2], tri1[0], tri1[1]);
                    Triangle tri1per5 = new Triangle(tri1[2], tri1[1], tri1[0]);

                    Triangle tri2 = new Triangle(o2, mesh[triangles[1]][(triInd2 + 1) % 3], o1);
                    Triangle tri2per1 = new Triangle(tri2[0], tri2[2], tri2[1]);
                    Triangle tri2per2 = new Triangle(tri2[1], tri2[0], tri2[2]);
                    Triangle tri2per3 = new Triangle(tri2[1], tri2[2], tri2[0]);
                    Triangle tri2per4 = new Triangle(tri2[2], tri2[0], tri2[1]);
                    Triangle tri2per5 = new Triangle(tri2[2], tri2[1], tri2[0]);

                    bool isNotIn1 = true;
                    bool isNotIn2 = true;

                    if (mesh.Contains(tri1) || mesh.Contains(tri1per1) || mesh.Contains(tri1per2) || mesh.Contains(tri1per3) || mesh.Contains(tri1per4) || mesh.Contains(tri1per5)) { isNotIn1 = false; }
                    if (mesh.Contains(tri2) || mesh.Contains(tri2per1) || mesh.Contains(tri2per2) || mesh.Contains(tri2per3) || mesh.Contains(tri2per4) || mesh.Contains(tri2per5)) { isNotIn2 = false; }

                    if (isNotIn1 && (mesh.IsTriangle(o1, mesh[triangles[0]][(triInd1 + 1) % 3], o2))) { mesh.Add(tri1); }
                    if (isNotIn2 && (mesh.IsTriangle(o2, mesh[triangles[1]][(triInd2 + 1) % 3], o1))) { mesh.Add(tri2); }

                    mesh.RemoveAt(triangles[1]);
                    mesh.RemoveAt(triangles[0]);

                    mesh.Finish(true);
                    RefreshControl();
                    if (view != null) { view.RefreshView(); }
                }
            }

            Cursor.Current = Cursors.Default;
        }

        #endregion

        #endregion

    }
}
