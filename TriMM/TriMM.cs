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
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;

namespace TriMM {
    /// <summary>
    /// The entry point for this program.
    /// </summary>
    public static class TriMM {

        #region Fields

        private static TriangleMesh mesh;
        private static Settings settings;

        #endregion

        #region Properties

        /// <value>Gets or sets the global TriangleMesh</value>
        public static TriangleMesh Mesh { get { return mesh; } set { mesh = value; } }

        /// <value>Gets or sets the global Settings</value>
        public static Settings Settings { get { return settings; } set { settings = value; } }

        #endregion

        #region Methods

        /// <summary>
        /// The Main method of this program.
        /// </summary>
        [STAThread]
        public static void Main() {
            Application.EnableVisualStyles();
            Application.CurrentCulture = new CultureInfo("en-US");
            FileInfo fi = new FileInfo("default.set");
            if (fi.Exists) {
                settings = new Settings("default.set");
            } else {
                settings = new Settings();
            }
            Application.Run(new TriMMForm());
        }
        #endregion
    }
}
