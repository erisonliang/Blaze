namespace WebSearch
{
    partial class EnginePicker
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
            _tooltip.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnginePicker));
            this.siteNameLabel = new System.Windows.Forms.Label();
            this.siteUrlLabel = new System.Windows.Forms.Label();
            this.siteQueryLabel = new System.Windows.Forms.Label();
            this.siteNameTextBox = new System.Windows.Forms.TextBox();
            this.siteUrlTextBox = new System.Windows.Forms.TextBox();
            this.siteQueyTextBox = new System.Windows.Forms.TextBox();
            this.tipLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // siteNameLabel
            // 
            this.siteNameLabel.AutoSize = true;
            this.siteNameLabel.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.siteNameLabel.Location = new System.Drawing.Point(12, 9);
            this.siteNameLabel.Name = "siteNameLabel";
            this.siteNameLabel.Size = new System.Drawing.Size(50, 16);
            this.siteNameLabel.TabIndex = 0;
            this.siteNameLabel.Text = "Name:";
            // 
            // siteUrlLabel
            // 
            this.siteUrlLabel.AutoSize = true;
            this.siteUrlLabel.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.siteUrlLabel.Location = new System.Drawing.Point(12, 56);
            this.siteUrlLabel.Name = "siteUrlLabel";
            this.siteUrlLabel.Size = new System.Drawing.Size(38, 16);
            this.siteUrlLabel.TabIndex = 1;
            this.siteUrlLabel.Text = "URL:";
            // 
            // siteQueryLabel
            // 
            this.siteQueryLabel.AutoSize = true;
            this.siteQueryLabel.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.siteQueryLabel.Location = new System.Drawing.Point(12, 103);
            this.siteQueryLabel.Name = "siteQueryLabel";
            this.siteQueryLabel.Size = new System.Drawing.Size(173, 16);
            this.siteQueryLabel.TabIndex = 2;
            this.siteQueryLabel.Text = "Search Query (optional):";
            // 
            // siteNameTextBox
            // 
            this.siteNameTextBox.Location = new System.Drawing.Point(15, 28);
            this.siteNameTextBox.Name = "siteNameTextBox";
            this.siteNameTextBox.Size = new System.Drawing.Size(255, 20);
            this.siteNameTextBox.TabIndex = 3;
            // 
            // siteUrlTextBox
            // 
            this.siteUrlTextBox.Location = new System.Drawing.Point(15, 75);
            this.siteUrlTextBox.Name = "siteUrlTextBox";
            this.siteUrlTextBox.Size = new System.Drawing.Size(255, 20);
            this.siteUrlTextBox.TabIndex = 4;
            // 
            // siteQueyTextBox
            // 
            this.siteQueyTextBox.Location = new System.Drawing.Point(15, 122);
            this.siteQueyTextBox.Name = "siteQueyTextBox";
            this.siteQueyTextBox.Size = new System.Drawing.Size(255, 20);
            this.siteQueyTextBox.TabIndex = 5;
            // 
            // tipLabel
            // 
            this.tipLabel.AutoSize = true;
            this.tipLabel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tipLabel.Location = new System.Drawing.Point(12, 145);
            this.tipLabel.Name = "tipLabel";
            this.tipLabel.Size = new System.Drawing.Size(239, 13);
            this.tipLabel.TabIndex = 6;
            this.tipLabel.Text = "Tip: use \'%s\' to specify the search term.";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.Location = new System.Drawing.Point(195, 179);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okButton.Location = new System.Drawing.Point(114, 179);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 8;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // EnginePicker
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(282, 214);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.tipLabel);
            this.Controls.Add(this.siteQueyTextBox);
            this.Controls.Add(this.siteUrlTextBox);
            this.Controls.Add(this.siteNameTextBox);
            this.Controls.Add(this.siteQueryLabel);
            this.Controls.Add(this.siteUrlLabel);
            this.Controls.Add(this.siteNameLabel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EnginePicker";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add New Search Engine";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.EnginePicker_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label siteNameLabel;
        private System.Windows.Forms.Label siteUrlLabel;
        private System.Windows.Forms.Label siteQueryLabel;
        private System.Windows.Forms.TextBox siteNameTextBox;
        private System.Windows.Forms.TextBox siteUrlTextBox;
        private System.Windows.Forms.TextBox siteQueyTextBox;
        private System.Windows.Forms.Label tipLabel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
    }
}