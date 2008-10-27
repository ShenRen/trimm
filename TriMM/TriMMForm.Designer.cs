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

namespace TriMM {
    partial class TriMMForm {
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
            this.manipulationGroupBox = new System.Windows.Forms.GroupBox();
            this.flipObservedTriangleButton = new System.Windows.Forms.Button();
            this.flipAllTrianglesButton = new System.Windows.Forms.Button();
            this.flipButton = new System.Windows.Forms.Button();
            this.e1NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.e2NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.moveObservedButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.removeObservedButton = new System.Windows.Forms.Button();
            this.addVertexButton = new System.Windows.Forms.Button();
            this.xNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.zNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.yNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.addTriangleButton = new System.Windows.Forms.Button();
            this.removeSinglesButton = new System.Windows.Forms.Button();
            this.removeTriangleButton = new System.Windows.Forms.Button();
            this.removeDoubleVertButton = new System.Windows.Forms.Button();
            this.cNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.bNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.removeColinButton = new System.Windows.Forms.Button();
            this.removeDoubleButton = new System.Windows.Forms.Button();
            this.aNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.remove2NVerticesButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.triangleLabel = new System.Windows.Forms.Label();
            this.edgeLabel = new System.Windows.Forms.Label();
            this.vertexLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.meshGroupBox = new System.Windows.Forms.GroupBox();
            this.transposeVertexButton = new System.Windows.Forms.Button();
            this.manipulationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.e1NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.e2NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aNumericUpDown)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.meshGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // manipulationGroupBox
            // 
            this.manipulationGroupBox.Controls.Add(this.transposeVertexButton);
            this.manipulationGroupBox.Controls.Add(this.flipObservedTriangleButton);
            this.manipulationGroupBox.Controls.Add(this.flipAllTrianglesButton);
            this.manipulationGroupBox.Controls.Add(this.flipButton);
            this.manipulationGroupBox.Controls.Add(this.e1NumericUpDown);
            this.manipulationGroupBox.Controls.Add(this.e2NumericUpDown);
            this.manipulationGroupBox.Controls.Add(this.moveObservedButton);
            this.manipulationGroupBox.Controls.Add(this.label3);
            this.manipulationGroupBox.Controls.Add(this.label2);
            this.manipulationGroupBox.Controls.Add(this.label1);
            this.manipulationGroupBox.Controls.Add(this.removeObservedButton);
            this.manipulationGroupBox.Controls.Add(this.addVertexButton);
            this.manipulationGroupBox.Controls.Add(this.xNumericUpDown);
            this.manipulationGroupBox.Controls.Add(this.zNumericUpDown);
            this.manipulationGroupBox.Controls.Add(this.yNumericUpDown);
            this.manipulationGroupBox.Controls.Add(this.addTriangleButton);
            this.manipulationGroupBox.Controls.Add(this.removeSinglesButton);
            this.manipulationGroupBox.Controls.Add(this.removeTriangleButton);
            this.manipulationGroupBox.Controls.Add(this.removeDoubleVertButton);
            this.manipulationGroupBox.Controls.Add(this.cNumericUpDown);
            this.manipulationGroupBox.Controls.Add(this.bNumericUpDown);
            this.manipulationGroupBox.Controls.Add(this.removeColinButton);
            this.manipulationGroupBox.Controls.Add(this.removeDoubleButton);
            this.manipulationGroupBox.Controls.Add(this.aNumericUpDown);
            this.manipulationGroupBox.Controls.Add(this.remove2NVerticesButton);
            this.manipulationGroupBox.Location = new System.Drawing.Point(12, 104);
            this.manipulationGroupBox.Name = "manipulationGroupBox";
            this.manipulationGroupBox.Size = new System.Drawing.Size(239, 564);
            this.manipulationGroupBox.TabIndex = 15;
            this.manipulationGroupBox.TabStop = false;
            this.manipulationGroupBox.Text = "Manipulation";
            this.manipulationGroupBox.Visible = false;
            // 
            // flipObservedTriangleButton
            // 
            this.flipObservedTriangleButton.Location = new System.Drawing.Point(6, 193);
            this.flipObservedTriangleButton.Name = "flipObservedTriangleButton";
            this.flipObservedTriangleButton.Size = new System.Drawing.Size(227, 23);
            this.flipObservedTriangleButton.TabIndex = 28;
            this.flipObservedTriangleButton.Text = "Flip Observed Triangle";
            this.flipObservedTriangleButton.UseVisualStyleBackColor = true;
            this.flipObservedTriangleButton.Click += new System.EventHandler(this.FlipObservedTriangleButton_Click);
            // 
            // flipAllTrianglesButton
            // 
            this.flipAllTrianglesButton.Location = new System.Drawing.Point(6, 164);
            this.flipAllTrianglesButton.Name = "flipAllTrianglesButton";
            this.flipAllTrianglesButton.Size = new System.Drawing.Size(227, 23);
            this.flipAllTrianglesButton.TabIndex = 27;
            this.flipAllTrianglesButton.Text = "Flip All Triangles";
            this.flipAllTrianglesButton.UseVisualStyleBackColor = true;
            this.flipAllTrianglesButton.Click += new System.EventHandler(this.FlipAllTrianglesButton_Click);
            // 
            // flipButton
            // 
            this.flipButton.Location = new System.Drawing.Point(7, 535);
            this.flipButton.Name = "flipButton";
            this.flipButton.Size = new System.Drawing.Size(226, 23);
            this.flipButton.TabIndex = 26;
            this.flipButton.Text = "Flip Selected Edge";
            this.flipButton.UseVisualStyleBackColor = true;
            this.flipButton.Click += new System.EventHandler(this.FlipButton_Click);
            // 
            // e1NumericUpDown
            // 
            this.e1NumericUpDown.Location = new System.Drawing.Point(30, 509);
            this.e1NumericUpDown.Name = "e1NumericUpDown";
            this.e1NumericUpDown.Size = new System.Drawing.Size(65, 20);
            this.e1NumericUpDown.TabIndex = 25;
            this.e1NumericUpDown.ThousandsSeparator = true;
            // 
            // e2NumericUpDown
            // 
            this.e2NumericUpDown.Location = new System.Drawing.Point(141, 509);
            this.e2NumericUpDown.Name = "e2NumericUpDown";
            this.e2NumericUpDown.Size = new System.Drawing.Size(65, 20);
            this.e2NumericUpDown.TabIndex = 24;
            this.e2NumericUpDown.ThousandsSeparator = true;
            // 
            // moveObservedButton
            // 
            this.moveObservedButton.Location = new System.Drawing.Point(6, 339);
            this.moveObservedButton.Name = "moveObservedButton";
            this.moveObservedButton.Size = new System.Drawing.Size(227, 23);
            this.moveObservedButton.TabIndex = 23;
            this.moveObservedButton.Text = "Move Observed Vertex To";
            this.moveObservedButton.UseVisualStyleBackColor = true;
            this.moveObservedButton.Click += new System.EventHandler(this.MoveObservedButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 454);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Z:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 428);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Y:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 402);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "X:";
            // 
            // removeObservedButton
            // 
            this.removeObservedButton.Location = new System.Drawing.Point(6, 310);
            this.removeObservedButton.Name = "removeObservedButton";
            this.removeObservedButton.Size = new System.Drawing.Size(227, 23);
            this.removeObservedButton.TabIndex = 19;
            this.removeObservedButton.Text = "Remove Observed Vertex";
            this.removeObservedButton.UseVisualStyleBackColor = true;
            this.removeObservedButton.Click += new System.EventHandler(this.RemoveObservedButton_Click);
            // 
            // addVertexButton
            // 
            this.addVertexButton.Location = new System.Drawing.Point(7, 478);
            this.addVertexButton.Name = "addVertexButton";
            this.addVertexButton.Size = new System.Drawing.Size(226, 23);
            this.addVertexButton.TabIndex = 18;
            this.addVertexButton.Text = "Add selected Vertex";
            this.addVertexButton.UseVisualStyleBackColor = true;
            this.addVertexButton.Click += new System.EventHandler(this.AddVertexButton_Click);
            // 
            // xNumericUpDown
            // 
            this.xNumericUpDown.DecimalPlaces = 15;
            this.xNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.xNumericUpDown.Location = new System.Drawing.Point(29, 400);
            this.xNumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.xNumericUpDown.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.xNumericUpDown.Name = "xNumericUpDown";
            this.xNumericUpDown.Size = new System.Drawing.Size(204, 20);
            this.xNumericUpDown.TabIndex = 17;
            this.xNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // zNumericUpDown
            // 
            this.zNumericUpDown.DecimalPlaces = 15;
            this.zNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.zNumericUpDown.Location = new System.Drawing.Point(29, 452);
            this.zNumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.zNumericUpDown.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.zNumericUpDown.Name = "zNumericUpDown";
            this.zNumericUpDown.Size = new System.Drawing.Size(204, 20);
            this.zNumericUpDown.TabIndex = 16;
            this.zNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // yNumericUpDown
            // 
            this.yNumericUpDown.DecimalPlaces = 15;
            this.yNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.yNumericUpDown.Location = new System.Drawing.Point(29, 426);
            this.yNumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.yNumericUpDown.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.yNumericUpDown.Name = "yNumericUpDown";
            this.yNumericUpDown.Size = new System.Drawing.Size(204, 20);
            this.yNumericUpDown.TabIndex = 15;
            this.yNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // addTriangleButton
            // 
            this.addTriangleButton.Location = new System.Drawing.Point(6, 281);
            this.addTriangleButton.Name = "addTriangleButton";
            this.addTriangleButton.Size = new System.Drawing.Size(227, 23);
            this.addTriangleButton.TabIndex = 14;
            this.addTriangleButton.Text = "Add selected Triangle";
            this.addTriangleButton.UseVisualStyleBackColor = true;
            this.addTriangleButton.Click += new System.EventHandler(this.AddTriangleButton_Click);
            // 
            // removeSinglesButton
            // 
            this.removeSinglesButton.Location = new System.Drawing.Point(6, 48);
            this.removeSinglesButton.Name = "removeSinglesButton";
            this.removeSinglesButton.Size = new System.Drawing.Size(227, 23);
            this.removeSinglesButton.TabIndex = 6;
            this.removeSinglesButton.Text = "Remove Single Vertices";
            this.removeSinglesButton.UseVisualStyleBackColor = true;
            this.removeSinglesButton.Click += new System.EventHandler(this.RemoveSinglesButton_Click);
            // 
            // removeTriangleButton
            // 
            this.removeTriangleButton.Location = new System.Drawing.Point(6, 252);
            this.removeTriangleButton.Name = "removeTriangleButton";
            this.removeTriangleButton.Size = new System.Drawing.Size(227, 23);
            this.removeTriangleButton.TabIndex = 13;
            this.removeTriangleButton.Text = "Remove selected Triangle";
            this.removeTriangleButton.UseVisualStyleBackColor = true;
            this.removeTriangleButton.Click += new System.EventHandler(this.RemoveTriangleButton_Click);
            // 
            // removeDoubleVertButton
            // 
            this.removeDoubleVertButton.Location = new System.Drawing.Point(6, 77);
            this.removeDoubleVertButton.Name = "removeDoubleVertButton";
            this.removeDoubleVertButton.Size = new System.Drawing.Size(227, 23);
            this.removeDoubleVertButton.TabIndex = 8;
            this.removeDoubleVertButton.Text = "Remove Double Vertices";
            this.removeDoubleVertButton.UseVisualStyleBackColor = true;
            this.removeDoubleVertButton.Click += new System.EventHandler(this.RemoveDoubleVertButton_Click);
            // 
            // cNumericUpDown
            // 
            this.cNumericUpDown.Location = new System.Drawing.Point(168, 222);
            this.cNumericUpDown.Name = "cNumericUpDown";
            this.cNumericUpDown.Size = new System.Drawing.Size(65, 20);
            this.cNumericUpDown.TabIndex = 11;
            this.cNumericUpDown.ThousandsSeparator = true;
            // 
            // bNumericUpDown
            // 
            this.bNumericUpDown.Location = new System.Drawing.Point(87, 222);
            this.bNumericUpDown.Name = "bNumericUpDown";
            this.bNumericUpDown.Size = new System.Drawing.Size(65, 20);
            this.bNumericUpDown.TabIndex = 12;
            this.bNumericUpDown.ThousandsSeparator = true;
            // 
            // removeColinButton
            // 
            this.removeColinButton.Location = new System.Drawing.Point(6, 19);
            this.removeColinButton.Name = "removeColinButton";
            this.removeColinButton.Size = new System.Drawing.Size(227, 23);
            this.removeColinButton.TabIndex = 7;
            this.removeColinButton.Text = "Remove Triangles with colinear Vertices";
            this.removeColinButton.UseVisualStyleBackColor = true;
            this.removeColinButton.Click += new System.EventHandler(this.RemoveColinButton_Click);
            // 
            // removeDoubleButton
            // 
            this.removeDoubleButton.Location = new System.Drawing.Point(6, 106);
            this.removeDoubleButton.Name = "removeDoubleButton";
            this.removeDoubleButton.Size = new System.Drawing.Size(227, 23);
            this.removeDoubleButton.TabIndex = 2;
            this.removeDoubleButton.Text = "Remove Double Triangles";
            this.removeDoubleButton.UseVisualStyleBackColor = true;
            this.removeDoubleButton.Click += new System.EventHandler(this.RemoveDoubleButton_Click);
            // 
            // aNumericUpDown
            // 
            this.aNumericUpDown.Location = new System.Drawing.Point(7, 222);
            this.aNumericUpDown.Name = "aNumericUpDown";
            this.aNumericUpDown.Size = new System.Drawing.Size(65, 20);
            this.aNumericUpDown.TabIndex = 10;
            this.aNumericUpDown.ThousandsSeparator = true;
            // 
            // remove2NVerticesButton
            // 
            this.remove2NVerticesButton.Location = new System.Drawing.Point(6, 135);
            this.remove2NVerticesButton.Name = "remove2NVerticesButton";
            this.remove2NVerticesButton.Size = new System.Drawing.Size(227, 23);
            this.remove2NVerticesButton.TabIndex = 3;
            this.remove2NVerticesButton.Text = "Remove Vertices with only two neighbors";
            this.remove2NVerticesButton.UseVisualStyleBackColor = true;
            this.remove2NVerticesButton.Click += new System.EventHandler(this.Remove2NVerticesButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.showViewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(262, 24);
            this.menuStrip1.TabIndex = 16;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Enabled = false;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.closeToolStripMenuItem.Text = "&Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.CloseFile);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(108, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // showViewToolStripMenuItem
            // 
            this.showViewToolStripMenuItem.Enabled = false;
            this.showViewToolStripMenuItem.Name = "showViewToolStripMenuItem";
            this.showViewToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.showViewToolStripMenuItem.Text = "Show &View";
            this.showViewToolStripMenuItem.Click += new System.EventHandler(this.ShowViewToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manualToolStripMenuItem,
            this.infoToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // manualToolStripMenuItem
            // 
            this.manualToolStripMenuItem.Name = "manualToolStripMenuItem";
            this.manualToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.manualToolStripMenuItem.Text = "&Manual";
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.infoToolStripMenuItem.Text = "&Info";
            // 
            // triangleLabel
            // 
            this.triangleLabel.AutoSize = true;
            this.triangleLabel.Location = new System.Drawing.Point(181, 26);
            this.triangleLabel.Name = "triangleLabel";
            this.triangleLabel.Size = new System.Drawing.Size(55, 13);
            this.triangleLabel.TabIndex = 21;
            this.triangleLabel.Text = "10000000";
            // 
            // edgeLabel
            // 
            this.edgeLabel.AutoSize = true;
            this.edgeLabel.Location = new System.Drawing.Point(55, 46);
            this.edgeLabel.Name = "edgeLabel";
            this.edgeLabel.Size = new System.Drawing.Size(55, 13);
            this.edgeLabel.TabIndex = 20;
            this.edgeLabel.Text = "10000000";
            // 
            // vertexLabel
            // 
            this.vertexLabel.AutoSize = true;
            this.vertexLabel.Location = new System.Drawing.Point(55, 26);
            this.vertexLabel.Name = "vertexLabel";
            this.vertexLabel.Size = new System.Drawing.Size(55, 13);
            this.vertexLabel.TabIndex = 19;
            this.vertexLabel.Text = "10000000";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(126, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Triangles:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Edges:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Vertices:";
            // 
            // meshGroupBox
            // 
            this.meshGroupBox.Controls.Add(this.vertexLabel);
            this.meshGroupBox.Controls.Add(this.edgeLabel);
            this.meshGroupBox.Controls.Add(this.label5);
            this.meshGroupBox.Controls.Add(this.triangleLabel);
            this.meshGroupBox.Controls.Add(this.label4);
            this.meshGroupBox.Controls.Add(this.label6);
            this.meshGroupBox.Location = new System.Drawing.Point(12, 27);
            this.meshGroupBox.Name = "meshGroupBox";
            this.meshGroupBox.Size = new System.Drawing.Size(239, 71);
            this.meshGroupBox.TabIndex = 22;
            this.meshGroupBox.TabStop = false;
            this.meshGroupBox.Text = "Mesh Information";
            this.meshGroupBox.Visible = false;
            // 
            // transposeVertexButton
            // 
            this.transposeVertexButton.Location = new System.Drawing.Point(7, 368);
            this.transposeVertexButton.Name = "transposeVertexButton";
            this.transposeVertexButton.Size = new System.Drawing.Size(226, 23);
            this.transposeVertexButton.TabIndex = 29;
            this.transposeVertexButton.Text = "Transpose Observed Vertex By";
            this.transposeVertexButton.UseVisualStyleBackColor = true;
            this.transposeVertexButton.Click += new System.EventHandler(this.TransposeVertexButton_Click);
            // 
            // TriMMForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 680);
            this.Controls.Add(this.meshGroupBox);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.manipulationGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "TriMMForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "TriMM";
            this.manipulationGroupBox.ResumeLayout(false);
            this.manipulationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.e1NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.e2NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aNumericUpDown)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.meshGroupBox.ResumeLayout(false);
            this.meshGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox manipulationGroupBox;
        private System.Windows.Forms.Button flipButton;
        private System.Windows.Forms.NumericUpDown e1NumericUpDown;
        private System.Windows.Forms.NumericUpDown e2NumericUpDown;
        private System.Windows.Forms.Button moveObservedButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button removeObservedButton;
        private System.Windows.Forms.Button addVertexButton;
        private System.Windows.Forms.NumericUpDown xNumericUpDown;
        private System.Windows.Forms.NumericUpDown zNumericUpDown;
        private System.Windows.Forms.NumericUpDown yNumericUpDown;
        private System.Windows.Forms.Button addTriangleButton;
        private System.Windows.Forms.Button removeSinglesButton;
        private System.Windows.Forms.Button removeTriangleButton;
        private System.Windows.Forms.Button removeDoubleVertButton;
        private System.Windows.Forms.NumericUpDown cNumericUpDown;
        private System.Windows.Forms.NumericUpDown bNumericUpDown;
        private System.Windows.Forms.Button removeColinButton;
        private System.Windows.Forms.Button removeDoubleButton;
        private System.Windows.Forms.NumericUpDown aNumericUpDown;
        private System.Windows.Forms.Button remove2NVerticesButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showViewToolStripMenuItem;
        private System.Windows.Forms.Label triangleLabel;
        private System.Windows.Forms.Label edgeLabel;
        private System.Windows.Forms.Label vertexLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox meshGroupBox;
        private System.Windows.Forms.Button flipObservedTriangleButton;
        private System.Windows.Forms.Button flipAllTrianglesButton;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manualToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private System.Windows.Forms.Button transposeVertexButton;
    }
}

