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

namespace TriMM {
    public partial class TriMMView {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TriMMView));
            this.xScrollBar = new System.Windows.Forms.HScrollBar();
            this.yScrollBar = new System.Windows.Forms.VScrollBar();
            this.panel = new System.Windows.Forms.TableLayoutPanel();
            this.centerButton = new System.Windows.Forms.Button();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.axesCheckBox = new System.Windows.Forms.CheckBox();
            this.vertexNormalsCheckBox = new System.Windows.Forms.CheckBox();
            this.pickingModeComboBox = new System.Windows.Forms.ComboBox();
            this.pickingModeLabel = new System.Windows.Forms.Label();
            this.smoothCheckBox = new System.Windows.Forms.CheckBox();
            this.clippingPlaneLabel = new System.Windows.Forms.Label();
            this.facetNormalsCheckBox = new System.Windows.Forms.CheckBox();
            this.verticesCheckBox = new System.Windows.Forms.CheckBox();
            this.meshCheckBox = new System.Windows.Forms.CheckBox();
            this.clippingPlaneNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.solidCheckBox = new System.Windows.Forms.CheckBox();
            this.observedNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.radiusNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.radiusLabel = new System.Windows.Forms.Label();
            this.clearObservedButton = new System.Windows.Forms.Button();
            this.observedLabel = new System.Windows.Forms.Label();
            this.screenshotButton = new System.Windows.Forms.Button();
            this.resetViewButton = new System.Windows.Forms.Button();
            this.colorPanelButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.zAxisColorTextBox = new System.Windows.Forms.TextBox();
            this.zAxisColorButton = new System.Windows.Forms.Button();
            this.yAxisColorTextBox = new System.Windows.Forms.TextBox();
            this.xAxisColorTextBox = new System.Windows.Forms.TextBox();
            this.yAxisColorButton = new System.Windows.Forms.Button();
            this.xAxisColorButton = new System.Windows.Forms.Button();
            this.colorsLabel = new System.Windows.Forms.Label();
            this.observedTriangleColorTextBox = new System.Windows.Forms.TextBox();
            this.observedTriangleButton = new System.Windows.Forms.Button();
            this.standardColorsButton = new System.Windows.Forms.Button();
            this.observedVertexColorTextBox = new System.Windows.Forms.TextBox();
            this.normalColorTextBox = new System.Windows.Forms.TextBox();
            this.vertexColorTextBox = new System.Windows.Forms.TextBox();
            this.meshColorTextBox = new System.Windows.Forms.TextBox();
            this.plainColorTextBox = new System.Windows.Forms.TextBox();
            this.textColorTextBox = new System.Windows.Forms.TextBox();
            this.backColorTextBox = new System.Windows.Forms.TextBox();
            this.observedVertexColorButton = new System.Windows.Forms.Button();
            this.normalColorButton = new System.Windows.Forms.Button();
            this.vertexColorButton = new System.Windows.Forms.Button();
            this.meshColorButton = new System.Windows.Forms.Button();
            this.plainColorButton = new System.Windows.Forms.Button();
            this.textColorButton = new System.Windows.Forms.Button();
            this.backColorButton = new System.Windows.Forms.Button();
            this.panel.SuspendLayout();
            this.controlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clippingPlaneNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.observedNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radiusNumericUpDown)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // xScrollBar
            // 
            this.xScrollBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xScrollBar.Location = new System.Drawing.Point(17, 605);
            this.xScrollBar.Maximum = 10000;
            this.xScrollBar.Minimum = -10000;
            this.xScrollBar.Name = "xScrollBar";
            this.xScrollBar.Size = new System.Drawing.Size(605, 17);
            this.xScrollBar.TabIndex = 0;
            this.xScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.XScrollBar_Scroll);
            // 
            // yScrollBar
            // 
            this.yScrollBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.yScrollBar.Location = new System.Drawing.Point(0, 0);
            this.yScrollBar.Maximum = 10000;
            this.yScrollBar.Minimum = -10000;
            this.yScrollBar.Name = "yScrollBar";
            this.yScrollBar.Size = new System.Drawing.Size(17, 605);
            this.yScrollBar.TabIndex = 1;
            this.yScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.YScrollBar_Scroll);
            // 
            // panel
            // 
            this.panel.ColumnCount = 3;
            this.panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.panel.Controls.Add(this.yScrollBar, 0, 0);
            this.panel.Controls.Add(this.xScrollBar, 1, 1);
            this.panel.Controls.Add(this.centerButton, 0, 1);
            this.panel.Controls.Add(this.controlPanel, 0, 2);
            this.panel.Controls.Add(this.colorPanelButton, 2, 0);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.RowCount = 3;
            this.panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 97F));
            this.panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.panel.Size = new System.Drawing.Size(642, 719);
            this.panel.TabIndex = 2;
            // 
            // centerButton
            // 
            this.centerButton.BackColor = System.Drawing.Color.Red;
            this.centerButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.centerButton.ForeColor = System.Drawing.Color.Red;
            this.centerButton.Location = new System.Drawing.Point(3, 608);
            this.centerButton.Name = "centerButton";
            this.centerButton.Size = new System.Drawing.Size(11, 11);
            this.centerButton.TabIndex = 2;
            this.centerButton.UseVisualStyleBackColor = false;
            this.centerButton.Click += new System.EventHandler(this.CenterButton_Click);
            // 
            // controlPanel
            // 
            this.panel.SetColumnSpan(this.controlPanel, 3);
            this.controlPanel.Controls.Add(this.axesCheckBox);
            this.controlPanel.Controls.Add(this.vertexNormalsCheckBox);
            this.controlPanel.Controls.Add(this.pickingModeComboBox);
            this.controlPanel.Controls.Add(this.pickingModeLabel);
            this.controlPanel.Controls.Add(this.smoothCheckBox);
            this.controlPanel.Controls.Add(this.clippingPlaneLabel);
            this.controlPanel.Controls.Add(this.facetNormalsCheckBox);
            this.controlPanel.Controls.Add(this.verticesCheckBox);
            this.controlPanel.Controls.Add(this.meshCheckBox);
            this.controlPanel.Controls.Add(this.clippingPlaneNumericUpDown);
            this.controlPanel.Controls.Add(this.solidCheckBox);
            this.controlPanel.Controls.Add(this.observedNumericUpDown);
            this.controlPanel.Controls.Add(this.radiusNumericUpDown);
            this.controlPanel.Controls.Add(this.radiusLabel);
            this.controlPanel.Controls.Add(this.clearObservedButton);
            this.controlPanel.Controls.Add(this.observedLabel);
            this.controlPanel.Controls.Add(this.screenshotButton);
            this.controlPanel.Controls.Add(this.resetViewButton);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlPanel.Location = new System.Drawing.Point(3, 625);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(636, 91);
            this.controlPanel.TabIndex = 4;
            // 
            // axesCheckBox
            // 
            this.axesCheckBox.AutoSize = true;
            this.axesCheckBox.Location = new System.Drawing.Point(367, 30);
            this.axesCheckBox.Name = "axesCheckBox";
            this.axesCheckBox.Size = new System.Drawing.Size(49, 17);
            this.axesCheckBox.TabIndex = 31;
            this.axesCheckBox.Text = "Axes";
            this.axesCheckBox.UseVisualStyleBackColor = true;
            this.axesCheckBox.CheckedChanged += new System.EventHandler(this.AxesCheckBox_CheckedChanged);
            // 
            // vertexNormalsCheckBox
            // 
            this.vertexNormalsCheckBox.AutoSize = true;
            this.vertexNormalsCheckBox.Location = new System.Drawing.Point(195, 30);
            this.vertexNormalsCheckBox.Name = "vertexNormalsCheckBox";
            this.vertexNormalsCheckBox.Size = new System.Drawing.Size(97, 17);
            this.vertexNormalsCheckBox.TabIndex = 30;
            this.vertexNormalsCheckBox.Text = "Vertex Normals";
            this.vertexNormalsCheckBox.UseVisualStyleBackColor = true;
            this.vertexNormalsCheckBox.CheckedChanged += new System.EventHandler(this.VertexNormalsCheckBox_CheckedChanged);
            // 
            // pickingModeComboBox
            // 
            this.pickingModeComboBox.FormattingEnabled = true;
            this.pickingModeComboBox.Items.AddRange(new object[] {
            "None",
            "Vertex",
            "Triangle"});
            this.pickingModeComboBox.Location = new System.Drawing.Point(92, 65);
            this.pickingModeComboBox.Name = "pickingModeComboBox";
            this.pickingModeComboBox.Size = new System.Drawing.Size(70, 21);
            this.pickingModeComboBox.TabIndex = 29;
            this.pickingModeComboBox.SelectedIndexChanged += new System.EventHandler(this.PickingModeComboBox_SelectedIndexChanged);
            // 
            // pickingModeLabel
            // 
            this.pickingModeLabel.AutoSize = true;
            this.pickingModeLabel.Location = new System.Drawing.Point(11, 68);
            this.pickingModeLabel.Name = "pickingModeLabel";
            this.pickingModeLabel.Size = new System.Drawing.Size(75, 13);
            this.pickingModeLabel.TabIndex = 28;
            this.pickingModeLabel.Text = "Picking Mode:";
            // 
            // smoothCheckBox
            // 
            this.smoothCheckBox.AutoSize = true;
            this.smoothCheckBox.Location = new System.Drawing.Point(14, 7);
            this.smoothCheckBox.Name = "smoothCheckBox";
            this.smoothCheckBox.Size = new System.Drawing.Size(62, 17);
            this.smoothCheckBox.TabIndex = 27;
            this.smoothCheckBox.Text = "Smooth";
            this.smoothCheckBox.UseVisualStyleBackColor = true;
            this.smoothCheckBox.CheckedChanged += new System.EventHandler(this.SmoothCheckBox_CheckedChanged);
            // 
            // clippingPlaneLabel
            // 
            this.clippingPlaneLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clippingPlaneLabel.AutoSize = true;
            this.clippingPlaneLabel.Location = new System.Drawing.Point(485, 8);
            this.clippingPlaneLabel.Name = "clippingPlaneLabel";
            this.clippingPlaneLabel.Size = new System.Drawing.Size(77, 13);
            this.clippingPlaneLabel.TabIndex = 27;
            this.clippingPlaneLabel.Text = "Clipping Plane:";
            // 
            // facetNormalsCheckBox
            // 
            this.facetNormalsCheckBox.AutoSize = true;
            this.facetNormalsCheckBox.Location = new System.Drawing.Point(14, 30);
            this.facetNormalsCheckBox.Name = "facetNormalsCheckBox";
            this.facetNormalsCheckBox.Size = new System.Drawing.Size(105, 17);
            this.facetNormalsCheckBox.TabIndex = 23;
            this.facetNormalsCheckBox.Text = "Triangle Normals";
            this.facetNormalsCheckBox.UseVisualStyleBackColor = true;
            this.facetNormalsCheckBox.CheckedChanged += new System.EventHandler(this.FacetNormalsCheckBox_CheckedChanged);
            // 
            // verticesCheckBox
            // 
            this.verticesCheckBox.AutoSize = true;
            this.verticesCheckBox.Location = new System.Drawing.Point(367, 7);
            this.verticesCheckBox.Name = "verticesCheckBox";
            this.verticesCheckBox.Size = new System.Drawing.Size(64, 17);
            this.verticesCheckBox.TabIndex = 26;
            this.verticesCheckBox.Text = "Vertices";
            this.verticesCheckBox.UseVisualStyleBackColor = true;
            this.verticesCheckBox.CheckedChanged += new System.EventHandler(this.VerticesCheckBox_CheckedChanged);
            // 
            // meshCheckBox
            // 
            this.meshCheckBox.AutoSize = true;
            this.meshCheckBox.Checked = true;
            this.meshCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.meshCheckBox.Location = new System.Drawing.Point(246, 7);
            this.meshCheckBox.Name = "meshCheckBox";
            this.meshCheckBox.Size = new System.Drawing.Size(52, 17);
            this.meshCheckBox.TabIndex = 25;
            this.meshCheckBox.Text = "Mesh";
            this.meshCheckBox.UseVisualStyleBackColor = true;
            this.meshCheckBox.CheckedChanged += new System.EventHandler(this.MeshCheckBox_CheckedChanged);
            // 
            // clippingPlaneNumericUpDown
            // 
            this.clippingPlaneNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clippingPlaneNumericUpDown.DecimalPlaces = 5;
            this.clippingPlaneNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.clippingPlaneNumericUpDown.Location = new System.Drawing.Point(569, 6);
            this.clippingPlaneNumericUpDown.Maximum = new decimal(new int[] {
            11,
            0,
            0,
            65536});
            this.clippingPlaneNumericUpDown.Minimum = new decimal(new int[] {
            11,
            0,
            0,
            -2147418112});
            this.clippingPlaneNumericUpDown.Name = "clippingPlaneNumericUpDown";
            this.clippingPlaneNumericUpDown.Size = new System.Drawing.Size(64, 20);
            this.clippingPlaneNumericUpDown.TabIndex = 25;
            this.clippingPlaneNumericUpDown.Value = new decimal(new int[] {
            11,
            0,
            0,
            65536});
            this.clippingPlaneNumericUpDown.ValueChanged += new System.EventHandler(this.ClippingPlaneNumericUpDown_ValueChanged);
            // 
            // solidCheckBox
            // 
            this.solidCheckBox.AutoSize = true;
            this.solidCheckBox.Checked = true;
            this.solidCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.solidCheckBox.Location = new System.Drawing.Point(125, 7);
            this.solidCheckBox.Name = "solidCheckBox";
            this.solidCheckBox.Size = new System.Drawing.Size(49, 17);
            this.solidCheckBox.TabIndex = 24;
            this.solidCheckBox.Text = "Solid";
            this.solidCheckBox.UseVisualStyleBackColor = true;
            this.solidCheckBox.CheckedChanged += new System.EventHandler(this.SolidCheckBox_CheckedChanged);
            // 
            // observedNumericUpDown
            // 
            this.observedNumericUpDown.Location = new System.Drawing.Point(230, 66);
            this.observedNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.observedNumericUpDown.Name = "observedNumericUpDown";
            this.observedNumericUpDown.Size = new System.Drawing.Size(84, 20);
            this.observedNumericUpDown.TabIndex = 21;
            this.observedNumericUpDown.ThousandsSeparator = true;
            this.observedNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.observedNumericUpDown.ValueChanged += new System.EventHandler(this.ObservedNumericUpDown_ValueChanged);
            // 
            // radiusNumericUpDown
            // 
            this.radiusNumericUpDown.DecimalPlaces = 7;
            this.radiusNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.radiusNumericUpDown.Location = new System.Drawing.Point(422, 66);
            this.radiusNumericUpDown.Name = "radiusNumericUpDown";
            this.radiusNumericUpDown.Size = new System.Drawing.Size(84, 20);
            this.radiusNumericUpDown.TabIndex = 20;
            this.radiusNumericUpDown.ValueChanged += new System.EventHandler(this.RadiusNumericUpDown_ValueChanged);
            // 
            // radiusLabel
            // 
            this.radiusLabel.AutoSize = true;
            this.radiusLabel.Location = new System.Drawing.Point(373, 70);
            this.radiusLabel.Name = "radiusLabel";
            this.radiusLabel.Size = new System.Drawing.Size(43, 13);
            this.radiusLabel.TabIndex = 18;
            this.radiusLabel.Text = "Radius:";
            // 
            // clearObservedButton
            // 
            this.clearObservedButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.clearObservedButton.Location = new System.Drawing.Point(320, 65);
            this.clearObservedButton.Name = "clearObservedButton";
            this.clearObservedButton.Size = new System.Drawing.Size(50, 23);
            this.clearObservedButton.TabIndex = 16;
            this.clearObservedButton.Text = "Clear";
            this.clearObservedButton.UseVisualStyleBackColor = true;
            this.clearObservedButton.Visible = false;
            this.clearObservedButton.Click += new System.EventHandler(this.ClearObservedButton_Click);
            // 
            // observedLabel
            // 
            this.observedLabel.AutoSize = true;
            this.observedLabel.Location = new System.Drawing.Point(168, 68);
            this.observedLabel.Name = "observedLabel";
            this.observedLabel.Size = new System.Drawing.Size(56, 13);
            this.observedLabel.TabIndex = 12;
            this.observedLabel.Text = "Observed:";
            // 
            // screenshotButton
            // 
            this.screenshotButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.screenshotButton.Location = new System.Drawing.Point(537, 65);
            this.screenshotButton.Name = "screenshotButton";
            this.screenshotButton.Size = new System.Drawing.Size(96, 23);
            this.screenshotButton.TabIndex = 11;
            this.screenshotButton.Text = "Screenshot";
            this.screenshotButton.UseVisualStyleBackColor = true;
            this.screenshotButton.Click += new System.EventHandler(this.ScreenshotButton_Click);
            // 
            // resetViewButton
            // 
            this.resetViewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.resetViewButton.Location = new System.Drawing.Point(537, 36);
            this.resetViewButton.Name = "resetViewButton";
            this.resetViewButton.Size = new System.Drawing.Size(96, 23);
            this.resetViewButton.TabIndex = 10;
            this.resetViewButton.Text = "Reset View";
            this.resetViewButton.UseVisualStyleBackColor = true;
            this.resetViewButton.Click += new System.EventHandler(this.ResetViewButton_Click);
            // 
            // colorPanelButton
            // 
            this.colorPanelButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.colorPanelButton.Location = new System.Drawing.Point(625, 3);
            this.colorPanelButton.Name = "colorPanelButton";
            this.panel.SetRowSpan(this.colorPanelButton, 2);
            this.colorPanelButton.Size = new System.Drawing.Size(14, 616);
            this.colorPanelButton.TabIndex = 5;
            this.colorPanelButton.Text = ">";
            this.colorPanelButton.UseVisualStyleBackColor = true;
            this.colorPanelButton.Click += new System.EventHandler(this.ColorPanelButton_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.zAxisColorTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.zAxisColorButton);
            this.splitContainer1.Panel2.Controls.Add(this.yAxisColorTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.xAxisColorTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.yAxisColorButton);
            this.splitContainer1.Panel2.Controls.Add(this.xAxisColorButton);
            this.splitContainer1.Panel2.Controls.Add(this.colorsLabel);
            this.splitContainer1.Panel2.Controls.Add(this.observedTriangleColorTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.observedTriangleButton);
            this.splitContainer1.Panel2.Controls.Add(this.standardColorsButton);
            this.splitContainer1.Panel2.Controls.Add(this.observedVertexColorTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.normalColorTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.vertexColorTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.meshColorTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.plainColorTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.textColorTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.backColorTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.observedVertexColorButton);
            this.splitContainer1.Panel2.Controls.Add(this.normalColorButton);
            this.splitContainer1.Panel2.Controls.Add(this.vertexColorButton);
            this.splitContainer1.Panel2.Controls.Add(this.meshColorButton);
            this.splitContainer1.Panel2.Controls.Add(this.plainColorButton);
            this.splitContainer1.Panel2.Controls.Add(this.textColorButton);
            this.splitContainer1.Panel2.Controls.Add(this.backColorButton);
            this.splitContainer1.Size = new System.Drawing.Size(782, 719);
            this.splitContainer1.SplitterDistance = 642;
            this.splitContainer1.TabIndex = 3;
            // 
            // zAxisColorTextBox
            // 
            this.zAxisColorTextBox.Location = new System.Drawing.Point(4, 484);
            this.zAxisColorTextBox.Name = "zAxisColorTextBox";
            this.zAxisColorTextBox.ReadOnly = true;
            this.zAxisColorTextBox.Size = new System.Drawing.Size(128, 20);
            this.zAxisColorTextBox.TabIndex = 25;
            // 
            // zAxisColorButton
            // 
            this.zAxisColorButton.Location = new System.Drawing.Point(4, 510);
            this.zAxisColorButton.Name = "zAxisColorButton";
            this.zAxisColorButton.Size = new System.Drawing.Size(128, 23);
            this.zAxisColorButton.TabIndex = 24;
            this.zAxisColorButton.Tag = "8";
            this.zAxisColorButton.Text = "Z-Axis";
            this.zAxisColorButton.UseVisualStyleBackColor = true;
            this.zAxisColorButton.Click += new System.EventHandler(this.ColorButton_Click);
            // 
            // yAxisColorTextBox
            // 
            this.yAxisColorTextBox.Location = new System.Drawing.Point(4, 429);
            this.yAxisColorTextBox.Name = "yAxisColorTextBox";
            this.yAxisColorTextBox.ReadOnly = true;
            this.yAxisColorTextBox.Size = new System.Drawing.Size(128, 20);
            this.yAxisColorTextBox.TabIndex = 23;
            // 
            // xAxisColorTextBox
            // 
            this.xAxisColorTextBox.Location = new System.Drawing.Point(4, 374);
            this.xAxisColorTextBox.Name = "xAxisColorTextBox";
            this.xAxisColorTextBox.ReadOnly = true;
            this.xAxisColorTextBox.Size = new System.Drawing.Size(128, 20);
            this.xAxisColorTextBox.TabIndex = 22;
            // 
            // yAxisColorButton
            // 
            this.yAxisColorButton.Location = new System.Drawing.Point(4, 455);
            this.yAxisColorButton.Name = "yAxisColorButton";
            this.yAxisColorButton.Size = new System.Drawing.Size(128, 23);
            this.yAxisColorButton.TabIndex = 21;
            this.yAxisColorButton.Tag = "7";
            this.yAxisColorButton.Text = "Y-Axis";
            this.yAxisColorButton.UseVisualStyleBackColor = true;
            this.yAxisColorButton.Click += new System.EventHandler(this.ColorButton_Click);
            // 
            // xAxisColorButton
            // 
            this.xAxisColorButton.Location = new System.Drawing.Point(4, 400);
            this.xAxisColorButton.Name = "xAxisColorButton";
            this.xAxisColorButton.Size = new System.Drawing.Size(128, 23);
            this.xAxisColorButton.TabIndex = 20;
            this.xAxisColorButton.Tag = "6";
            this.xAxisColorButton.Text = "X-Axis";
            this.xAxisColorButton.UseVisualStyleBackColor = true;
            this.xAxisColorButton.Click += new System.EventHandler(this.ColorButton_Click);
            // 
            // colorsLabel
            // 
            this.colorsLabel.AutoSize = true;
            this.colorsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorsLabel.Location = new System.Drawing.Point(39, 9);
            this.colorsLabel.Name = "colorsLabel";
            this.colorsLabel.Size = new System.Drawing.Size(60, 20);
            this.colorsLabel.TabIndex = 19;
            this.colorsLabel.Text = "Colors";
            // 
            // observedTriangleColorTextBox
            // 
            this.observedTriangleColorTextBox.Location = new System.Drawing.Point(4, 594);
            this.observedTriangleColorTextBox.Name = "observedTriangleColorTextBox";
            this.observedTriangleColorTextBox.ReadOnly = true;
            this.observedTriangleColorTextBox.Size = new System.Drawing.Size(128, 20);
            this.observedTriangleColorTextBox.TabIndex = 18;
            // 
            // observedTriangleButton
            // 
            this.observedTriangleButton.Location = new System.Drawing.Point(4, 620);
            this.observedTriangleButton.Name = "observedTriangleButton";
            this.observedTriangleButton.Size = new System.Drawing.Size(128, 23);
            this.observedTriangleButton.TabIndex = 17;
            this.observedTriangleButton.Tag = "10";
            this.observedTriangleButton.Text = "Observed Triangle";
            this.observedTriangleButton.UseVisualStyleBackColor = true;
            this.observedTriangleButton.Click += new System.EventHandler(this.ColorButton_Click);
            // 
            // standardColorsButton
            // 
            this.standardColorsButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.standardColorsButton.Location = new System.Drawing.Point(3, 688);
            this.standardColorsButton.Name = "standardColorsButton";
            this.standardColorsButton.Size = new System.Drawing.Size(130, 23);
            this.standardColorsButton.TabIndex = 14;
            this.standardColorsButton.Text = "Standard Colors";
            this.standardColorsButton.UseVisualStyleBackColor = true;
            this.standardColorsButton.Click += new System.EventHandler(this.StandardColorsButton_Click);
            // 
            // observedVertexColorTextBox
            // 
            this.observedVertexColorTextBox.Location = new System.Drawing.Point(4, 539);
            this.observedVertexColorTextBox.Name = "observedVertexColorTextBox";
            this.observedVertexColorTextBox.ReadOnly = true;
            this.observedVertexColorTextBox.Size = new System.Drawing.Size(128, 20);
            this.observedVertexColorTextBox.TabIndex = 13;
            // 
            // normalColorTextBox
            // 
            this.normalColorTextBox.Location = new System.Drawing.Point(4, 319);
            this.normalColorTextBox.Name = "normalColorTextBox";
            this.normalColorTextBox.ReadOnly = true;
            this.normalColorTextBox.Size = new System.Drawing.Size(128, 20);
            this.normalColorTextBox.TabIndex = 12;
            // 
            // vertexColorTextBox
            // 
            this.vertexColorTextBox.Location = new System.Drawing.Point(4, 264);
            this.vertexColorTextBox.Name = "vertexColorTextBox";
            this.vertexColorTextBox.ReadOnly = true;
            this.vertexColorTextBox.Size = new System.Drawing.Size(128, 20);
            this.vertexColorTextBox.TabIndex = 11;
            // 
            // meshColorTextBox
            // 
            this.meshColorTextBox.Location = new System.Drawing.Point(4, 209);
            this.meshColorTextBox.Name = "meshColorTextBox";
            this.meshColorTextBox.ReadOnly = true;
            this.meshColorTextBox.Size = new System.Drawing.Size(128, 20);
            this.meshColorTextBox.TabIndex = 10;
            // 
            // plainColorTextBox
            // 
            this.plainColorTextBox.Location = new System.Drawing.Point(4, 154);
            this.plainColorTextBox.Name = "plainColorTextBox";
            this.plainColorTextBox.ReadOnly = true;
            this.plainColorTextBox.Size = new System.Drawing.Size(128, 20);
            this.plainColorTextBox.TabIndex = 9;
            // 
            // textColorTextBox
            // 
            this.textColorTextBox.Location = new System.Drawing.Point(4, 99);
            this.textColorTextBox.Name = "textColorTextBox";
            this.textColorTextBox.ReadOnly = true;
            this.textColorTextBox.Size = new System.Drawing.Size(128, 20);
            this.textColorTextBox.TabIndex = 8;
            // 
            // backColorTextBox
            // 
            this.backColorTextBox.Location = new System.Drawing.Point(4, 44);
            this.backColorTextBox.Name = "backColorTextBox";
            this.backColorTextBox.ReadOnly = true;
            this.backColorTextBox.Size = new System.Drawing.Size(128, 20);
            this.backColorTextBox.TabIndex = 7;
            // 
            // observedVertexColorButton
            // 
            this.observedVertexColorButton.Location = new System.Drawing.Point(4, 565);
            this.observedVertexColorButton.Name = "observedVertexColorButton";
            this.observedVertexColorButton.Size = new System.Drawing.Size(128, 23);
            this.observedVertexColorButton.TabIndex = 6;
            this.observedVertexColorButton.Tag = "9";
            this.observedVertexColorButton.Text = "Observed Vertex";
            this.observedVertexColorButton.UseVisualStyleBackColor = true;
            this.observedVertexColorButton.Click += new System.EventHandler(this.ColorButton_Click);
            // 
            // normalColorButton
            // 
            this.normalColorButton.Location = new System.Drawing.Point(4, 345);
            this.normalColorButton.Name = "normalColorButton";
            this.normalColorButton.Size = new System.Drawing.Size(128, 23);
            this.normalColorButton.TabIndex = 5;
            this.normalColorButton.Tag = "5";
            this.normalColorButton.Text = "Normals";
            this.normalColorButton.UseVisualStyleBackColor = true;
            this.normalColorButton.Click += new System.EventHandler(this.ColorButton_Click);
            // 
            // vertexColorButton
            // 
            this.vertexColorButton.Location = new System.Drawing.Point(3, 290);
            this.vertexColorButton.Name = "vertexColorButton";
            this.vertexColorButton.Size = new System.Drawing.Size(129, 23);
            this.vertexColorButton.TabIndex = 4;
            this.vertexColorButton.Tag = "4";
            this.vertexColorButton.Text = "Vertices";
            this.vertexColorButton.UseVisualStyleBackColor = true;
            this.vertexColorButton.Click += new System.EventHandler(this.ColorButton_Click);
            // 
            // meshColorButton
            // 
            this.meshColorButton.Location = new System.Drawing.Point(3, 235);
            this.meshColorButton.Name = "meshColorButton";
            this.meshColorButton.Size = new System.Drawing.Size(130, 23);
            this.meshColorButton.TabIndex = 3;
            this.meshColorButton.Tag = "3";
            this.meshColorButton.Text = "Mesh";
            this.meshColorButton.UseVisualStyleBackColor = true;
            this.meshColorButton.Click += new System.EventHandler(this.ColorButton_Click);
            // 
            // plainColorButton
            // 
            this.plainColorButton.Location = new System.Drawing.Point(3, 180);
            this.plainColorButton.Name = "plainColorButton";
            this.plainColorButton.Size = new System.Drawing.Size(130, 23);
            this.plainColorButton.TabIndex = 2;
            this.plainColorButton.Tag = "2";
            this.plainColorButton.Text = "Plain";
            this.plainColorButton.UseVisualStyleBackColor = true;
            this.plainColorButton.Click += new System.EventHandler(this.ColorButton_Click);
            // 
            // textColorButton
            // 
            this.textColorButton.Location = new System.Drawing.Point(3, 125);
            this.textColorButton.Name = "textColorButton";
            this.textColorButton.Size = new System.Drawing.Size(130, 23);
            this.textColorButton.TabIndex = 1;
            this.textColorButton.Tag = "1";
            this.textColorButton.Text = "Text";
            this.textColorButton.UseVisualStyleBackColor = true;
            this.textColorButton.Click += new System.EventHandler(this.ColorButton_Click);
            // 
            // backColorButton
            // 
            this.backColorButton.Location = new System.Drawing.Point(3, 70);
            this.backColorButton.Name = "backColorButton";
            this.backColorButton.Size = new System.Drawing.Size(130, 23);
            this.backColorButton.TabIndex = 0;
            this.backColorButton.Tag = "0";
            this.backColorButton.Text = "Background";
            this.backColorButton.UseVisualStyleBackColor = true;
            this.backColorButton.Click += new System.EventHandler(this.ColorButton_Click);
            // 
            // TriMMView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 719);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(790, 753);
            this.Name = "TriMMView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "TriMM View";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.View_FormClosing);
            this.panel.ResumeLayout(false);
            this.controlPanel.ResumeLayout(false);
            this.controlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clippingPlaneNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.observedNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radiusNumericUpDown)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.HScrollBar xScrollBar;
        private System.Windows.Forms.VScrollBar yScrollBar;
        private System.Windows.Forms.TableLayoutPanel panel;
        private System.Windows.Forms.Button centerButton;
        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.Button resetViewButton;
        private System.Windows.Forms.Button screenshotButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button colorPanelButton;
        private System.Windows.Forms.Button normalColorButton;
        private System.Windows.Forms.Button vertexColorButton;
        private System.Windows.Forms.Button meshColorButton;
        private System.Windows.Forms.Button plainColorButton;
        private System.Windows.Forms.Button textColorButton;
        private System.Windows.Forms.Button backColorButton;
        private System.Windows.Forms.TextBox observedVertexColorTextBox;
        private System.Windows.Forms.TextBox normalColorTextBox;
        private System.Windows.Forms.TextBox vertexColorTextBox;
        private System.Windows.Forms.TextBox meshColorTextBox;
        private System.Windows.Forms.TextBox plainColorTextBox;
        private System.Windows.Forms.TextBox textColorTextBox;
        private System.Windows.Forms.TextBox backColorTextBox;
        private System.Windows.Forms.Button observedVertexColorButton;
        private System.Windows.Forms.Button standardColorsButton;
        private System.Windows.Forms.Label observedLabel;
        private System.Windows.Forms.Label radiusLabel;
        private System.Windows.Forms.Button clearObservedButton;
        private System.Windows.Forms.NumericUpDown radiusNumericUpDown;
        private System.Windows.Forms.NumericUpDown observedNumericUpDown;
        private System.Windows.Forms.NumericUpDown clippingPlaneNumericUpDown;
        private System.Windows.Forms.Label clippingPlaneLabel;
        private System.Windows.Forms.TextBox observedTriangleColorTextBox;
        private System.Windows.Forms.Button observedTriangleButton;
        private System.Windows.Forms.CheckBox smoothCheckBox;
        private System.Windows.Forms.CheckBox facetNormalsCheckBox;
        private System.Windows.Forms.CheckBox verticesCheckBox;
        private System.Windows.Forms.CheckBox meshCheckBox;
        private System.Windows.Forms.CheckBox solidCheckBox;
        private System.Windows.Forms.ComboBox pickingModeComboBox;
        private System.Windows.Forms.Label pickingModeLabel;
        private System.Windows.Forms.CheckBox vertexNormalsCheckBox;
        private System.Windows.Forms.CheckBox axesCheckBox;
        private System.Windows.Forms.TextBox zAxisColorTextBox;
        private System.Windows.Forms.Button zAxisColorButton;
        private System.Windows.Forms.TextBox yAxisColorTextBox;
        private System.Windows.Forms.TextBox xAxisColorTextBox;
        private System.Windows.Forms.Button yAxisColorButton;
        private System.Windows.Forms.Button xAxisColorButton;
        private System.Windows.Forms.Label colorsLabel;
    }
}