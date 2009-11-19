namespace Blaze.Debug
{
    partial class DebugWindow
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
            _timer.Tick -= _timer_Tick;
            ActionsListBox.SelectedIndexChanged -= ActionsListBox_SelectedIndexChanged;
            GeneralizationsListBox.SelectedIndexChanged -= GeneralizationsListBox_SelectedIndexChanged;
            RepetitionsListBox.SelectedIndexChanged -= RepetitionsListBox_SelectedIndexChanged;
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.ObserverTab1 = new System.Windows.Forms.TabPage();
            this.ToClipboardIndicator1 = new System.Windows.Forms.Label();
            this.ToClipboard1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ActionsListBox = new System.Windows.Forms.ListBox();
            this.NumActionsIndicator = new System.Windows.Forms.Label();
            this.WorkingPathIndicator = new System.Windows.Forms.Label();
            this.RecordingIndicator = new System.Windows.Forms.Label();
            this.MonitoringIndicator = new System.Windows.Forms.Label();
            this.ActionsListLabel = new System.Windows.Forms.Label();
            this.NumActionsLabel = new System.Windows.Forms.Label();
            this.WorkingPathLabel = new System.Windows.Forms.Label();
            this.RecordingLabel = new System.Windows.Forms.Label();
            this.MonitoringLabel = new System.Windows.Forms.Label();
            this.ObserverTab2 = new System.Windows.Forms.TabPage();
            this.ToClipboardIndicator2 = new System.Windows.Forms.Label();
            this.ToClipboard2 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.GeneralizationsLabel = new System.Windows.Forms.Label();
            this.CompressionIndicator = new System.Windows.Forms.Label();
            this.CompressionLabel = new System.Windows.Forms.Label();
            this.GeneralizationsListBox = new System.Windows.Forms.ListBox();
            this.ApprenticeTab = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.RepetitionsLabel = new System.Windows.Forms.Label();
            this.RepetitionsListBox = new System.Windows.Forms.ListBox();
            this.ToClipboardIndicator3 = new System.Windows.Forms.Label();
            this.ToClipboard3 = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.ObserverTab1.SuspendLayout();
            this.ObserverTab2.SuspendLayout();
            this.ApprenticeTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.ObserverTab1);
            this.tabControl.Controls.Add(this.ObserverTab2);
            this.tabControl.Controls.Add(this.ApprenticeTab);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(670, 372);
            this.tabControl.TabIndex = 0;
            // 
            // ObserverTab1
            // 
            this.ObserverTab1.Controls.Add(this.ToClipboardIndicator1);
            this.ObserverTab1.Controls.Add(this.ToClipboard1);
            this.ObserverTab1.Controls.Add(this.label1);
            this.ObserverTab1.Controls.Add(this.ActionsListBox);
            this.ObserverTab1.Controls.Add(this.NumActionsIndicator);
            this.ObserverTab1.Controls.Add(this.WorkingPathIndicator);
            this.ObserverTab1.Controls.Add(this.RecordingIndicator);
            this.ObserverTab1.Controls.Add(this.MonitoringIndicator);
            this.ObserverTab1.Controls.Add(this.ActionsListLabel);
            this.ObserverTab1.Controls.Add(this.NumActionsLabel);
            this.ObserverTab1.Controls.Add(this.WorkingPathLabel);
            this.ObserverTab1.Controls.Add(this.RecordingLabel);
            this.ObserverTab1.Controls.Add(this.MonitoringLabel);
            this.ObserverTab1.Location = new System.Drawing.Point(4, 22);
            this.ObserverTab1.Name = "ObserverTab1";
            this.ObserverTab1.Padding = new System.Windows.Forms.Padding(3);
            this.ObserverTab1.Size = new System.Drawing.Size(662, 346);
            this.ObserverTab1.TabIndex = 0;
            this.ObserverTab1.Text = "Observer (Actions)";
            this.ObserverTab1.UseVisualStyleBackColor = true;
            // 
            // ToClipboardIndicator1
            // 
            this.ToClipboardIndicator1.AutoSize = true;
            this.ToClipboardIndicator1.Location = new System.Drawing.Point(543, 28);
            this.ToClipboardIndicator1.Name = "ToClipboardIndicator1";
            this.ToClipboardIndicator1.Size = new System.Drawing.Size(27, 13);
            this.ToClipboardIndicator1.TabIndex = 13;
            this.ToClipboardIndicator1.Text = "N/A";
            // 
            // ToClipboard1
            // 
            this.ToClipboard1.AutoSize = true;
            this.ToClipboard1.Location = new System.Drawing.Point(467, 28);
            this.ToClipboard1.Name = "ToClipboard1";
            this.ToClipboard1.Size = new System.Drawing.Size(70, 13);
            this.ToClipboard1.TabIndex = 12;
            this.ToClipboard1.Text = "To Clipboard:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(416, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Tip: press an item to copy  its info to the clipboard";
            // 
            // ActionsListBox
            // 
            this.ActionsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ActionsListBox.FormattingEnabled = true;
            this.ActionsListBox.Location = new System.Drawing.Point(7, 70);
            this.ActionsListBox.Name = "ActionsListBox";
            this.ActionsListBox.Size = new System.Drawing.Size(649, 264);
            this.ActionsListBox.TabIndex = 10;
            // 
            // NumActionsIndicator
            // 
            this.NumActionsIndicator.AutoSize = true;
            this.NumActionsIndicator.Location = new System.Drawing.Point(318, 28);
            this.NumActionsIndicator.Name = "NumActionsIndicator";
            this.NumActionsIndicator.Size = new System.Drawing.Size(35, 13);
            this.NumActionsIndicator.TabIndex = 9;
            this.NumActionsIndicator.Text = "label4";
            // 
            // WorkingPathIndicator
            // 
            this.WorkingPathIndicator.AutoSize = true;
            this.WorkingPathIndicator.Location = new System.Drawing.Point(318, 7);
            this.WorkingPathIndicator.Name = "WorkingPathIndicator";
            this.WorkingPathIndicator.Size = new System.Drawing.Size(35, 13);
            this.WorkingPathIndicator.TabIndex = 8;
            this.WorkingPathIndicator.Text = "label3";
            // 
            // RecordingIndicator
            // 
            this.RecordingIndicator.AutoSize = true;
            this.RecordingIndicator.Location = new System.Drawing.Point(72, 28);
            this.RecordingIndicator.Name = "RecordingIndicator";
            this.RecordingIndicator.Size = new System.Drawing.Size(35, 13);
            this.RecordingIndicator.TabIndex = 7;
            this.RecordingIndicator.Text = "label2";
            // 
            // MonitoringIndicator
            // 
            this.MonitoringIndicator.AutoSize = true;
            this.MonitoringIndicator.Location = new System.Drawing.Point(72, 7);
            this.MonitoringIndicator.Name = "MonitoringIndicator";
            this.MonitoringIndicator.Size = new System.Drawing.Size(35, 13);
            this.MonitoringIndicator.TabIndex = 6;
            this.MonitoringIndicator.Text = "label1";
            // 
            // ActionsListLabel
            // 
            this.ActionsListLabel.AutoSize = true;
            this.ActionsListLabel.Location = new System.Drawing.Point(7, 53);
            this.ActionsListLabel.Name = "ActionsListLabel";
            this.ActionsListLabel.Size = new System.Drawing.Size(45, 13);
            this.ActionsListLabel.TabIndex = 5;
            this.ActionsListLabel.Text = "Actions:";
            // 
            // NumActionsLabel
            // 
            this.NumActionsLabel.AutoSize = true;
            this.NumActionsLabel.Location = new System.Drawing.Point(237, 28);
            this.NumActionsLabel.Name = "NumActionsLabel";
            this.NumActionsLabel.Size = new System.Drawing.Size(55, 13);
            this.NumActionsLabel.TabIndex = 4;
            this.NumActionsLabel.Text = "# Actions:";
            // 
            // WorkingPathLabel
            // 
            this.WorkingPathLabel.AutoSize = true;
            this.WorkingPathLabel.Location = new System.Drawing.Point(237, 7);
            this.WorkingPathLabel.Name = "WorkingPathLabel";
            this.WorkingPathLabel.Size = new System.Drawing.Size(75, 13);
            this.WorkingPathLabel.TabIndex = 3;
            this.WorkingPathLabel.Text = "Working Path:";
            // 
            // RecordingLabel
            // 
            this.RecordingLabel.AutoSize = true;
            this.RecordingLabel.Location = new System.Drawing.Point(7, 28);
            this.RecordingLabel.Name = "RecordingLabel";
            this.RecordingLabel.Size = new System.Drawing.Size(59, 13);
            this.RecordingLabel.TabIndex = 2;
            this.RecordingLabel.Text = "Recording:";
            // 
            // MonitoringLabel
            // 
            this.MonitoringLabel.AutoSize = true;
            this.MonitoringLabel.Location = new System.Drawing.Point(7, 7);
            this.MonitoringLabel.Name = "MonitoringLabel";
            this.MonitoringLabel.Size = new System.Drawing.Size(59, 13);
            this.MonitoringLabel.TabIndex = 1;
            this.MonitoringLabel.Text = "Monitoring:";
            // 
            // ObserverTab2
            // 
            this.ObserverTab2.Controls.Add(this.ToClipboardIndicator2);
            this.ObserverTab2.Controls.Add(this.ToClipboard2);
            this.ObserverTab2.Controls.Add(this.label2);
            this.ObserverTab2.Controls.Add(this.GeneralizationsLabel);
            this.ObserverTab2.Controls.Add(this.CompressionIndicator);
            this.ObserverTab2.Controls.Add(this.CompressionLabel);
            this.ObserverTab2.Controls.Add(this.GeneralizationsListBox);
            this.ObserverTab2.Location = new System.Drawing.Point(4, 22);
            this.ObserverTab2.Name = "ObserverTab2";
            this.ObserverTab2.Padding = new System.Windows.Forms.Padding(3);
            this.ObserverTab2.Size = new System.Drawing.Size(662, 346);
            this.ObserverTab2.TabIndex = 1;
            this.ObserverTab2.Text = "Observer (Generalizations)";
            this.ObserverTab2.UseVisualStyleBackColor = true;
            // 
            // ToClipboardIndicator2
            // 
            this.ToClipboardIndicator2.AutoSize = true;
            this.ToClipboardIndicator2.Location = new System.Drawing.Point(313, 7);
            this.ToClipboardIndicator2.Name = "ToClipboardIndicator2";
            this.ToClipboardIndicator2.Size = new System.Drawing.Size(27, 13);
            this.ToClipboardIndicator2.TabIndex = 15;
            this.ToClipboardIndicator2.Text = "N/A";
            // 
            // ToClipboard2
            // 
            this.ToClipboard2.AutoSize = true;
            this.ToClipboard2.Location = new System.Drawing.Point(237, 7);
            this.ToClipboard2.Name = "ToClipboard2";
            this.ToClipboard2.Size = new System.Drawing.Size(70, 13);
            this.ToClipboard2.TabIndex = 14;
            this.ToClipboard2.Text = "To Clipboard:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(416, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(240, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Tip: press an item to copy  its info to the clipboard";
            // 
            // GeneralizationsLabel
            // 
            this.GeneralizationsLabel.AutoSize = true;
            this.GeneralizationsLabel.Location = new System.Drawing.Point(7, 32);
            this.GeneralizationsLabel.Name = "GeneralizationsLabel";
            this.GeneralizationsLabel.Size = new System.Drawing.Size(82, 13);
            this.GeneralizationsLabel.TabIndex = 3;
            this.GeneralizationsLabel.Text = "Generalizations:";
            // 
            // CompressionIndicator
            // 
            this.CompressionIndicator.AutoSize = true;
            this.CompressionIndicator.Location = new System.Drawing.Point(83, 7);
            this.CompressionIndicator.Name = "CompressionIndicator";
            this.CompressionIndicator.Size = new System.Drawing.Size(35, 13);
            this.CompressionIndicator.TabIndex = 2;
            this.CompressionIndicator.Text = "label1";
            // 
            // CompressionLabel
            // 
            this.CompressionLabel.AutoSize = true;
            this.CompressionLabel.Location = new System.Drawing.Point(7, 7);
            this.CompressionLabel.Name = "CompressionLabel";
            this.CompressionLabel.Size = new System.Drawing.Size(70, 13);
            this.CompressionLabel.TabIndex = 1;
            this.CompressionLabel.Text = "Compression:";
            // 
            // GeneralizationsListBox
            // 
            this.GeneralizationsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GeneralizationsListBox.FormattingEnabled = true;
            this.GeneralizationsListBox.Location = new System.Drawing.Point(6, 48);
            this.GeneralizationsListBox.Name = "GeneralizationsListBox";
            this.GeneralizationsListBox.Size = new System.Drawing.Size(650, 290);
            this.GeneralizationsListBox.TabIndex = 0;
            // 
            // ApprenticeTab
            // 
            this.ApprenticeTab.Controls.Add(this.label3);
            this.ApprenticeTab.Controls.Add(this.RepetitionsLabel);
            this.ApprenticeTab.Controls.Add(this.RepetitionsListBox);
            this.ApprenticeTab.Controls.Add(this.ToClipboardIndicator3);
            this.ApprenticeTab.Controls.Add(this.ToClipboard3);
            this.ApprenticeTab.Location = new System.Drawing.Point(4, 22);
            this.ApprenticeTab.Name = "ApprenticeTab";
            this.ApprenticeTab.Padding = new System.Windows.Forms.Padding(3);
            this.ApprenticeTab.Size = new System.Drawing.Size(662, 346);
            this.ApprenticeTab.TabIndex = 2;
            this.ApprenticeTab.Text = "Apprentice";
            this.ApprenticeTab.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(416, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(240, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Tip: press an item to copy  its info to the clipboard";
            // 
            // RepetitionsLabel
            // 
            this.RepetitionsLabel.AutoSize = true;
            this.RepetitionsLabel.Location = new System.Drawing.Point(7, 32);
            this.RepetitionsLabel.Name = "RepetitionsLabel";
            this.RepetitionsLabel.Size = new System.Drawing.Size(63, 13);
            this.RepetitionsLabel.TabIndex = 19;
            this.RepetitionsLabel.Text = "Repetitions:";
            // 
            // RepetitionsListBox
            // 
            this.RepetitionsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.RepetitionsListBox.FormattingEnabled = true;
            this.RepetitionsListBox.Location = new System.Drawing.Point(6, 48);
            this.RepetitionsListBox.Name = "RepetitionsListBox";
            this.RepetitionsListBox.Size = new System.Drawing.Size(650, 290);
            this.RepetitionsListBox.TabIndex = 18;
            // 
            // ToClipboardIndicator3
            // 
            this.ToClipboardIndicator3.AutoSize = true;
            this.ToClipboardIndicator3.Location = new System.Drawing.Point(83, 7);
            this.ToClipboardIndicator3.Name = "ToClipboardIndicator3";
            this.ToClipboardIndicator3.Size = new System.Drawing.Size(27, 13);
            this.ToClipboardIndicator3.TabIndex = 17;
            this.ToClipboardIndicator3.Text = "N/A";
            // 
            // ToClipboard3
            // 
            this.ToClipboard3.AutoSize = true;
            this.ToClipboard3.Location = new System.Drawing.Point(7, 7);
            this.ToClipboard3.Name = "ToClipboard3";
            this.ToClipboard3.Size = new System.Drawing.Size(70, 13);
            this.ToClipboard3.TabIndex = 16;
            this.ToClipboard3.Text = "To Clipboard:";
            // 
            // DebugWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 396);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DebugWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Blaze Debug Window";
            this.Load += new System.EventHandler(this.DebugWindow_Load);
            this.tabControl.ResumeLayout(false);
            this.ObserverTab1.ResumeLayout(false);
            this.ObserverTab1.PerformLayout();
            this.ObserverTab2.ResumeLayout(false);
            this.ObserverTab2.PerformLayout();
            this.ApprenticeTab.ResumeLayout(false);
            this.ApprenticeTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage ObserverTab1;
        private System.Windows.Forms.TabPage ObserverTab2;
        private System.Windows.Forms.TabPage ApprenticeTab;
        private System.Windows.Forms.Label MonitoringLabel;
        private System.Windows.Forms.Label WorkingPathLabel;
        private System.Windows.Forms.Label RecordingLabel;
        private System.Windows.Forms.Label NumActionsLabel;
        private System.Windows.Forms.Label NumActionsIndicator;
        private System.Windows.Forms.Label WorkingPathIndicator;
        private System.Windows.Forms.Label RecordingIndicator;
        private System.Windows.Forms.Label MonitoringIndicator;
        private System.Windows.Forms.Label ActionsListLabel;
        private System.Windows.Forms.ListBox ActionsListBox;
        private System.Windows.Forms.ListBox GeneralizationsListBox;
        private System.Windows.Forms.Label GeneralizationsLabel;
        private System.Windows.Forms.Label CompressionIndicator;
        private System.Windows.Forms.Label CompressionLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label ToClipboardIndicator1;
        private System.Windows.Forms.Label ToClipboard1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label ToClipboardIndicator2;
        private System.Windows.Forms.Label ToClipboard2;
        private System.Windows.Forms.Label ToClipboardIndicator3;
        private System.Windows.Forms.Label ToClipboard3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label RepetitionsLabel;
        private System.Windows.Forms.ListBox RepetitionsListBox;

    }
}