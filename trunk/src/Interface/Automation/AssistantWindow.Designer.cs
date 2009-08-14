namespace Blaze.Automation
{
    partial class AssistantWindow
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
            Gma.UserActivityMonitor.HookManager.KeyDown -= HookManager_KeyDown;
            SuggestionDisplay.KeyDown -= AssistantWindow_KeyDown;
            this.GotFocus -= AssistantWindow_GotFocus;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssistantWindow));
            this.ToolStrip = new System.Windows.Forms.ToolStrip();
            this.PreviousSuggestion = new System.Windows.Forms.ToolStripButton();
            this.CurrentSuggestion = new System.Windows.Forms.ToolStripTextBox();
            this.NextSuggestion = new System.Windows.Forms.ToolStripButton();
            this.Separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.RepeatLabel = new System.Windows.Forms.ToolStripLabel();
            this.DecreaseRepetitionNumber = new System.Windows.Forms.ToolStripButton();
            this.Iterations = new System.Windows.Forms.ToolStripTextBox();
            this.IncreaseRepetitionNumber = new System.Windows.Forms.ToolStripButton();
            this.Separator2 = new System.Windows.Forms.ToolStripSeparator();
            this.SpeedLabel = new System.Windows.Forms.ToolStripLabel();
            this.DecreaseSpeed = new System.Windows.Forms.ToolStripButton();
            this.Speed = new System.Windows.Forms.ToolStripTextBox();
            this.IncreaseSpeed = new System.Windows.Forms.ToolStripButton();
            this.Separator3 = new System.Windows.Forms.ToolStripSeparator();
            this.NewButton = new System.Windows.Forms.ToolStripButton();
            this.SaveButton = new System.Windows.Forms.ToolStripSplitButton();
            this.SaveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ModifyButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.Separator4 = new System.Windows.Forms.ToolStripSeparator();
            this.DoitButton = new System.Windows.Forms.ToolStripButton();
            this.SuggestionDisplay = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.ToolStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolStrip
            // 
            this.ToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PreviousSuggestion,
            this.CurrentSuggestion,
            this.NextSuggestion,
            this.Separator1,
            this.RepeatLabel,
            this.DecreaseRepetitionNumber,
            this.Iterations,
            this.IncreaseRepetitionNumber,
            this.Separator2,
            this.SpeedLabel,
            this.DecreaseSpeed,
            this.Speed,
            this.IncreaseSpeed,
            this.Separator3,
            this.NewButton,
            this.SaveButton,
            this.ModifyButton,
            this.toolStripButton1,
            this.Separator4,
            this.DoitButton});
            this.ToolStrip.Location = new System.Drawing.Point(0, 314);
            this.ToolStrip.Name = "ToolStrip";
            this.ToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.ToolStrip.Size = new System.Drawing.Size(474, 25);
            this.ToolStrip.TabIndex = 0;
            this.ToolStrip.Text = "toolStrip1";
            // 
            // PreviousSuggestion
            // 
            this.PreviousSuggestion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PreviousSuggestion.Image = ((System.Drawing.Image)(resources.GetObject("PreviousSuggestion.Image")));
            this.PreviousSuggestion.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PreviousSuggestion.Name = "PreviousSuggestion";
            this.PreviousSuggestion.Size = new System.Drawing.Size(23, 22);
            this.PreviousSuggestion.Text = "Previous suggestion (Left Arrow)";
            this.PreviousSuggestion.Click += new System.EventHandler(this.PreviousSuggestion_Click);
            // 
            // CurrentSuggestion
            // 
            this.CurrentSuggestion.BackColor = System.Drawing.SystemColors.Window;
            this.CurrentSuggestion.Name = "CurrentSuggestion";
            this.CurrentSuggestion.ReadOnly = true;
            this.CurrentSuggestion.Size = new System.Drawing.Size(30, 25);
            this.CurrentSuggestion.Text = "1/1";
            this.CurrentSuggestion.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // NextSuggestion
            // 
            this.NextSuggestion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.NextSuggestion.Image = ((System.Drawing.Image)(resources.GetObject("NextSuggestion.Image")));
            this.NextSuggestion.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NextSuggestion.Name = "NextSuggestion";
            this.NextSuggestion.Size = new System.Drawing.Size(23, 22);
            this.NextSuggestion.Text = "Next suggestion (Right Arrow)";
            this.NextSuggestion.Click += new System.EventHandler(this.NextSuggestion_Click);
            // 
            // Separator1
            // 
            this.Separator1.Name = "Separator1";
            this.Separator1.Size = new System.Drawing.Size(6, 25);
            // 
            // RepeatLabel
            // 
            this.RepeatLabel.Name = "RepeatLabel";
            this.RepeatLabel.Size = new System.Drawing.Size(46, 22);
            this.RepeatLabel.Text = "Repeat:";
            // 
            // DecreaseRepetitionNumber
            // 
            this.DecreaseRepetitionNumber.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DecreaseRepetitionNumber.Image = ((System.Drawing.Image)(resources.GetObject("DecreaseRepetitionNumber.Image")));
            this.DecreaseRepetitionNumber.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DecreaseRepetitionNumber.Name = "DecreaseRepetitionNumber";
            this.DecreaseRepetitionNumber.Size = new System.Drawing.Size(23, 22);
            this.DecreaseRepetitionNumber.Text = "Decrease number of repetitions to be performed (Down Arrow)";
            this.DecreaseRepetitionNumber.Click += new System.EventHandler(this.DecreaseRepetitionNumber_Click);
            // 
            // Iterations
            // 
            this.Iterations.BackColor = System.Drawing.SystemColors.Window;
            this.Iterations.Name = "Iterations";
            this.Iterations.ReadOnly = true;
            this.Iterations.Size = new System.Drawing.Size(25, 25);
            this.Iterations.Text = "1";
            this.Iterations.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // IncreaseRepetitionNumber
            // 
            this.IncreaseRepetitionNumber.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.IncreaseRepetitionNumber.Image = ((System.Drawing.Image)(resources.GetObject("IncreaseRepetitionNumber.Image")));
            this.IncreaseRepetitionNumber.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.IncreaseRepetitionNumber.Name = "IncreaseRepetitionNumber";
            this.IncreaseRepetitionNumber.Size = new System.Drawing.Size(23, 22);
            this.IncreaseRepetitionNumber.Text = "Increase number of repetitions to be performed (Up Arrow)";
            this.IncreaseRepetitionNumber.Click += new System.EventHandler(this.IncreaseRepetitionNumber_Click);
            // 
            // Separator2
            // 
            this.Separator2.Name = "Separator2";
            this.Separator2.Size = new System.Drawing.Size(6, 25);
            // 
            // SpeedLabel
            // 
            this.SpeedLabel.Name = "SpeedLabel";
            this.SpeedLabel.Size = new System.Drawing.Size(42, 22);
            this.SpeedLabel.Text = "Speed:";
            // 
            // DecreaseSpeed
            // 
            this.DecreaseSpeed.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DecreaseSpeed.Image = ((System.Drawing.Image)(resources.GetObject("DecreaseSpeed.Image")));
            this.DecreaseSpeed.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DecreaseSpeed.Name = "DecreaseSpeed";
            this.DecreaseSpeed.Size = new System.Drawing.Size(23, 22);
            this.DecreaseSpeed.Text = "Decrease speed (Num Minus)";
            this.DecreaseSpeed.Click += new System.EventHandler(this.DecreaseSpeed_Click);
            // 
            // Speed
            // 
            this.Speed.BackColor = System.Drawing.SystemColors.Window;
            this.Speed.Name = "Speed";
            this.Speed.ReadOnly = true;
            this.Speed.Size = new System.Drawing.Size(35, 25);
            this.Speed.Text = "1.0x";
            this.Speed.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // IncreaseSpeed
            // 
            this.IncreaseSpeed.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.IncreaseSpeed.Image = ((System.Drawing.Image)(resources.GetObject("IncreaseSpeed.Image")));
            this.IncreaseSpeed.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.IncreaseSpeed.Name = "IncreaseSpeed";
            this.IncreaseSpeed.Size = new System.Drawing.Size(23, 22);
            this.IncreaseSpeed.Text = "Increase speed (Num Plus)";
            this.IncreaseSpeed.Click += new System.EventHandler(this.IncreaseSpeed_Click);
            // 
            // Separator3
            // 
            this.Separator3.Name = "Separator3";
            this.Separator3.Size = new System.Drawing.Size(6, 25);
            // 
            // NewButton
            // 
            this.NewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.NewButton.Image = ((System.Drawing.Image)(resources.GetObject("NewButton.Image")));
            this.NewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NewButton.Name = "NewButton";
            this.NewButton.Size = new System.Drawing.Size(23, 22);
            this.NewButton.Text = "Create a new automation (Ctrl + N)";
            this.NewButton.Click += new System.EventHandler(this.NewButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveAsMenuItem,
            this.SaveMenuItem});
            this.SaveButton.Enabled = false;
            this.SaveButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveButton.Image")));
            this.SaveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(32, 22);
            this.SaveButton.Text = "Save automation (Ctrl + S)";
            this.SaveButton.ButtonClick += new System.EventHandler(this.SaveButton_ButtonClick);
            // 
            // SaveAsMenuItem
            // 
            this.SaveAsMenuItem.Name = "SaveAsMenuItem";
            this.SaveAsMenuItem.Size = new System.Drawing.Size(202, 22);
            this.SaveAsMenuItem.Text = "Save As... (Ctrl + Alt + S)";
            this.SaveAsMenuItem.Click += new System.EventHandler(this.SaveAsMenuItem_Click);
            // 
            // SaveMenuItem
            // 
            this.SaveMenuItem.Name = "SaveMenuItem";
            this.SaveMenuItem.Size = new System.Drawing.Size(202, 22);
            this.SaveMenuItem.Text = "Save (Ctrl + S)";
            this.SaveMenuItem.Click += new System.EventHandler(this.SaveMenuItem_Click);
            // 
            // ModifyButton
            // 
            this.ModifyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ModifyButton.Image = ((System.Drawing.Image)(resources.GetObject("ModifyButton.Image")));
            this.ModifyButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ModifyButton.Name = "ModifyButton";
            this.ModifyButton.Size = new System.Drawing.Size(23, 22);
            this.ModifyButton.Text = "Modify this automation (Ctrl + M)";
            this.ModifyButton.Click += new System.EventHandler(this.ModifyButton_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Help (F1)";
            // 
            // Separator4
            // 
            this.Separator4.Name = "Separator4";
            this.Separator4.Size = new System.Drawing.Size(6, 25);
            // 
            // DoitButton
            // 
            this.DoitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DoitButton.Image = ((System.Drawing.Image)(resources.GetObject("DoitButton.Image")));
            this.DoitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DoitButton.Name = "DoitButton";
            this.DoitButton.Size = new System.Drawing.Size(23, 22);
            this.DoitButton.Text = "Do it! (Enter)";
            this.DoitButton.Click += new System.EventHandler(this.DoitButton_Click);
            // 
            // SuggestionDisplay
            // 
            this.SuggestionDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SuggestionDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SuggestionDisplay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SuggestionDisplay.Location = new System.Drawing.Point(8, 19);
            this.SuggestionDisplay.Name = "SuggestionDisplay";
            this.SuggestionDisplay.ReadOnly = true;
            this.SuggestionDisplay.Size = new System.Drawing.Size(436, 265);
            this.SuggestionDisplay.TabIndex = 1;
            this.SuggestionDisplay.Text = "Sorry but no repetitions were detected yet.";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.SuggestionDisplay);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(450, 290);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Blaze has a suggestion for you:";
            // 
            // SaveFileDialog
            // 
            this.SaveFileDialog.DefaultExt = "py";
            this.SaveFileDialog.FileName = "My Automation";
            this.SaveFileDialog.Filter = "IronPython files|*.py|All files|*.*";
            // 
            // AssistantWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 339);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ToolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(368, 145);
            this.Name = "AssistantWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Blaze Assistant";
            this.Load += new System.EventHandler(this.AssistantWindow_Load);
            this.ToolStrip.ResumeLayout(false);
            this.ToolStrip.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip ToolStrip;
        private System.Windows.Forms.ToolStripButton DoitButton;
        private System.Windows.Forms.ToolStripButton ModifyButton;
        private System.Windows.Forms.ToolStripSeparator Separator1;
        private System.Windows.Forms.ToolStripButton PreviousSuggestion;
        private System.Windows.Forms.ToolStripTextBox CurrentSuggestion;
        private System.Windows.Forms.ToolStripButton NextSuggestion;
        private System.Windows.Forms.RichTextBox SuggestionDisplay;
        private System.Windows.Forms.ToolStripSeparator Separator2;
        private System.Windows.Forms.ToolStripLabel RepeatLabel;
        private System.Windows.Forms.ToolStripButton DecreaseRepetitionNumber;
        private System.Windows.Forms.ToolStripTextBox Iterations;
        private System.Windows.Forms.ToolStripButton IncreaseRepetitionNumber;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStripSeparator Separator3;
        private System.Windows.Forms.ToolStripLabel SpeedLabel;
        private System.Windows.Forms.ToolStripButton DecreaseSpeed;
        private System.Windows.Forms.ToolStripTextBox Speed;
        private System.Windows.Forms.ToolStripButton IncreaseSpeed;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator Separator4;
        private System.Windows.Forms.ToolStripButton NewButton;
        private System.Windows.Forms.ToolStripSplitButton SaveButton;
        private System.Windows.Forms.ToolStripMenuItem SaveAsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveMenuItem;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
    }
}