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
//
//
// This class is based on the class OpenGLControl
// from PAVEl: PAVEl (Paretoset Analysis Visualization and Evaluation) is a tool for
// interactively displaying and evaluating large sets of highdimensional data.
// Its main intended use is the analysis of result sets from multi-objective evolutionary algorithms.
//
// Copyright (C) 2007  PG500, ISF, University of Dortmund
//      PG500 are: Christoph Begau, Christoph Heuel, Raffael Joliet, Jan Kolanski,
//                 Mandy Kröller, Christian Moritz, Daniel Niggemann, Mathias Stöber,
//                 Timo Stönner, Jan Varwig, Dafan Zhai
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
    /// An EventHandler for the EdgePicked Event. This alerts the owner of Edges being picked.
    /// </summary>
    /// <param name="picked">A list of indices of picked Vertices.</param>
    public delegate void EdgePickedEventHandler(List<int> picked);

    /// <summary>
    /// An EventHandler for the TrianglePicked Event. This alerts the owner of Triangles being picked.
    /// </summary>
    /// <param name="picked">A list of indices of picked Triangles.</param>
    public delegate void TrianglePickedEventHandler(List<int> picked);

    #endregion

    /// <summary>
    /// The Control visualizing the data.
    /// </summary>
    public class TriMMControl : Control {

        #region Fields

        #region OpenGL Settings

        private float zoom;
        private float xRot, yRot, zRot, xDiff, yDiff, zDiff;
        private float clippingPlane = 1.1f;
        private float[] origin = new float[2] { 0, 0 };

        // Font Settings
        private Font baseFont = new Font("Roman", 12);
        private int fontbase;
        private Gdi.GLYPHMETRICSFLOAT[] gmf = new Gdi.GLYPHMETRICSFLOAT[256];

        // Contexts
        private IntPtr deviceContext = IntPtr.Zero;
        private IntPtr renderContext = IntPtr.Zero;

        #endregion

        #region Display Settings

        private double observedRadius = 0.1f;
        private List<string> info = new List<string>();

        private bool picking = false;
        private bool useColorArray = false;

        #endregion

        #region Events

        /// <value>The event thrown when a Vertex is picked.</value>
        public event VertexPickedEventHandler VertexPicked;

        /// <value>The event thrown when an Edge is picked.</value>
        public event EdgePickedEventHandler EdgePicked;

        /// <value>The event thrown when a Triangle is picked.</value>
        public event TrianglePickedEventHandler TrianglePicked;

        #endregion

        #endregion

        #region Properties

        #region OpenGL Settings

        /// <value>Sets the zoom value.</value>
        public float Zoom { set { zoom = value; } }

        /// <value>Sets the position of the z-axis clipping plane.</value>
        public float ClippingPlane { set { clippingPlane = value; } }

        /// <value>Gets the point the camera looks at or sets it.</value>
        public float[] Origin { get { return origin; } set { origin = value; } }

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

        /// <value>Gets the radius of the sphere drawn around the observed Vertex or sets it.</value>
        public double ObservedRadius { get { return observedRadius; } set { observedRadius = value; } }

        /// <value>Gets the information to be displayed in the top left corner or sets it.</value>
        public List<string> Info { get { return info; } set { info = value; } }

        /// <value>Set to true, if the color array should used.</value>
        public bool UseColorArray { set { useColorArray = value; } }

        #endregion

        /// <value> Keeps the objects proportions. </value>
        public float WindowAspect { get { return (float)this.Width / this.Height; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the TriMMControl and binds the needed events.
        /// </summary>
        public TriMMControl() {
            this.InitStyles();
            this.InitContexts();
            this.InitOpenGL();
            this.Dock = DockStyle.Fill;

            this.MouseWheel += this.MouseWheelEvent;
            this.MouseDown += this.MouseDownEvent;
            this.MouseMove += this.MouseMoveEvent;
            TriMM.Mesh.ScaleChanged += new ScaleChangedEventHandler(Mesh_ScaleChanged);
            TriMM.Settings.BackColorChanged += new BackColorChangedEventHandler(Settings_BackColorChanged);
        }

        #endregion

        #region Methods

        #region Basic

        /// <summary>
        /// Initialize the properties of OpenGL.
        /// </summary>
        private void InitOpenGL() {
            Gl.glClearColor(TriMM.Settings.BackColor.R, TriMM.Settings.BackColor.G, TriMM.Settings.BackColor.B, 1.0f);
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
            Gl.glOrtho((-TriMM.Mesh.Scale + zoom) * WindowAspect, (TriMM.Mesh.Scale - zoom) * WindowAspect,
                -TriMM.Mesh.Scale + zoom, TriMM.Mesh.Scale - zoom, -2 * TriMM.Mesh.Scale, 2 * TriMM.Mesh.Scale);
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
            clippingPlane = 1.1f;
            observedRadius = TriMM.Mesh.MinEdgeLength / 2;

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
                Gl.glClipPlane(Gl.GL_CLIP_PLANE0, new double[] { 0.0, 0.0, 1.0, 2 * clippingPlane * TriMM.Mesh.Scale });
            } else {
                Gl.glClipPlane(Gl.GL_CLIP_PLANE0, new double[] { 0.0, 0.0, 1.0, clippingPlane * TriMM.Mesh.Scale / 2 });
            }

            Gl.glLoadIdentity();

            Gl.glDrawBuffer(Gl.GL_BACK);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glPushMatrix();

            Gl.glTranslatef(origin[0], origin[1], 0);
            Gl.glRotatef(-this.xRot, 1.0f, 0.0f, 0.0f);
            Gl.glRotatef(this.yRot, 0.0f, 1.0f, 0.0f);
            Gl.glRotatef(this.zRot, 0.0f, 0.0f, 1.0f);
            Gl.glTranslated(-TriMM.Mesh.Center[0], -TriMM.Mesh.Center[1], -TriMM.Mesh.Center[2]);

            if (!picking || (TriMM.Settings.PickingMode == 0)) {
                if (TriMM.Settings.Solid) { DrawModell(); }
                if (TriMM.Settings.Mesh) { DrawMesh(); }
                if (TriMM.Settings.Vertices) { DrawVertices(); }
                if (TriMM.Settings.TriangleNormalVectors) { DrawFacetNormals(); }
                if (TriMM.Settings.VertexNormalVectors) { DrawVertexNormals(); }
                if (TriMM.Settings.Axes) { DrawAxes(); }
                if (TriMM.Mesh.ObservedVertex != -1) { DrawObservedVertex(); }
                if (TriMM.Mesh.ObservedEdge != -1) { DrawObservedEdge(); }
                if (info.Count > 0) { DrawInfo(); }
            } else if (picking && (TriMM.Settings.PickingMode == 1)) {
                DrawPickingVertices();
            } else if (picking && (TriMM.Settings.PickingMode == 2)) {
                DrawPickingEdges();
            } else if (picking && (TriMM.Settings.PickingMode == 3)) {
                DrawPickingTriangles();
            }
            Gl.glPopMatrix();
        }

        /// <summary>
        /// Draws the colored workpiece.
        /// </summary>
        private void DrawModell() {
            Gl.glLineWidth(1.0f);
            Gl.glVertexPointer(3, Gl.GL_DOUBLE, 0, TriMM.Mesh.TriangleArray);

            if (TriMM.Settings.Smooth) {
                // Vertex normals are used.
                Gl.glNormalPointer(Gl.GL_DOUBLE, 0, TriMM.Mesh.SmoothNormalArray);
            } else {
                // The Triangle normals are expanded to the corners of the Triangles.
                Gl.glNormalPointer(Gl.GL_DOUBLE, 0, TriMM.Mesh.NormalArray);
            }
            if (useColorArray) {
                Gl.glEnableClientState(Gl.GL_COLOR_ARRAY);

                Gl.glColorPointer(3, Gl.GL_FLOAT, 0, TriMM.Mesh.ColorArray);
                Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 3 * TriMM.Mesh.Count);

                Gl.glDisableClientState(Gl.GL_COLOR_ARRAY);
            } else {
                Gl.glColor3fv(TriMM.Settings.PlainColor.RGB);
                Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 3 * TriMM.Mesh.Count);
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
            Gl.glColor3fv(TriMM.Settings.MeshColor.RGB);

            Gl.glVertexPointer(3, Gl.GL_DOUBLE, 0, TriMM.Mesh.EdgeArray);
            Gl.glDrawArrays(Gl.GL_LINES, 0, 2 * TriMM.Mesh.Edges.Count);

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
            Gl.glColor3fv(TriMM.Settings.VertexColor.RGB);

            Gl.glVertexPointer(3, Gl.GL_DOUBLE, 0, TriMM.Mesh.VertexArray);
            Gl.glDrawArrays(Gl.GL_POINTS, 0, TriMM.Mesh.Vertices.Count);

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
            Gl.glColor3fv(TriMM.Settings.NormalColor.RGB);

            Gl.glVertexPointer(3, Gl.GL_DOUBLE, 0, TriMM.Mesh.FacetNormalVectorArray);
            Gl.glDrawArrays(Gl.GL_LINES, 0, 2 * TriMM.Mesh.Count);

            Gl.glEnableClientState(Gl.GL_NORMAL_ARRAY);
            Gl.glEnable(Gl.GL_LIGHTING);
        }

        /// <summary>
        /// Draw the Vertex normals als lines.
        /// Lighting is disabled.
        /// </summary>
        private void DrawVertexNormals() {
            Gl.glDisable(Gl.GL_LIGHTING);
            Gl.glDisableClientState(Gl.GL_NORMAL_ARRAY);

            Gl.glLineWidth(1.5f);
            Gl.glColor3fv(TriMM.Settings.NormalColor.RGB);

            Gl.glVertexPointer(3, Gl.GL_DOUBLE, 0, TriMM.Mesh.VertexNormalVectorArray);
            Gl.glDrawArrays(Gl.GL_LINES, 0, 2 * TriMM.Mesh.Vertices.Count);

            Gl.glEnableClientState(Gl.GL_NORMAL_ARRAY);
            Gl.glEnable(Gl.GL_LIGHTING);
        }

        /// <summary>
        /// Draws the coordinate axes as lines.
        /// Lighting is disabled.
        /// </summary>
        private void DrawAxes() {
            Gl.glDisable(Gl.GL_LIGHTING);
            Gl.glDisableClientState(Gl.GL_NORMAL_ARRAY);

            Gl.glLineWidth(1.5f);

            Gl.glBegin(Gl.GL_LINES);
            Gl.glColor3fv(TriMM.Settings.XAxisColor.RGB);
            Gl.glVertex3f(0.0f, 0.0f, 0.0f);
            Gl.glVertex3f(1.05f * TriMM.Mesh.Scale, 0.0f, 0.0f);
            Gl.glColor3fv(TriMM.Settings.YAxisColor.RGB);
            Gl.glVertex3f(0.0f, 0.0f, 0.0f);
            Gl.glVertex3f(0.0f, 1.05f * TriMM.Mesh.Scale, 0.0f);
            Gl.glColor3fv(TriMM.Settings.ZAxisColor.RGB);
            Gl.glVertex3f(0.0f, 0.0f, 0.0f);
            Gl.glVertex3f(0.0f, 0.0f, 1.05f * TriMM.Mesh.Scale);
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

            Gl.glColor3fv(TriMM.Settings.ObservedVertexColor.RGB);
            Gl.glTranslated(TriMM.Mesh.Vertices[TriMM.Mesh.ObservedVertex][0], TriMM.Mesh.Vertices[TriMM.Mesh.ObservedVertex][1], TriMM.Mesh.Vertices[TriMM.Mesh.ObservedVertex][2]);
            Glu.gluSphere(quadobj, observedRadius, 40, 40);

            Gl.glPopMatrix();
            Glu.gluDeleteQuadric(quadobj);
        }

        /// <summary>
        /// Draws the observed Vertex as a white sphere.
        /// </summary>
        private void DrawObservedEdge() {
            Glu.GLUquadric quadobj = Glu.gluNewQuadric();
            Glu.gluQuadricDrawStyle(quadobj, Glu.GLU_FILL);
            Gl.glPushMatrix();

            Gl.glColor3fv(TriMM.Settings.ObservedVertexColor.RGB);
            Gl.glTranslated(TriMM.Mesh.EdgeArray[6 * TriMM.Mesh.ObservedEdge], TriMM.Mesh.EdgeArray[6 * TriMM.Mesh.ObservedEdge + 1], TriMM.Mesh.EdgeArray[6 * TriMM.Mesh.ObservedEdge + 2]);    // Translates to the first Vertex
            if (Math.Abs(TriMM.Mesh.EdgeArray[6 * TriMM.Mesh.ObservedEdge + 5] - TriMM.Mesh.EdgeArray[6 * TriMM.Mesh.ObservedEdge + 2]) < 0.000000001) {
                Gl.glRotated(90.0, 0, 1, 0.0);			                    // Rotates & aligns with x-axis
                Gl.glRotated(TriMM.Mesh.EdgePickingArray[4 * TriMM.Mesh.ObservedEdge], -1.0, 0.0, 0.0);		// Rotates to second Vertex in x-y plane
            } else {
                Gl.glRotated(TriMM.Mesh.EdgePickingArray[4 * TriMM.Mesh.ObservedEdge], TriMM.Mesh.EdgePickingArray[4 * TriMM.Mesh.ObservedEdge + 1], TriMM.Mesh.EdgePickingArray[4 * TriMM.Mesh.ObservedEdge + 2], 0.0); // Rotates about rotation vector
            }
            Glu.gluCylinder(quadobj, 0.1 * observedRadius, 0.1 * observedRadius, TriMM.Mesh.EdgePickingArray[4 * TriMM.Mesh.ObservedEdge + 3], 10, 10);      // Draws cylinder

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

            Gl.glColor3fv(TriMM.Settings.TextColor.RGB);
            for (int i = 0; i < info.Count; i++) { this.PrintText(0.027f, 0.973f - i * 0.024f, 0.0f, 0.024f, info[i]); }

            Gl.glEnable(Gl.GL_DEPTH_TEST);
            this.PopMatrices();
            Gl.glEnable(Gl.GL_CLIP_PLANE0);
            Gl.glEnable(Gl.GL_LIGHTING);
        }

        #endregion

        #region Picking

        #region Vertices

        /// <summary>
        /// Draws a sphere for every Vertex.
        /// All spheres have different colors depending on the Vertex they represent.
        /// The picked Vertex can be determined by the detected color.
        /// </summary>
        private void DrawPickingVertices() {
            Glu.GLUquadric quadobj = Glu.gluNewQuadric();
            Glu.gluQuadricDrawStyle(quadobj, Glu.GLU_FILL);

            for (int i = 0; i < TriMM.Mesh.Vertices.Count; i++) {
                Gl.glPushMatrix();

                Gl.glColor3f(TriMM.Mesh.VertexPickingColors[i * 3], TriMM.Mesh.VertexPickingColors[i * 3 + 1], TriMM.Mesh.VertexPickingColors[i * 3 + 2]);
                Gl.glTranslated(TriMM.Mesh.VertexArray[i * 3], TriMM.Mesh.VertexArray[i * 3 + 1], TriMM.Mesh.VertexArray[i * 3 + 2]);
                Glu.gluSphere(quadobj, TriMM.Mesh.MinEdgeLength / 2, 40, 40);

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
            if (VertexPicked != null) { VertexPicked(ColorOGL.UniqueSelection(color, TriMM.Mesh.VertexColorDist, TriMM.Mesh.Vertices.Count)); }
        }

        /// <summary>
        /// Picks the Vertex determined by the given integer.
        /// </summary>
        /// <param name="picked">Index of the picked Vertex</param>
        public void PickVertex(int picked) {
            if ((0 <= picked) && (picked < TriMM.Mesh.Vertices.Count)) {
                if (VertexPicked != null) { VertexPicked(new List<int>(new int[1] { picked })); }
            }
        }

        #endregion

        #region Edges

        /// <summary>
        /// Draws a cylinder for every Edge.
        /// All cylinders have different colors depending on the Edge they represent.
        /// The picked Edge can be determined by the detected color.
        /// </summary>
        private void DrawPickingEdges() {
            Glu.GLUquadric quadobj = Glu.gluNewQuadric();
            Glu.gluQuadricDrawStyle(quadobj, Glu.GLU_FILL);

            for (int i = 0; i < TriMM.Mesh.Edges.Count; i++) {
                PushMatrices();

                Gl.glColor3f(TriMM.Mesh.EdgePickingColors[i * 3], TriMM.Mesh.EdgePickingColors[i * 3 + 1], TriMM.Mesh.EdgePickingColors[i * 3 + 2]);
                Gl.glTranslated(TriMM.Mesh.EdgeArray[6 * i], TriMM.Mesh.EdgeArray[6 * i + 1], TriMM.Mesh.EdgeArray[6 * i + 2]);    // Translates to the first Vertex

                if (Math.Abs(TriMM.Mesh.EdgeArray[6 * i + 5] - TriMM.Mesh.EdgeArray[6 * i + 2]) < 0.000000001) {
                    Gl.glRotated(90.0, 0, 1, 0.0);			                    // Rotates & aligns with x-axis
                    Gl.glRotated(TriMM.Mesh.EdgePickingArray[4 * i], -1.0, 0.0, 0.0);		// Rotates to second Vertex in x-y plane
                } else {
                    Gl.glRotated(TriMM.Mesh.EdgePickingArray[4 * i], TriMM.Mesh.EdgePickingArray[4 * i + 1], TriMM.Mesh.EdgePickingArray[4 * i + 2], 0.0);  // Rotates about rotation vector
                }

                Glu.gluCylinder(quadobj, 0.05 * TriMM.Mesh.MinEdgeLength, 0.05 * TriMM.Mesh.MinEdgeLength, TriMM.Mesh.EdgePickingArray[4 * i + 3], 10, 10);      // Draws cylinder v = Länge des Zylinders
                PopMatrices();
            }
            Glu.gluDeleteQuadric(quadobj);
        }

        /// <summary>
        /// Reads the color of the pixel at the given position and picks the corresponding Edge.
        /// </summary>
        /// <param name="rect">Contains, in this order,
        /// the X- and Y-coordinates of the lower left corner of the picking rectangle
        /// and its width and height.</param>
        /// <param name="additive">True, if the selected Edge is to be added to the selection</param>
        private void PickEdge(int[] rect) {
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

            // The EdgePicked event is thrown, passing the picked Edges to the attached EventHandler.
            if (EdgePicked != null) { EdgePicked(ColorOGL.UniqueSelection(color, TriMM.Mesh.EdgeColorDist, TriMM.Mesh.Edges.Count)); }
        }

        /// <summary>
        /// Picks the Edge determined by the given integer.
        /// </summary>
        /// <param name="picked">Index of the picked Edge</param>
        public void PickEdge(int picked) {
            if ((0 <= picked) && (picked < TriMM.Mesh.Edges.Count)) {
                if (EdgePicked != null) { EdgePicked(new List<int>(new int[1] { picked })); }
            }
        }

        #endregion

        #region Triangles

        /// <summary>
        /// Draws the modell with a different color for every Triangle.
        /// The picked Triangle can be determined by the detected color.
        /// </summary>
        private void DrawPickingTriangles() {
            Gl.glLineWidth(1.0f);
            Gl.glVertexPointer(3, Gl.GL_DOUBLE, 0, TriMM.Mesh.TriangleArray);
            Gl.glNormalPointer(Gl.GL_DOUBLE, 0, TriMM.Mesh.NormalArray);

            Gl.glEnableClientState(Gl.GL_COLOR_ARRAY);

            Gl.glColorPointer(3, Gl.GL_FLOAT, 0, TriMM.Mesh.TrianglePickingColors);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 3 * TriMM.Mesh.Count);

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
            if (TrianglePicked != null) { TrianglePicked(ColorOGL.UniqueSelection(color, TriMM.Mesh.TriangleColorDist, 3 * TriMM.Mesh.Count)); }
        }

        /// <summary>
        /// Picks the Triangle determined by the given integer.
        /// </summary>
        /// <param name="picked">Index of the picked Triangle</param>
        /// <param name="additive">True, if the selected Triangle is to be added to the selection.</param>
        public void PickTriangle(int picked) {
            if ((0 <= picked) && (picked < TriMM.Mesh.Count)) {
                if (TrianglePicked != null) { TrianglePicked(new List<int>(new int[1] { picked })); }
            }
        }

        #endregion

        /// <summary>
        /// Calculates the bottom left corner and the width and height of a rectangle
        /// given by two opposing corners (<paramref name="x1"/>, <paramref name="y1"/>)
        /// and (<paramref name="x2"/>, <paramref name="y2"/>).
        /// </summary>
        /// <param name="x1">X-coordinate of the first corner</param>
        /// <param name="y1">Y-coordinate of the first corner</param>
        /// <param name="x2">X-coordinate of the second corner</param>
        /// <param name="y2">Y-coordinate of the second corner</param>
        /// <returns>[0]: X-coordinate of bottom left corner
        /// [1]: Y-coordinate of bottom left corner
        /// [2]: Width of picking rectangle
        /// [3]: Height of picking rectangle</returns>
        private int[] GetPickingRectangle(int x1, int y1, int x2, int y2) {
            int[] rect = new int[4];

            if (x1 <= x2) { rect[0] = x1; } else { rect[0] = x2; }
            if (y1 <= y2) { rect[1] = y1; } else { rect[1] = y2; }
            rect[2] = Math.Abs(x1 - x2) + 1; //width
            rect[3] = Math.Abs(y1 - y2) + 1; //height

            return rect;
        }

        #endregion

        /// <summary>
        /// Destroys the contexts when the TriMMControl is disposed.
        /// </summary>
        protected override void Dispose(bool disposing) {
            DestroyContexts();
            base.Dispose(disposing);
        }

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
                int[] pickingRectangle = GetPickingRectangle(ev.X, this.Height - ev.Y, ev.X, this.Height - ev.Y);
                if (TriMM.Settings.PickingMode == 1) {
                    this.PickVertex(pickingRectangle);
                } else if (TriMM.Settings.PickingMode == 2) {
                    this.PickEdge(pickingRectangle);
                } else if (TriMM.Settings.PickingMode == 3) {
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
            if (this.zoom + TriMM.Mesh.Scale * ev.Delta / 10000 < TriMM.Mesh.Scale) {
                this.zoom += TriMM.Mesh.Scale * ev.Delta / 10000;
                this.SetView();
                this.Invalidate();
            } else if (this.zoom + TriMM.Mesh.Scale * ev.Delta / 100000 < TriMM.Mesh.Scale) {
                this.zoom += TriMM.Mesh.Scale * ev.Delta / 100000;
                this.SetView();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Changes the light position and resets the zoom, when the models scale is changed.
        /// </summary>
        private void Mesh_ScaleChanged() {
            float[] lightPosition = { 20.0f * TriMM.Mesh.Scale, -20.0f * TriMM.Mesh.Scale, 100.0f * TriMM.Mesh.Scale, 1.0f };
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, lightPosition);
            zoom = 0.0f;
        }

        /// <summary>
        /// Changes OpenGls ClearColor, when the background color in the global settings is changed.
        /// </summary>
        private void Settings_BackColorChanged() { Gl.glClearColor(TriMM.Settings.BackColor.R, TriMM.Settings.BackColor.G, TriMM.Settings.BackColor.B, 1.0f); }

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

        #endregion
    }
}
