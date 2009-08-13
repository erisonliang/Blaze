namespace Blaze.Automation.Wizard
{
    partial class WizardWindowPicker
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
            ProcessNameTextBox.TextChanged -= ProcessTextBox_TextChanged;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WizardWindowPicker));
            this.SplitContainer1 = new System.Windows.Forms.SplitContainer();
            this.WelcomeLabel = new System.Windows.Forms.Label();
            this.SplitContainer2 = new System.Windows.Forms.SplitContainer();
            this.EscTipLabel = new System.Windows.Forms.Label();
            this.PickWindowButton = new System.Windows.Forms.Button();
            this.PickWindowLabel = new System.Windows.Forms.Label();
            this.OptionalLabel1 = new System.Windows.Forms.Label();
            this.RequiredLabel1 = new System.Windows.Forms.Label();
            this.ProcessNameTextBox = new System.Windows.Forms.TextBox();
            this.TitleTextBox = new System.Windows.Forms.TextBox();
            this.ProcessName = new System.Windows.Forms.Label();
            this.TitleLabel = new System.Windows.Forms.Label();
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
            this.WelcomeLabel.Size = new System.Drawing.Size(196, 25);
            this.WelcomeLabel.TabIndex = 0;
            this.WelcomeLabel.Text = "Which Application?";
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
            this.SplitContainer2.Panel1.Controls.Add(this.EscTipLabel);
            this.SplitContainer2.Panel1.Controls.Add(this.PickWindowButton);
            this.SplitContainer2.Panel1.Controls.Add(this.PickWindowLabel);
            this.SplitContainer2.Panel1.Controls.Add(this.OptionalLabel1);
            this.SplitContainer2.Panel1.Controls.Add(this.RequiredLabel1);
            this.SplitContainer2.Panel1.Controls.Add(this.ProcessNameTextBox);
            this.SplitContainer2.Panel1.Controls.Add(this.TitleTextBox);
            this.SplitContainer2.Panel1.Controls.Add(this.ProcessName);
            this.SplitContainer2.Panel1.Controls.Add(this.TitleLabel);
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
            // EscTipLabel
            // 
            this.EscTipLabel.AutoSize = true;
            this.EscTipLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EscTipLabel.Location = new System.Drawing.Point(140, 200);
            this.EscTipLabel.Name = "EscTipLabel";
            this.EscTipLabel.Size = new System.Drawing.Size(152, 13);
            this.EscTipLabel.TabIndex = 12;
            this.EscTipLabel.Text = "Tip 2: Press Escape to cancel.";
            // 
            // PickWindowButton
            // 
            this.PickWindowButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PickWindowButton.Location = new System.Drawing.Point(143, 147);
            this.PickWindowButton.Name = "PickWindowButton";
            this.PickWindowButton.Size = new System.Drawing.Size(200, 23);
            this.PickWindowButton.TabIndex = 11;
            this.PickWindowButton.Text = "Pick a Window";
            this.PickWindowButton.UseVisualStyleBackColor = true;
            this.PickWindowButton.Click += new System.EventHandler(this.PickWindowButton_Click);
            // 
            // PickWindowLabel
            // 
            this.PickWindowLabel.AutoSize = true;
            this.PickWindowLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PickWindowLabel.Location = new System.Drawing.Point(140, 173);
            this.PickWindowLabel.Name = "PickWindowLabel";
            this.PickWindowLabel.Size = new System.Drawing.Size(373, 13);
            this.PickWindowLabel.TabIndex = 10;
            this.PickWindowLabel.Text = "Tip 1: After pressing this buttom, Alt-Tab to the desired window and click on it." +
                "";
            // 
            // OptionalLabel1
            // 
            this.OptionalLabel1.AutoSize = true;
            this.OptionalLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OptionalLabel1.Location = new System.Drawing.Point(363, 64);
            this.OptionalLabel1.Name = "OptionalLabel1";
            this.OptionalLabel1.Size = new System.Drawing.Size(52, 13);
            this.OptionalLabel1.TabIndex = 8;
            this.OptionalLabel1.Text = "(Optional)";
            // 
            // RequiredLabel1
            // 
            this.RequiredLabel1.AutoSize = true;
            this.RequiredLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RequiredLabel1.Location = new System.Drawing.Point(363, 93);
            this.RequiredLabel1.Name = "RequiredLabel1";
            this.RequiredLabel1.Size = new System.Drawing.Size(56, 13);
            this.RequiredLabel1.TabIndex = 7;
            this.RequiredLabel1.Text = "(Required)";
            // 
            // ProcessNameTextBox
            // 
            this.ProcessNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProcessNameTextBox.Location = new System.Drawing.Point(107, 88);
            this.ProcessNameTextBox.Name = "ProcessNameTextBox";
            this.ProcessNameTextBox.Size = new System.Drawing.Size(250, 20);
            this.ProcessNameTextBox.TabIndex = 6;
            // 
            // TitleTextBox
            // 
            this.TitleTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleTextBox.Location = new System.Drawing.Point(107, 59);
            this.TitleTextBox.Name = "TitleTextBox";
            this.TitleTextBox.Size = new System.Drawing.Size(250, 20);
            this.TitleTextBox.TabIndex = 4;
            // 
            // ProcessName
            // 
            this.ProcessName.AutoSize = true;
            this.ProcessName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProcessName.Location = new System.Drawing.Point(14, 91);
            this.ProcessName.Name = "ProcessName";
            this.ProcessName.Size = new System.Drawing.Size(79, 13);
            this.ProcessName.TabIndex = 3;
            this.ProcessName.Text = "Process Name:";
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.Location = new System.Drawing.Point(14, 62);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(72, 13);
            this.TitleLabel.TabIndex = 1;
            this.TitleLabel.Text = "Window Title:";
            // 
            // TextLabel1
            // 
            this.TextLabel1.AutoSize = true;
            this.TextLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextLabel1.Location = new System.Drawing.Point(14, 16);
            this.TextLabel1.Name = "TextLabel1";
            this.TextLabel1.Size = new System.Drawing.Size(247, 13);
            this.TextLabel1.TabIndex = 0;
            this.TextLabel1.Text = "Which application should this action be applied to?";
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
            // WizardWindowPicker
            // 
            this.AcceptButton = this.NextButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 324);
            this.Controls.Add(this.SplitContainer1);
            this.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WizardWindowPicker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Blaze Action Wizard";
            this.Load += new System.EventHandler(this.WizardWindowPicker_Load);
            this.SplitContainer1.Panel1.ResumeLayout(false);
            this.SplitContainer1.Panel1.PerformLayout();
            this.SplitContainer1.Panel2.ResumeLayout(false);
            this.SplitContainer1.ResumeLayout(false);
            this.SplitContainer2.Panel1.ResumeLayout(false);
            this.SplitContainer2.Panel1.PerformLayout();
            this.SplitContainer2.Panel2.ResumeLayout(false);
            this.SplitContainer2.ResumeLayout(false);
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
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.TextBox ProcessNameTextBox;
        private System.Windows.Forms.TextBox TitleTextBox;
        private System.Windows.Forms.Label ProcessName;
        private System.Windows.Forms.Label OptionalLabel1;
        private System.Windows.Forms.Label RequiredLabel1;
        private System.Windows.Forms.Button PickWindowButton;
        private System.Windows.Forms.Label PickWindowLabel;
        private System.Windows.Forms.Label EscTipLabel;
    }
}