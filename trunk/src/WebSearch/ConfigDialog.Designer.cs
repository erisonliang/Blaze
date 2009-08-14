namespace WebSearch
{
    partial class ConfigDialog
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
            DefaultLinkLabel.LinkClicked -= DefaultLinkLabel_LinkClicked;
            dataGridView1.RowPostPaint -= dataGridView1_RowPostPaint;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigDialog));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.addButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.defaultLabel = new System.Windows.Forms.Label();
            this.DefaultLinkLabel = new System.Windows.Forms.LinkLabel();
            this.defaultButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.tipLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(11, 63);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(545, 185);
            this.dataGridView1.TabIndex = 0;
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(121, 254);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Location = new System.Drawing.Point(370, 254);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(75, 23);
            this.removeButton.TabIndex = 2;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(12, 9);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(86, 13);
            this.descriptionLabel.TabIndex = 3;
            this.descriptionLabel.Text = "DescriptionLabel";
            // 
            // defaultLabel
            // 
            this.defaultLabel.AutoSize = true;
            this.defaultLabel.Location = new System.Drawing.Point(12, 47);
            this.defaultLabel.Name = "defaultLabel";
            this.defaultLabel.Size = new System.Drawing.Size(81, 13);
            this.defaultLabel.TabIndex = 4;
            this.defaultLabel.Text = "Default Search:";
            // 
            // DefaultLinkLabel
            // 
            this.DefaultLinkLabel.AutoSize = true;
            this.DefaultLinkLabel.Location = new System.Drawing.Point(97, 47);
            this.DefaultLinkLabel.Name = "DefaultLinkLabel";
            this.DefaultLinkLabel.Size = new System.Drawing.Size(55, 13);
            this.DefaultLinkLabel.TabIndex = 5;
            this.DefaultLinkLabel.TabStop = true;
            this.DefaultLinkLabel.Text = "linkLabel1";
            // 
            // defaultButton
            // 
            this.defaultButton.Location = new System.Drawing.Point(233, 254);
            this.defaultButton.Name = "defaultButton";
            this.defaultButton.Size = new System.Drawing.Size(99, 23);
            this.defaultButton.TabIndex = 6;
            this.defaultButton.Text = "Make Default";
            this.defaultButton.UseVisualStyleBackColor = true;
            this.defaultButton.Click += new System.EventHandler(this.defaultButton_Click);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(401, 301);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 7;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(482, 301);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // tipLabel
            // 
            this.tipLabel.AutoSize = true;
            this.tipLabel.Location = new System.Drawing.Point(296, 47);
            this.tipLabel.Name = "tipLabel";
            this.tipLabel.Size = new System.Drawing.Size(260, 13);
            this.tipLabel.TabIndex = 9;
            this.tipLabel.Text = "Tip: use \'%s\' to specify the search term inside a query.";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.descriptionLabel);
            this.panel1.Controls.Add(this.tipLabel);
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Controls.Add(this.addButton);
            this.panel1.Controls.Add(this.removeButton);
            this.panel1.Controls.Add(this.defaultButton);
            this.panel1.Controls.Add(this.defaultLabel);
            this.panel1.Controls.Add(this.DefaultLinkLabel);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(569, 290);
            this.panel1.TabIndex = 10;
            // 
            // ConfigDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(569, 336);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WebSearch settings";
            this.Load += new System.EventHandler(this.ConfigDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Label defaultLabel;
        private System.Windows.Forms.LinkLabel DefaultLinkLabel;
        private System.Windows.Forms.Button defaultButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label tipLabel;
        private System.Windows.Forms.Panel panel1;
    }
}