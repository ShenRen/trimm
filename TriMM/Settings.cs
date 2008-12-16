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
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;

namespace TriMM {

    #region EventHandler

    /// <summary>
    /// An EventHandler for the SettingsChanged Event.
    /// Alerts listeners of changes to the Settings.
    /// </summary>
    public delegate void SettingsChangedEventHandler();

    /// <summary>
    /// An EventHandler for the BackColorChanged Event.
    /// Alerts listeners of changes to the background color.
    /// </summary>
    public delegate void BackColorChangedEventHandler();

    #endregion

    /// <summary>
    /// A class to store the settings for the visualization.
    /// </summary>
    public class Settings {

        #region Fields

        #region Standards

        #region Colors

        private ColorOGL sBackColor;
        private ColorOGL sTextColor;
        private ColorOGL sPlainColor;
        private ColorOGL sMeshColor;
        private ColorOGL sVertexColor;
        private ColorOGL sNormalColor;
        private ColorOGL sObservedVertexColor;
        private ColorOGL sObservedTriangleColor;
        private ColorOGL sXAxisColor;
        private ColorOGL sYAxisColor;
        private ColorOGL sZAxisColor;

        #endregion

        #region Display Settings

        private int sPickingMode;
        private bool sSmooth;
        private bool sSolid;
        private bool sMesh;
        private bool sVertices;
        private bool sTriangleNormalVectors;
        private bool sVertexNormalVectors;
        private bool sAxes;

        #endregion

        #endregion

        #region Colors

        private ColorOGL backColor;
        private ColorOGL textColor;
        private ColorOGL plainColor;
        private ColorOGL meshColor;
        private ColorOGL vertexColor;
        private ColorOGL normalColor;
        private ColorOGL observedVertexColor;
        private ColorOGL observedTriangleColor;
        private ColorOGL xAxisColor;
        private ColorOGL yAxisColor;
        private ColorOGL zAxisColor;

        #endregion

        #region Display Settings

        private int pickingMode;
        private bool smooth;
        private bool solid;
        private bool mesh;
        private bool vertices;
        private bool triangleNormalVectors;
        private bool vertexNormalVectors;
        private bool axes;

        #endregion

        #region Events

        /// <value>The event thrown when the Settings are changed.</value>
        public event SettingsChangedEventHandler SettingsChanged;

        /// <value>The event thrown when the background color is changed.</value>
        public event BackColorChangedEventHandler BackColorChanged;

        #endregion

        #endregion

        #region Properties

        #region Standards

        #region Colors

        /// <value>Gets the standard background color or sets it.</value>
        public ColorOGL SBackColor { get { return sBackColor; } set { sBackColor = value; } }

        /// <value>Gets the standard color for the info text or sets it.</value>
        public ColorOGL STextColor { get { return sTextColor; } set { sTextColor = value; } }

        /// <value>Gets the standard plain color for displaying the modell or sets it.</value>
        public ColorOGL SPlainColor { get { return sPlainColor; } set { sPlainColor = value; } }

        /// <value>Gets the standard color for the mesh lines or sets it.</value>
        public ColorOGL SMeshColor { get { return sMeshColor; } set { sMeshColor = value; } }

        /// <value>Gets thestandard  color for displaying the Vertices or sets it.</value>
        public ColorOGL SVertexColor { get { return sVertexColor; } set { sVertexColor = value; } }

        /// <value>Gets the standard color for displaying the normals or sets it.</value>
        public ColorOGL SNormalColor { get { return sNormalColor; } set { sNormalColor = value; } }

        /// <value>Gets the standard color for the sphere around the observed Vertex or sets it.</value>
        public ColorOGL SObservedVertexColor { get { return sObservedVertexColor; } set { sObservedVertexColor = value; } }

        /// <value>Gets the standard color for the sphere around the observed Vertex or sets it.</value>
        public ColorOGL SObservedTriangleColor { get { return sObservedTriangleColor; } set { sObservedTriangleColor = value; } }

        /// <value>Gets the standard color for the x-axis or sets it.</value>
        public ColorOGL SXAxisColor { get { return sXAxisColor; } set { sXAxisColor = value; } }

        /// <value>Gets the standard color for the y-axis or sets it.</value>
        public ColorOGL SYAxisColor { get { return sYAxisColor; } set { sYAxisColor = value; } }

        /// <value>Gets the standard color for the z-axis or sets it.</value>
        public ColorOGL SZAxisColor { get { return sZAxisColor; } set { sZAxisColor = value; } }

        #endregion

        #region Display Settings

        /// <value>Gets the standard picking mode (0=none, 1=vertex, 2=edge, 3=triangle) or sets it.</value>
        public int SPickingMode { get { return sPickingMode; } set { sPickingMode = value; } }

        /// <value>Standard Value: If true, the modell is drawn smooth.</value>
        public bool SSmooth { get { return sSmooth; } set { sSmooth = value; } }

        /// <value>Standard Value: If true, the modell is drawn as a solid object.</value>
        public bool SSolid { get { return sSolid; } set { sSolid = value; } }

        /// <value>Standard Value: If true, the modell is drawn as a mesh.</value>
        public bool SMesh { get { return sMesh; } set { sMesh = value; } }

        /// <value>Standard Value: If true, the Vertices of the modell are drawn.</value>
        public bool SVertices { get { return sVertices; } set { sVertices = value; } }

        /// <value>Standard Value: If true, the Triangle normal vectors of the modell are drawn.</value>
        public bool STriangleNormalVectors { get { return sTriangleNormalVectors; } set { sTriangleNormalVectors = value; } }

        /// <value>Standard Value: If true, the Vertex normal vectors of the modell are drawn.</value>
        public bool SVertexNormalVectors { get { return sVertexNormalVectors; } set { sVertexNormalVectors = value; } }

        /// <value>Standard Value: If true, the coordinate-axes are drawn.</value>
        public bool SAxes { get { return sAxes; } set { sAxes = value; } }

        #endregion

        #endregion

        #region Colors

        /// <value>Gets the background color or sets it.</value>
        public ColorOGL BackColor {
            get { return backColor; }
            set {
                backColor = value;
                if (BackColorChanged != null) { BackColorChanged(); }
                if (SettingsChanged != null) { SettingsChanged(); }
            }
        }

        /// <value>Gets the color for the info text or sets it.</value>
        public ColorOGL TextColor {
            get { return textColor; }
            set {
                textColor = value;
                if (SettingsChanged != null) { SettingsChanged(); }
            }
        }

        /// <value>Gets the plain color for displaying the modell or sets it.</value>
        public ColorOGL PlainColor {
            get { return plainColor; }
            set {
                plainColor = value;
                if (SettingsChanged != null) { SettingsChanged(); }
            }
        }

        /// <value>Gets the color for the mesh lines or sets it.</value>
        public ColorOGL MeshColor {
            get { return meshColor; }
            set {
                meshColor = value;
                if (SettingsChanged != null) { SettingsChanged(); }
            }
        }

        /// <value>Gets the color for displaying the Vertices or sets it.</value>
        public ColorOGL VertexColor {
            get { return vertexColor; }
            set {
                vertexColor = value;
                if (SettingsChanged != null) { SettingsChanged(); }
            }
        }

        /// <value>Gets the color for displaying the normals or sets it.</value>
        public ColorOGL NormalColor {
            get { return normalColor; }
            set {
                normalColor = value;
                if (SettingsChanged != null) { SettingsChanged(); }
            }
        }

        /// <value>Gets the color for the sphere around the observed Vertex or sets it.</value>
        public ColorOGL ObservedVertexColor {
            get { return observedVertexColor; }
            set {
                observedVertexColor = value;
                if (SettingsChanged != null) { SettingsChanged(); }
            }
        }

        /// <value>Gets the color for the sphere around the observed Vertex or sets it.</value>
        public ColorOGL ObservedTriangleColor {
            get { return observedTriangleColor; }
            set {
                observedTriangleColor = value;
                if (SettingsChanged != null) { SettingsChanged(); }
            }
        }

        /// <value>Gets the color for the x-axis or sets it.</value>
        public ColorOGL XAxisColor {
            get { return xAxisColor; }
            set {
                xAxisColor = value;
                if (SettingsChanged != null) { SettingsChanged(); }
            }
        }

        /// <value>Gets the color for the y-axis or sets it.</value>
        public ColorOGL YAxisColor {
            get { return yAxisColor; }
            set {
                yAxisColor = value;
                if (SettingsChanged != null) { SettingsChanged(); }
            }
        }

        /// <value>Gets the color for the z-axis or sets it.</value>
        public ColorOGL ZAxisColor {
            get { return zAxisColor; }
            set {
                zAxisColor = value;
                if (SettingsChanged != null) { SettingsChanged(); }
            }
        }

        #endregion

        #region Display Settings

        /// <value>Gets the picking mode (0=none, 1=vertex, 2=edge, 3=triangle) or sets it.</value>
        public int PickingMode {
            get { return pickingMode; }
            set {
                pickingMode = value;
                if (SettingsChanged != null) { SettingsChanged(); }
            }
        }

        /// <value>If true, the modell is drawn smooth.</value>
        public bool Smooth {
            get { return smooth; }
            set {
                smooth = value;
                if (SettingsChanged != null) { SettingsChanged(); }
            }
        }

        /// <value>If true, the modell is drawn as a solid object.</value>
        public bool Solid {
            get { return solid; }
            set {
                solid = value;
                if (SettingsChanged != null) { SettingsChanged(); }
            }
        }

        /// <value>If true, the modell is drawn as a mesh.</value>
        public bool Mesh {
            get { return mesh; }
            set {
                mesh = value;
                if (SettingsChanged != null) { SettingsChanged(); }
            }
        }

        /// <value>If true, the Vertices of the modell are drawn.</value>
        public bool Vertices {
            get { return vertices; }
            set {
                vertices = value;
                if (SettingsChanged != null) { SettingsChanged(); }
            }
        }

        /// <value>If true, the Triangle normal vectors of the modell are drawn.</value>
        public bool TriangleNormalVectors {
            get { return triangleNormalVectors; }
            set {
                triangleNormalVectors = value;
                if (SettingsChanged != null) { SettingsChanged(); }
            }
        }

        /// <value>If true, the Vertex normal vectors of the modell are drawn.</value>
        public bool VertexNormalVectors {
            get { return vertexNormalVectors; }
            set {
                vertexNormalVectors = value;
                if (SettingsChanged != null) { SettingsChanged(); }
            }
        }

        /// <value>If true, the coordinate-axes are drawn.</value>
        public bool Axes {
            get { return axes; }
            set {
                axes = value;
                if (SettingsChanged != null) { SettingsChanged(); }
            }
        }

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the Settings with the default values
        /// and sets the standard values to the default values as well.
        /// </summary>
        public Settings() {
            SetToDefault();
            MakeStandard();
            WriteSET("default.set");
        }

        /// <summary>
        /// Initializes the Settings with the default values (in case something is missing in the file)
        /// and imports the real values from the given <paramref name="file"/>.
        /// </summary>
        /// <param name="file">The *.SET file to be imported.</param>
        public Settings(String file) {
            SetToDefault();
            Parse(file);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the color settings to default values.
        /// </summary>
        public void SetToDefaultColors() {
            backColor = new ColorOGL(Color.Black);
            if (BackColorChanged != null) { BackColorChanged(); }

            textColor = new ColorOGL();
            plainColor = new ColorOGL(0.0f, 0.8f, 0.8f);
            meshColor = new ColorOGL(0.5f, 0.5f, 0.5f);
            vertexColor = new ColorOGL(0.8f, 0.8f, 0.0f);
            normalColor = new ColorOGL(0.8f, 0.0f, 0.8f);
            observedVertexColor = new ColorOGL();
            observedTriangleColor = new ColorOGL(1.0f, 0.0f, 0.0f);
            xAxisColor = new ColorOGL(0.8f, 0.0f, 0.0f);
            yAxisColor = new ColorOGL(0.0f, 0.8f, 0.0f);
            zAxisColor = new ColorOGL(0.0f, 0.0f, 0.8f);

            if (SettingsChanged != null) { SettingsChanged(); }
        }

        /// <summary>
        /// Sets the display settings to default values.
        /// </summary>
        public void SetToDefaultDisplay() {
            pickingMode = 0;
            smooth = false;
            solid = true;
            mesh = true;
            vertices = false;
            triangleNormalVectors = false;
            vertexNormalVectors = false;
            axes = false;

            if (SettingsChanged != null) { SettingsChanged(); }
        }

        /// <summary>
        /// Sets all settings to default values.
        /// </summary>
        public void SetToDefault() {
            // Colors
            backColor = new ColorOGL(Color.Black);
            if (BackColorChanged != null) { BackColorChanged(); }

            textColor = new ColorOGL();
            plainColor = new ColorOGL(0.0f, 0.8f, 0.8f);
            meshColor = new ColorOGL(0.5f, 0.5f, 0.5f);
            vertexColor = new ColorOGL(0.8f, 0.8f, 0.0f);
            normalColor = new ColorOGL(0.8f, 0.0f, 0.8f);
            observedVertexColor = new ColorOGL();
            observedTriangleColor = new ColorOGL(1.0f, 0.0f, 0.0f);
            xAxisColor = new ColorOGL(0.8f, 0.0f, 0.0f);
            yAxisColor = new ColorOGL(0.0f, 0.8f, 0.0f);
            zAxisColor = new ColorOGL(0.0f, 0.0f, 0.8f);

            // Display Settings
            pickingMode = 0;
            smooth = false;
            solid = true;
            mesh = true;
            vertices = false;
            triangleNormalVectors = false;
            vertexNormalVectors = false;
            axes = false;

            if (SettingsChanged != null) { SettingsChanged(); }
        }

        /// <summary>
        /// Sets the color settings to the standard values.
        /// </summary>
        public void SetToStandardColors() {
            backColor = sBackColor;
            if (BackColorChanged != null) { BackColorChanged(); }

            textColor = sTextColor;
            plainColor = sPlainColor;
            meshColor = sMeshColor;
            vertexColor = sVertexColor;
            normalColor = sNormalColor;
            observedVertexColor = sObservedVertexColor;
            observedTriangleColor = sObservedTriangleColor;
            xAxisColor = sXAxisColor;
            yAxisColor = sYAxisColor;
            zAxisColor = sZAxisColor;

            if (SettingsChanged != null) { SettingsChanged(); }
        }

        /// <summary>
        /// Sets the display settings to the standard values.
        /// </summary>
        public void SetToStandardDisplay() {
            pickingMode = sPickingMode;
            smooth = sSmooth;
            solid = sSolid;
            mesh = sMesh;
            vertices = sVertices;
            triangleNormalVectors = sTriangleNormalVectors;
            vertexNormalVectors = sVertexNormalVectors;
            axes = sAxes;

            if (SettingsChanged != null) { SettingsChanged(); }
        }

        /// <summary>
        /// Sets all settings to the standard values.
        /// </summary>
        public void SetToStandard() {
            // Colors
            backColor = sBackColor;
            if (BackColorChanged != null) { BackColorChanged(); }

            textColor = sTextColor;
            plainColor = sPlainColor;
            meshColor = sMeshColor;
            vertexColor = sVertexColor;
            normalColor = sNormalColor;
            observedVertexColor = sObservedVertexColor;
            observedTriangleColor = sObservedTriangleColor;
            xAxisColor = sXAxisColor;
            yAxisColor = sYAxisColor;
            zAxisColor = sZAxisColor;

            // Display Settings
            pickingMode = sPickingMode;
            smooth = sSmooth;
            solid = sSolid;
            mesh = sMesh;
            vertices = sVertices;
            triangleNormalVectors = sTriangleNormalVectors;
            vertexNormalVectors = sVertexNormalVectors;
            axes = sAxes;

            if (SettingsChanged != null) { SettingsChanged(); }
        }

        /// <summary>
        /// Sets the standard color settings to the current values.
        /// </summary>
        public void MakeStandardColors() {
            sBackColor = backColor;
            sTextColor = textColor;
            sPlainColor = plainColor;
            sMeshColor = meshColor;
            sVertexColor = vertexColor;
            sNormalColor = normalColor;
            sObservedVertexColor = observedVertexColor;
            sObservedTriangleColor = observedTriangleColor;
            sXAxisColor = xAxisColor;
            sYAxisColor = yAxisColor;
            sZAxisColor = zAxisColor;

            if (SettingsChanged != null) { SettingsChanged(); }
        }

        /// <summary>
        /// Sets the standard display settings to the current values.
        /// </summary>
        public void MakeStandardDisplay() {
            sPickingMode = pickingMode;
            sSmooth = smooth;
            sSolid = solid;
            sMesh = mesh;
            sVertices = vertices;
            sTriangleNormalVectors = triangleNormalVectors;
            sVertexNormalVectors = vertexNormalVectors;
            sAxes = axes;

            if (SettingsChanged != null) { SettingsChanged(); }
        }

        /// <summary>
        /// Sets all standard settings to the current values.
        /// </summary>
        public void MakeStandard() {
            // Colors
            sBackColor = backColor;
            sTextColor = textColor;
            sPlainColor = plainColor;
            sMeshColor = meshColor;
            sVertexColor = vertexColor;
            sNormalColor = normalColor;
            sObservedVertexColor = observedVertexColor;
            sObservedTriangleColor = observedTriangleColor;
            sXAxisColor = xAxisColor;
            sYAxisColor = yAxisColor;
            sZAxisColor = zAxisColor;

            // Display Settings
            sPickingMode = pickingMode;
            sSmooth = smooth;
            sSolid = solid;
            sMesh = mesh;
            sVertices = vertices;
            sTriangleNormalVectors = triangleNormalVectors;
            sVertexNormalVectors = vertexNormalVectors;
            sAxes = axes;

            if (SettingsChanged != null) { SettingsChanged(); }
        }

        /// <summary>
        /// The given StreamReader <paramref name="file"/> is parsed and the values set.
        /// </summary>
        /// <param name="file">The path to the *.SET file to be parsed.</param>
        /// <param name="normalAlgo">The algorithm to calculate the Vertex normals with.</param>
        public void Parse(String fileName) {
            StreamReader file = new StreamReader(fileName);

            // The numbers in the file must have the decimal separator ".".
            NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
            numberFormatInfo.NumberDecimalSeparator = ".";

#if !DEBUG
            try {
#endif
                String input = null;
                while ((input = file.ReadLine()) != null) {
                    input = input.Trim();
                    if (!input.StartsWith("#")) {
                        // RemoveEmptyEntities removes empty entities, resulting from more than one whitespace
                        String[] line = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (line.Length == 4) {
                            ColorOGL color = new ColorOGL(float.Parse(line[1], NumberStyles.Float, numberFormatInfo), float.Parse(line[2], NumberStyles.Float, numberFormatInfo), float.Parse(line[3], NumberStyles.Float, numberFormatInfo));
                            if (line[0] == "BACKC") { backColor = color; }
                            else if (line[0] == "TEXTC") { textColor = color; }
                            else if (line[0] == "PLAINC") { plainColor = color; }
                            else if (line[0] == "MESHC") { meshColor = color; }
                            else if (line[0] == "VERTEXC") { vertexColor = color; }
                            else if (line[0] == "NORMALC") { normalColor = color; }
                            else if (line[0] == "OVERTC") { observedVertexColor = color; }
                            else if (line[0] == "OTRIC") { observedTriangleColor = color; }
                            else if (line[0] == "XAXISC") { xAxisColor = color; }
                            else if (line[0] == "YAXISC") { yAxisColor = color; }
                            else if (line[0] == "ZAXISC") { zAxisColor = color; }
                        } else if (line.Length == 2) {
                            int mode = int.Parse(line[1]);
                            if (line[0] == "PICK") {
                                if ((mode < 0) || (mode > 3)) { throw new ArgumentException("The picking mode (PICK) must be either 0, 1, 2 or 3!"); }
                                pickingMode = mode;
                            } else if (line[0] == "SMOOTH") {
                                if ((mode < 0) || (mode > 1)) { throw new ArgumentException("Only the values 0 and 1 are allowed for the SMOOTH setting!"); }
                                if (mode == 0) { smooth = false; } else { smooth = true; }
                            } else if (line[0] == "SOLID") {
                                if ((mode < 0) || (mode > 1)) { throw new ArgumentException("Only the values 0 and 1 are allowed for the SOLID setting!"); }
                                if (mode == 0) { solid = false; } else { solid = true; }
                            } else if (line[0] == "MESH") {
                                if ((mode < 0) || (mode > 1)) { throw new ArgumentException("Only the values 0 and 1 are allowed for the MESH setting!"); }
                                if (mode == 0) { mesh = false; } else { mesh = true; }
                            } else if (line[0] == "VERT") {
                                if ((mode < 0) || (mode > 1)) { throw new ArgumentException("Only the values 0 and 1 are allowed for the VERT setting!"); }
                                if (mode == 0) { vertices = false; } else { vertices = true; }
                            } else if (line[0] == "TRINORM") {
                                if ((mode < 0) || (mode > 1)) { throw new ArgumentException("Only the values 0 and 1 are allowed for the TRINORM setting!"); }
                                if (mode == 0) { triangleNormalVectors = false; } else { triangleNormalVectors = true; }
                            } else if (line[0] == "VERTNORM") {
                                if ((mode < 0) || (mode > 1)) { throw new ArgumentException("Only the values 0 and 1 are allowed for the VERTNORM setting!"); }
                                if (mode == 0) { vertexNormalVectors = false; } else { vertexNormalVectors = true; }
                            } else if (line[0] == "AXES") {
                                if ((mode < 0) || (mode > 1)) { throw new ArgumentException("Only the values 0 and 1 are allowed for the AXES setting!"); }
                                if (mode == 0) { axes = false; } else { axes = true; }
                            }
                        }
                    }
                }
#if !DEBUG
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } catch {
                MessageBox.Show("This Settings file is defect or not a TriMM-Settings file at all!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
#endif
                file.Close();
                MakeStandard();
#if !DEBUG
            }
#endif
        }

        /// <summary>
        /// Exports the Settings to a .set file.
        /// </summary>
        /// <param name="filename">Path to the file to be written.</param>
        public void WriteSET(string filename) {
            StreamWriter sw = new StreamWriter(filename);
#if !DEBUG
            try {
#endif
                sw.WriteLine("# Settings for TriMM, the TriangleMesh Manipulator");
                // Colors
                sw.WriteLine("BACKC " + backColor.R + " " + backColor.G + " " + backColor.B);
                sw.WriteLine("TEXTC " + textColor.R + " " + textColor.G + " " + textColor.B);
                sw.WriteLine("PLAINC " + plainColor.R + " " + plainColor.G + " " + plainColor.B);
                sw.WriteLine("MESHC " + meshColor.R + " " + meshColor.G + " " + meshColor.B);
                sw.WriteLine("VERTEXC " + vertexColor.R + " " + vertexColor.G + " " + vertexColor.B);
                sw.WriteLine("NORMALC " + normalColor.R + " " + normalColor.G + " " + normalColor.B);
                sw.WriteLine("OVERTC " + observedVertexColor.R + " " + observedVertexColor.G + " " + observedVertexColor.B);
                sw.WriteLine("OTRIC " + observedTriangleColor.R + " " + observedTriangleColor.G + " " + observedTriangleColor.B);
                sw.WriteLine("XAXISC " + xAxisColor.R + " " + xAxisColor.G + " " + xAxisColor.B);
                sw.WriteLine("YAXISC " + yAxisColor.R + " " + yAxisColor.G + " " + yAxisColor.B);
                sw.WriteLine("ZAXISC " + zAxisColor.R + " " + zAxisColor.G + " " + zAxisColor.B);

                // Display
                sw.WriteLine("PICK " + pickingMode);
                if (smooth) { sw.WriteLine("SMOOTH 1"); } else { sw.WriteLine("SMOOTH 0"); }
                if (solid) { sw.WriteLine("SOLID 1"); } else { sw.WriteLine("SOLID 0"); }
                if (mesh) { sw.WriteLine("MESH 1"); } else { sw.WriteLine("MESH 0"); }
                if (vertices) { sw.WriteLine("VERT 1"); } else { sw.WriteLine("VERT 0"); }
                if (triangleNormalVectors) { sw.WriteLine("TRINORM 1"); } else { sw.WriteLine("TRINORM 0"); }
                if (vertexNormalVectors) { sw.WriteLine("VERTNORM 1"); } else { sw.WriteLine("VERTNORM 0"); }
                if (axes) { sw.WriteLine("AXES 1"); } else { sw.WriteLine("AXES 0"); }
#if !DEBUG
            } catch (Exception exception) {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
#endif
                sw.Close();
#if !DEBUG
            }
#endif
        }

        #endregion
    }
}
