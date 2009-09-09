namespace Blaze.Automation
{
    partial class AssistantModifyWindow
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
            ActionsView.ItemSelectionChanged -= ActionsView_ItemSelectionChanged;
            ActionsView.MouseClick -= ActionsView_MouseClick;
            ActionsView.MouseDoubleClick -= ActionsView_MouseDoubleClick;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssistantModifyWindow));
            this.CancelButton = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.ActionsView = new System.Windows.Forms.ListView();
            this.LargeImageList = new System.Windows.Forms.ImageList(this.components);
            this.SmallImageList = new System.Windows.Forms.ImageList(this.components);
            this.ActionsGroupBox = new System.Windows.Forms.GroupBox();
            this.MoveDownButton = new System.Windows.Forms.Button();
            this.RemoveActionButton = new System.Windows.Forms.Button();
            this.MoveUpButton = new System.Windows.Forms.Button();
            this.AddActionButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.AutomationPreviewGroupBox = new System.Windows.Forms.GroupBox();
            this.AutomationPreviewBox = new System.Windows.Forms.RichTextBox();
            this.ActionToolStrip = new System.Windows.Forms.ToolStrip();
            this.AlternativesLabel = new System.Windows.Forms.ToolStripLabel();
            this.PreviousAlternativeButton = new System.Windows.Forms.ToolStripButton();
            this.AlternativesDisplay = new System.Windows.Forms.ToolStripTextBox();
            this.NextAlternativeButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.EditAlternativeButton = new System.Windows.Forms.ToolStripButton();
            this.ActionDescriptionGroupBox = new System.Windows.Forms.GroupBox();
            this.ActionDescriptionBox = new System.Windows.Forms.RichTextBox();
            this.ItemContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.clearAllToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ActionsGroupBox.SuspendLayout();
            this.AutomationPreviewGroupBox.SuspendLayout();
            this.ActionToolStrip.SuspendLayout();
            this.ActionDescriptionGroupBox.SuspendLayout();
            this.ItemContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelButton.Location = new System.Drawing.Point(487, 457);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 1;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // OkButton
            // 
            this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OkButton.Location = new System.Drawing.Point(406, 457);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 2;
            this.OkButton.Text = "Ok";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // ActionsView
            // 
            this.ActionsView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ActionsView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActionsView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.ActionsView.HideSelection = false;
            this.ActionsView.LargeImageList = this.LargeImageList;
            this.ActionsView.Location = new System.Drawing.Point(6, 19);
            this.ActionsView.MultiSelect = false;
            this.ActionsView.Name = "ActionsView";
            this.ActionsView.ShowItemToolTips = true;
            this.ActionsView.Size = new System.Drawing.Size(117, 413);
            this.ActionsView.SmallImageList = this.SmallImageList;
            this.ActionsView.TabIndex = 3;
            this.ActionsView.UseCompatibleStateImageBehavior = false;
            this.ActionsView.View = System.Windows.Forms.View.Tile;
            // 
            // LargeImageList
            // 
            this.LargeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("LargeImageList.ImageStream")));
            this.LargeImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.LargeImageList.Images.SetKeyName(0, "filesystem.ico");
            this.LargeImageList.Images.SetKeyName(1, "keyboard.ico");
            this.LargeImageList.Images.SetKeyName(2, "mouse.ico");
            this.LargeImageList.Images.SetKeyName(3, "text.ico");
            // 
            // SmallImageList
            // 
            this.SmallImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("SmallImageList.ImageStream")));
            this.SmallImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.SmallImageList.Images.SetKeyName(0, "filesystem-small.ico");
            this.SmallImageList.Images.SetKeyName(1, "keyboard-small.ico");
            this.SmallImageList.Images.SetKeyName(2, "mouse-small.ico");
            this.SmallImageList.Images.SetKeyName(3, "text-small.ico");
            // 
            // ActionsGroupBox
            // 
            this.ActionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.ActionsGroupBox.Controls.Add(this.MoveDownButton);
            this.ActionsGroupBox.Controls.Add(this.RemoveActionButton);
            this.ActionsGroupBox.Controls.Add(this.MoveUpButton);
            this.ActionsGroupBox.Controls.Add(this.ActionsView);
            this.ActionsGroupBox.Controls.Add(this.AddActionButton);
            this.ActionsGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActionsGroupBox.Location = new System.Drawing.Point(12, 12);
            this.ActionsGroupBox.Name = "ActionsGroupBox";
            this.ActionsGroupBox.Size = new System.Drawing.Size(129, 468);
            this.ActionsGroupBox.TabIndex = 4;
            this.ActionsGroupBox.TabStop = false;
            this.ActionsGroupBox.Text = "Actions";
            // 
            // MoveDownButton
            // 
            this.MoveDownButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MoveDownButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MoveDownButton.BackgroundImage")));
            this.MoveDownButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MoveDownButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MoveDownButton.Location = new System.Drawing.Point(68, 438);
            this.MoveDownButton.Name = "MoveDownButton";
            this.MoveDownButton.Size = new System.Drawing.Size(24, 23);
            this.MoveDownButton.TabIndex = 2;
            this.MoveDownButton.UseVisualStyleBackColor = true;
            this.MoveDownButton.Click += new System.EventHandler(this.MoveDownButton_Click);
            // 
            // RemoveActionButton
            // 
            this.RemoveActionButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoveActionButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RemoveActionButton.Location = new System.Drawing.Point(37, 438);
            this.RemoveActionButton.Name = "RemoveActionButton";
            this.RemoveActionButton.Size = new System.Drawing.Size(24, 23);
            this.RemoveActionButton.TabIndex = 10;
            this.RemoveActionButton.Text = "-";
            this.RemoveActionButton.UseVisualStyleBackColor = true;
            this.RemoveActionButton.Click += new System.EventHandler(this.RemoveActionButton_Click);
            // 
            // MoveUpButton
            // 
            this.MoveUpButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MoveUpButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MoveUpButton.BackgroundImage")));
            this.MoveUpButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MoveUpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MoveUpButton.Location = new System.Drawing.Point(99, 438);
            this.MoveUpButton.Name = "MoveUpButton";
            this.MoveUpButton.Size = new System.Drawing.Size(24, 23);
            this.MoveUpButton.TabIndex = 1;
            this.MoveUpButton.UseVisualStyleBackColor = true;
            this.MoveUpButton.Click += new System.EventHandler(this.MoveUpButton_Click);
            // 
            // AddActionButton
            // 
            this.AddActionButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.AddActionButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddActionButton.Location = new System.Drawing.Point(6, 438);
            this.AddActionButton.Name = "AddActionButton";
            this.AddActionButton.Size = new System.Drawing.Size(24, 23);
            this.AddActionButton.TabIndex = 9;
            this.AddActionButton.Text = "+";
            this.AddActionButton.UseVisualStyleBackColor = true;
            this.AddActionButton.Click += new System.EventHandler(this.AddActionButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ResetButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResetButton.Location = new System.Drawing.Point(306, 457);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(75, 23);
            this.ResetButton.TabIndex = 5;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // AutomationPreviewGroupBox
            // 
            this.AutomationPreviewGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.AutomationPreviewGroupBox.Controls.Add(this.AutomationPreviewBox);
            this.AutomationPreviewGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AutomationPreviewGroupBox.Location = new System.Drawing.Point(147, 161);
            this.AutomationPreviewGroupBox.Name = "AutomationPreviewGroupBox";
            this.AutomationPreviewGroupBox.Size = new System.Drawing.Size(415, 290);
            this.AutomationPreviewGroupBox.TabIndex = 6;
            this.AutomationPreviewGroupBox.TabStop = false;
            this.AutomationPreviewGroupBox.Text = "Automation Preview";
            // 
            // AutomationPreviewBox
            // 
            this.AutomationPreviewBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.AutomationPreviewBox.BackColor = System.Drawing.SystemColors.Control;
            this.AutomationPreviewBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.AutomationPreviewBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AutomationPreviewBox.Location = new System.Drawing.Point(8, 19);
            this.AutomationPreviewBox.Name = "AutomationPreviewBox";
            this.AutomationPreviewBox.ReadOnly = true;
            this.AutomationPreviewBox.Size = new System.Drawing.Size(401, 265);
            this.AutomationPreviewBox.TabIndex = 2;
            this.AutomationPreviewBox.Text = "Sorry but no repetitions were detected yet.";
            // 
            // ActionToolStrip
            // 
            this.ActionToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ActionToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ActionToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AlternativesLabel,
            this.PreviousAlternativeButton,
            this.AlternativesDisplay,
            this.NextAlternativeButton,
            this.toolStripSeparator1,
            this.EditAlternativeButton});
            this.ActionToolStrip.Location = new System.Drawing.Point(3, 115);
            this.ActionToolStrip.Name = "ActionToolStrip";
            this.ActionToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.ActionToolStrip.ShowItemToolTips = false;
            this.ActionToolStrip.Size = new System.Drawing.Size(409, 25);
            this.ActionToolStrip.TabIndex = 0;
            this.ActionToolStrip.Text = "ToolStrip";
            // 
            // AlternativesLabel
            // 
            this.AlternativesLabel.Name = "AlternativesLabel";
            this.AlternativesLabel.Size = new System.Drawing.Size(72, 22);
            this.AlternativesLabel.Text = "Alternatives:";
            // 
            // PreviousAlternativeButton
            // 
            this.PreviousAlternativeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.PreviousAlternativeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PreviousAlternativeButton.Image = ((System.Drawing.Image)(resources.GetObject("PreviousAlternativeButton.Image")));
            this.PreviousAlternativeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PreviousAlternativeButton.Name = "PreviousAlternativeButton";
            this.PreviousAlternativeButton.Size = new System.Drawing.Size(23, 22);
            this.PreviousAlternativeButton.Text = "<";
            this.PreviousAlternativeButton.Click += new System.EventHandler(this.PreviousAlternativeButton_Click);
            // 
            // AlternativesDisplay
            // 
            this.AlternativesDisplay.BackColor = System.Drawing.SystemColors.Window;
            this.AlternativesDisplay.Name = "AlternativesDisplay";
            this.AlternativesDisplay.ReadOnly = true;
            this.AlternativesDisplay.Size = new System.Drawing.Size(35, 25);
            this.AlternativesDisplay.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // NextAlternativeButton
            // 
            this.NextAlternativeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.NextAlternativeButton.Image = ((System.Drawing.Image)(resources.GetObject("NextAlternativeButton.Image")));
            this.NextAlternativeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NextAlternativeButton.Name = "NextAlternativeButton";
            this.NextAlternativeButton.Size = new System.Drawing.Size(23, 22);
            this.NextAlternativeButton.Text = ">";
            this.NextAlternativeButton.Click += new System.EventHandler(this.NextAlternativeButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // EditAlternativeButton
            // 
            this.EditAlternativeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.EditAlternativeButton.Image = ((System.Drawing.Image)(resources.GetObject("EditAlternativeButton.Image")));
            this.EditAlternativeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EditAlternativeButton.Name = "EditAlternativeButton";
            this.EditAlternativeButton.Size = new System.Drawing.Size(31, 22);
            this.EditAlternativeButton.Text = "Edit";
            this.EditAlternativeButton.Click += new System.EventHandler(this.EditAlternativeButton_Click);
            // 
            // ActionDescriptionGroupBox
            // 
            this.ActionDescriptionGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ActionDescriptionGroupBox.Controls.Add(this.ActionDescriptionBox);
            this.ActionDescriptionGroupBox.Controls.Add(this.ActionToolStrip);
            this.ActionDescriptionGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActionDescriptionGroupBox.Location = new System.Drawing.Point(147, 12);
            this.ActionDescriptionGroupBox.Name = "ActionDescriptionGroupBox";
            this.ActionDescriptionGroupBox.Size = new System.Drawing.Size(415, 143);
            this.ActionDescriptionGroupBox.TabIndex = 8;
            this.ActionDescriptionGroupBox.TabStop = false;
            this.ActionDescriptionGroupBox.Text = "Action Description";
            // 
            // ActionDescriptionBox
            // 
            this.ActionDescriptionBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ActionDescriptionBox.BackColor = System.Drawing.SystemColors.Control;
            this.ActionDescriptionBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ActionDescriptionBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActionDescriptionBox.Location = new System.Drawing.Point(8, 19);
            this.ActionDescriptionBox.Name = "ActionDescriptionBox";
            this.ActionDescriptionBox.ReadOnly = true;
            this.ActionDescriptionBox.Size = new System.Drawing.Size(401, 93);
            this.ActionDescriptionBox.TabIndex = 3;
            this.ActionDescriptionBox.Text = "No action selected.";
            // 
            // ItemContextMenu
            // 
            this.ItemContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripSeparator2,
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem,
            this.toolStripSeparator4,
            this.clearAllToolStripMenuItem1});
            this.ItemContextMenu.Name = "ItemContextMenu";
            this.ItemContextMenu.Size = new System.Drawing.Size(136, 126);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(132, 6);
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.moveUpToolStripMenuItem.Text = "MoveUp";
            this.moveUpToolStripMenuItem.Click += new System.EventHandler(this.moveUpToolStripMenuItem_Click);
            // 
            // moveDownToolStripMenuItem
            // 
            this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.moveDownToolStripMenuItem.Text = "MoveDown";
            this.moveDownToolStripMenuItem.Click += new System.EventHandler(this.moveDownToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(132, 6);
            // 
            // clearAllToolStripMenuItem1
            // 
            this.clearAllToolStripMenuItem1.Name = "clearAllToolStripMenuItem1";
            this.clearAllToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
            this.clearAllToolStripMenuItem1.Text = "Clear All";
            this.clearAllToolStripMenuItem1.Click += new System.EventHandler(this.clearAllToolStripMenuItem1_Click);
            // 
            // AssistantModifyWindow
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(569, 494);
            this.Controls.Add(this.ActionDescriptionGroupBox);
            this.Controls.Add(this.AutomationPreviewGroupBox);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.ActionsGroupBox);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.CancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(426, 322);
            this.Name = "AssistantModifyWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Automation Editor";
            this.Load += new System.EventHandler(this.AssistantModifyWindow_Load);
            this.ActionsGroupBox.ResumeLayout(false);
            this.AutomationPreviewGroupBox.ResumeLayout(false);
            this.ActionToolStrip.ResumeLayout(false);
            this.ActionToolStrip.PerformLayout();
            this.ActionDescriptionGroupBox.ResumeLayout(false);
            this.ActionDescriptionGroupBox.PerformLayout();
            this.ItemContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.ListView ActionsView;
        private System.Windows.Forms.ImageList LargeImageList;
        private System.Windows.Forms.GroupBox ActionsGroupBox;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.GroupBox AutomationPreviewGroupBox;
        private System.Windows.Forms.ToolStrip ActionToolStrip;
        private System.Windows.Forms.GroupBox ActionDescriptionGroupBox;
        private System.Windows.Forms.Button AddActionButton;
        private System.Windows.Forms.Button RemoveActionButton;
        private System.Windows.Forms.RichTextBox AutomationPreviewBox;
        private System.Windows.Forms.Button MoveDownButton;
        private System.Windows.Forms.Button MoveUpButton;
        private System.Windows.Forms.RichTextBox ActionDescriptionBox;
        private System.Windows.Forms.ImageList SmallImageList;
        private System.Windows.Forms.ToolStripButton PreviousAlternativeButton;
        private System.Windows.Forms.ToolStripLabel AlternativesLabel;
        private System.Windows.Forms.ToolStripTextBox AlternativesDisplay;
        private System.Windows.Forms.ToolStripButton NextAlternativeButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton EditAlternativeButton;
        private System.Windows.Forms.ContextMenuStrip ItemContextMenu;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem moveUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveDownToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem clearAllToolStripMenuItem1;
    }
}