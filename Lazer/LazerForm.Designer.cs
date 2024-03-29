﻿namespace Lazer
{
    partial class LazerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LazerForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSolve = new System.Windows.Forms.Button();
            this.listBoxBoards = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnClear = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radio_select = new System.Windows.Forms.RadioButton();
            this.radio_place = new System.Windows.Forms.RadioButton();
            this.btnRemove = new System.Windows.Forms.Button();
            this.listBlockView = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxLevel = new System.Windows.Forms.TextBox();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.fileSystemWatcher = new System.IO.FileSystemWatcher();
            this.controlBoard = new Lazer.ControlBoard();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.controlBoard)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(16, 17);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(255, 495);
            this.tabControl1.TabIndex = 3;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.TabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnLoad);
            this.tabPage1.Controls.Add(this.btnSolve);
            this.tabPage1.Controls.Add(this.listBoxBoards);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Size = new System.Drawing.Size(247, 462);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Level";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoad.Location = new System.Drawing.Point(8, 417);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(109, 35);
            this.btnLoad.TabIndex = 6;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // btnSolve
            // 
            this.btnSolve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSolve.Location = new System.Drawing.Point(130, 417);
            this.btnSolve.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSolve.Name = "btnSolve";
            this.btnSolve.Size = new System.Drawing.Size(109, 35);
            this.btnSolve.TabIndex = 5;
            this.btnSolve.Text = "Solve";
            this.toolTips.SetToolTip(this.btnSolve, "Right click a block to mark as fixed.");
            this.btnSolve.UseVisualStyleBackColor = true;
            this.btnSolve.Click += new System.EventHandler(this.BtnSolve_Click);
            // 
            // listBoxBoards
            // 
            this.listBoxBoards.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.listBoxBoards.FormattingEnabled = true;
            this.listBoxBoards.ItemHeight = 20;
            this.listBoxBoards.Location = new System.Drawing.Point(8, 9);
            this.listBoxBoards.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listBoxBoards.Name = "listBoxBoards";
            this.listBoxBoards.Size = new System.Drawing.Size(227, 384);
            this.listBoxBoards.Sorted = true;
            this.listBoxBoards.TabIndex = 3;
            this.listBoxBoards.Click += new System.EventHandler(this.ListBoxBoards_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnClear);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Controls.Add(this.btnRemove);
            this.tabPage2.Controls.Add(this.listBlockView);
            this.tabPage2.Controls.Add(this.propertyGrid1);
            this.tabPage2.Controls.Add(this.btnSave);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.textBoxLevel);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Size = new System.Drawing.Size(247, 505);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Editor";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(165, 51);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(71, 35);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radio_select);
            this.panel1.Controls.Add(this.radio_place);
            this.panel1.Location = new System.Drawing.Point(12, 95);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(227, 23);
            this.panel1.TabIndex = 5;
            // 
            // radio_select
            // 
            this.radio_select.AutoSize = true;
            this.radio_select.Checked = true;
            this.radio_select.Location = new System.Drawing.Point(0, 0);
            this.radio_select.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radio_select.Name = "radio_select";
            this.radio_select.Size = new System.Drawing.Size(121, 24);
            this.radio_select.TabIndex = 9;
            this.radio_select.TabStop = true;
            this.radio_select.Text = "Select / Move";
            this.radio_select.UseVisualStyleBackColor = true;
            this.radio_select.CheckedChanged += new System.EventHandler(this.Radio_CheckedChanged);
            // 
            // radio_place
            // 
            this.radio_place.AutoSize = true;
            this.radio_place.Location = new System.Drawing.Point(132, 0);
            this.radio_place.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radio_place.Name = "radio_place";
            this.radio_place.Size = new System.Drawing.Size(65, 24);
            this.radio_place.TabIndex = 10;
            this.radio_place.Text = "Place";
            this.radio_place.UseVisualStyleBackColor = true;
            this.radio_place.CheckedChanged += new System.EventHandler(this.Radio_CheckedChanged);
            // 
            // btnRemove
            // 
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(83, 51);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(73, 35);
            this.btnRemove.TabIndex = 8;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.BtnRemove_Click);
            // 
            // listBlockView
            // 
            this.listBlockView.Enabled = false;
            this.listBlockView.HideSelection = false;
            this.listBlockView.Location = new System.Drawing.Point(9, 128);
            this.listBlockView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listBlockView.Name = "listBlockView";
            this.listBlockView.Size = new System.Drawing.Size(225, 158);
            this.listBlockView.SmallImageList = this.imageList1;
            this.listBlockView.TabIndex = 7;
            this.listBlockView.UseCompatibleStateImageBehavior = false;
            this.listBlockView.View = System.Windows.Forms.View.SmallIcon;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.propertyGrid1.Location = new System.Drawing.Point(8, 296);
            this.propertyGrid1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(228, 199);
            this.propertyGrid1.TabIndex = 6;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PropertyGrid1_PropertyValueChanged);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(9, 51);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(65, 35);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Level";
            // 
            // textBoxLevel
            // 
            this.textBoxLevel.Location = new System.Drawing.Point(60, 9);
            this.textBoxLevel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxLevel.Name = "textBoxLevel";
            this.textBoxLevel.Size = new System.Drawing.Size(175, 27);
            this.textBoxLevel.TabIndex = 0;
            // 
            // fileSystemWatcher
            // 
            this.fileSystemWatcher.EnableRaisingEvents = true;
            this.fileSystemWatcher.Filter = "*.xml";
            this.fileSystemWatcher.NotifyFilter = System.IO.NotifyFilters.LastWrite;
            this.fileSystemWatcher.Path = ".";
            this.fileSystemWatcher.SynchronizingObject = this;
            this.fileSystemWatcher.Changed += new System.IO.FileSystemEventHandler(this.FileSystemWatcher_Changed);
            // 
            // controlBoard
            // 
            this.controlBoard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controlBoard.IsEditing = false;
            this.controlBoard.Location = new System.Drawing.Point(279, 18);
            this.controlBoard.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.controlBoard.Name = "controlBoard";
            this.controlBoard.OverPoint = new System.Drawing.Point(0, 0);
            this.controlBoard.Size = new System.Drawing.Size(494, 494);
            this.controlBoard.TabIndex = 4;
            this.controlBoard.TabStop = false;
            // 
            // LazerForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(790, 523);
            this.Controls.Add(this.controlBoard);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "LazerForm";
            this.Text = "Lazer";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.controlBoard)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSolve;
        private System.Windows.Forms.ListBox listBoxBoards;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxLevel;
        private ControlBoard controlBoard;
        private System.Windows.Forms.ListView listBlockView;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.ToolTip toolTips;
        private System.IO.FileSystemWatcher fileSystemWatcher;
        private System.Windows.Forms.RadioButton radio_place;
        private System.Windows.Forms.RadioButton radio_select;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClear;
    }
}

