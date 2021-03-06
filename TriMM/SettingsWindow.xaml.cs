﻿// Part of TriMM, the TriangleMesh Manipulator.
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
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;

namespace TriMM {

    /// <summary>
    /// A Window to change the programs settings.
    /// </summary>
    public partial class SettingsWindow : Window {

        #region Fields

        private List<string> languageFiles = new List<string>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the SettingsWindow with all its controls.
        /// </summary>
        public SettingsWindow() {
            InitializeComponent();
            this.Icon = TriMMApp.Image;

            normalComboBox.ItemsSource = TriMMApp.VertexNormalAlgorithms;
            normalComboBox.SelectedIndex = TriMMApp.Settings.NormalAlgo;
            colorComboBox.SelectedIndex = 0;

            DirectoryInfo di = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "lang");
            FileInfo[] files = di.GetFiles("*.xml");

            for (int i = 0; i < files.Length; i++) { languageFiles.Add(files[i].Name.Substring(0, files[i].Name.IndexOf(".xml"))); }
            languageComboBox.ItemsSource = languageFiles;
            int index = languageFiles.IndexOf(TriMMApp.Settings.Language);
            if (index == -1) { index = languageComboBox.Items.IndexOf("english"); }
            languageComboBox.SelectedIndex = index;

            smoothCheckBox.IsChecked = TriMMApp.Settings.Smooth;
            solidCheckBox.IsChecked = TriMMApp.Settings.Solid;
            meshCheckBox.IsChecked = TriMMApp.Settings.Mesh;
            verticesCheckBox.IsChecked = TriMMApp.Settings.Vertices;
            xAxisCheckBox.IsChecked = TriMMApp.Settings.XAxis;
            yAxisCheckBox.IsChecked = TriMMApp.Settings.YAxis;
            zAxisCheckBox.IsChecked = TriMMApp.Settings.ZAxis;
            triangleNormalsCheckBox.IsChecked = TriMMApp.Settings.TriangleNormalVectors;
            vertexNormalsCheckBox.IsChecked = TriMMApp.Settings.VertexNormalVectors;
            pickingModeComboBox.SelectedIndex = TriMMApp.Settings.PickingMode;

            TriMMApp.Settings.SettingsChanged += new SettingsChangedEventHandler(Settings_SettingsChanged);
            TriMMApp.Settings.LanguageChanged += new LanguageChangedEventHandler(Settings_LanguageChanged);
        }

        #endregion

        #region Event Handling Stuff

        #region Menu

        /// <summary>
        /// Imports the Settings from a .set file.
        /// </summary>
        /// <param name="sender">loadSettingsMenuItem</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void LoadSettingsMenuItem_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.DefaultExt = "set";
            ofd.Filter = "TriMM " + TriMMApp.Lang.GetElementsByTagName("SETFilter")[0].InnerText + " (*.set)|*.set";
            ofd.Multiselect = false;
            ofd.Title = TriMMApp.Lang.GetElementsByTagName("OpenFileTitle")[0].InnerText;
            if (ofd.ShowDialog() == true) { TriMMApp.Settings.Parse(ofd.FileName); }
        }

        /// <summary>
        /// Exports the Settings to a .set file.
        /// </summary>
        /// <param name="sender">saveSettingsMenuItem</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void SaveSettingsMenuItem_Click(object sender, RoutedEventArgs e) {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.OverwritePrompt = true;
            sfd.DefaultExt = "set";
            sfd.Filter = "TriMM " + TriMMApp.Lang.GetElementsByTagName("SETFilter")[0].InnerText + " (*.set)|*.set";
            sfd.Title = TriMMApp.Lang.GetElementsByTagName("SaveFileTitle")[0].InnerText;
            if (sfd.ShowDialog() == true) { TriMMApp.Settings.WriteSET(sfd.FileName); }
        }


        /// <summary>
        /// Resets the settings to the standard values.
        /// </summary>
        /// <param name="sender">resetAllMenuItem</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void ResetAllMenuItem_Click(object sender, RoutedEventArgs e) { TriMMApp.Settings.SetToStandard(); }

        /// <summary>
        /// Accepts the current settings as the standard values.
        /// </summary>
        /// <param name="sender">makeStandardMenuItem</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void MakeStandardMenuItem_Click(object sender, RoutedEventArgs e) { TriMMApp.Settings.MakeStandard(); }

        /// <summary>
        /// When a new VertexNormalAlgorithm is selected the normals are calculated and the TriMMControl is refreshed.
        /// </summary>
        /// <param name="sender">normalComboBox</param>
        /// <param name="e">Standard SelectionChangedEventArgs</param>
        private void NormalComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) { TriMMApp.Settings.NormalAlgo = normalComboBox.SelectedIndex; }

        /// <summary>
        /// When a different language is selected the necessary changes are made.
        /// </summary>
        /// <param name="sender">languageComboBox</param>
        /// <param name="e">Standard SelectionChangedEventArgs</param>
        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            TriMMApp.Settings.Language = languageComboBox.SelectedItem.ToString();

            normalComboBox.SelectionChanged -= NormalComboBox_SelectionChanged;
            int temp = normalComboBox.SelectedIndex;
            normalComboBox.ItemsSource = null;
            normalComboBox.ItemsSource = TriMMApp.VertexNormalAlgorithms;
            normalComboBox.SelectedIndex = temp;
            normalComboBox.SelectionChanged += NormalComboBox_SelectionChanged;
        }

        #endregion

        #region Colors

        /// <summary>
        /// Adjusts the text in the colorTextBox and the color of the colorButton to match the
        /// color selected in the colorComboBox.
        /// </summary>
        /// <param name="sender">colorComboBox</param>
        /// <param name="e">Standard SelectionChangedEventArgs</param>
        private void ColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            switch (colorComboBox.SelectedIndex) {
                case 0:
                    colorTextBox.Text = TriMMApp.Settings.BackColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.BackColor.Color);
                    break;
                case 1:
                    colorTextBox.Text = TriMMApp.Settings.TextColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.TextColor.Color);
                    break;
                case 2:
                    colorTextBox.Text = TriMMApp.Settings.SolidColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.SolidColor.Color);
                    break;
                case 3:
                    colorTextBox.Text = TriMMApp.Settings.MeshColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.MeshColor.Color);
                    break;
                case 4:
                    colorTextBox.Text = TriMMApp.Settings.VertexColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.VertexColor.Color);
                    break;
                case 5:
                    colorTextBox.Text = TriMMApp.Settings.TriNormalColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.TriNormalColor.Color);
                    break;
                case 6:
                    colorTextBox.Text = TriMMApp.Settings.VertNormalColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.VertNormalColor.Color);
                    break;
                case 7:
                    colorTextBox.Text = TriMMApp.Settings.XAxisColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.XAxisColor.Color);
                    break;
                case 8:
                    colorTextBox.Text = TriMMApp.Settings.YAxisColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.YAxisColor.Color);
                    break;
                case 9:
                    colorTextBox.Text = TriMMApp.Settings.ZAxisColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.ZAxisColor.Color);
                    break;
                case 10:
                    colorTextBox.Text = TriMMApp.Settings.ObservedVertexColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.ObservedVertexColor.Color);
                    break;
                case 11:
                    colorTextBox.Text = TriMMApp.Settings.ObservedEdgeColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.ObservedEdgeColor.Color);
                    break;
                case 12:
                    colorTextBox.Text = TriMMApp.Settings.ObservedTriangleColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.ObservedTriangleColor.Color);
                    break;
            }
        }

        /// <summary>
        /// Opens a ColorDialog allowing the user to choose a color to draw
        /// the elements chosen in the colorComboBox.
        /// </summary>
        /// <param name="sender">colorButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void ColorButton_Click(object sender, RoutedEventArgs e) {
            System.Windows.Forms.ColorDialog cd = new System.Windows.Forms.ColorDialog();

            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {

                ColorOGL newColor = new ColorOGL(Color.FromRgb(cd.Color.R, cd.Color.G, cd.Color.B));

                switch (colorComboBox.SelectedIndex) {
                    case 0:
                        TriMMApp.Settings.BackColor = newColor;
                        break;
                    case 1:
                        TriMMApp.Settings.TextColor = newColor;
                        break;
                    case 2:
                        TriMMApp.Settings.SolidColor = newColor;
                        break;
                    case 3:
                        TriMMApp.Settings.MeshColor = newColor;
                        break;
                    case 4:
                        TriMMApp.Settings.VertexColor = newColor;
                        break;
                    case 5:
                        TriMMApp.Settings.TriNormalColor = newColor;
                        break;
                    case 6:
                        TriMMApp.Settings.VertNormalColor = newColor;
                        break;
                    case 7:
                        TriMMApp.Settings.XAxisColor = newColor;
                        break;
                    case 8:
                        TriMMApp.Settings.YAxisColor = newColor;
                        break;
                    case 9:
                        TriMMApp.Settings.ZAxisColor = newColor;
                        break;
                    case 10:
                        TriMMApp.Settings.ObservedVertexColor = newColor;
                        break;
                    case 11:
                        TriMMApp.Settings.ObservedEdgeColor = newColor;
                        break;
                    case 12:
                        TriMMApp.Settings.ObservedTriangleColor = newColor;
                        break;
                }
            }
        }

        /// <summary>
        /// Resets the color settings to the standard values.
        /// </summary>
        /// <param name="sender">resetColorsButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void ResetColorsButton_Click(object sender, RoutedEventArgs e) { TriMMApp.Settings.SetToStandardColors(); }

        #endregion

        #region Display

        /// <summary>
        /// Changes the setting for drawing the modell using the vertex normals.
        /// </summary>
        /// <param name="sender">smoothCheckBox</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void SmoothCheckBox_Checked(object sender, RoutedEventArgs e) { TriMMApp.Settings.Smooth = smoothCheckBox.IsChecked.Value; }

        /// <summary>
        /// Changes the setting for drawing the modell as a solid object.
        /// </summary>
        /// <param name="sender">solidCheckBox</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void SolidCheckBox_Checked(object sender, RoutedEventArgs e) { TriMMApp.Settings.Solid = solidCheckBox.IsChecked.Value; }

        /// <summary>
        /// Changes the setting for drawing the modells mesh.
        /// </summary>
        /// <param name="sender">meshCheckBox</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void MeshCheckBox_Checked(object sender, RoutedEventArgs e) { TriMMApp.Settings.Mesh = meshCheckBox.IsChecked.Value; }

        /// <summary>
        /// Changes the setting for drawing the modells vertices.
        /// </summary>
        /// <param name="sender">verticesCheckBox</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void VerticesCheckBox_Checked(object sender, RoutedEventArgs e) { TriMMApp.Settings.Vertices = verticesCheckBox.IsChecked.Value; }

        /// <summary>
        /// Changes the setting for drawing the triangle normals.
        /// </summary>
        /// <param name="sender">triangleNormalsCheckBox</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void TriangleNormalsCheckBox_Checked(object sender, RoutedEventArgs e) { TriMMApp.Settings.TriangleNormalVectors = triangleNormalsCheckBox.IsChecked.Value; }

        /// <summary>
        /// Changes the setting for drawing the Vertex normals.
        /// </summary>
        /// <param name="sender">vertexNormalsCheckBox</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void VertexNormalsCheckBox_Checked(object sender, RoutedEventArgs e) { TriMMApp.Settings.VertexNormalVectors = vertexNormalsCheckBox.IsChecked.Value; }

        /// <summary>
        /// Changes the setting for drawing the coordinate x-axis.
        /// </summary>
        /// <param name="sender">xAxisCheckBox</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void XAxisCheckBox_Checked(object sender, RoutedEventArgs e) { TriMMApp.Settings.XAxis = xAxisCheckBox.IsChecked.Value; }

        /// <summary>
        /// Changes the setting for drawing the coordinate y-axis.
        /// </summary>
        /// <param name="sender">yAxisCheckBox</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void YAxisCheckBox_Checked(object sender, RoutedEventArgs e) { TriMMApp.Settings.YAxis = yAxisCheckBox.IsChecked.Value; }

        /// <summary>
        /// Changes the setting for drawing the coordinate z-axis.
        /// </summary>
        /// <param name="sender">zAxisCheckBox</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void ZAxisCheckBox_Checked(object sender, RoutedEventArgs e) { TriMMApp.Settings.ZAxis = zAxisCheckBox.IsChecked.Value; }

        /// <summary>
        /// Changes the picking mode, possible values: None, Vertex, Edge, Triangle,  and makes the necessary adjustments.
        /// </summary>
        /// <param name="sender">pickingModeComboBox</param>
        /// <param name="e">Standard SelectionChangedEventArgs</param>
        private void PickingModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) { TriMMApp.Settings.PickingMode = pickingModeComboBox.SelectedIndex; }

        /// <summary>
        /// Resets the display settings to the standard values.
        /// </summary>
        /// <param name="sender">resetDisplayButton</param>
        /// <param name="e">Standard RoutedEventArgs</param>
        private void ResetDisplayButton_Click(object sender, RoutedEventArgs e) { TriMMApp.Settings.SetToStandardDisplay(); }

        #endregion

        /// <summary>
        /// Adjusts the values in the SettingsWindow, when the Settings were changed.
        /// </summary>
        private void Settings_SettingsChanged() {
            // Vertexnormal Algorithm
            normalComboBox.SelectedIndex = TriMMApp.Settings.NormalAlgo;

            // Colors
            switch (colorComboBox.SelectedIndex) {
                case 0:
                    colorTextBox.Text = TriMMApp.Settings.BackColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.BackColor.Color);
                    break;
                case 1:
                    colorTextBox.Text = TriMMApp.Settings.TextColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.TextColor.Color);
                    break;
                case 2:
                    colorTextBox.Text = TriMMApp.Settings.SolidColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.SolidColor.Color);
                    break;
                case 3:
                    colorTextBox.Text = TriMMApp.Settings.MeshColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.MeshColor.Color);
                    break;
                case 4:
                    colorTextBox.Text = TriMMApp.Settings.VertexColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.VertexColor.Color);
                    break;
                case 5:
                    colorTextBox.Text = TriMMApp.Settings.TriNormalColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.TriNormalColor.Color);
                    break;
                case 6:
                    colorTextBox.Text = TriMMApp.Settings.VertNormalColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.VertNormalColor.Color);
                    break;
                case 7:
                    colorTextBox.Text = TriMMApp.Settings.XAxisColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.XAxisColor.Color);
                    break;
                case 8:
                    colorTextBox.Text = TriMMApp.Settings.YAxisColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.YAxisColor.Color);
                    break;
                case 9:
                    colorTextBox.Text = TriMMApp.Settings.ZAxisColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.ZAxisColor.Color);
                    break;
                case 10:
                    colorTextBox.Text = TriMMApp.Settings.ObservedVertexColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.ObservedVertexColor.Color);
                    break;
                case 11:
                    colorTextBox.Text = TriMMApp.Settings.ObservedEdgeColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.ObservedEdgeColor.Color);
                    break;
                case 12:
                    colorTextBox.Text = TriMMApp.Settings.ObservedTriangleColor.ToString();
                    colorButton.Background = new SolidColorBrush(TriMMApp.Settings.ObservedTriangleColor.Color);
                    break;
            }

            // Display
            smoothCheckBox.IsChecked = TriMMApp.Settings.Smooth;
            solidCheckBox.IsChecked = TriMMApp.Settings.Solid;
            meshCheckBox.IsChecked = TriMMApp.Settings.Mesh;
            verticesCheckBox.IsChecked = TriMMApp.Settings.Vertices;
            xAxisCheckBox.IsChecked = TriMMApp.Settings.XAxis;
            yAxisCheckBox.IsChecked = TriMMApp.Settings.YAxis;
            zAxisCheckBox.IsChecked = TriMMApp.Settings.ZAxis;
            triangleNormalsCheckBox.IsChecked = TriMMApp.Settings.TriangleNormalVectors;
            vertexNormalsCheckBox.IsChecked = TriMMApp.Settings.VertexNormalVectors;
            pickingModeComboBox.SelectedIndex = TriMMApp.Settings.PickingMode;
        }

        /// <summary>
        /// Adjusts the language ComboBox, when the language is changed.
        /// </summary>
        private void Settings_LanguageChanged() {
            languageComboBox.SelectionChanged -= LanguageComboBox_SelectionChanged;
            languageComboBox.SelectedItem = TriMMApp.Settings.Language;
            languageComboBox.SelectionChanged += LanguageComboBox_SelectionChanged;
        }

        #endregion

    }
}
