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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;


namespace TriMM {
    /// <summary>
    /// The Form displayed, when choosing Help -> Info,
    /// showing a logo and the information from AssemblyInfo.
    /// </summary>
    partial class About : Form {

        #region Properties

        /// <value> Gets the AssemblyTitle </value>
        internal string AssemblyTitle {
            get {
                // Get all title-attributes in this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                // At least one title-attribute exists
                if (attributes.Length > 0) {
                    // Choose first one
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    // Return title, if it isn't empty
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                // If there is no title-attribute or the title is empty, return the exe-name
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        /// <value> Gets the AssemblyVersion </value>
        internal string AssemblyVersion {
            get {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString() + "alpha";
            }
        }

        /// <value> Gets the AssemblyDescription </value>
        internal string AssemblyDescription {
            get {
                // Get all description-attributes in this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                // Return an empty string, if there is no description-attribute
                if (attributes.Length == 0)
                    return "";
                // Return the value of the description-attribute, if it exists
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        /// <value> Gets the AssemblyProduct </value>
        internal string AssemblyProduct {
            get {
                // Get all product-attributes in this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                // Return an empty string, if there is no product-attribute
                if (attributes.Length == 0)
                    return "";
                // Return the value of the product-attribute, if it exists
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        /// <value> Gets the AssemblyCopyright </value>
        internal string AssemblyCopyright {
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

        /// <value> Gets the AssemblyCompany </value>
        internal string AssemblyCompany {
            get {
                // Get all company-attributes in this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                // Return an empty string, if there is no company-attribute
                if (attributes.Length == 0)
                    return "";
                // Return the value of the company-attribute, if it exists
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Fills the Form with the information from AssemblyInfo.
        /// </summary>
        internal About() {
            InitializeComponent();

            // Initialize AboutBox to display productinformations from the Assemblyinformation.
            // Change the Assemblyinformation in one of the following ways:
            //  - Projekt->Eigenschaften->Anwendung->Assemblyinformationen
            //  - AssemblyInfo.cs
            this.Text = String.Format("About {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("Version: {0}", AssemblyVersion);
            this.labelCopyright.Text = String.Format("Author: {0}", AssemblyCopyright);
            this.textBoxDescription.Text = AssemblyDescription;
        }

        #endregion

    }
}
