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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using TriMM.VertexNormalAlgorithms;

namespace TriMM {

    /// <summary>
    /// The main Window of this program.
    /// </summary>
    public partial class TriMMWindow : Window {

        #region Fields

        // NumericUpDowns for Triangles, Vertices and Edges
        NumericUpDown aNumericUpDown = new NumericUpDown();
        NumericUpDown bNumericUpDown = new NumericUpDown();
        NumericUpDown cNumericUpDown = new NumericUpDown();
        NumericUpDown xNumericUpDown = new NumericUpDown();
        NumericUpDown yNumericUpDown = new NumericUpDown();
        NumericUpDown zNumericUpDown = new NumericUpDown();
        NumericUpDown distanceNumericUpDown = new NumericUpDown();
        NumericUpDown e1NumericUpDown = new NumericUpDown();
        NumericUpDown e2NumericUpDown = new NumericUpDown();

        private TriMMView view;
        private SettingsWindow setWin;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the TriMMWindow and includes some Windows.Forms.NumericUpDowns.
        /// </summary>
        public TriMMWindow() {
            InitializeComponent();

            xNumericUpDown.DecimalPlaces = yNumericUpDown.DecimalPlaces = zNumericUpDown.DecimalPlaces = distanceNumericUpDown.DecimalPlaces = 15;
            xNumericUpDown.TextAlign = yNumericUpDown.TextAlign = zNumericUpDown.TextAlign = distanceNumericUpDown.TextAlign
                = aNumericUpDown.TextAlign = bNumericUpDown.TextAlign = cNumericUpDown.TextAlign = e1NumericUpDown.TextAlign = e2NumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            xNumericUpDown.Minimum = yNumericUpDown.Minimum = zNumericUpDown.Minimum = distanceNumericUpDown.Minimum = -100000000;
            xNumericUpDown.Maximum = yNumericUpDown.Maximum = zNumericUpDown.Maximum = distanceNumericUpDown.Maximum = 100000000;
            xNumericUpDown.Increment = yNumericUpDown.Increment = zNumericUpDown.Increment = distanceNumericUpDown.Increment = 0.001m;

            aWFHost.Child = aNumericUpDown;
            bWFHost.Child = bNumericUpDown;
            cWFHost.Child = cNumericUpDown;
            xWFHost.Child = xNumericUpDown;
            yWFHost.Child = yNumericUpDown;
            zWFHost.Child = zNumericUpDown;
            distanceWFHost.Child = distanceNumericUpDown;
            e1WFHost.Child = e1NumericUpDown;
            e2WFHost.Child = e2NumericUpDown;

            this.Icon = TriMMApp.Image;

            normalComboBox.ItemsSource = TriMMApp.VertexNormalAlgorithms;
            normalComboBox.SelectedIndex = TriMMApp.Settings.NormalAlgo;

            TriMMApp.Settings.NormalAlgoChanged += new NormalAlgoChangedEventHandler(Settings_NormalAlgoChanged);
            TriMMApp.Settings.LanguageChanged += new LanguageChangedEventHandler(Settings_LanguageChanged);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the control with the drawing arrays that are always available.
        /// </summary>
        private void InitializeControl() {
            TriMMApp.Control = new TriMMControl();
            TriMMApp.Control.ResetView();

            aNumericUpDown.Maximum = TriMMApp.Mesh.Vertices.Count - 1;
            bNumericUpDown.Maximum = TriMMApp.Mesh.Vertices.Count - 1;
            cNumericUpDown.Maximum = TriMMApp.Mesh.Vertices.Count - 1;
            e1NumericUpDown.Maximum = TriMMApp.Mesh.Vertices.Count - 1;
            e2NumericUpDown.Maximum = TriMMApp.Mesh.Vertices.Count - 1;
            if (TriMMApp.Mesh.Vertices.Count > 0) {
                aNumericUpDown.Minimum = aNumericUpDown.Value = 0;
                bNumericUpDown.Minimum = bNumericUpDown.Value = 0;
                cNumericUpDown.Minimum = cNumericUpDown.Value = 0;
            } else {
                aNumericUpDown.Minimum = aNumericUpDown.Value = -1;
                bNumericUpDown.Minimum = bNumericUpDown.Value = -1;
                cNumericUpDown.Minimum = cNumericUpDown.Value = -1;
            }
            if (TriMMApp.Mesh.Edges.Count > 0) {
                e1NumericUpDown.Minimum = e1NumericUpDown.Value = 0;
                e2NumericUpDown.Minimum = e2NumericUpDown.Value = 0;
            } else {
                e1NumericUpDown.Minimum = e1NumericUpDown.Value = -1;
                e2NumericUpDown.Minimum = e2NumericUpDown.Value = -1;
            }

            verticesTextBox.Text = TriMMApp.Mesh.Vertices.Count.ToString();
            trianglesTextBox.Text = TriMMApp.Mesh.Count.ToString();
            edgesTextBox.Text = TriMMApp.Mesh.Edges.Count.ToString();

            TriMMApp.Control.VertexPicked += new VertexPickedEventHandler(Control_VertexPicked);
            TriMMApp.Control.EdgePicked += new EdgePickedEventHandler(Control_EdgePicked);
            TriMMApp.Control.TrianglePicked += new TrianglePickedEventHandler(Control_TrianglePicked);
        }


        /// <summary>
        /// Refreshes the values of the control.
        /// </summary>
        private void RefreshControl() {
            aNumericUpDown.Maximum = TriMMApp.Mesh.Vertices.Count - 1;
            bNumericUpDown.Maximum = TriMMApp.Mesh.Vertices.Count - 1;
            cNumericUpDown.Maximum = TriMMApp.Mesh.Vertices.Count - 1;
            e1NumericUpDown.Maximum = TriMMApp.Mesh.Vertices.Count - 1;
            e2NumericUpDown.Maximum = TriMMApp.Mesh.Vertices.Count - 1;
            verticesTextBox.Text = TriMMApp.Mesh.Vertices.Count.ToString();
            trianglesTextBox.Text = TriMMApp.Mesh.Count.ToString();
            edgesTextBox.Text = TriMMApp.Mesh.Edges.Count.ToString();

            if (view != null) { view.RefreshView(); }
            TriMMApp.Control.Refresh();
        }

        /// <summary>
        /// Clears the information about the observed Vertex, Edge and Triangle.
        /// </summary>
        private void ClearObserved() {
            TriMMApp.Control.Info.Clear();
            TriMMApp.Control.UseColorArray = false;
            TriMMApp.Mesh.ObservedVertex = -1;
            TriMMApp.Mesh.ObservedEdge = -1;
            TriMMApp.Mesh.ObservedTriangle = -1;
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
                Cursor = System.Windows.Input.Cursors.Wait;

                // Parses the file.
                if (file.EndsWith(".off")) {
                    OffParser.Parse(reader);
                } else if (file.EndsWith(".stl")) {
                    STLParser.Parse(reader);
                } else if (file.EndsWith(".ply")) {
                    PlyParser.Parse(reader);
                } else if (file.EndsWith(".obj")) {
                    ObjParser.Parse(reader);
                }

                InitializeControl();
                saveMenuItem.Visibility = closeMenuItem.Visibility = viewMenuItem.Visibility = meshGroupBox.Visibility = manipulationTabControl.Visibility = Visibility.Visible;

#if !DEBUG
            } catch (Exception exception) {
                System.Windows.MessageBox.Show(exception.Message, TriMMApp.Lang.GetElementsByTagName("ErrorTitle")[0].InnerText, MessageBoxButton.OK, MessageBoxImage.Error);
            } finally {
#endif
                reader.Close();
                Cursor = System.Windows.Input.Cursors.Arrow;
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
        /// <param name="sender">openMenuItem</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void OpenMenuItem_Click(object sender, RoutedEventArgs e) {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
#if !DEBUG
            try {
#endif
                ofd.CheckFileExists = true;
                ofd.DefaultExt = "off";
                string files = TriMMApp.Lang.GetElementsByTagName("Files")[0].InnerText;
                ofd.Filter = "OOGL " + files + " (*.off)|*.off|STL " + files + " (*.stl)|*.stl|PLY " + files + " (ascii) (*.ply)|*.ply|Wavefront OBJ " + files + " (*.obj)|*.obj";
                ofd.Multiselect = false;
                ofd.Title = TriMMApp.Lang.GetElementsByTagName("OpenFileTitle")[0].InnerText;
                if (ofd.ShowDialog() == true) {
                    CloseFile(sender, e);
                    OpenFile(ofd.FileName);
                }
#if !DEBUG
            } catch (Exception exception) {
                System.Windows.MessageBox.Show(exception.Message, TriMMApp.Lang.GetElementsByTagName("ErrorTitle")[0].InnerText, MessageBoxButton.OK, MessageBoxImage.Error);
            }
#endif
        }

        /// <summary>
        /// Saves the TriangleMesh to an *.OFF file.
        /// </summary>
        /// <param name="sender">saveMenuItem</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void SaveMenuItem_Click(object sender, RoutedEventArgs e) {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
#if !DEBUG
            try {
#endif
                sfd.AddExtension = true;
                sfd.OverwritePrompt = true;
                sfd.DefaultExt = "off";
                string files = TriMMApp.Lang.GetElementsByTagName("Files")[0].InnerText;
                sfd.Filter = "OOGL " + files + " (*.off)|*.off|STL ASCII-" + files + " (*.stl)|*.stl|STL Binary-" + files + " (*.stl)|*.stl|PLY " + files + " (ascii) (*.ply)|*.ply|Wavefront OBJ " + files + " (*.obj)|*.obj";
                sfd.Title = TriMMApp.Lang.GetElementsByTagName("SaveFileTitle")[0].InnerText;
                if (sfd.ShowDialog() == true) {
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
                System.Windows.MessageBox.Show(exception.Message, TriMMApp.Lang.GetElementsByTagName("ErrorTitle")[0].InnerText, MessageBoxButton.OK, MessageBoxImage.Error);
            }
#endif
        }

        /// <summary>
        /// Destroys the TriMMControl and resets the GUI-elements.
        /// </summary>
        /// <param name="sender">closeMenuItem</param>
        /// <param name="e">Standard EventArgs</param>
        private void CloseFile(object sender, RoutedEventArgs e) {
            if (setWin != null) { setWin.Close(); }
            if (view != null) { view.Close(); }
            saveMenuItem.Visibility = closeMenuItem.Visibility = viewMenuItem.Visibility = meshGroupBox.Visibility = manipulationTabControl.Visibility = Visibility.Collapsed;
            normalComboBox.SelectedIndex = 0;
            TriMMApp.Mesh = null;
            if (TriMMApp.Control != null) {
                TriMMApp.Control.DestroyContexts();
                TriMMApp.Control = null;
            }
        }

        /// <summary>
        /// Closes the Window.
        /// </summary>
        /// <param name="sender">exitMenuItem</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void ExitMenuItem_Click(object sender, RoutedEventArgs e) { this.Close(); }

        /// <summary>
        /// Opens a new TriMMView, if none exists, or focuses the existing TriMMView.
        /// When the TriMMView is opened, the visualGroupBox is shown and new options for interaction are available.
        /// The TriMMView is located next to this Form. An EventHandler for the FormClosed event is bound.
        /// </summary>
        /// <param name="sender">viewMenuItem</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void ViewMenuItem_Click(object sender, RoutedEventArgs e) {
            if (view == null) {
                view = new TriMMView();
                view.Left = this.Left + this.ActualWidth;
                view.Top = this.Top;
                view.Show();
                view.Closed += new EventHandler(View_Closed);
            } else {
                view.Left = this.Left + this.ActualWidth;
                view.Top = this.Top;
                view.Focus();
            }
        }

        /// <summary>
        /// When the TriMMView is closed, several CheckBoxes are reset and view is set to null.
        /// The EventHandler for the FormClosed event is also unbound.
        /// </summary>
        /// <param name="sender">View</param>
        /// <param name="e">Standard FormClosedEventArgs</param>
        private void View_Closed(object sender, EventArgs e) {
            view.Closed -= View_Closed;
            view = null;
        }

        /// <summary>
        /// Opens the SettingsWindow to change the standard Settings.
        /// </summary>
        /// <param name="sender">settingsMenuItem</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void SettingsMenuItem_Click(object sender, RoutedEventArgs e) {
            if (setWin == null) {
                setWin = new SettingsWindow();
                setWin.Top = this.Top + this.ActualHeight;
                setWin.Left = 0;
                setWin.Show();
                setWin.Closed += new EventHandler(SetWin_Closed);
            } else {
                setWin.Top = this.Top + this.ActualHeight;
                setWin.Left = 0;
                setWin.Focus(); }
        }

        /// <summary>
        /// When the SettingsWindow is closed, setWin is set to null.
        /// The EventHandler for the FormClosed event is also unbound.
        /// </summary>
        /// <param name="sender">SettingsWindow</param>
        /// <param name="e">Standard EventArgs</param>
        void SetWin_Closed(object sender, EventArgs e) {
            setWin.Closed -= SetWin_Closed;
            setWin = null;
        }

        /// <summary>
        /// Shows the About dialog.
        /// </summary>
        /// <param name="sender">infoMenuItem</param>
        /// <param name="e">Standard EventArgs</param>
        private void InfoMenuItem_Click(object sender, RoutedEventArgs e) {
            About about = new About();
            about.ShowDialog();
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
                TriMMApp.Control.Info.Add(TriMMApp.Lang.GetElementsByTagName("Vertex")[0].InnerText + " " + picked[0] + " = " + TriMMApp.Mesh.Vertices[picked[0]].ToString());
                TriMMApp.Mesh.ObservedVertex = picked[0];
                xNumericUpDown.Value = (decimal)TriMMApp.Mesh.Vertices[picked[0]][0];
                yNumericUpDown.Value = (decimal)TriMMApp.Mesh.Vertices[picked[0]][1];
                zNumericUpDown.Value = (decimal)TriMMApp.Mesh.Vertices[picked[0]][2];
            }
            TriMMApp.Control.Refresh();
        }

        /// <summary>
        /// Shows information about the picked Edge.
        /// </summary>
        /// <param name="picked">The index of the picked Edge, or an empty list.</param>
        private void Control_EdgePicked(List<int> picked) {
            ClearObserved();
            if (picked.Count != 0) {
                TriMMApp.Control.Info.Add(TriMMApp.Lang.GetElementsByTagName("Edge")[0].InnerText + " " + picked[0] + " = " + TriMMApp.Mesh.Edges.Values[picked[0]].ToString());
                TriMMApp.Control.Info.Add(TriMMApp.Lang.GetElementsByTagName("Vertex")[0].InnerText + " " + TriMMApp.Mesh.Edges.Values[picked[0]][0] + " = " + TriMMApp.Mesh.Vertices[TriMMApp.Mesh.Edges.Values[picked[0]][0]].ToString());
                TriMMApp.Control.Info.Add(TriMMApp.Lang.GetElementsByTagName("Vertex")[0].InnerText + " " + TriMMApp.Mesh.Edges.Values[picked[0]][1] + " = " + TriMMApp.Mesh.Vertices[TriMMApp.Mesh.Edges.Values[picked[0]][1]].ToString());
                TriMMApp.Mesh.ObservedEdge = picked[0];
                e1NumericUpDown.Value = (decimal)TriMMApp.Mesh.Edges.Values[picked[0]][0];
                e2NumericUpDown.Value = (decimal)TriMMApp.Mesh.Edges.Values[picked[0]][1];
            }
            TriMMApp.Control.Refresh();
        }

        /// <summary>
        /// Shows information about the picked Triangle.
        /// </summary>
        /// <param name="picked">The index of the picked Triangle, or an empty list.</param>
        private void Control_TrianglePicked(List<int> picked) {
            ClearObserved();
            if (picked.Count != 0) {
                TriMMApp.Control.Info.Add(TriMMApp.Lang.GetElementsByTagName("Triangle")[0].InnerText + " " + picked[0] + " = (" + TriMMApp.Mesh[picked[0]][0] + ", " + TriMMApp.Mesh[picked[0]][1] + ", " + TriMMApp.Mesh[picked[0]][2] + ")");
                TriMMApp.Control.Info.Add(TriMMApp.Lang.GetElementsByTagName("Vertex")[0].InnerText + " " + TriMMApp.Mesh[picked[0]][0] + " = " + TriMMApp.Mesh[picked[0], 0].ToString());
                TriMMApp.Control.Info.Add(TriMMApp.Lang.GetElementsByTagName("Vertex")[0].InnerText + " " + TriMMApp.Mesh[picked[0]][1] + " = " + TriMMApp.Mesh[picked[0], 1].ToString());
                TriMMApp.Control.Info.Add(TriMMApp.Lang.GetElementsByTagName("Vertex")[0].InnerText + " " + TriMMApp.Mesh[picked[0]][2] + " = " + TriMMApp.Mesh[picked[0], 2].ToString());
                TriMMApp.Mesh.ObservedTriangle = picked[0];
                TriMMApp.Mesh.SetMarkedTriangleColorArray(picked[0]);
                TriMMApp.Control.UseColorArray = true;
                aNumericUpDown.Value = (decimal)TriMMApp.Mesh[picked[0]][0];
                bNumericUpDown.Value = (decimal)TriMMApp.Mesh[picked[0]][1];
                cNumericUpDown.Value = (decimal)TriMMApp.Mesh[picked[0]][2];
            }
            TriMMApp.Control.Refresh();

        }

        #endregion

        #region Manipulation

        #region Vertices

        /// <summary>
        /// Removes single Vertices that are not part of any triangles.
        /// </summary>
        /// <param name="sender">removeSinglesButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void RemoveSinglesButton_Click(object sender, RoutedEventArgs e) {
            Cursor = System.Windows.Input.Cursors.Wait;

            ClearObserved();

            List<int> markedVertices = new List<int>();

            for (int i = 0; i < TriMMApp.Mesh.Vertices.Count; i++) { if (TriMMApp.Mesh.Vertices[i].Neighborhood.Count == 0) { markedVertices.Add(i); } }
            markedVertices.Sort();

            for (int i = markedVertices.Count - 1; i >= 0; i--) {
                // Remove Vertex
                TriMMApp.Mesh.Vertices.RemoveAt(markedVertices[i]);

                // Adjust Triangles
                for (int j = 0; j < TriMMApp.Mesh.Count; j++) {
                    List<int> ordered = new List<int>();
                    for (int k = 0; k < 3; k++) { ordered.Add(TriMMApp.Mesh[j][k]); }
                    ordered.Sort();
                    for (int k = 0; k < 3; k++) { if (ordered[k] >= markedVertices[i]) { TriMMApp.Mesh[j].Replace(ordered[k], ordered[k] - 1); } }
                }
            }

            TriMMApp.Mesh.Finish(true, true);

            RefreshControl();
            Cursor = System.Windows.Input.Cursors.Arrow;
        }

        /// <summary>
        /// Removes Vertices that are in the mesh more than once and adjusts the attached Triangles.
        /// </summary>
        /// <param name="sender">removeDoubleVertButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void RemoveDoubleVertButton_Click(object sender, RoutedEventArgs e) {
            Cursor = System.Windows.Input.Cursors.Wait;

            ClearObserved();

            List<List<int>> equivalent = new List<List<int>>();
            List<int> remove = new List<int>();

            List<int> first = new List<int>();
            first.Add(0);
            equivalent.Add(first);
            bool added;

            // Find equivalent Vertices.
            for (int i = 1; i < TriMMApp.Mesh.Vertices.Count; i++) {
                added = false;
                for (int j = 0; j < equivalent.Count; j++) {
                    if (TriMMApp.Mesh.Vertices[i].Equals(TriMMApp.Mesh.Vertices[equivalent[j][0]])) {
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
                    for (int k = 0; k < TriMMApp.Mesh.Count; k++) { if (TriMMApp.Mesh[k].Contains(equivalent[i][j])) { TriMMApp.Mesh[k].Replace(equivalent[i][j], equivalent[i][0]); } }
                }
            }

            remove.Sort();
            // Remove Vertices
            for (int i = remove.Count - 1; i >= 0; i--) { TriMMApp.Mesh.Vertices.RemoveAt(remove[i]); }

            TriMMApp.Mesh.Finish(true, true);

            RefreshControl();
            Cursor = System.Windows.Input.Cursors.Arrow;
        }

        /// <summary>
        /// Removes Vertices and the attached Triangles, that have only two neighbors.
        /// </summary>
        /// <param name="sender">remove2NVerticesButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void Remove2NVerticesButton_Click(object sender, RoutedEventArgs e) {
            Cursor = System.Windows.Input.Cursors.Wait;

            ClearObserved();

            List<int> markedVertices;

            do {
                markedVertices = new List<int>();

                for (int i = 0; i < TriMMApp.Mesh.Vertices.Count; i++) { if (TriMMApp.Mesh.Vertices[i].Neighborhood.Count == 2) { markedVertices.Add(i); } }
                markedVertices.Sort();

                for (int i = markedVertices.Count - 1; i >= 0; i--) {
                    List<int> adjacent = TriMMApp.Mesh.Vertices[markedVertices[i]].Triangles;
                    adjacent.Sort();

                    // Remove Vertex
                    TriMMApp.Mesh.Vertices.RemoveAt(markedVertices[i]);

                    // Remove Triangles attached to this Vertex.
                    for (int j = adjacent.Count - 1; j >= 0; j--) { TriMMApp.Mesh.RemoveAt(adjacent[j]); }

                    // Adjust Triangles
                    for (int j = 0; j < TriMMApp.Mesh.Count; j++) {
                        List<int> ordered = new List<int>();
                        for (int k = 0; k < 3; k++) { ordered.Add(TriMMApp.Mesh[j][k]); }
                        ordered.Sort();
                        for (int k = 0; k < 3; k++) { if (ordered[k] >= markedVertices[i]) { TriMMApp.Mesh[j].Replace(ordered[k], ordered[k] - 1); } }
                    }

                    TriMMApp.Mesh.Finish(true, true);
                }

            } while (markedVertices.Count != 0);

            RefreshControl();
            Cursor = System.Windows.Input.Cursors.Arrow;
        }

        /// <summary>
        /// Removes the Vertex given by the coordinates in the NumericUpDowns, if it exists, and all the Triangles attached to it.
        /// </summary>
        /// <param name="sender">removeSelectedButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void RemoveSelectedButton_Click(object sender, RoutedEventArgs e) {
            Cursor = System.Windows.Input.Cursors.Wait;

            Vertex vertex = new Vertex((double)xNumericUpDown.Value, (double)yNumericUpDown.Value, (double)zNumericUpDown.Value);
            int remove = TriMMApp.Mesh.Vertices.IndexOf(vertex);
            if (remove != -1) {
                ClearObserved();

                List<int> adjacent = TriMMApp.Mesh.Vertices[remove].Triangles;
                adjacent.Sort();

                // Remove Vertex
                TriMMApp.Mesh.Vertices.RemoveAt(remove);

                // Remove Triangles attached to this Vertex.
                for (int j = adjacent.Count - 1; j >= 0; j--) { TriMMApp.Mesh.RemoveAt(adjacent[j]); }

                // Adjust Triangles
                for (int j = 0; j < TriMMApp.Mesh.Count; j++) {
                    List<int> ordered = new List<int>();
                    for (int k = 0; k < 3; k++) { ordered.Add(TriMMApp.Mesh[j][k]); }
                    ordered.Sort();
                    for (int k = 0; k < 3; k++) { if (ordered[k] >= remove) { TriMMApp.Mesh[j].Replace(ordered[k], ordered[k] - 1); } }
                }

                TriMMApp.Mesh.Finish(true, true);
                RefreshControl();
            }

            Cursor = System.Windows.Input.Cursors.Arrow;
        }

        /// <summary>
        /// Adds the Vertex given by the values in the corresponding NumericUpDowns.
        /// </summary>
        /// <param name="sender">addVertexButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void AddVertexButton_Click(object sender, RoutedEventArgs e) {
            Cursor = System.Windows.Input.Cursors.Wait;

            ClearObserved();

            Vertex newVertex = new Vertex((double)xNumericUpDown.Value, (double)yNumericUpDown.Value, (double)zNumericUpDown.Value);
            TriMMApp.Mesh.Vertices.Add(newVertex);

            TriMMApp.Mesh.Finish(true, true);

            RefreshControl();
            Cursor = System.Windows.Input.Cursors.Arrow;
        }

        /// <summary>
        /// Moves the observed Vertex to the values in the corresponding NumericUpDowns.
        /// </summary>
        /// <param name="sender">moveObservedButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void MoveObservedButton_Click(object sender, RoutedEventArgs e) {
            Cursor = System.Windows.Input.Cursors.Wait;

            if (TriMMApp.Mesh.ObservedVertex != -1) {
                TriMMApp.Mesh.Vertices[TriMMApp.Mesh.ObservedVertex] = new Vertex((double)xNumericUpDown.Value, (double)yNumericUpDown.Value, (double)zNumericUpDown.Value);

                ClearObserved();
                TriMMApp.Mesh.Finish(true, true);
                RefreshControl();
            }

            Cursor = System.Windows.Input.Cursors.Arrow;
        }

        /// <summary>
        /// Transposes the observed Vertex by the values in the x-, y- and zNumericUpDown.
        /// </summary>
        /// <param name="sender">transposeVertexButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void TransposeVertexButton_Click(object sender, RoutedEventArgs e) {
            Cursor = System.Windows.Input.Cursors.Wait;

            if (TriMMApp.Mesh.ObservedVertex != -1) {
                TriMMApp.Mesh.Vertices[TriMMApp.Mesh.ObservedVertex] = (TriMMApp.Mesh.Vertices[TriMMApp.Mesh.ObservedVertex] + new Vertex((double)xNumericUpDown.Value, (double)yNumericUpDown.Value, (double)zNumericUpDown.Value)).ToVertex();

                ClearObserved();
                TriMMApp.Mesh.Finish(true, true);
                RefreshControl();
            }

            Cursor = System.Windows.Input.Cursors.Arrow;
        }


        /// <summary>
        /// Moves the Vertex determined by the coordinates chosen in the NumericUpDowns along its normalvector by the distance selected in the distanceNumericUpDown.
        /// </summary>
        /// <param name="sender">moveAlongNormalButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void MoveAlongNormalButton_Click(object sender, RoutedEventArgs e) {
            Cursor = System.Windows.Input.Cursors.Wait;

            Vertex vertex = new Vertex((double)xNumericUpDown.Value, (double)yNumericUpDown.Value, (double)zNumericUpDown.Value);
            int move = TriMMApp.Mesh.Vertices.IndexOf(vertex);
            if (move != -1) {
                TriMMApp.Mesh.Vertices[move] = (TriMMApp.Mesh.Vertices[move] + (double)distanceNumericUpDown.Value * TriMMApp.Mesh.Vertices[move].Normal).ToVertex();

                ClearObserved();
                TriMMApp.Mesh.Finish(true, true);
                RefreshControl();
            }

            Cursor = System.Windows.Input.Cursors.Arrow;
        }

        #endregion

        #region Edges

        /// <summary>
        /// Flips the Edge given by the two selected Vertices.
        /// </summary>
        /// <param name="sender">flipEdgeButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void FlipEdgeButton_Click(object sender, RoutedEventArgs e) {
            Cursor = System.Windows.Input.Cursors.Wait;

            int ind1 = (int)e1NumericUpDown.Value;
            int ind2 = (int)e2NumericUpDown.Value;

            if ((ind1 != -1) && (ind2 != -1)) {
                double length = Vector.Distance(TriMMApp.Mesh.Vertices[ind1], TriMMApp.Mesh.Vertices[ind2]);
                Edge theEdge = new Edge(ind1, ind2, length);

                if (TriMMApp.Mesh.Edges.ContainsKey(theEdge.Key)) {
                    List<int> triangles = TriMMApp.Mesh.Edges[theEdge.Key].Triangles;
                    triangles.Sort();

                    // Get the Triangles that Edge belongs to, connects the Vertices opposite the Edge with a new Edge and removes the old Edge.
                    if (triangles.Count == 2) {
                        int o1 = TriMMApp.Mesh[triangles[0]].GetOppositeCorner(theEdge);
                        int o2 = TriMMApp.Mesh[triangles[1]].GetOppositeCorner(theEdge);

                        int triInd1 = TriMMApp.Mesh[triangles[0]].IndexOf(o1);
                        int triInd2 = TriMMApp.Mesh[triangles[1]].IndexOf(o2);

                        Triangle tri1 = new Triangle(o1, TriMMApp.Mesh[triangles[0]][(triInd1 + 1) % 3], o2);
                        Triangle tri1per1 = new Triangle(tri1[0], tri1[2], tri1[1]);
                        Triangle tri1per2 = new Triangle(tri1[1], tri1[0], tri1[2]);
                        Triangle tri1per3 = new Triangle(tri1[1], tri1[2], tri1[0]);
                        Triangle tri1per4 = new Triangle(tri1[2], tri1[0], tri1[1]);
                        Triangle tri1per5 = new Triangle(tri1[2], tri1[1], tri1[0]);

                        Triangle tri2 = new Triangle(o2, TriMMApp.Mesh[triangles[1]][(triInd2 + 1) % 3], o1);
                        Triangle tri2per1 = new Triangle(tri2[0], tri2[2], tri2[1]);
                        Triangle tri2per2 = new Triangle(tri2[1], tri2[0], tri2[2]);
                        Triangle tri2per3 = new Triangle(tri2[1], tri2[2], tri2[0]);
                        Triangle tri2per4 = new Triangle(tri2[2], tri2[0], tri2[1]);
                        Triangle tri2per5 = new Triangle(tri2[2], tri2[1], tri2[0]);

                        bool isNotIn1 = true;
                        bool isNotIn2 = true;

                        if (TriMMApp.Mesh.Contains(tri1) || TriMMApp.Mesh.Contains(tri1per1) || TriMMApp.Mesh.Contains(tri1per2) || TriMMApp.Mesh.Contains(tri1per3) || TriMMApp.Mesh.Contains(tri1per4) || TriMMApp.Mesh.Contains(tri1per5)) { isNotIn1 = false; }
                        if (TriMMApp.Mesh.Contains(tri2) || TriMMApp.Mesh.Contains(tri2per1) || TriMMApp.Mesh.Contains(tri2per2) || TriMMApp.Mesh.Contains(tri2per3) || TriMMApp.Mesh.Contains(tri2per4) || TriMMApp.Mesh.Contains(tri2per5)) { isNotIn2 = false; }

                        if (isNotIn1 && (TriMMApp.Mesh.IsTriangle(o1, TriMMApp.Mesh[triangles[0]][(triInd1 + 1) % 3], o2))) { TriMMApp.Mesh.Add(tri1); }
                        if (isNotIn2 && (TriMMApp.Mesh.IsTriangle(o2, TriMMApp.Mesh[triangles[1]][(triInd2 + 1) % 3], o1))) { TriMMApp.Mesh.Add(tri2); }

                        TriMMApp.Mesh.RemoveAt(triangles[1]);
                        TriMMApp.Mesh.RemoveAt(triangles[0]);

                        TriMMApp.Mesh.Finish(true, true);
                        ClearObserved();
                        RefreshControl();
                    }
                }
            }

            Cursor = System.Windows.Input.Cursors.Arrow;
        }

        /// <summary>
        /// Subdivides the selected Edge by adding the midedge Vertex
        /// and subdividing the incident Triangles with the new Vertex.
        /// </summary>
        /// <param name="sender">subdivideEdgeButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void SubdivideEdgeButton_Click(object sender, RoutedEventArgs e) {
            Cursor = System.Windows.Input.Cursors.Wait;

            ClearObserved();

            int ind1 = (int)e1NumericUpDown.Value;
            int ind2 = (int)e2NumericUpDown.Value;

            if ((ind1 != -1) && (ind2 != -1)) {
                double length = Vector.Distance(TriMMApp.Mesh.Vertices[ind1], TriMMApp.Mesh.Vertices[ind2]);
                Edge theEdge = new Edge(ind1, ind2, length);

                if (TriMMApp.Mesh.Edges.ContainsKey(theEdge.Key)) {
                    List<int> triangles = TriMMApp.Mesh.Edges[theEdge.Key].Triangles;
                    triangles.Sort();
                    Vertex midedge = (0.5 * (TriMMApp.Mesh.Vertices[ind1] + TriMMApp.Mesh.Vertices[ind2])).ToVertex();
                    TriMMApp.Mesh.Vertices.Add(midedge);

                    for (int i = triangles.Count - 1; i >= 0; i--) {
                        Triangle replace = new Triangle(TriMMApp.Mesh[triangles[i]][0], TriMMApp.Mesh[triangles[i]][1], TriMMApp.Mesh[triangles[i]][2]);
                        replace.Replace(ind1, TriMMApp.Mesh.Vertices.Count - 1);
                        TriMMApp.Mesh.Add(replace);
                        replace = new Triangle(TriMMApp.Mesh[triangles[i]][0], TriMMApp.Mesh[triangles[i]][1], TriMMApp.Mesh[triangles[i]][2]);
                        replace.Replace(ind2, TriMMApp.Mesh.Vertices.Count - 1);
                        TriMMApp.Mesh.Add(replace);
                        TriMMApp.Mesh.RemoveAt(triangles[i]);
                    }

                    TriMMApp.Mesh.Finish(true, true);
                }
            }

            RefreshControl();
            Cursor = System.Windows.Input.Cursors.Arrow;
        }

        /// <summary>
        /// Removes the Edge given by the two selected Vertices.
        /// Also removes the incident Triangles.
        /// </summary>
        /// <param name="sender">removeEdgeButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void RemoveEdgeButton_Click(object sender, RoutedEventArgs e) {
            Cursor = System.Windows.Input.Cursors.Wait;

            ClearObserved();

            int ind1 = (int)e1NumericUpDown.Value;
            int ind2 = (int)e2NumericUpDown.Value;

            if ((ind1 != -1) && (ind2 != -1)) {
                double length = Vector.Distance(TriMMApp.Mesh.Vertices[ind1], TriMMApp.Mesh.Vertices[ind2]);
                Edge theEdge = new Edge(ind1, ind2, length);

                if (TriMMApp.Mesh.Edges.ContainsKey(theEdge.Key)) {
                    List<int> triangles = TriMMApp.Mesh.Edges[theEdge.Key].Triangles;
                    triangles.Sort();
                    for (int i = triangles.Count - 1; i >= 0; i--) { TriMMApp.Mesh.RemoveAt(triangles[i]); }

                    TriMMApp.Mesh.Finish(true, true);
                }
            }

            RefreshControl();
            Cursor = System.Windows.Input.Cursors.Arrow;
        }

        #endregion

        #region Triangles

        /// <summary>
        /// Removes Triangles, that are not really Triangles, because they have three colinear Vertices.
        /// </summary>
        /// <param name="sender">removeColinButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void RemoveColinButton_Click(object sender, RoutedEventArgs e) {
            Cursor = System.Windows.Input.Cursors.Wait;

            ClearObserved();

            for (int i = TriMMApp.Mesh.Count - 1; i >= 0; i--) { if (!TriMMApp.Mesh.IsTriangle(i)) { TriMMApp.Mesh.RemoveAt(i); } }

            TriMMApp.Mesh.Finish(true, true);
            RefreshControl();
            Cursor = System.Windows.Input.Cursors.Arrow;
        }

        /// <summary>
        /// Removes double Triangles regardless of their orientation.
        /// </summary>
        /// <param name="sender">removeDoubleButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void RemoveDoubleButton_Click(object sender, RoutedEventArgs e) {
            Cursor = System.Windows.Input.Cursors.Wait;

            ClearObserved();

            for (int i = TriMMApp.Mesh.Count - 1; i >= 0; i--) {
                List<Triangle> equals = TriMMApp.Mesh.Where(t => t.Equals(TriMMApp.Mesh[i])).ToList();
                if (equals.Count > 1) { TriMMApp.Mesh.RemoveAt(i); }
            }
            TriMMApp.Mesh.Finish(true, true);
            RefreshControl();
            Cursor = System.Windows.Input.Cursors.Arrow;
        }

        /// <summary>
        /// Changes the orientation of all Triangles.
        /// </summary>
        /// <param name="sender">flipAllTrianglesButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void FlipAllTrianglesButton_Click(object sender, RoutedEventArgs e) {
            Cursor = System.Windows.Input.Cursors.Wait;
            TriMMApp.Mesh.FlipAllTriangles();
            if (TriMMApp.Mesh.ObservedTriangle != -1) {
                aNumericUpDown.Value = (decimal)TriMMApp.Mesh[TriMMApp.Mesh.ObservedTriangle][0];
                bNumericUpDown.Value = (decimal)TriMMApp.Mesh[TriMMApp.Mesh.ObservedTriangle][1];
                cNumericUpDown.Value = (decimal)TriMMApp.Mesh[TriMMApp.Mesh.ObservedTriangle][2];
            }
            RefreshControl();
            Cursor = System.Windows.Input.Cursors.Arrow;
        }

        /// <summary>
        /// Changes the orientation of the Triangle determined by the Vertices selected above.
        /// </summary>
        /// <param name="sender">flipSelectedTriangleButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void FlipSelectedTriangleButton_Click(object sender, RoutedEventArgs e) {
            if (TriMMApp.Mesh.IsTriangle((int)aNumericUpDown.Value, (int)bNumericUpDown.Value, (int)cNumericUpDown.Value)) {
                Triangle triangle = new Triangle((int)aNumericUpDown.Value, (int)bNumericUpDown.Value, (int)cNumericUpDown.Value);
                int index = TriMMApp.Mesh.IndexOf(triangle);
                if (index != -1) {
                    Cursor = System.Windows.Input.Cursors.Wait;
                    TriMMApp.Mesh.FlipTriangle(index);
                    aNumericUpDown.Value = (decimal)TriMMApp.Mesh[index][0];
                    bNumericUpDown.Value = (decimal)TriMMApp.Mesh[index][1];
                    cNumericUpDown.Value = (decimal)TriMMApp.Mesh[index][2];
                    RefreshControl();
                    Cursor = System.Windows.Input.Cursors.Arrow;
                }
            }

        }

        /// <summary>
        /// Subdivides all Triangles, one subdivision-step at a time.
        /// </summary>
        /// <param name="sender">subdivideMeshButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void SubdivideMeshButton_Click(object sender, RoutedEventArgs e) {
            Cursor = System.Windows.Input.Cursors.Wait;

            ClearObserved();
            TriMMApp.Mesh.Subdivide(1);

            RefreshControl();
            Cursor = System.Windows.Input.Cursors.Arrow;
        }

        /// <summary>
        /// Subdivides the Triangle currently observed.
        /// </summary>
        /// <param name="sender">subdivideTriangleButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void SubdivideTriangleButton_Click(object sender, RoutedEventArgs e) {
            if (TriMMApp.Mesh.IsTriangle((int)aNumericUpDown.Value, (int)bNumericUpDown.Value, (int)cNumericUpDown.Value)) {
                Triangle triangle = new Triangle((int)aNumericUpDown.Value, (int)bNumericUpDown.Value, (int)cNumericUpDown.Value);
                int index = TriMMApp.Mesh.IndexOf(triangle);
                if (index != -1) {
                    Cursor = System.Windows.Input.Cursors.Wait;
                    TriMMApp.Mesh.SubdivideTriangle(index);

                    ClearObserved();
                    RefreshControl();
                    Cursor = System.Windows.Input.Cursors.Arrow;
                }
            }
        }

        /// <summary>
        /// Removes a Triangle given by the three Vertices selected in the NumericUpDowns (all permutations), if it exists.
        /// </summary>
        /// <param name="sender">removeTriangleButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void RemoveTriangleButton_Click(object sender, RoutedEventArgs e) {
            if (TriMMApp.Mesh.IsTriangle((int)aNumericUpDown.Value, (int)bNumericUpDown.Value, (int)cNumericUpDown.Value)) {
                Cursor = System.Windows.Input.Cursors.Wait;
                ClearObserved();

                Triangle remove = new Triangle((int)aNumericUpDown.Value, (int)bNumericUpDown.Value, (int)cNumericUpDown.Value);
                TriMMApp.Mesh.Remove(remove);
                remove = new Triangle((int)aNumericUpDown.Value, (int)cNumericUpDown.Value, (int)bNumericUpDown.Value);
                TriMMApp.Mesh.Remove(remove);
                remove = new Triangle((int)bNumericUpDown.Value, (int)aNumericUpDown.Value, (int)cNumericUpDown.Value);
                TriMMApp.Mesh.Remove(remove);
                remove = new Triangle((int)bNumericUpDown.Value, (int)cNumericUpDown.Value, (int)aNumericUpDown.Value);
                TriMMApp.Mesh.Remove(remove);
                remove = new Triangle((int)cNumericUpDown.Value, (int)aNumericUpDown.Value, (int)bNumericUpDown.Value);
                TriMMApp.Mesh.Remove(remove);
                remove = new Triangle((int)cNumericUpDown.Value, (int)bNumericUpDown.Value, (int)aNumericUpDown.Value);
                TriMMApp.Mesh.Remove(remove);

                TriMMApp.Mesh.Finish(true, true);

                RefreshControl();
                Cursor = System.Windows.Input.Cursors.Arrow;
            }
        }

        /// <summary>
        /// Adds a Triangle given by the three Vertices selected in the NumericUpDowns.
        /// </summary>
        /// <param name="sender">addTriangleButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void AddTriangleButton_Click(object sender, RoutedEventArgs e) {
            Cursor = System.Windows.Input.Cursors.Wait;

            ClearObserved();

            if (((int)aNumericUpDown.Value != -1) && ((int)bNumericUpDown.Value != -1) && ((int)cNumericUpDown.Value != -1)) {
                if (TriMMApp.Mesh.IsTriangle((int)aNumericUpDown.Value, (int)bNumericUpDown.Value, (int)cNumericUpDown.Value)) {
                    Triangle newTriangle = new Triangle((int)aNumericUpDown.Value, (int)bNumericUpDown.Value, (int)cNumericUpDown.Value);
                    TriMMApp.Mesh.Add(newTriangle);
                    TriMMApp.Mesh.Finish(true, true);
                }

                RefreshControl();
            }

            Cursor = System.Windows.Input.Cursors.Arrow;
        }

        #endregion

        #endregion

        /// <summary>
        /// When a new VertexNormalAlgorithm is selected the normals are calculated and the TriMMControl is refreshed.
        /// </summary>
        /// <param name="sender">normalComboBox</param>
        /// <param name="e">Standard SelectionChangedEventArgs</param>
        private void NormalComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            TriMMApp.Settings.NormalAlgoChanged -= Settings_NormalAlgoChanged;
            TriMMApp.Settings.NormalAlgo = normalComboBox.SelectedIndex;
            TriMMApp.Settings.NormalAlgoChanged += Settings_NormalAlgoChanged;

            if (TriMMApp.Mesh != null) {
                TriMMApp.Mesh.Finish(true, true);
                RefreshControl();
            }
        }

        /// <summary>
        /// When the setting for the Vertex normal algorithm is changed, the comboBox is adjusted.
        /// </summary>
        private void Settings_NormalAlgoChanged() { normalComboBox.SelectedIndex = TriMMApp.Settings.NormalAlgo; }

        /// <summary>
        /// When the language is changed, the NumericalUpDowns are triggered to update the decimal separator.
        /// </summary>
        private void Settings_LanguageChanged() {
            xNumericUpDown.UpButton(); xNumericUpDown.DownButton();
            yNumericUpDown.UpButton(); yNumericUpDown.DownButton();
            zNumericUpDown.UpButton(); zNumericUpDown.DownButton();
            distanceNumericUpDown.UpButton(); distanceNumericUpDown.DownButton();

            normalComboBox.SelectionChanged -= NormalComboBox_SelectionChanged;
            int temp = normalComboBox.SelectedIndex;
            normalComboBox.ItemsSource = null;
            normalComboBox.ItemsSource = TriMMApp.VertexNormalAlgorithms;
            normalComboBox.SelectedIndex = temp;
            normalComboBox.SelectionChanged += NormalComboBox_SelectionChanged;
        }

        /// <summary>
        /// Cleans up when the TriMMWindow is closed.
        /// </summary>
        /// <param name="sender">The TriMMWindow</param>
        /// <param name="e">Standard CancelEventArgs</param>
        private void Window_Closing(object sender, CancelEventArgs e) { CloseFile(sender, new RoutedEventArgs()); }

        #endregion

    }
}
