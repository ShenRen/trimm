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

using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using TriMM.VertexNormalAlgorithms;

namespace TriMM {
    /// <summary>
    /// The entry point for this program.
    /// </summary>
    public partial class TriMMApp : Application {

        #region Fields

        private static TriangleMesh mesh;
        private static TriMMControl control;
        private static Settings settings;
        private static XmlDocument lang;
        private static ImageSource image;
        private static string currentPath;
        private static int currentFormat;

        private static IVertexNormalAlgorithm[] vertexNormalAlgorithms = new IVertexNormalAlgorithm[] { new Gouraud(), new Max(), new Taubin(), new InverseTaubin(),
            new ThuermerAndWuethrich(), new ExtendedThuermerAndWuethrich(), new ChenAndWu(), new ExtendedChenAndWu(), new Rusinkiewicz(),  
            new AdjacentEdgesWeights(), new InverseAdjacentEdgesWeights(), new EdgeNormals(), new InverseEdgeNormals()};

        #endregion

        #region Properties

        /// <value>Gets or sets the global TriangleMesh.</value>
        public static TriangleMesh Mesh { get { return mesh; } set { mesh = value; } }

        /// <value>Gets or sets the global TriMMControl.</value>
        public static TriMMControl Control { get { return control; } set { control = value; } }

        /// <value>Gets or sets the global Settings.</value>
        public static Settings Settings { get { return settings; } set { settings = value; } }

        /// <value>Gets or sets the global language file.</value>
        public static XmlDocument Lang { get { return lang; } set { lang = value; } }

        /// <value>Gets the global window icon.</value>
        public static ImageSource Image { get { return image; } }

        /// <value>Gets or sets the path to the current file.</value>
        public static string CurrentPath { get { return currentPath; } set { currentPath = value; } }

        /// <value>Gets or sets the format of the current file.</value>
        public static int CurrentFormat { get { return currentFormat; } set { currentFormat = value; } }

        /// <value>Gets the array of Vertex normal algorithms.</value>
        public static IVertexNormalAlgorithm[] VertexNormalAlgorithms { get { return vertexNormalAlgorithms; } }

        /// <value>Gets the current Vertex normal algorithm.</value>
        public static IVertexNormalAlgorithm VertexNormalAlgorithm { get { return vertexNormalAlgorithms[settings.NormalAlgo]; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Loads the default Settings and sets the window icon.
        /// </summary>
        public TriMMApp() {
            this.InitializeComponent();

            settings = new Settings();

            Icon icon = TriMM.Properties.Resources.LogoKlein;
            MemoryStream iconStream = new MemoryStream();
            icon.Save(iconStream);
            iconStream.Seek(0, SeekOrigin.Begin);
            image = BitmapFrame.Create(iconStream);
        }

        #endregion
    }
}
