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
//
//
// This class is based on the class OpenGLControl
// from PAVEl: PAVEl (Paretoset Analysis Visualization and Evaluation) is a tool for
// interactively displaying and evaluating large sets of highdimensional data.
// Its main intended use is the analysis of result sets from multi-objective evolutionary algorithms.
//
// Copyright (C) 2007  PG500, ISF, University of Dortmund
//      PG500 are: Christoph Begau, Christoph Heuel, Raffael Joliet, Jan Kolanski,
//                 Mandy Kr�ller, Christian Moritz, Daniel Niggemann, Mathias St�ber,
//                 Timo St�nner, Jan Varwig, Dafan Zhai
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
// For more information and contact details visit http://pavel.googlecode.com

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using Tao.OpenGl;
using Tao.Platform.Windows;


namespace TriMM {

    #region EventHandler

    /// <summary>
    /// An EventHandler for the VertexPicked Event. This alerts the owner of Vertices being picked.
    /// </summary>
    /// <param name="picked">A list of indices of picked Vertices.</param>
    public delegate void VertexPickedEventHandler(List<int> picked);

    /// <summary>
    /// An EventHandler for the TrianglePicked Event. This alerts the owner of Triangles being picked.
    /// </summary>
    /// <param name="picked">A list of indices of picked Triangles.</param>
    public delegate void TrianglePickedEventHandler(List<int> picked);

    /// <summary>
    /// An EventHandler for the PickCleared Event. Alerts the owner of the clearing of the selection.
    /// </summary>
    public delegate void PickClearedEventHandler();

    #endregion

    /// <summary>
    /// The Control visualizing the data.
    /// </summary>
    public class TriMMControl : Control {

        #region Fields

        #region OpenGL Settings

        private float zoom;
        private float scale;
        private float xRot, yRot, zRot, xDiff, yDiff, zDiff;
        private float clippingPlane = 1.1f;

        private float[] origin = new float[2] { 0, 0 };
        private double[] center = new double[3] { 0, 0, 0 };

        private int vertexColorDist;
        private int triangleColorDist;

        // Font Settings
        private Font baseFont = new Font("Roman", 12);
        private int fontbase;
        private Gdi.GLYPHMETRICSFLOAT[] gmf = new Gdi.GLYPHMETRICSFLOAT[256];

        // Contexts
        private IntPtr deviceContext = IntPtr.Zero;
        private IntPtr renderContext = IntPtr.Zero;

        #endregion

        #region Display Settings

        private ColorOGL clearColor = new ColorOGL(Color.Black);
        private ColorOGL textColor = new ColorOGL();
        private ColorOGL plainColor = new ColorOGL(0.0f, 0.8f, 0.8f);
        private ColorOGL meshColor = new ColorOGL(0.5f, 0.5f, 0.5f);
        private ColorOGL vertexColor = new ColorOGL(0.8f, 0.8f, 0.0f);
        private ColorOGL normalColor = new ColorOGL(0.8f, 0.0f, 0.8f);
        private ColorOGL observedVertexColor = new ColorOGL();
        private ColorOGL observedTriangleColor = new ColorOGL(1.0f, 0.0f, 0.0f);
        private ColorOGL xAxisColor = new ColorOGL(0.8f, 0.0f, 0.0f);
        private ColorOGL yAxisColor = new ColorOGL(0.0f, 0.8f, 0.0f);
        private ColorOGL zAxisColor = new ColorOGL(0.0f, 0.0f, 0.8f);

        private Vertex observedVertex;
        private float observedRadius = 0.1f;
        private float pickingRadius = 0.1f;
        private List<string> info = new List<string>();

        private bool picking = false;
        private int pickingMode = 0;
        private bool useColorArray = false;
        private bool smooth = false;
        private bool showModell = true;
        private bool showMesh = false;
        private bool showVertices = false;
        private bool showFacetNormalVectors = false;
        private bool showVertexNormalVectors = false;
        private bool showAxes = false;

        #endregion

        #region Drawing Arrays

        private float[] vertexPickingColors;
        private float[] trianglePickingColors;
        private float[] colorArray;
        private double[] triangleArray;
        private double[] edgeArray;
        private double[] vertexArray;
        private double[] normalArray;
        private double[] smoothNormalArray;
        private double[] facetNormalVectorArray;
        private double[] vertexNormalVectorArray;

        #endregion

        #region Events

        /// <value>The event thrown when a Vertex is picked.</value>
        public event VertexPickedEventHandler VertexPicked;

        /// <value>The event thrown when a Triangle is picked.</value>
        public event TrianglePickedEventHandler TrianglePicked;

        /// <value>The event thrown when all Vertices are unpicked.</value>
        public event PickClearedEventHandler PickCleared;

        #endregion

        #endregion

        #region Properties

        #region OpenGL Settings

        /// <value>Sets the scale and adjusts the light position.</value>
        public float MyScale {
            get { return scale; }
            set {
                scale = value;
                float[] lightPosition = { 20.0f * scale, -20.0f * scale, 100.0f * scale, 1.0f };
                Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, lightPosition);
            }
        }

        /// <value>Sets the position of the z-axis clipping plane.</value>
        public float ClippingPlane { set { clippingPlane = value; } }

        /// <value>Gets the point the camera looks at or sets it.</value>
        public float[] Origin { get { return origin; } set { origin = value; } }

        /// <value>Sets the center of the object.</value>
        public double[] Center { set { center = value; } }

        /// <value>
        /// Sets vertexColorDist. vertexColorDist is used to space the picking colors with the maximum possible distance,
        /// depending on the number of Vertices in the TriangleMesh.
        /// </value>
        public int VertexColorDist { set { vertexColorDist = value; } }

        /// <value>
        /// Sets triangleColorDist. triangleColorDist is used to space the picking colors with the maximum possible distance,
        /// depending on the number of Triangles in the TriangleMesh.
        /// </value>
        public int TriangleColorDist { set { triangleColorDist = value; } }

        /// <value>Gets the baseFont or set it</value>
        public Font BaseFont {
            get { return baseFont; }
            set {
                baseFont = value;
                BuildFont();
                RenderScene();
                SwapBuffers();
            }
        }

        #endregion

        #region Display Settings

        /// <value>Gets the background color or sets it.</value>
        public ColorOGL ClearColor {
            get { return clearColor; }
            set {
                clearColor = value;
                Gl.glClearColor(clearColor.R, clearColor.G, clearColor.B, 1.0f);
            }
        }

        /// <value>Gets the color for the info text or sets it.</value>
        public ColorOGL TextColor { get { return textColor; } set { textColor = value; } }

        /// <value>Gets the plain color for displaying the modell or sets it.</value>
        public ColorOGL PlainColor { get { return plainColor; } set { plainColor = value; } }

        /// <value>Gets the color for the mesh lines or sets it.</value>
        public ColorOGL MeshColor { get { return meshColor; } set { meshColor = value; } }

        /// <value>Gets the color for displaying the Vertices or sets it.</value>
        public ColorOGL VertexColor { get { return vertexColor; } set { vertexColor = value; } }

        /// <value>Gets the color for displaying the normals or sets it.</value>
        public ColorOGL NormalColor { get { return normalColor; } set { normalColor = value; } }

        /// <value>Gets the color for the sphere around the observed Vertex or sets it.</value>
        public ColorOGL ObservedVertexColor { get { return observedVertexColor; } set { observedVertexColor = value; } }

        /// <value>Gets the color for the sphere around the observed Vertex or sets it.</value>
        public ColorOGL ObservedTriangleColor { get { return observedTriangleColor; } set { observedTriangleColor = value; } }

        /// <value>Gets the color for the x-axis or sets it.</value>
        public ColorOGL XAxisColor { get { return xAxisColor; } set { xAxisColor = value; } }

        /// <value>Gets the color for the y-axis or sets it.</value>
        public ColorOGL YAxisColor { get { return yAxisColor; } set { yAxisColor = value; } }

        /// <value>Gets the color for the z-axis or sets it.</value>
        public ColorOGL ZAxisColor { get { return zAxisColor; } set { zAxisColor = value; } }

        /// <value>Sets the currently observed Vertex.</value>
        public Vertex ObservedVertex {
            set {
                observedVertex = value;
                if ((observedVertex == null) && (PickCleared != null)) { PickCleared(); }
            }
        }

        /// <value>Gets the radius of the sphere drawn around the observed Vertex or sets it.</value>
        public float ObservedRadius { get { return observedRadius; } set { observedRadius = value; } }

        /// <value>Sets the radius of the spheres drawn drawn for picking.</value>
        public float PickingRadius { set { pickingRadius = value; } }

        /// <value>Gets the information to be displayed in the top left corner or sets it.</value>
        public List<string> Info { get { return info; } set { info = value; } }

        public int PickingMode { set { pickingMode = value; } }

        /// <value>Set to true, if the color array should used.</value>
        public bool UseColorArray { set { useColorArray = value; } }

        /// <value>If true, the modell is drawn smooth.</value>
        public bool Smooth { set { smooth = value; } }

        /// <value>If true, the modell is drawn as a solid object.</value>
        public bool ShowModell { set { showModell = value; } }

        /// <value>If true, the modell is drawn as a mesh.</value>
        public bool ShowMesh { set { showMesh = value; } }

        /// <value>If true, the Vertices of the modell are drawn.</value>
        public bool ShowVertices { set { showVertices = value; } }

        /// <value>If true, the Triangle normal vectors of the modell are drawn.</value>
        public bool ShowFacetNormalVectors { set { showFacetNormalVectors = value; } }

        /// <value>If true, the Vertex normal vectors of the modell are drawn.</value>
        public bool ShowVertexNormalVectors { set { showVertexNormalVectors = value; } }

        /// <value>If true, the coordinate-axes are drawn.</value>
        public bool ShowAxes { set { showAxes = value; } }

        #endregion

        #region Drawing Arrays

        /// <value>Sets the vertexPickingColors.</value>
        public float[] VertexPickingColors { set { vertexPickingColors = value; } }

        /// <value>Sets the trianglePickingColors.</value>
        public float[] TrianglePickingColors { set { trianglePickingColors = value; } }

        /// <value>Sets the colorArray.</value>
        public float[] ColorArray { set { colorArray = value; } }

        /// <value>Sets the array of all Vertices of all Triangles.</value>
        public double[] TriangleArray { get { return triangleArray; } set { triangleArray = value; } }

        /// <value>Sets the array of Edges.</value>
        public double[] EdgeArray { set { edgeArray = value; } }

        /// <value>Sets the array of Vertices.</value>
        public double[] VertexArray { get { return vertexArray; } set { vertexArray = value; } }

        /// <value>Sets the array of normal vectors of all triangles expanded to all corners.</value>
        public double[] NormalArray { set { normalArray = value; } }

        /// <value>Sets the array of normal vectors of all Vertices of all Triangles.</value>
        public double[] SmoothNormalArray { set { smoothNormalArray = value; } }

        /// <value>Sets the array of normal vectors of all Triangles as lines.</value>
        public double[] FacetNormalVectorArray { set { facetNormalVectorArray = value; } }

        /// <value>Sets the array of normal vectors of all Triangles as lines.</value>
        public double[] VertexNormalVectorArray { set { vertexNormalVectorArray = value; } }

        #endregion

        /// <value> Keeps the objects proportions. </value>
        public float WindowAspect { get { return (float)this.Width / this.Height; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the OGLControl and binds the needed events.
        /// </summary>
        public TriMMControl() {
            this.InitStyles();
            this.InitContexts();
            this.InitOpenGL();
            this.Dock = DockStyle.Fill;

            this.MouseWheel += this.MouseWheelEvent;
            this.MouseDown += this.MouseDownEvent;
            this.MouseMove += this.MouseMoveEvent;
        }

        #endregion

        #region Methods

        #region Basic

        /// <summary>
        /// Initialize the properties of OpenGL.
        /// </summary>
        private void InitOpenGL() {
            Gl.glClearColor(clearColor.R, clearColor.G, clearColor.B, 1.0f);
            Gl.glShadeModel(Gl.GL_SMOOTH);

            Gl.glEnable(Gl.GL_LINE_SMOOTH);
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glEnable(Gl.GL_NORMALIZE);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
            Gl.glHint(Gl.GL_LINE_SMOOTH_HINT, Gl.GL_NICEST);
            Gl.glEnableClientState(Gl.GL_VERTEX_ARRAY);
            Gl.glEnableClientState(Gl.GL_NORMAL_ARRAY);

            // Light settings
            float[] ambientLight = { 0.4f, 0.4f, 0.4f, 1.0f };
            float[] diffuseLight = { 0.8f, 0.8f, 0.8f, 1.0f };
            float[] matSpecular = { 0.1f, 0.1f, 0.1f, 1.0f };
            float[] matShininess = { 128.0f };
            float[] lmodelAmbient = { 0.2f, 0.2f, 0.2f, 1.0f };
            Gl.glEnable(Gl.GL_COLOR_MATERIAL);
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_SPECULAR, matSpecular);
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_SHININESS, matShininess);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_AMBIENT, ambientLight);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, diffuseLight);
            Gl.glLightModelfv(Gl.GL_LIGHT_MODEL_AMBIENT, lmodelAmbient);
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);
            Gl.glEnable(Gl.GL_CLIP_PLANE0);
        }

        /// <summary>
        /// Initializes the styles.
        /// </summary>
        private void InitStyles() {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, false);
            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        /// <summary>
        /// Creates and sets the pixel format and creates and connects the deviceContext and renderContext.
        /// </summary>
        private void InitContexts() {
            int selectedPixelFormat;

            // Make sure the handle for this control has been created
            if (this.Handle == IntPtr.Zero) {
                throw new Exception("InitContexts: The control's window handle has not been created!");
            }

            // Setup pixel format.
            Gdi.PIXELFORMATDESCRIPTOR pixelFormat = new Gdi.PIXELFORMATDESCRIPTOR();
            pixelFormat.nSize = (short)Marshal.SizeOf(pixelFormat);
            pixelFormat.nVersion = 1;
            pixelFormat.dwFlags = Gdi.PFD_DRAW_TO_WINDOW | Gdi.PFD_SUPPORT_OPENGL |
                Gdi.PFD_DOUBLEBUFFER;
            pixelFormat.iPixelType = (byte)Gdi.PFD_TYPE_RGBA;
            pixelFormat.cColorBits = 32;
            pixelFormat.cRedBits = 0;
            pixelFormat.cRedShift = 0;
            pixelFormat.cGreenBits = 0;
            pixelFormat.cGreenShift = 0;
            pixelFormat.cBlueBits = 0;
            pixelFormat.cBlueShift = 0;
            pixelFormat.cAlphaBits = 0;
            pixelFormat.cAlphaShift = 0;
            pixelFormat.cAccumBits = 0;
            pixelFormat.cAccumRedBits = 0;
            pixelFormat.cAccumGreenBits = 0;
            pixelFormat.cAccumBlueBits = 0;
            pixelFormat.cAccumAlphaBits = 0;
            pixelFormat.cDepthBits = 16;
            pixelFormat.cStencilBits = 0;
            pixelFormat.cAuxBuffers = 0;
            pixelFormat.iLayerType = (byte)Gdi.PFD_MAIN_PLANE;
            pixelFormat.bReserved = 0;
            pixelFormat.dwLayerMask = 0;
            pixelFormat.dwVisibleMask = 0;
            pixelFormat.dwDamageMask = 0;

            // Create device context
            this.deviceContext = User.GetDC(this.Handle);
            if (this.deviceContext == IntPtr.Zero) {
                MessageBox.Show("InitContexts: Unable to create an OpenGL device context!", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }

            // Chooses the Pixel Format that is the closest to our pixelFormat.
            selectedPixelFormat = Gdi.ChoosePixelFormat(this.deviceContext, ref pixelFormat);

            // Makes sure the requested pixel format is available.
            if (selectedPixelFormat == 0) {
                MessageBox.Show("InitContexts: Unable to find a suitable pixel format!", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }

            // Sets the selected Pixel Format.
            if (!Gdi.SetPixelFormat(this.deviceContext, selectedPixelFormat, ref pixelFormat)) {
                MessageBox.Show("InitContexts: Unable to set the requested pixel format!", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }

            // Creates rendering context.
            this.renderContext = Wgl.wglCreateContext(this.deviceContext);
            if (this.renderContext == IntPtr.Zero) {
                MessageBox.Show("InitContexts: Unable to create an OpenGL rendering context!", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }

            this.MakeCurrentContext();
            this.BuildFont();
        }

        /// <summary>
        /// Connects the deviceContext with the renderContext.
        /// </summary>
        private void MakeCurrentContext() {
            if (!Wgl.wglMakeCurrent(this.deviceContext, this.renderContext)) {
                MessageBox.Show("MakeCurrentContext: Unable to activate this control's OpenGL rendering context!", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }
        }

        /// <summary>
        /// Deletes the deviceContext and renderContext.
        /// This should always be done before the program is ended, or it will prevent the program from fully closing.
        /// Without this, the program will stay in the memory.
        /// </summary>
        public void DestroyContexts() {
            if (this.renderContext != IntPtr.Zero) {
                Wgl.wglMakeCurrent(IntPtr.Zero, IntPtr.Zero);
                Wgl.wglDeleteContext(this.renderContext);
                this.renderContext = IntPtr.Zero;
            }

            if (this.deviceContext != IntPtr.Zero) {
                if (this.Handle != IntPtr.Zero) { User.ReleaseDC(this.Handle, this.deviceContext); }
                this.deviceContext = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Swaps the back and front buffer.
        /// </summary>
        private void SwapBuffers() { Gdi.SwapBuffersFast(this.deviceContext); }

        /// <summary>
        /// Performs PushMatrix for both projection and modelview matrices.
        /// Doesn't change the current MatrixMode.
        /// </summary>
        private void PushMatrices() {
            Gl.glPushAttrib(Gl.GL_TRANSFORM_BIT);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glPushMatrix();
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glPushMatrix();

            Gl.glPopAttrib();
        }

        /// <summary>
        /// Performs PopMatrix for both projection and modelview matrices.
        /// Doesn't change the current MatrixMode.
        /// </summary>
        private void PopMatrices() {
            Gl.glPushAttrib(Gl.GL_TRANSFORM_BIT);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glPopMatrix();
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glPopMatrix();

            Gl.glPopAttrib();
        }

        /// <summary>
        /// Builds a font for OpenGL.
        /// </summary>
        private void BuildFont() {
            IntPtr font;                                                        // Windows Font ID
            fontbase = Gl.glGenLists(256);                                      // Storage For 256 Characters

            font = Gdi.CreateFont(                                              // Create The Font
                -(int)(BaseFont.Size),                                          // Height Of Font
                0,                                                              // Width Of Font
                0,                                                              // Angle Of Escapement
                0,                                                              // Orientation Angle
                300,                                                            // Font Weight
                false,                                                          // Italic
                false,                                                          // Underline
                false,                                                          // Strikeout
                Gdi.ANSI_CHARSET,                                               // Character Set Identifier
                Gdi.OUT_TT_PRECIS,                                              // Output Precision
                Gdi.CLIP_DEFAULT_PRECIS,                                        // Clipping Precision
                Gdi.CLEARTYPE_QUALITY,                                          // Output Quality
                Gdi.FF_DONTCARE | Gdi.DEFAULT_PITCH,                            // Family And Pitch
                BaseFont.FontFamily.Name);                                      // Font Name
            IntPtr hDC = User.GetDC(this.Handle);
            Gdi.SelectObject(hDC, font);                                        // Selects The Font We Created
            Wgl.wglUseFontOutlinesW(
                hDC,                                                            // Select The Current DC
                0,                                                              // Starting Character
                255,                                                            // Number Of Display Lists To Build
                fontbase,                                                       // Starting Display Lists
                0,                                                              // Deviation From The True Outlines
                1f,                                                             // Font Thickness In The Z Direction
                Wgl.WGL_FONT_POLYGONS,                                          // Use Polygons, Not Lines
                gmf);                                                           // Address Of Buffer To Receive Data
        }

        /// <summary>
        /// This method draws a string with OpenGl. It needs the coordinates to draw the String at, a 
        /// degree for rotating the String counter clockwise, a size modifier and the string to be drawn.
        /// </summary>
        /// <param name="x">x-coordinate</param>
        /// <param name="y">y-coordinate</param>
        /// <param name="degree">Degree for rotating counter clockwise</param>
        /// <param name="scaling">Size modifier for scaling</param>
        /// <param name="text">String to be drawn</param>
        private void PrintText(float x, float y, float degree, float scaling, string text) {
            if (text == null || text.Length == 0) { return; }                            // If There's No Text Do Nothing
            float length = 0;                                                   // Used To Find The Length Of The Text
            char[] chars = text.ToCharArray();                                  // Holds Our String

            for (int loop = 0; loop < text.Length; loop++) {                    // Loop To Find Text Length
                length += gmf[chars[loop]].gmfCellIncX;                         // Increase Length By Each Characters Width
            }

            Gl.glPushMatrix();
            Gl.glPushAttrib(Gl.GL_LIST_BIT);                                    // Pushes The Display List Bits
            Gl.glEnable(Gl.GL_POLYGON_SMOOTH);
            Gl.glHint(Gl.GL_POLYGON_SMOOTH_HINT, Gl.GL_NICEST);

            Gl.glListBase(fontbase);

            Gl.glTranslatef(x, y, 0);
            Gl.glRotatef(degree, 0.0F, 0.0F, 1.0F);
            Gl.glScalef(scaling, scaling, 0.0F);

            byte[] textbytes = new byte[text.Length];
            for (int i = 0; i < text.Length; i++) { textbytes[i] = (byte)text[i]; }
            Gl.glCallLists(text.Length, Gl.GL_UNSIGNED_BYTE, textbytes);        // Draws The Display List Text
            Gl.glPopMatrix();
            Gl.glPopAttrib();
            Gl.glDisable(Gl.GL_POLYGON_SMOOTH);
        }


        /// <summary>
        /// Sets up a flat projection matrix for drawing in 2D.
        /// You still have to adjust the modelview accordingly by loading the identity matrix. Does not alter the current matrix mode.
        /// </summary>
        private void SetupProjectionFlat() {
            Gl.glPushAttrib(Gl.GL_TRANSFORM_BIT);

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Gl.glOrtho(0, WindowAspect, 0, 1.0f, -100, +100);
            Gl.glPopAttrib();
            Gl.glLoadIdentity();
        }

        /// <summary>
        /// Sets view to correct size.
        /// </summary>
        private void SetView() {
            Gl.glViewport(0, 0, this.Width, this.Height);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Gl.glOrtho((-scale + zoom) * WindowAspect, (scale - zoom) * WindowAspect,
                -scale + zoom, scale - zoom, -2 * scale, 2 * scale);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glFlush();
        }

        /// <summary>
        /// Translates everything back to the original position.
        /// </summary>
        public void ResetView() {
            origin = new float[2] { 0.0f, 0.0f };
            zoom = 0;
            xRot = 0.0f;
            yRot = 0.0f;
            zRot = 0.0f;
            xDiff = 0.0f;
            yDiff = 0.0f;
            zDiff = 0.0f;
            pickingMode = 0;
            clippingPlane = 1.1f;
            observedRadius = pickingRadius;

            this.Refresh();
        }

        /// <summary>
        /// Resets the colors to the standard values.
        /// </summary>
        public void ResetColors() {
            clearColor = new ColorOGL(Color.Black);
            Gl.glClearColor(clearColor.R, clearColor.G, clearColor.B, 1.0f);
            textColor = new ColorOGL();
            plainColor = new ColorOGL(0.0f, 0.8f, 0.8f);
            meshColor = new ColorOGL(0.5f, 0.5f, 0.5f);
            vertexColor = new ColorOGL(0.8f, 0.8f, 0.0f);
            normalColor = new ColorOGL(0.8f, 0.0f, 0.8f);
            xAxisColor = new ColorOGL(0.8f, 0.0f, 0.0f);
            yAxisColor = new ColorOGL(0.0f, 0.8f, 0.0f);
            zAxisColor = new ColorOGL(0.0f, 0.0f, 0.8f);
            observedVertexColor = new ColorOGL();
            observedTriangleColor = new ColorOGL(1.0f, 0.0f, 0.0f);

            this.Refresh();
        }

        /// <summary>
        /// Make a screenshot.
        /// </summary>
        /// <returns>A bitmap-image of the displayed scene.</returns>
        public Bitmap Screenshot() {
            // Make this current first!
            this.MakeCurrentContext();

            // Get size from viewport
            int[] viewport = new int[4];
            Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);

            // Array for pixeldata
            byte[] read_data = new byte[4 * viewport[2] * viewport[3]];

            // Swap buffers to be save working on every card.
            this.SwapBuffers();
            // Read back buffer.
            Gl.glReadBuffer(Gl.GL_BACK);

            // Read the pixels in BGRA-format (for some graphic cards faster).
            Gl.glReadPixels(0, 0, viewport[2], viewport[3], Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, read_data);

            // Swap it back.
            this.SwapBuffers();

            // Create clean bitmap.
            Bitmap nbmp = new Bitmap(viewport[2], viewport[3], PixelFormat.Format24bppRgb);

            // Get the real data from bitmap, lock the original data.
            BitmapData bmpData = nbmp.LockBits(new Rectangle(0, 0, viewport[2], viewport[3]), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            // Get pointer from first data.
            IntPtr intPtr = bmpData.Scan0;

            // Calculate the offset for a row.
            int nOffset = bmpData.Stride - viewport[2] * 4;
            int loc = 0;

            // Set the pixels.
            for (int i = 0; i < viewport[3]; i++) {
                loc = bmpData.Stride * (viewport[3] - i - 1);
                for (int j = 0; j < viewport[2] * 4; j += 4) {
                    // Construct pointers for the pixels color.
                    IntPtr Alpha = new IntPtr(intPtr.ToInt32() + 3);
                    IntPtr Red = new IntPtr(intPtr.ToInt32() + 2);
                    IntPtr Green = new IntPtr(intPtr.ToInt32() + 1);
                    IntPtr Blue = intPtr;
                    // Set pixels to color from buffer.
                    Marshal.WriteByte(Alpha, read_data[loc + j + 3]);
                    Marshal.WriteByte(Red, read_data[loc + j + 2]);
                    Marshal.WriteByte(Green, read_data[loc + j + 1]);
                    Marshal.WriteByte(Blue, read_data[loc + j]);
                    intPtr = new IntPtr(intPtr.ToInt32() + 4);
                }
                intPtr = new IntPtr(intPtr.ToInt32() + nOffset);
            }
            // Unlock the data from bitmap.
            nbmp.UnlockBits(bmpData);

            return nbmp;
        }

        #endregion

        #region Rendering

        /// <summary>
        /// Makes the necessary settings and transformations and draws the scene.
        /// </summary>
        private void RenderScene() {
            this.SetView();
            if ((clippingPlane == -1.1f) || (clippingPlane == 1.1f)) {
                Gl.glClipPlane(Gl.GL_CLIP_PLANE0, new double[] { 0.0, 0.0, 1.0, 2 * clippingPlane * scale });
            } else {
                Gl.glClipPlane(Gl.GL_CLIP_PLANE0, new double[] { 0.0, 0.0, 1.0, clippingPlane * scale / 2 });
            }

            Gl.glLoadIdentity();

            Gl.glDrawBuffer(Gl.GL_BACK);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glPushMatrix();

            Gl.glTranslatef(origin[0], origin[1], 0);
            Gl.glRotatef(-this.xRot, 1.0f, 0.0f, 0.0f);
            Gl.glRotatef(this.yRot, 0.0f, 1.0f, 0.0f);
            Gl.glRotatef(this.zRot, 0.0f, 0.0f, 1.0f);
            Gl.glTranslated(-center[0], -center[1], -center[2]);

            if (!picking || (pickingMode == 0)) {
                if (showModell) { DrawModell(); }
                if (showMesh) { DrawMesh(); }
                if (showVertices) { DrawVertices(); }
                if (showFacetNormalVectors) { DrawFacetNormals(); }
                if (showVertexNormalVectors) { DrawVertexNormals(); }
                if (showAxes) { DrawAxes(); }
                if (observedVertex != null) { DrawObservedVertex(); }
                if (info.Count > 0) { DrawInfo(); }
            } else if (picking && (pickingMode == 1)) {
                DrawPickingVertices();
            } else if (picking && (pickingMode == 2)) {
                DrawPickingTriangles();
            }
            Gl.glPopMatrix();
        }

        /// <summary>
        /// Draws the colored workpiece.
        /// </summary>
        private void DrawModell() {
            Gl.glLineWidth(1.0f);
            Gl.glVertexPointer(3, Gl.GL_DOUBLE, 0, triangleArray);

            if (smooth) {
                // Vertex normals are used.
                Gl.glNormalPointer(Gl.GL_DOUBLE, 0, smoothNormalArray);
            } else {
                // The Triangle normals are expanded to the corners of the Triangles.
                Gl.glNormalPointer(Gl.GL_DOUBLE, 0, normalArray);
            }
            if (useColorArray) {
                Gl.glEnableClientState(Gl.GL_COLOR_ARRAY);

                Gl.glColorPointer(3, Gl.GL_FLOAT, 0, colorArray);
                Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, triangleArray.Length / 3);

                Gl.glDisableClientState(Gl.GL_COLOR_ARRAY);
            } else {
                Gl.glColor3fv(plainColor.RGB);
                Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, triangleArray.Length / 3);
            }
        }

        /// <summary>
        /// Draws the modell as a mesh by drawing only its edges as lines.
        /// Lighting is disabled, so the normal vectors for the edges do not need to be calculated as well.
        /// </summary>
        private void DrawMesh() {
            Gl.glDisable(Gl.GL_LIGHTING);
            Gl.glDisableClientState(Gl.GL_NORMAL_ARRAY);

            Gl.glLineWidth(1.0f);
            Gl.glColor3fv(meshColor.RGB);

            Gl.glVertexPointer(3, Gl.GL_DOUBLE, 0, edgeArray);
            Gl.glDrawArrays(Gl.GL_LINES, 0, edgeArray.Length / 3);

            Gl.glEnableClientState(Gl.GL_NORMAL_ARRAY);
            Gl.glEnable(Gl.GL_LIGHTING);
        }


        /// <summary>
        /// Draws the Vertices of the modell.
        /// Lighting is disabled, so the array of normal vectors for the vertices does not have to be calculated as well.
        /// </summary>
        private void DrawVertices() {
            Gl.glDisable(Gl.GL_LIGHTING);
            Gl.glDisableClientState(Gl.GL_NORMAL_ARRAY);

            Gl.glPointSize(3.0f);
            Gl.glColor3fv(vertexColor.RGB);

            Gl.glVertexPointer(3, Gl.GL_DOUBLE, 0, vertexArray);
            Gl.glDrawArrays(Gl.GL_POINTS, 0, vertexArray.Length / 3);

            Gl.glEnableClientState(Gl.GL_NORMAL_ARRAY);
            Gl.glEnable(Gl.GL_LIGHTING);
        }

        /// <summary>
        /// Draws the face normals as lines.
        /// Lighting is disabled.
        /// </summary>
        private void DrawFacetNormals() {
            Gl.glDisable(Gl.GL_LIGHTING);
            Gl.glDisableClientState(Gl.GL_NORMAL_ARRAY);

            Gl.glLineWidth(1.5f);
            Gl.glColor3fv(normalColor.RGB);

            Gl.glVertexPointer(3, Gl.GL_DOUBLE, 0, facetNormalVectorArray);
            Gl.glDrawArrays(Gl.GL_LINES, 0, facetNormalVectorArray.Length / 3);

            Gl.glEnableClientState(Gl.GL_NORMAL_ARRAY);
            Gl.glEnable(Gl.GL_LIGHTING);
        }


        private void DrawVertexNormals() {
            Gl.glDisable(Gl.GL_LIGHTING);
            Gl.glDisableClientState(Gl.GL_NORMAL_ARRAY);

            Gl.glLineWidth(1.5f);
            Gl.glColor3fv(normalColor.RGB);

            Gl.glVertexPointer(3, Gl.GL_DOUBLE, 0, vertexNormalVectorArray);
            Gl.glDrawArrays(Gl.GL_LINES, 0, vertexNormalVectorArray.Length / 3);

            Gl.glEnableClientState(Gl.GL_NORMAL_ARRAY);
            Gl.glEnable(Gl.GL_LIGHTING);
        }


        private void DrawAxes() {
            Gl.glDisable(Gl.GL_LIGHTING);
            Gl.glDisableClientState(Gl.GL_NORMAL_ARRAY);

            Gl.glLineWidth(1.5f);

            Gl.glBegin(Gl.GL_LINES);
            Gl.glColor3fv(xAxisColor.RGB);
            Gl.glVertex3f(0.0f, 0.0f, 0.0f);
            Gl.glVertex3f(1.05f * scale, 0.0f, 0.0f);
            Gl.glColor3fv(yAxisColor.RGB);
            Gl.glVertex3f(0.0f, 0.0f, 0.0f);
            Gl.glVertex3f(0.0f, 1.05f * scale, 0.0f);
            Gl.glColor3fv(zAxisColor.RGB);
            Gl.glVertex3f(0.0f, 0.0f, 0.0f);
            Gl.glVertex3f(0.0f, 0.0f, 1.05f * scale);
            Gl.glEnd();

            Gl.glEnableClientState(Gl.GL_NORMAL_ARRAY);
            Gl.glEnable(Gl.GL_LIGHTING);
        }

        /// <summary>
        /// Draws the observed Vertex as a white sphere.
        /// </summary>
        private void DrawObservedVertex() {
            Glu.GLUquadric quadobj = Glu.gluNewQuadric();
            Glu.gluQuadricDrawStyle(quadobj, Glu.GLU_FILL);
            Gl.glPushMatrix();

            Gl.glColor3fv(observedVertexColor.RGB);
            Gl.glTranslated(observedVertex[0], observedVertex[1], observedVertex[2]);
            Glu.gluSphere(quadobj, observedRadius, 40, 40);

            Gl.glPopMatrix();
            Glu.gluDeleteQuadric(quadobj);
        }

        /// <summary>
        /// Draws the strings contained in info to the top left corner.
        /// This can for example be used to display the curvature information for a Vertex.
        /// Lighting is disabled.
        /// </summary>
        private void DrawInfo() {
            Gl.glDisable(Gl.GL_LIGHTING);
            Gl.glDisable(Gl.GL_CLIP_PLANE0);
            this.PushMatrices();
            this.SetupProjectionFlat();
            Gl.glDisable(Gl.GL_DEPTH_TEST);

            Gl.glColor3fv(textColor.RGB);
            for (int i = 0; i < info.Count; i++) { this.PrintText(0.027f, 0.973f - i * 0.024f, 0.0f, 0.024f, info[i]); }

            Gl.glEnable(Gl.GL_DEPTH_TEST);
            this.PopMatrices();
            Gl.glEnable(Gl.GL_CLIP_PLANE0);
            Gl.glEnable(Gl.GL_LIGHTING);
        }

        #endregion

        #region Picking

        /// <summary>
        /// Draws a sphere for every Vertex.
        /// All spheres have different colors depending on the Vertex they represent.
        /// The picked Vertex can be determined by the detected color.
        /// </summary>
        private void DrawPickingVertices() {
            Glu.GLUquadric quadobj = Glu.gluNewQuadric();
            Glu.gluQuadricDrawStyle(quadobj, Glu.GLU_FILL);

            for (int i = 0; i < vertexArray.Length / 3; i++) {
                Gl.glPushMatrix();

                Gl.glColor3f(vertexPickingColors[i * 3], vertexPickingColors[i * 3 + 1], vertexPickingColors[i * 3 + 2]);
                Gl.glTranslated(vertexArray[i * 3], vertexArray[i * 3 + 1], vertexArray[i * 3 + 2]);
                Glu.gluSphere(quadobj, pickingRadius, 40, 40);

                Gl.glPopMatrix();
            }
            Glu.gluDeleteQuadric(quadobj);
        }

        /// <summary>
        /// Reads the color of the pixel at the given position and picks the corresponding Vertex.
        /// </summary>
        /// <param name="rect">Contains, in this order,
        /// the X- and Y-coordinates of the lower left corner of the picking rectangle
        /// and its width and height.</param>
        /// <param name="additive">True, if the selected Vertex is to be added to the selection</param>
        private void PickVertex(int[] rect) {
            Gl.glDisable(Gl.GL_LIGHTING);
            Gl.glDisable(Gl.GL_LIGHT0);
            Gl.glDisableClientState(Gl.GL_NORMAL_ARRAY);
            this.picking = true;

            float[] color = new float[3 * rect[2] * rect[3]];

            RenderScene();
            Gl.glReadBuffer(Gl.GL_BACK);
            Gl.glReadPixels(rect[0], rect[1], rect[2], rect[3], Gl.GL_RGB, Gl.GL_FLOAT, color);

            Gl.glEnableClientState(Gl.GL_NORMAL_ARRAY);
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);
            this.picking = false;

            // The VertexPicked event is thrown, passing the picked Vertices to the attached EventHandler.
            if (VertexPicked != null) { VertexPicked(Helpers.UniqueSelection(color, vertexColorDist, vertexArray.Length / 3)); }
        }

        /// <summary>
        /// Picks the Vertex determined by the given integer.
        /// </summary>
        /// <param name="picked">Index of the picked Vertex</param>
        public void PickVertex(int picked) {
            if ((0 <= picked) && (picked < vertexArray.Length / 3)) {
                if (VertexPicked != null) { VertexPicked(new List<int>(new int[1] { picked })); }
            }
        }

        /// <summary>
        /// Draws the modell with a different color for every Triangle.
        /// The picked Triangle can be determined by the detected color.
        /// </summary>
        private void DrawPickingTriangles() {
            Gl.glLineWidth(1.0f);
            Gl.glVertexPointer(3, Gl.GL_DOUBLE, 0, triangleArray);
            Gl.glNormalPointer(Gl.GL_DOUBLE, 0, normalArray);

            Gl.glEnableClientState(Gl.GL_COLOR_ARRAY);

            Gl.glColorPointer(3, Gl.GL_FLOAT, 0, trianglePickingColors);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, triangleArray.Length / 3);

            Gl.glDisableClientState(Gl.GL_COLOR_ARRAY);
        }

        /// <summary>
        /// Reads the color of the pixel at the given position and picks the corresponding Triangle.
        /// </summary>
        /// <param name="rect">Contains, in this order,
        /// the X- and Y-coordinates of the lower left corner of the picking rectangle
        /// and its width and height.</param>
        private void PickTriangle(int[] rect) {
            Gl.glDisable(Gl.GL_LIGHTING);
            Gl.glDisable(Gl.GL_LIGHT0);
            this.picking = true;

            float[] color = new float[3 * rect[2] * rect[3]];

            RenderScene();
            Gl.glReadBuffer(Gl.GL_BACK);
            Gl.glReadPixels(rect[0], rect[1], rect[2], rect[3], Gl.GL_RGB, Gl.GL_FLOAT, color);

            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);
            this.picking = false;

            // The TrianglePicked event is thrown, passing the picked Triangles to the attached EventHandler.
            if (TrianglePicked != null) { TrianglePicked(Helpers.UniqueSelection(color, triangleColorDist, triangleArray.Length / 9)); }
        }

        /// <summary>
        /// Picks the Triangle determined by the given integer.
        /// </summary>
        /// <param name="picked">Index of the picked Triangle</param>
        /// <param name="additive">True, if the selected Triangle is to be added to the selection.</param>
        public void PickTriangle(int picked) {
            if ((0 <= picked) && (picked < triangleArray.Length / 3)) {
                if (TrianglePicked != null) { TrianglePicked(new List<int>(new int[1] { picked })); }
            }
        }

        #endregion

        #endregion

        #region Event Handling Stuff

        /// <summary>
        /// When a mouse button is pressed, the values for the rotation are initialized.
        /// With the left mouse button a Vertex can be picked.
        /// </summary>
        /// <param name="sender"> The mouse </param>
        /// <param name="ev"> Standard MouseEventArgs </param>
        private void MouseDownEvent(object sender, MouseEventArgs ev) {
            if (ev.Button == MouseButtons.Left) {
                int[] pickingRectangle = Helpers.GetPickingRectangle(ev.X, this.Height - ev.Y, ev.X, this.Height - ev.Y);
                if (pickingMode == 1) {
                    this.PickVertex(pickingRectangle);
                } else if (pickingMode == 2) {
                    this.PickTriangle(pickingRectangle);
                }
            }
            this.zDiff = ev.Y + this.xRot;
            this.xDiff = ev.X - this.yRot;
            this.yDiff = ev.Y + this.xRot;
        }

        /// <summary>
        /// When the mouse is moved with the right mouse button pressed, a rotation will be conducted.
        /// </summary>
        /// <param name="sender"> The mouse </param>
        /// <param name="ev"> Standard MouseEventArgs </param>
        private void MouseMoveEvent(object sender, MouseEventArgs ev) {
            if (ev.Button == MouseButtons.Right) {
                if (ModifierKeys == Keys.Alt) {
                    this.zRot = (ev.X - this.zDiff) % 360.0f;
                } else {
                    this.xRot = (-ev.Y + this.yDiff) % 360.0f;
                    this.yRot = (ev.X - this.xDiff) % 360.0f;
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// When the mouse wheel is moved, the view is zoomed in or out. 
        /// </summary>
        /// <param name="sender"> The mouse wheel </param>
        /// <param name="ev"> Standard MouseEventArgs </param>
        private void MouseWheelEvent(object sender, MouseEventArgs ev) {
            if (this.zoom + scale * ev.Delta / 10000 < scale) {
                this.zoom += scale * ev.Delta / 10000;
                this.SetView();
                this.Invalidate();
            } else if (this.zoom + scale * ev.Delta / 100000 < scale) {
                this.zoom += scale * ev.Delta / 100000;
                this.SetView();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Handles the MouseEnter event to focus this Control.
        /// </summary>
        /// <param name="e">Standard EventArgs</param>
        protected override void OnMouseEnter(EventArgs e) {
            base.OnMouseEnter(e);
            this.Focus();
        }

        /// <summary>
        /// Handles the Paint event to correctly paint the scene.
        /// </summary>
        /// <param name="e">Standard PaintEventArgs</param>
        protected override void OnPaint(PaintEventArgs e) {
            if (this.deviceContext == IntPtr.Zero || this.renderContext == IntPtr.Zero) {
                MessageBox.Show("No device or rendering context available!");
                return;
            }

            base.OnPaint(e);

            //Only switch contexts if this is already not the current context
            if (this.renderContext != Wgl.wglGetCurrentContext()) {
                this.MakeCurrentContext();
            }
            this.RenderScene();
            this.SwapBuffers();
        }

        /// <summary>
        /// Destroys the contexts when the TriMMControl is disposed.
        /// </summary>
        protected override void Dispose(bool disposing) {
            DestroyContexts();
            base.Dispose(disposing);
        }

        #endregion
    }
}
