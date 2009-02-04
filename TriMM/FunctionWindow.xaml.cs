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
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Windows.Forms;

namespace TriMM {

    /// <summary>
    /// A Window to create a new TriangleMesh representing a function over x and y.
    /// </summary>
    public partial class FunctionWindow : Window {

        #region Fields

        private NumericUpDown lengthNumericUpDown = new NumericUpDown();
        private NumericUpDown stepsNumericUpDown = new NumericUpDown();
        private NumericUpDown xWidthNumericUpDown = new NumericUpDown();
        private NumericUpDown xStepsNumericUpDown = new NumericUpDown();
        private NumericUpDown yWidthNumericUpDown = new NumericUpDown();
        private NumericUpDown yStepsNumericUpDown = new NumericUpDown();

        private int meshType = 0;

        #endregion

        #region Constructors

        public FunctionWindow() {
            InitializeComponent();
            this.Icon = TriMMApp.Image;

            BitmapSource bitmapSource1 = Imaging.CreateBitmapSourceFromHBitmap(
                    TriMM.Properties.Resources.tri6.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            image1.Source = bitmapSource1;

            BitmapSource bitmapSource2 = Imaging.CreateBitmapSourceFromHBitmap(
                    TriMM.Properties.Resources.tri8.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            image2.Source = bitmapSource2;

            stepsNumericUpDown.TextAlign = lengthNumericUpDown.TextAlign = xWidthNumericUpDown.TextAlign = xStepsNumericUpDown.TextAlign
                = yWidthNumericUpDown.TextAlign = yStepsNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            lengthNumericUpDown.Minimum = xWidthNumericUpDown.Minimum = yWidthNumericUpDown.Minimum = 0.001m;
            lengthNumericUpDown.Maximum = xWidthNumericUpDown.Maximum = yWidthNumericUpDown.Maximum = 1000;
            lengthNumericUpDown.Increment = xWidthNumericUpDown.Increment = yWidthNumericUpDown.Increment = 0.001m;
            lengthNumericUpDown.Value = xWidthNumericUpDown.Value = yWidthNumericUpDown.Value = 1;
            stepsNumericUpDown.Minimum = 0;
            stepsNumericUpDown.Maximum = 10;
            xStepsNumericUpDown.Minimum = yStepsNumericUpDown.Minimum = 1;
            xStepsNumericUpDown.Maximum = yStepsNumericUpDown.Maximum = 100;

            stepsWFHost.Child = stepsNumericUpDown;
            lengthWFHost.Child = lengthNumericUpDown;
            xWidthWFHost.Child = xWidthNumericUpDown;
            xStepsWFHost.Child = xStepsNumericUpDown;
            yWidthWFHost.Child = yWidthNumericUpDown;
            yStepsWFHost.Child = yStepsNumericUpDown;
        }

        #endregion

        #region Event Handling Stuff

        private void RadioButton1_Checked(object sender, RoutedEventArgs e) {
            meshType = 0;
            lengthNumericUpDown.Maximum = xWidthNumericUpDown.Maximum = xStepsNumericUpDown.Maximum = 50;
            if (stepsLabel != null) {
                stepsLabel.Visibility = stepsDockPanel.Visibility = lengthLabel.Visibility = lengthDockPanel.Visibility = Visibility.Visible;
                xWidthLabel.Visibility = xWidthDockPanel.Visibility = xStepsLabel.Visibility = xStepsDockPanel.Visibility
                    = yWidthLabel.Visibility = yWidthDockPanel.Visibility = yStepsLabel.Visibility = yStepsDockPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void RadioButton2_Checked(object sender, RoutedEventArgs e) {
            meshType = 1;
            lengthNumericUpDown.Maximum = xWidthNumericUpDown.Maximum = xStepsNumericUpDown.Maximum = 100;
            if (stepsLabel != null) {
                stepsLabel.Visibility = stepsDockPanel.Visibility = lengthLabel.Visibility = lengthDockPanel.Visibility = Visibility.Collapsed;
                xWidthLabel.Visibility = xWidthDockPanel.Visibility = xStepsLabel.Visibility = xStepsDockPanel.Visibility
                    = yWidthLabel.Visibility = yWidthDockPanel.Visibility = yStepsLabel.Visibility = yStepsDockPanel.Visibility = Visibility.Visible;
            }
        }

        #endregion
    }
}