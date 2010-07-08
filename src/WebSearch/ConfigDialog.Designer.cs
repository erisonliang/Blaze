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
            this.iconCacheGroupBox = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.deleteIconCacheButton = new System.Windows.Forms.Button();
            this.iconCacheTTL = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.iconCacheGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconCacheTTL)).BeginInit();
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
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(401, 361);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 7;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(482, 361);
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
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.iconCacheGroupBox);
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
            this.panel1.Size = new System.Drawing.Size(569, 350);
            this.panel1.TabIndex = 10;
            // 
            // iconCacheGroupBox
            // 
            this.iconCacheGroupBox.Controls.Add(this.label2);
            this.iconCacheGroupBox.Controls.Add(this.deleteIconCacheButton);
            this.iconCacheGroupBox.Controls.Add(this.iconCacheTTL);
            this.iconCacheGroupBox.Controls.Add(this.label1);
            this.iconCacheGroupBox.Location = new System.Drawing.Point(15, 283);
            this.iconCacheGroupBox.Name = "iconCacheGroupBox";
            this.iconCacheGroupBox.Size = new System.Drawing.Size(505, 56);
            this.iconCacheGroupBox.TabIndex = 10;
            this.iconCacheGroupBox.TabStop = false;
            this.iconCacheGroupBox.Text = "Icon Cache";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(227, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "(0 for no cleanup)";
            // 
            // deleteIconCacheButton
            // 
            this.deleteIconCacheButton.Location = new System.Drawing.Point(325, 20);
            this.deleteIconCacheButton.Name = "deleteIconCacheButton";
            this.deleteIconCacheButton.Size = new System.Drawing.Size(170, 23);
            this.deleteIconCacheButton.TabIndex = 2;
            this.deleteIconCacheButton.Text = "Clean Icon Cache Now!";
            this.deleteIconCacheButton.UseVisualStyleBackColor = true;
            this.deleteIconCacheButton.Click += new System.EventHandler(this.deleteIconCacheButton_Click);
            // 
            // iconCacheTTL
            // 
            this.iconCacheTTL.Location = new System.Drawing.Point(182, 23);
            this.iconCacheTTL.Maximum = new decimal(new int[] {
            744,
            0,
            0,
            0});
            this.iconCacheTTL.Name = "iconCacheTTL";
            this.iconCacheTTL.Size = new System.Drawing.Size(39, 20);
            this.iconCacheTTL.TabIndex = 1;
            this.iconCacheTTL.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.iconCacheTTL.Value = new decimal(new int[] {
            168,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cache Periodic Cleanup (in hours):";
            // 
            // ConfigDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(569, 396);
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
            this.iconCacheGroupBox.ResumeLayout(false);
            this.iconCacheGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconCacheTTL)).EndInit();
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
        private System.Windows.Forms.GroupBox iconCacheGroupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button deleteIconCacheButton;
        private System.Windows.Forms.NumericUpDown iconCacheTTL;
        private System.Windows.Forms.Label label2;
    }
}