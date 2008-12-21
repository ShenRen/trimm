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
using System.Windows;
using System.Windows.Media;
using System.Windows.Documents;
using System.Reflection;
using System.IO;

namespace TriMM {
    /// <summary>
    /// Interaktionslogik für About.xaml
    /// </summary>
    public partial class About : Window {

        #region Properties

        /// <value> Gets the AssemblyVersion </value>
        public string AssemblyVersion { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }

        /// <value> Gets the AssemblyCopyright </value>
        public string AssemblyCopyright {
            get {
                // Get all copyright-attributes in this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                // Return an empty string, if there is no copyright-attribute
                if (attributes.Length == 0)
                    return "";
                // Return the value of the copyright-attribute, if it exists
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the Window with the information from the assembly.
        /// </summary>
        public About() {
            InitializeComponent();
            this.Icon = TriMMApp.Image;
            versionLabel.Content = String.Format("Version: {0}", AssemblyVersion) + " beta";
            authorLabel.Content = String.Format(AssemblyCopyright);

            FlowDocument doc = new FlowDocument();
            doc.FontSize = 11;
            doc.FontFamily = new FontFamily("Tahoma");
            Paragraph para = new Paragraph();

            string[] lines = Properties.Resources.license.Split(new[] { "\r\n" }, StringSplitOptions.None);
            foreach (string line in lines) {
                para.Inlines.Add(new Run(line));
                para.Inlines.Add(new LineBreak());
            }
            doc.Blocks.Add(para);
            licenseViewer.Document = doc;
        }

        #endregion
    }
}
