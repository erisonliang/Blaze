namespace Blaze
{
    partial class MainForm
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
            this.FormClosing -= MainForm_FormClosing;
            this.VisibleChanged -= MainForm_VisibleChanged;
            this.Shown -= MainForm_Shown;
            TextInput.KeyDown -= TextInput_KeyDown;
            TextInput.KeyPress -= TextInput_KeyPress;
            NotifyIcon.DoubleClick -= NotifyIcon_DoubleClick;
            CustomLabel.Paint -= CustomLabel_Paint;
            TextInput.TextChanged -= TextInput_TextChanged;
            MouseUp -= MainForm_MouseUp;
            MouseDown -= MainForm_MouseDown;
            MouseMove -= MainForm_MouseMove;
            IconBox.MouseUp -= MainForm_MouseUp;
            IconBox.MouseDown -= MainForm_MouseDown;
            IconBox.MouseMove -= MainForm_MouseMove;
            NameDisplay.MouseUp -= MainForm_MouseUp;
            NameDisplay.MouseDown -= MainForm_MouseDown;
            NameDisplay.MouseMove -= MainForm_MouseMove;
            CustomLabel.MouseUp -= MainForm_MouseUp;
            CustomLabel.MouseDown -= MainForm_MouseDown;
            CustomLabel.MouseMove -= MainForm_MouseMove;
            Gma.UserActivityMonitor.HookManager.KeyDown -= HookManager_KeyDown;
            ContextLib.UserContext.Instance.AssistantObject.NewSuggestion += AssistantObject_NewSuggestion;
            ContextLib.UserContext.Instance.AssistantObject.NoNewSuggestion += AssistantObject_NoNewSuggestion;
            _tooltip.Draw -= _tooltip_Draw;
            _tooltip.Popup -= _tooltip_Popup;
            _tooltip.Dispose();
            //HookManager.KeyDown -= HookManager_KeyDown;
            //HookManager.KeyUp -= HookManager_KeyUp;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.NotifyIconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.restoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.rebuildIndexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.showAssistantWindowMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.showDebugWindowMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.blazeWebpageToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.NotifyIconContextMenu_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.TextInput = new System.Windows.Forms.TextBox();
            this.MainContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.hideToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.rebuildIndexToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.showAssistantWindowMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.showDebugWindowMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.blazeWebpageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.IconBox = new System.Windows.Forms.PictureBox();
            this.NameDisplay = new System.Windows.Forms.Label();
            this.CustomLabel = new System.Windows.Forms.Panel();
            this.NotifyIconContextMenu.SuspendLayout();
            this.MainContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconBox)).BeginInit();
            this.SuspendLayout();
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.ContextMenuStrip = this.NotifyIconContextMenu;
            this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
            this.NotifyIcon.Text = "Blaze";
            this.NotifyIcon.Visible = true;
            // 
            // NotifyIconContextMenu
            // 
            this.NotifyIconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restoreToolStripMenuItem,
            this.toolStripSeparator3,
            this.rebuildIndexToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.toolStripSeparator1,
            this.showAssistantWindowMenuItem2,
            this.showDebugWindowMenuItem2,
            this.toolStripSeparator6,
            this.blazeWebpageToolStripMenuItem1,
            this.helpToolStripMenuItem1,
            this.toolStripSeparator8,
            this.NotifyIconContextMenu_Exit});
            this.NotifyIconContextMenu.Name = "NotifyIconContextMenu";
            this.NotifyIconContextMenu.Size = new System.Drawing.Size(201, 204);
            this.NotifyIconContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.NotifyIconContextMenu_Opening);
            // 
            // restoreToolStripMenuItem
            // 
            this.restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
            this.restoreToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.restoreToolStripMenuItem.Text = "Show";
            this.restoreToolStripMenuItem.Click += new System.EventHandler(this.restoreToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(197, 6);
            // 
            // rebuildIndexToolStripMenuItem
            // 
            this.rebuildIndexToolStripMenuItem.Name = "rebuildIndexToolStripMenuItem";
            this.rebuildIndexToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.rebuildIndexToolStripMenuItem.Text = "Rebuild Index";
            this.rebuildIndexToolStripMenuItem.Click += new System.EventHandler(this.rebuildIndexToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(197, 6);
            // 
            // showAssistantWindowMenuItem2
            // 
            this.showAssistantWindowMenuItem2.CheckOnClick = true;
            this.showAssistantWindowMenuItem2.Name = "showAssistantWindowMenuItem2";
            this.showAssistantWindowMenuItem2.Size = new System.Drawing.Size(200, 22);
            this.showAssistantWindowMenuItem2.Text = "Show Assistant Window";
            this.showAssistantWindowMenuItem2.Click += new System.EventHandler(this.showAssistantWindowMenuItem2_Click);
            // 
            // showDebugWindowMenuItem2
            // 
            this.showDebugWindowMenuItem2.CheckOnClick = true;
            this.showDebugWindowMenuItem2.Name = "showDebugWindowMenuItem2";
            this.showDebugWindowMenuItem2.Size = new System.Drawing.Size(200, 22);
            this.showDebugWindowMenuItem2.Text = "Show Debug Window";
            this.showDebugWindowMenuItem2.Click += new System.EventHandler(this.showDebugWindowMenuItem2_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(197, 6);
            // 
            // blazeWebpageToolStripMenuItem1
            // 
            this.blazeWebpageToolStripMenuItem1.Name = "blazeWebpageToolStripMenuItem1";
            this.blazeWebpageToolStripMenuItem1.Size = new System.Drawing.Size(200, 22);
            this.blazeWebpageToolStripMenuItem1.Text = "Blaze Webpage";
            this.blazeWebpageToolStripMenuItem1.Click += new System.EventHandler(this.blazeWebpageToolStripMenuItem1_Click);
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(200, 22);
            this.helpToolStripMenuItem1.Text = "Help";
            this.helpToolStripMenuItem1.Click += new System.EventHandler(this.helpToolStripMenuItem1_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(197, 6);
            // 
            // NotifyIconContextMenu_Exit
            // 
            this.NotifyIconContextMenu_Exit.Name = "NotifyIconContextMenu_Exit";
            this.NotifyIconContextMenu_Exit.Size = new System.Drawing.Size(200, 22);
            this.NotifyIconContextMenu_Exit.Text = "Exit";
            this.NotifyIconContextMenu_Exit.Click += new System.EventHandler(this.NotifyIconContextMenu_Exit_Click);
            // 
            // TextInput
            // 
            this.TextInput.BackColor = System.Drawing.Color.PeachPuff;
            this.TextInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextInput.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextInput.ForeColor = System.Drawing.Color.Black;
            this.TextInput.Location = new System.Drawing.Point(85, 64);
            this.TextInput.Name = "TextInput";
            this.TextInput.Size = new System.Drawing.Size(287, 20);
            this.TextInput.TabIndex = 0;
            // 
            // MainContextMenu
            // 
            this.MainContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hideToolStripMenuItem1,
            this.toolStripSeparator4,
            this.rebuildIndexToolStripMenuItem1,
            this.settingsToolStripMenuItem1,
            this.toolStripSeparator2,
            this.showAssistantWindowMenuItem1,
            this.showDebugWindowMenuItem1,
            this.toolStripSeparator5,
            this.blazeWebpageToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.toolStripSeparator7,
            this.exitToolStripMenuItem});
            this.MainContextMenu.Name = "MainContextMenu";
            this.MainContextMenu.Size = new System.Drawing.Size(201, 226);
            // 
            // hideToolStripMenuItem1
            // 
            this.hideToolStripMenuItem1.Name = "hideToolStripMenuItem1";
            this.hideToolStripMenuItem1.Size = new System.Drawing.Size(200, 22);
            this.hideToolStripMenuItem1.Text = "Hide";
            this.hideToolStripMenuItem1.Click += new System.EventHandler(this.hideToolStripMenuItem1_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(197, 6);
            // 
            // rebuildIndexToolStripMenuItem1
            // 
            this.rebuildIndexToolStripMenuItem1.Name = "rebuildIndexToolStripMenuItem1";
            this.rebuildIndexToolStripMenuItem1.Size = new System.Drawing.Size(200, 22);
            this.rebuildIndexToolStripMenuItem1.Text = "Rebuild Index";
            this.rebuildIndexToolStripMenuItem1.Click += new System.EventHandler(this.rebuildIndexToolStripMenuItem1_Click);
            // 
            // settingsToolStripMenuItem1
            // 
            this.settingsToolStripMenuItem1.Name = "settingsToolStripMenuItem1";
            this.settingsToolStripMenuItem1.Size = new System.Drawing.Size(200, 22);
            this.settingsToolStripMenuItem1.Text = "Settings";
            this.settingsToolStripMenuItem1.Click += new System.EventHandler(this.settingsToolStripMenuItem1_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(197, 6);
            // 
            // showAssistantWindowMenuItem1
            // 
            this.showAssistantWindowMenuItem1.CheckOnClick = true;
            this.showAssistantWindowMenuItem1.Name = "showAssistantWindowMenuItem1";
            this.showAssistantWindowMenuItem1.Size = new System.Drawing.Size(200, 22);
            this.showAssistantWindowMenuItem1.Text = "Show Assistant Window";
            this.showAssistantWindowMenuItem1.Click += new System.EventHandler(this.showAssistantWindowMenuItem1_Click);
            // 
            // showDebugWindowMenuItem1
            // 
            this.showDebugWindowMenuItem1.CheckOnClick = true;
            this.showDebugWindowMenuItem1.Name = "showDebugWindowMenuItem1";
            this.showDebugWindowMenuItem1.Size = new System.Drawing.Size(200, 22);
            this.showDebugWindowMenuItem1.Text = "Show Debug Window";
            this.showDebugWindowMenuItem1.Click += new System.EventHandler(this.showDebugWindowMenuItem1_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(197, 6);
            // 
            // blazeWebpageToolStripMenuItem
            // 
            this.blazeWebpageToolStripMenuItem.Name = "blazeWebpageToolStripMenuItem";
            this.blazeWebpageToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.blazeWebpageToolStripMenuItem.Text = "Blaze Webpage";
            this.blazeWebpageToolStripMenuItem.Click += new System.EventHandler(this.blazeWebpageToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(197, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // IconBox
            // 
            this.IconBox.BackColor = System.Drawing.Color.Transparent;
            this.IconBox.Location = new System.Drawing.Point(28, 32);
            this.IconBox.Name = "IconBox";
            this.IconBox.Size = new System.Drawing.Size(32, 32);
            this.IconBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.IconBox.TabIndex = 2;
            this.IconBox.TabStop = false;
            // 
            // NameDisplay
            // 
            this.NameDisplay.AutoEllipsis = true;
            this.NameDisplay.BackColor = System.Drawing.Color.Transparent;
            this.NameDisplay.Font = new System.Drawing.Font("Verdana", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameDisplay.ForeColor = System.Drawing.Color.Black;
            this.NameDisplay.Location = new System.Drawing.Point(82, 11);
            this.NameDisplay.Name = "NameDisplay";
            this.NameDisplay.Size = new System.Drawing.Size(289, 23);
            this.NameDisplay.TabIndex = 4;
            this.NameDisplay.Text = "NameDisplay";
            // 
            // CustomLabel
            // 
            this.CustomLabel.BackColor = System.Drawing.Color.Transparent;
            this.CustomLabel.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CustomLabel.ForeColor = System.Drawing.Color.Black;
            this.CustomLabel.Location = new System.Drawing.Point(85, 36);
            this.CustomLabel.Name = "CustomLabel";
            this.CustomLabel.Size = new System.Drawing.Size(286, 18);
            this.CustomLabel.TabIndex = 5;
            this.CustomLabel.TabStop = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Chartreuse;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(384, 96);
            this.ContextMenuStrip = this.MainContextMenu;
            this.Controls.Add(this.TextInput);
            this.Controls.Add(this.NameDisplay);
            this.Controls.Add(this.IconBox);
            this.Controls.Add(this.CustomLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = global::Blaze.Properties.Resources.blaze_big;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Blaze";
            this.TransparencyKey = System.Drawing.Color.Chartreuse;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.NotifyIconContextMenu.ResumeLayout(false);
            this.MainContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.IconBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon NotifyIcon;
        //private AlphaBlendTextBox TextInput;
        //private AlphaBlendTextBox TextInput;
        private System.Windows.Forms.TextBox TextInput;
        private System.Windows.Forms.ContextMenuStrip NotifyIconContextMenu;
        private System.Windows.Forms.ToolStripMenuItem NotifyIconContextMenu_Exit;
        private System.Windows.Forms.ToolStripMenuItem restoreToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ContextMenuStrip MainContextMenu;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem rebuildIndexToolStripMenuItem1;
        private System.Windows.Forms.PictureBox IconBox;
        private System.Windows.Forms.Label NameDisplay;
        private System.Windows.Forms.Panel CustomLabel;
        private System.Windows.Forms.ToolStripMenuItem rebuildIndexToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem showDebugWindowMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem showDebugWindowMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem showAssistantWindowMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem showAssistantWindowMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem blazeWebpageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem blazeWebpageToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
    }
}

