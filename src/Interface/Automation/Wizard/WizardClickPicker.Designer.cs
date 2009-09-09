namespace Blaze.Automation.Wizard
{
    partial class WizardClickPicker
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
            ButtonComboBox.SelectionChangeCommitted -= ButtonComboBox_SelectionChangeCommitted;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WizardClickPicker));
            this.SplitContainer1 = new System.Windows.Forms.SplitContainer();
            this.WelcomeLabel = new System.Windows.Forms.Label();
            this.SplitContainer2 = new System.Windows.Forms.SplitContainer();
            this.YBox = new System.Windows.Forms.NumericUpDown();
            this.XBox = new System.Windows.Forms.NumericUpDown();
            this.EscTipLabel = new System.Windows.Forms.Label();
            this.ButtonLabel = new System.Windows.Forms.Label();
            this.CaptureClickButton = new System.Windows.Forms.Button();
            this.WinModifier = new System.Windows.Forms.CheckBox();
            this.ButtonComboBox = new System.Windows.Forms.ComboBox();
            this.ShiftModifier = new System.Windows.Forms.CheckBox();
            this.TextLabel2 = new System.Windows.Forms.Label();
            this.CtrlModifier = new System.Windows.Forms.CheckBox();
            this.YLabel = new System.Windows.Forms.Label();
            this.ModifiersLabel = new System.Windows.Forms.Label();
            this.AltModifier = new System.Windows.Forms.CheckBox();
            this.XLabel = new System.Windows.Forms.Label();
            this.TextLabel1 = new System.Windows.Forms.Label();
            this.CancelButton = new System.Windows.Forms.Button();
            this.NextButton = new System.Windows.Forms.Button();
            this.BackButton = new System.Windows.Forms.Button();
            this.SplitContainer1.Panel1.SuspendLayout();
            this.SplitContainer1.Panel2.SuspendLayout();
            this.SplitContainer1.SuspendLayout();
            this.SplitContainer2.Panel1.SuspendLayout();
            this.SplitContainer2.Panel2.SuspendLayout();
            this.SplitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.YBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XBox)).BeginInit();
            this.SuspendLayout();
            // 
            // SplitContainer1
            // 
            this.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SplitContainer1.IsSplitterFixed = true;
            this.SplitContainer1.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer1.Name = "SplitContainer1";
            this.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer1.Panel1
            // 
            this.SplitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Window;
            this.SplitContainer1.Panel1.Controls.Add(this.WelcomeLabel);
            // 
            // SplitContainer1.Panel2
            // 
            this.SplitContainer1.Panel2.Controls.Add(this.SplitContainer2);
            this.SplitContainer1.Size = new System.Drawing.Size(494, 324);
            this.SplitContainer1.SplitterDistance = 45;
            this.SplitContainer1.SplitterWidth = 1;
            this.SplitContainer1.TabIndex = 0;
            // 
            // WelcomeLabel
            // 
            this.WelcomeLabel.AutoSize = true;
            this.WelcomeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WelcomeLabel.Location = new System.Drawing.Point(12, 9);
            this.WelcomeLabel.Name = "WelcomeLabel";
            this.WelcomeLabel.Size = new System.Drawing.Size(277, 25);
            this.WelcomeLabel.TabIndex = 0;
            this.WelcomeLabel.Text = "Which Button and Position?";
            // 
            // SplitContainer2
            // 
            this.SplitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SplitContainer2.IsSplitterFixed = true;
            this.SplitContainer2.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer2.Name = "SplitContainer2";
            this.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer2.Panel1
            // 
            this.SplitContainer2.Panel1.Controls.Add(this.YBox);
            this.SplitContainer2.Panel1.Controls.Add(this.XBox);
            this.SplitContainer2.Panel1.Controls.Add(this.EscTipLabel);
            this.SplitContainer2.Panel1.Controls.Add(this.ButtonLabel);
            this.SplitContainer2.Panel1.Controls.Add(this.CaptureClickButton);
            this.SplitContainer2.Panel1.Controls.Add(this.WinModifier);
            this.SplitContainer2.Panel1.Controls.Add(this.ButtonComboBox);
            this.SplitContainer2.Panel1.Controls.Add(this.ShiftModifier);
            this.SplitContainer2.Panel1.Controls.Add(this.TextLabel2);
            this.SplitContainer2.Panel1.Controls.Add(this.CtrlModifier);
            this.SplitContainer2.Panel1.Controls.Add(this.YLabel);
            this.SplitContainer2.Panel1.Controls.Add(this.ModifiersLabel);
            this.SplitContainer2.Panel1.Controls.Add(this.AltModifier);
            this.SplitContainer2.Panel1.Controls.Add(this.XLabel);
            this.SplitContainer2.Panel1.Controls.Add(this.TextLabel1);
            // 
            // SplitContainer2.Panel2
            // 
            this.SplitContainer2.Panel2.Controls.Add(this.CancelButton);
            this.SplitContainer2.Panel2.Controls.Add(this.NextButton);
            this.SplitContainer2.Panel2.Controls.Add(this.BackButton);
            this.SplitContainer2.Size = new System.Drawing.Size(494, 278);
            this.SplitContainer2.SplitterDistance = 239;
            this.SplitContainer2.SplitterWidth = 1;
            this.SplitContainer2.TabIndex = 0;
            // 
            // YBox
            // 
            this.YBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YBox.Location = new System.Drawing.Point(114, 40);
            this.YBox.Maximum = new decimal(new int[] {
            2400,
            0,
            0,
            0});
            this.YBox.Name = "YBox";
            this.YBox.Size = new System.Drawing.Size(50, 20);
            this.YBox.TabIndex = 17;
            this.YBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // XBox
            // 
            this.XBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XBox.Location = new System.Drawing.Point(37, 40);
            this.XBox.Maximum = new decimal(new int[] {
            3840,
            0,
            0,
            0});
            this.XBox.Name = "XBox";
            this.XBox.Size = new System.Drawing.Size(50, 20);
            this.XBox.TabIndex = 16;
            this.XBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // EscTipLabel
            // 
            this.EscTipLabel.AutoSize = true;
            this.EscTipLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EscTipLabel.Location = new System.Drawing.Point(178, 200);
            this.EscTipLabel.Name = "EscTipLabel";
            this.EscTipLabel.Size = new System.Drawing.Size(143, 13);
            this.EscTipLabel.TabIndex = 15;
            this.EscTipLabel.Text = "Tip: Press Escape to cancel.";
            // 
            // ButtonLabel
            // 
            this.ButtonLabel.AutoSize = true;
            this.ButtonLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonLabel.Location = new System.Drawing.Point(14, 101);
            this.ButtonLabel.Name = "ButtonLabel";
            this.ButtonLabel.Size = new System.Drawing.Size(41, 13);
            this.ButtonLabel.TabIndex = 13;
            this.ButtonLabel.Text = "Button:";
            // 
            // CaptureClickButton
            // 
            this.CaptureClickButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CaptureClickButton.Location = new System.Drawing.Point(143, 174);
            this.CaptureClickButton.Name = "CaptureClickButton";
            this.CaptureClickButton.Size = new System.Drawing.Size(200, 23);
            this.CaptureClickButton.TabIndex = 14;
            this.CaptureClickButton.Text = "Pick a Click";
            this.CaptureClickButton.UseVisualStyleBackColor = true;
            this.CaptureClickButton.Click += new System.EventHandler(this.CaptureClickButton_Click);
            // 
            // WinModifier
            // 
            this.WinModifier.AutoSize = true;
            this.WinModifier.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WinModifier.Location = new System.Drawing.Point(239, 128);
            this.WinModifier.Name = "WinModifier";
            this.WinModifier.Size = new System.Drawing.Size(45, 17);
            this.WinModifier.TabIndex = 12;
            this.WinModifier.Text = "Win";
            this.WinModifier.UseVisualStyleBackColor = true;
            // 
            // ButtonComboBox
            // 
            this.ButtonComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ButtonComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonComboBox.FormattingEnabled = true;
            this.ButtonComboBox.Location = new System.Drawing.Point(83, 98);
            this.ButtonComboBox.Name = "ButtonComboBox";
            this.ButtonComboBox.Size = new System.Drawing.Size(100, 21);
            this.ButtonComboBox.TabIndex = 6;
            // 
            // ShiftModifier
            // 
            this.ShiftModifier.AutoSize = true;
            this.ShiftModifier.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShiftModifier.Location = new System.Drawing.Point(182, 128);
            this.ShiftModifier.Name = "ShiftModifier";
            this.ShiftModifier.Size = new System.Drawing.Size(47, 17);
            this.ShiftModifier.TabIndex = 11;
            this.ShiftModifier.Text = "Shift";
            this.ShiftModifier.UseVisualStyleBackColor = true;
            // 
            // TextLabel2
            // 
            this.TextLabel2.AutoSize = true;
            this.TextLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextLabel2.Location = new System.Drawing.Point(14, 77);
            this.TextLabel2.Name = "TextLabel2";
            this.TextLabel2.Size = new System.Drawing.Size(167, 13);
            this.TextLabel2.TabIndex = 5;
            this.TextLabel2.Text = "Which Button should be pressed?";
            // 
            // CtrlModifier
            // 
            this.CtrlModifier.AutoSize = true;
            this.CtrlModifier.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CtrlModifier.Location = new System.Drawing.Point(130, 128);
            this.CtrlModifier.Name = "CtrlModifier";
            this.CtrlModifier.Size = new System.Drawing.Size(41, 17);
            this.CtrlModifier.TabIndex = 10;
            this.CtrlModifier.Text = "Ctrl";
            this.CtrlModifier.UseVisualStyleBackColor = true;
            // 
            // YLabel
            // 
            this.YLabel.AutoSize = true;
            this.YLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YLabel.Location = new System.Drawing.Point(93, 42);
            this.YLabel.Name = "YLabel";
            this.YLabel.Size = new System.Drawing.Size(17, 13);
            this.YLabel.TabIndex = 4;
            this.YLabel.Text = "Y:";
            // 
            // ModifiersLabel
            // 
            this.ModifiersLabel.AutoSize = true;
            this.ModifiersLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ModifiersLabel.Location = new System.Drawing.Point(14, 128);
            this.ModifiersLabel.Name = "ModifiersLabel";
            this.ModifiersLabel.Size = new System.Drawing.Size(52, 13);
            this.ModifiersLabel.TabIndex = 9;
            this.ModifiersLabel.Text = "Modifiers:";
            // 
            // AltModifier
            // 
            this.AltModifier.AutoSize = true;
            this.AltModifier.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AltModifier.Location = new System.Drawing.Point(83, 128);
            this.AltModifier.Name = "AltModifier";
            this.AltModifier.Size = new System.Drawing.Size(38, 17);
            this.AltModifier.TabIndex = 8;
            this.AltModifier.Text = "Alt";
            this.AltModifier.UseVisualStyleBackColor = true;
            // 
            // XLabel
            // 
            this.XLabel.AutoSize = true;
            this.XLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XLabel.Location = new System.Drawing.Point(14, 42);
            this.XLabel.Name = "XLabel";
            this.XLabel.Size = new System.Drawing.Size(17, 13);
            this.XLabel.TabIndex = 3;
            this.XLabel.Text = "X:";
            // 
            // TextLabel1
            // 
            this.TextLabel1.AutoSize = true;
            this.TextLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextLabel1.Location = new System.Drawing.Point(14, 16);
            this.TextLabel1.Name = "TextLabel1";
            this.TextLabel1.Size = new System.Drawing.Size(228, 13);
            this.TextLabel1.TabIndex = 0;
            this.TextLabel1.Text = "Where should the mouse action be performed?";
            // 
            // CancelButton
            // 
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelButton.Location = new System.Drawing.Point(248, 6);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 2;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // NextButton
            // 
            this.NextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NextButton.Location = new System.Drawing.Point(410, 6);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(75, 23);
            this.NextButton.TabIndex = 0;
            this.NextButton.Text = "Next >";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // BackButton
            // 
            this.BackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BackButton.Location = new System.Drawing.Point(329, 6);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(75, 23);
            this.BackButton.TabIndex = 1;
            this.BackButton.Text = "< Back";
            this.BackButton.UseVisualStyleBackColor = true;
            this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // WizardClickPicker
            // 
            this.AcceptButton = this.NextButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 324);
            this.Controls.Add(this.SplitContainer1);
            this.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WizardClickPicker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Blaze Action Wizard";
            this.Load += new System.EventHandler(this.WizardClickPicker_Load);
            this.SplitContainer1.Panel1.ResumeLayout(false);
            this.SplitContainer1.Panel1.PerformLayout();
            this.SplitContainer1.Panel2.ResumeLayout(false);
            this.SplitContainer1.ResumeLayout(false);
            this.SplitContainer2.Panel1.ResumeLayout(false);
            this.SplitContainer2.Panel1.PerformLayout();
            this.SplitContainer2.Panel2.ResumeLayout(false);
            this.SplitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.YBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer SplitContainer1;
        private System.Windows.Forms.Label WelcomeLabel;
        private System.Windows.Forms.SplitContainer SplitContainer2;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button BackButton;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.Label TextLabel1;
        private System.Windows.Forms.Label YLabel;
        private System.Windows.Forms.Label XLabel;
        private System.Windows.Forms.ComboBox ButtonComboBox;
        private System.Windows.Forms.Label TextLabel2;
        private System.Windows.Forms.Label ButtonLabel;
        private System.Windows.Forms.CheckBox WinModifier;
        private System.Windows.Forms.CheckBox ShiftModifier;
        private System.Windows.Forms.CheckBox CtrlModifier;
        private System.Windows.Forms.Label ModifiersLabel;
        private System.Windows.Forms.CheckBox AltModifier;
        private System.Windows.Forms.Label EscTipLabel;
        private System.Windows.Forms.Button CaptureClickButton;
        private System.Windows.Forms.NumericUpDown YBox;
        private System.Windows.Forms.NumericUpDown XBox;
    }
}