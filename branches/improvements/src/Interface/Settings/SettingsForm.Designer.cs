namespace Blaze
{
    partial class SettingsForm
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
            DirectoriesListBox.SelectedIndexChanged -= DirectoriesListBox_SelectedIndexChanged;
            OptionsListBox.ItemCheck -= OptionsListBox_ItemCheck;
            PluginsListBox.SelectedIndexChanged -= PluginsListBox_SelectedIndexChanged;
            PluginsListBox.ItemCheck -= PluginsListBox_ItemCheck;
            HomePageLink.LinkClicked -= HomePageLink_LinkClicked;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.GeneralTab = new System.Windows.Forms.TabPage();
            this.autoUpdatesGroupBox = new System.Windows.Forms.GroupBox();
            this.autoUpdatesCheckBox = new System.Windows.Forms.CheckBox();
            this.automationGroupBox = new System.Windows.Forms.GroupBox();
            this.stopMonitoringCheckBox = new System.Windows.Forms.CheckBox();
            this.noAutomationRadioButton = new System.Windows.Forms.RadioButton();
            this.yesAutomationRadioButton = new System.Windows.Forms.RadioButton();
            this.monitorLabel = new System.Windows.Forms.Label();
            this.indexingGroupBox = new System.Windows.Forms.GroupBox();
            this.stopIndexingCheckBox = new System.Windows.Forms.CheckBox();
            this.manualUpdatesLabel = new System.Windows.Forms.Label();
            this.updateTimeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.updateTimeLabel = new System.Windows.Forms.Label();
            this.interfaceGroupBox = new System.Windows.Forms.GroupBox();
            this.suggestionsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.suggestionsLabel = new System.Windows.Forms.Label();
            this.InteractionGroupBox = new System.Windows.Forms.GroupBox();
            this.AssistantKeyComboBox = new System.Windows.Forms.ComboBox();
            this.AssistantHotKeyLabel = new System.Windows.Forms.Label();
            this.HotKeyPlusLabel = new System.Windows.Forms.Label();
            this.BlazeHotKeyLabel = new System.Windows.Forms.Label();
            this.MainKeyComboBox = new System.Windows.Forms.ComboBox();
            this.ModifierComboBox = new System.Windows.Forms.ComboBox();
            this.SkinsTab = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.IndexingTab = new System.Windows.Forms.TabPage();
            this.DirectoryOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.OptionsListBox = new System.Windows.Forms.CheckedListBox();
            this.FileTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.RemoveTypeButton = new System.Windows.Forms.Button();
            this.AddTypeButton = new System.Windows.Forms.Button();
            this.FileTypesListBox = new System.Windows.Forms.ListBox();
            this.DirectoriesGroupBox = new System.Windows.Forms.GroupBox();
            this.DirectoriesListBox = new System.Windows.Forms.ListBox();
            this.RemoveDirectoryButton = new System.Windows.Forms.Button();
            this.AddDirectoryButton = new System.Windows.Forms.Button();
            this.PluginsTab = new System.Windows.Forms.TabPage();
            this.PluginInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.ConfigurePluginButton = new System.Windows.Forms.Button();
            this.PluginWebsiteEditableLabel = new System.Windows.Forms.LinkLabel();
            this.PluginDescriptionEditableLayer = new System.Windows.Forms.Label();
            this.PluginVersionEditableLabel = new System.Windows.Forms.Label();
            this.PluginNameEditableLabel = new System.Windows.Forms.Label();
            this.PluginWebsiteLabel = new System.Windows.Forms.Label();
            this.PluginVersionLabel = new System.Windows.Forms.Label();
            this.PluginDescriptionLabel = new System.Windows.Forms.Label();
            this.PluginNameLabel = new System.Windows.Forms.Label();
            this.AvailablePluginsGroupBox = new System.Windows.Forms.GroupBox();
            this.PluginsListBox = new System.Windows.Forms.CheckedListBox();
            this.AboutTab = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.HomePageLink = new System.Windows.Forms.LinkLabel();
            this.HomePageLabel = new System.Windows.Forms.Label();
            this.ThanksToLabel = new System.Windows.Forms.Label();
            this.AutomatorInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.IndexingTimeEditableLabel = new System.Windows.Forms.Label();
            this.IndexedItemsEditableLabel = new System.Windows.Forms.Label();
            this.StartTimeEditableLabel = new System.Windows.Forms.Label();
            this.MemoryEditableLabel = new System.Windows.Forms.Label();
            this.IndexingDuratiomLabel = new System.Windows.Forms.Label();
            this.StartTimeLabel = new System.Windows.Forms.Label();
            this.IndexedItemsLabel = new System.Windows.Forms.Label();
            this.MemoryLabel = new System.Windows.Forms.Label();
            this.AutomatorDescriptionLabel = new System.Windows.Forms.Label();
            this.AutomatorLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.LastIndexLabel = new System.Windows.Forms.Label();
            this.LastIndexEditableLabel = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.GeneralTab.SuspendLayout();
            this.autoUpdatesGroupBox.SuspendLayout();
            this.automationGroupBox.SuspendLayout();
            this.indexingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updateTimeNumericUpDown)).BeginInit();
            this.interfaceGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.suggestionsNumericUpDown)).BeginInit();
            this.InteractionGroupBox.SuspendLayout();
            this.SkinsTab.SuspendLayout();
            this.IndexingTab.SuspendLayout();
            this.DirectoryOptionsGroupBox.SuspendLayout();
            this.FileTypeGroupBox.SuspendLayout();
            this.DirectoriesGroupBox.SuspendLayout();
            this.PluginsTab.SuspendLayout();
            this.PluginInfoGroupBox.SuspendLayout();
            this.AvailablePluginsGroupBox.SuspendLayout();
            this.AboutTab.SuspendLayout();
            this.AutomatorInfoGroupBox.SuspendLayout();
            this.autoUpdatesGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.GeneralTab);
            this.tabControl.Controls.Add(this.SkinsTab);
            this.tabControl.Controls.Add(this.IndexingTab);
            this.tabControl.Controls.Add(this.PluginsTab);
            this.tabControl.Controls.Add(this.AboutTab);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(568, 313);
            this.tabControl.TabIndex = 0;
            // 
            // GeneralTab
            // 
            this.GeneralTab.Controls.Add(this.autoUpdatesGroupBox);
            this.GeneralTab.Controls.Add(this.automationGroupBox);
            this.GeneralTab.Controls.Add(this.indexingGroupBox);
            this.GeneralTab.Controls.Add(this.interfaceGroupBox);
            this.GeneralTab.Controls.Add(this.InteractionGroupBox);
            this.GeneralTab.Location = new System.Drawing.Point(4, 22);
            this.GeneralTab.Name = "GeneralTab";
            this.GeneralTab.Padding = new System.Windows.Forms.Padding(3);
            this.GeneralTab.Size = new System.Drawing.Size(560, 287);
            this.GeneralTab.TabIndex = 3;
            this.GeneralTab.Text = "General";
            this.GeneralTab.UseVisualStyleBackColor = true;
            // 
            // autoUpdatesGroupBox
            // 
            this.autoUpdatesGroupBox.Controls.Add(this.autoUpdatesCheckBox);
            this.autoUpdatesGroupBox.Location = new System.Drawing.Point(208, 107);
            this.autoUpdatesGroupBox.Name = "autoUpdatesGroupBox";
            this.autoUpdatesGroupBox.Size = new System.Drawing.Size(220, 57);
            this.autoUpdatesGroupBox.TabIndex = 4;
            this.autoUpdatesGroupBox.TabStop = false;
            this.autoUpdatesGroupBox.Text = "Automatic Updates";
            // 
            // autoUpdatesCheckBox
            // 
            this.autoUpdatesCheckBox.AutoSize = true;
            this.autoUpdatesCheckBox.Location = new System.Drawing.Point(13, 24);
            this.autoUpdatesCheckBox.Name = "autoUpdatesCheckBox";
            this.autoUpdatesCheckBox.Size = new System.Drawing.Size(200, 17);
            this.autoUpdatesCheckBox.TabIndex = 0;
            this.autoUpdatesCheckBox.Text = "Automatically check for new updates";
            this.autoUpdatesCheckBox.UseVisualStyleBackColor = true;
            // 
            // automationGroupBox
            // 
            this.automationGroupBox.Controls.Add(this.stopMonitoringCheckBox);
            this.automationGroupBox.Controls.Add(this.noAutomationRadioButton);
            this.automationGroupBox.Controls.Add(this.yesAutomationRadioButton);
            this.automationGroupBox.Controls.Add(this.monitorLabel);
            this.automationGroupBox.Location = new System.Drawing.Point(335, 6);
            this.automationGroupBox.Name = "automationGroupBox";
            this.automationGroupBox.Size = new System.Drawing.Size(210, 96);
            this.automationGroupBox.TabIndex = 3;
            this.automationGroupBox.TabStop = false;
            this.automationGroupBox.Text = "Automation";
            // 
            // stopMonitoringCheckBox
            // 
            this.stopMonitoringCheckBox.AutoSize = true;
            this.stopMonitoringCheckBox.Location = new System.Drawing.Point(9, 59);
            this.stopMonitoringCheckBox.Name = "stopMonitoringCheckBox";
            this.stopMonitoringCheckBox.Size = new System.Drawing.Size(274, 17);
            this.stopMonitoringCheckBox.TabIndex = 3;
            this.stopMonitoringCheckBox.Text = "Stop monitoring when my laptop is running on battery";
            this.stopMonitoringCheckBox.UseVisualStyleBackColor = true;
            // 
            // noAutomationRadioButton
            // 
            this.noAutomationRadioButton.AutoSize = true;
            this.noAutomationRadioButton.Location = new System.Drawing.Point(109, 36);
            this.noAutomationRadioButton.Name = "noAutomationRadioButton";
            this.noAutomationRadioButton.Size = new System.Drawing.Size(39, 17);
            this.noAutomationRadioButton.TabIndex = 2;
            this.noAutomationRadioButton.TabStop = true;
            this.noAutomationRadioButton.Text = "No";
            this.noAutomationRadioButton.UseVisualStyleBackColor = true;
            // 
            // yesAutomationRadioButton
            // 
            this.yesAutomationRadioButton.AutoSize = true;
            this.yesAutomationRadioButton.Location = new System.Drawing.Point(109, 12);
            this.yesAutomationRadioButton.Name = "yesAutomationRadioButton";
            this.yesAutomationRadioButton.Size = new System.Drawing.Size(43, 17);
            this.yesAutomationRadioButton.TabIndex = 1;
            this.yesAutomationRadioButton.TabStop = true;
            this.yesAutomationRadioButton.Text = "Yes";
            this.yesAutomationRadioButton.UseVisualStyleBackColor = true;
            // 
            // monitorLabel
            // 
            this.monitorLabel.AutoSize = true;
            this.monitorLabel.Location = new System.Drawing.Point(6, 16);
            this.monitorLabel.Name = "monitorLabel";
            this.monitorLabel.Size = new System.Drawing.Size(97, 13);
            this.monitorLabel.TabIndex = 0;
            this.monitorLabel.Text = "Monitor my activity:";
            // 
            // indexingGroupBox
            // 
            this.indexingGroupBox.Controls.Add(this.stopIndexingCheckBox);
            this.indexingGroupBox.Controls.Add(this.manualUpdatesLabel);
            this.indexingGroupBox.Controls.Add(this.updateTimeNumericUpDown);
            this.indexingGroupBox.Controls.Add(this.updateTimeLabel);
            this.indexingGroupBox.Location = new System.Drawing.Point(7, 171);
            this.indexingGroupBox.Name = "indexingGroupBox";
            this.indexingGroupBox.Size = new System.Drawing.Size(376, 80);
            this.indexingGroupBox.TabIndex = 2;
            this.indexingGroupBox.TabStop = false;
            this.indexingGroupBox.Text = "Indexing";
            // 
            // stopIndexingCheckBox
            // 
            this.stopIndexingCheckBox.AutoSize = true;
            this.stopIndexingCheckBox.Location = new System.Drawing.Point(10, 50);
            this.stopIndexingCheckBox.Name = "stopIndexingCheckBox";
            this.stopIndexingCheckBox.Size = new System.Drawing.Size(292, 17);
            this.stopIndexingCheckBox.TabIndex = 3;
            this.stopIndexingCheckBox.Text = "Stop index updates when my laptop is running on battery";
            this.stopIndexingCheckBox.UseVisualStyleBackColor = true;
            // 
            // manualUpdatesLabel
            // 
            this.manualUpdatesLabel.AutoSize = true;
            this.manualUpdatesLabel.Location = new System.Drawing.Point(228, 25);
            this.manualUpdatesLabel.Name = "manualUpdatesLabel";
            this.manualUpdatesLabel.Size = new System.Drawing.Size(134, 13);
            this.manualUpdatesLabel.TabIndex = 2;
            this.manualUpdatesLabel.Text = "(0 for manual updates only)";
            // 
            // updateTimeNumericUpDown
            // 
            this.updateTimeNumericUpDown.Location = new System.Drawing.Point(173, 23);
            this.updateTimeNumericUpDown.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
            this.updateTimeNumericUpDown.Name = "updateTimeNumericUpDown";
            this.updateTimeNumericUpDown.Size = new System.Drawing.Size(40, 20);
            this.updateTimeNumericUpDown.TabIndex = 1;
            this.updateTimeNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.updateTimeNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // updateTimeLabel
            // 
            this.updateTimeLabel.AutoSize = true;
            this.updateTimeLabel.Location = new System.Drawing.Point(7, 25);
            this.updateTimeLabel.Name = "updateTimeLabel";
            this.updateTimeLabel.Size = new System.Drawing.Size(160, 13);
            this.updateTimeLabel.TabIndex = 0;
            this.updateTimeLabel.Text = "Minutes between index updates:";
            // 
            // interfaceGroupBox
            // 
            this.interfaceGroupBox.Controls.Add(this.suggestionsNumericUpDown);
            this.interfaceGroupBox.Controls.Add(this.suggestionsLabel);
            this.interfaceGroupBox.Location = new System.Drawing.Point(7, 107);
            this.interfaceGroupBox.Name = "interfaceGroupBox";
            this.interfaceGroupBox.Size = new System.Drawing.Size(182, 57);
            this.interfaceGroupBox.TabIndex = 1;
            this.interfaceGroupBox.TabStop = false;
            this.interfaceGroupBox.Text = "Interface";
            // 
            // suggestionsNumericUpDown
            // 
            this.suggestionsNumericUpDown.Location = new System.Drawing.Point(133, 23);
            this.suggestionsNumericUpDown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.suggestionsNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.suggestionsNumericUpDown.Name = "suggestionsNumericUpDown";
            this.suggestionsNumericUpDown.Size = new System.Drawing.Size(40, 20);
            this.suggestionsNumericUpDown.TabIndex = 1;
            this.suggestionsNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.suggestionsNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // suggestionsLabel
            // 
            this.suggestionsLabel.AutoSize = true;
            this.suggestionsLabel.Location = new System.Drawing.Point(7, 25);
            this.suggestionsLabel.Name = "suggestionsLabel";
            this.suggestionsLabel.Size = new System.Drawing.Size(120, 13);
            this.suggestionsLabel.TabIndex = 0;
            this.suggestionsLabel.Text = "Number of Suggestions:";
            // 
            // InteractionGroupBox
            // 
            this.InteractionGroupBox.Controls.Add(this.AssistantKeyComboBox);
            this.InteractionGroupBox.Controls.Add(this.AssistantHotKeyLabel);
            this.InteractionGroupBox.Controls.Add(this.HotKeyPlusLabel);
            this.InteractionGroupBox.Controls.Add(this.BlazeHotKeyLabel);
            this.InteractionGroupBox.Controls.Add(this.MainKeyComboBox);
            this.InteractionGroupBox.Controls.Add(this.ModifierComboBox);
            this.InteractionGroupBox.Location = new System.Drawing.Point(6, 6);
            this.InteractionGroupBox.Name = "InteractionGroupBox";
            this.InteractionGroupBox.Size = new System.Drawing.Size(317, 96);
            this.InteractionGroupBox.TabIndex = 0;
            this.InteractionGroupBox.TabStop = false;
            this.InteractionGroupBox.Text = "Interaction";
            // 
            // AssistantKeyComboBox
            // 
            this.AssistantKeyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AssistantKeyComboBox.FormattingEnabled = true;
            this.AssistantKeyComboBox.Location = new System.Drawing.Point(103, 57);
            this.AssistantKeyComboBox.Name = "AssistantKeyComboBox";
            this.AssistantKeyComboBox.Size = new System.Drawing.Size(90, 21);
            this.AssistantKeyComboBox.TabIndex = 7;
            // 
            // AssistantHotKeyLabel
            // 
            this.AssistantHotKeyLabel.AutoSize = true;
            this.AssistantHotKeyLabel.Location = new System.Drawing.Point(7, 60);
            this.AssistantHotKeyLabel.Name = "AssistantHotKeyLabel";
            this.AssistantHotKeyLabel.Size = new System.Drawing.Size(90, 13);
            this.AssistantHotKeyLabel.TabIndex = 6;
            this.AssistantHotKeyLabel.Text = "Assistant HotKey:";
            // 
            // HotKeyPlusLabel
            // 
            this.HotKeyPlusLabel.AutoSize = true;
            this.HotKeyPlusLabel.Location = new System.Drawing.Point(199, 25);
            this.HotKeyPlusLabel.Name = "HotKeyPlusLabel";
            this.HotKeyPlusLabel.Size = new System.Drawing.Size(13, 13);
            this.HotKeyPlusLabel.TabIndex = 5;
            this.HotKeyPlusLabel.Text = "+";
            // 
            // BlazeHotKeyLabel
            // 
            this.BlazeHotKeyLabel.AutoSize = true;
            this.BlazeHotKeyLabel.Location = new System.Drawing.Point(6, 25);
            this.BlazeHotKeyLabel.Name = "BlazeHotKeyLabel";
            this.BlazeHotKeyLabel.Size = new System.Drawing.Size(74, 13);
            this.BlazeHotKeyLabel.TabIndex = 4;
            this.BlazeHotKeyLabel.Text = "Blaze HotKey:";
            // 
            // MainKeyComboBox
            // 
            this.MainKeyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MainKeyComboBox.FormattingEnabled = true;
            this.MainKeyComboBox.Location = new System.Drawing.Point(218, 22);
            this.MainKeyComboBox.Name = "MainKeyComboBox";
            this.MainKeyComboBox.Size = new System.Drawing.Size(90, 21);
            this.MainKeyComboBox.TabIndex = 3;
            // 
            // ModifierComboBox
            // 
            this.ModifierComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ModifierComboBox.FormattingEnabled = true;
            this.ModifierComboBox.Location = new System.Drawing.Point(103, 22);
            this.ModifierComboBox.Name = "ModifierComboBox";
            this.ModifierComboBox.Size = new System.Drawing.Size(90, 21);
            this.ModifierComboBox.TabIndex = 2;
            // 
            // SkinsTab
            // 
            this.SkinsTab.Controls.Add(this.label1);
            this.SkinsTab.Location = new System.Drawing.Point(4, 22);
            this.SkinsTab.Name = "SkinsTab";
            this.SkinsTab.Padding = new System.Windows.Forms.Padding(3);
            this.SkinsTab.Size = new System.Drawing.Size(560, 287);
            this.SkinsTab.TabIndex = 4;
            this.SkinsTab.Text = "Skins";
            this.SkinsTab.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(181, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(214, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Not yet implemented.";
            // 
            // IndexingTab
            // 
            this.IndexingTab.Controls.Add(this.DirectoryOptionsGroupBox);
            this.IndexingTab.Controls.Add(this.FileTypeGroupBox);
            this.IndexingTab.Controls.Add(this.DirectoriesGroupBox);
            this.IndexingTab.Location = new System.Drawing.Point(4, 22);
            this.IndexingTab.Name = "IndexingTab";
            this.IndexingTab.Padding = new System.Windows.Forms.Padding(3);
            this.IndexingTab.Size = new System.Drawing.Size(560, 287);
            this.IndexingTab.TabIndex = 0;
            this.IndexingTab.Text = "Indexer";
            this.IndexingTab.UseVisualStyleBackColor = true;
            // 
            // DirectoryOptionsGroupBox
            // 
            this.DirectoryOptionsGroupBox.Controls.Add(this.OptionsListBox);
            this.DirectoryOptionsGroupBox.Location = new System.Drawing.Point(396, 174);
            this.DirectoryOptionsGroupBox.Name = "DirectoryOptionsGroupBox";
            this.DirectoryOptionsGroupBox.Size = new System.Drawing.Size(158, 107);
            this.DirectoryOptionsGroupBox.TabIndex = 2;
            this.DirectoryOptionsGroupBox.TabStop = false;
            this.DirectoryOptionsGroupBox.Text = "Options";
            // 
            // OptionsListBox
            // 
            this.OptionsListBox.FormattingEnabled = true;
            this.OptionsListBox.Location = new System.Drawing.Point(6, 19);
            this.OptionsListBox.Name = "OptionsListBox";
            this.OptionsListBox.Size = new System.Drawing.Size(146, 79);
            this.OptionsListBox.TabIndex = 2;
            // 
            // FileTypeGroupBox
            // 
            this.FileTypeGroupBox.Controls.Add(this.RemoveTypeButton);
            this.FileTypeGroupBox.Controls.Add(this.AddTypeButton);
            this.FileTypeGroupBox.Controls.Add(this.FileTypesListBox);
            this.FileTypeGroupBox.Location = new System.Drawing.Point(396, 7);
            this.FileTypeGroupBox.Name = "FileTypeGroupBox";
            this.FileTypeGroupBox.Size = new System.Drawing.Size(158, 160);
            this.FileTypeGroupBox.TabIndex = 1;
            this.FileTypeGroupBox.TabStop = false;
            this.FileTypeGroupBox.Text = "FileTypes";
            // 
            // RemoveTypeButton
            // 
            this.RemoveTypeButton.Location = new System.Drawing.Point(89, 131);
            this.RemoveTypeButton.Name = "RemoveTypeButton";
            this.RemoveTypeButton.Size = new System.Drawing.Size(26, 23);
            this.RemoveTypeButton.TabIndex = 2;
            this.RemoveTypeButton.Text = "-";
            this.RemoveTypeButton.UseVisualStyleBackColor = true;
            this.RemoveTypeButton.Click += new System.EventHandler(this.RemoveTypeButton_Click);
            // 
            // AddTypeButton
            // 
            this.AddTypeButton.Location = new System.Drawing.Point(46, 131);
            this.AddTypeButton.Name = "AddTypeButton";
            this.AddTypeButton.Size = new System.Drawing.Size(26, 23);
            this.AddTypeButton.TabIndex = 1;
            this.AddTypeButton.Text = "+";
            this.AddTypeButton.UseVisualStyleBackColor = true;
            this.AddTypeButton.Click += new System.EventHandler(this.AddTypeButton_Click);
            // 
            // FileTypesListBox
            // 
            this.FileTypesListBox.FormattingEnabled = true;
            this.FileTypesListBox.Location = new System.Drawing.Point(8, 18);
            this.FileTypesListBox.Name = "FileTypesListBox";
            this.FileTypesListBox.Size = new System.Drawing.Size(142, 108);
            this.FileTypesListBox.TabIndex = 0;
            // 
            // DirectoriesGroupBox
            // 
            this.DirectoriesGroupBox.Controls.Add(this.DirectoriesListBox);
            this.DirectoriesGroupBox.Controls.Add(this.RemoveDirectoryButton);
            this.DirectoriesGroupBox.Controls.Add(this.AddDirectoryButton);
            this.DirectoriesGroupBox.Location = new System.Drawing.Point(6, 6);
            this.DirectoriesGroupBox.Name = "DirectoriesGroupBox";
            this.DirectoriesGroupBox.Size = new System.Drawing.Size(384, 275);
            this.DirectoriesGroupBox.TabIndex = 0;
            this.DirectoriesGroupBox.TabStop = false;
            this.DirectoriesGroupBox.Text = "Directories";
            // 
            // DirectoriesListBox
            // 
            this.DirectoriesListBox.FormattingEnabled = true;
            this.DirectoriesListBox.Location = new System.Drawing.Point(9, 19);
            this.DirectoriesListBox.Name = "DirectoriesListBox";
            this.DirectoriesListBox.Size = new System.Drawing.Size(366, 212);
            this.DirectoriesListBox.TabIndex = 2;
            // 
            // RemoveDirectoryButton
            // 
            this.RemoveDirectoryButton.Location = new System.Drawing.Point(223, 246);
            this.RemoveDirectoryButton.Name = "RemoveDirectoryButton";
            this.RemoveDirectoryButton.Size = new System.Drawing.Size(75, 23);
            this.RemoveDirectoryButton.TabIndex = 1;
            this.RemoveDirectoryButton.Text = "Remove";
            this.RemoveDirectoryButton.UseVisualStyleBackColor = true;
            this.RemoveDirectoryButton.Click += new System.EventHandler(this.RemoveDirectoryButton_Click);
            // 
            // AddDirectoryButton
            // 
            this.AddDirectoryButton.Location = new System.Drawing.Point(100, 246);
            this.AddDirectoryButton.Name = "AddDirectoryButton";
            this.AddDirectoryButton.Size = new System.Drawing.Size(75, 23);
            this.AddDirectoryButton.TabIndex = 0;
            this.AddDirectoryButton.Text = "Add";
            this.AddDirectoryButton.UseVisualStyleBackColor = true;
            this.AddDirectoryButton.Click += new System.EventHandler(this.AddDirectoryButton_Click);
            // 
            // PluginsTab
            // 
            this.PluginsTab.Controls.Add(this.PluginInfoGroupBox);
            this.PluginsTab.Controls.Add(this.AvailablePluginsGroupBox);
            this.PluginsTab.Location = new System.Drawing.Point(4, 22);
            this.PluginsTab.Name = "PluginsTab";
            this.PluginsTab.Padding = new System.Windows.Forms.Padding(3);
            this.PluginsTab.Size = new System.Drawing.Size(560, 287);
            this.PluginsTab.TabIndex = 1;
            this.PluginsTab.Text = "Plugins";
            this.PluginsTab.UseVisualStyleBackColor = true;
            // 
            // PluginInfoGroupBox
            // 
            this.PluginInfoGroupBox.Controls.Add(this.ConfigurePluginButton);
            this.PluginInfoGroupBox.Controls.Add(this.PluginWebsiteEditableLabel);
            this.PluginInfoGroupBox.Controls.Add(this.PluginDescriptionEditableLayer);
            this.PluginInfoGroupBox.Controls.Add(this.PluginVersionEditableLabel);
            this.PluginInfoGroupBox.Controls.Add(this.PluginNameEditableLabel);
            this.PluginInfoGroupBox.Controls.Add(this.PluginWebsiteLabel);
            this.PluginInfoGroupBox.Controls.Add(this.PluginVersionLabel);
            this.PluginInfoGroupBox.Controls.Add(this.PluginDescriptionLabel);
            this.PluginInfoGroupBox.Controls.Add(this.PluginNameLabel);
            this.PluginInfoGroupBox.Location = new System.Drawing.Point(197, 7);
            this.PluginInfoGroupBox.Name = "PluginInfoGroupBox";
            this.PluginInfoGroupBox.Size = new System.Drawing.Size(357, 274);
            this.PluginInfoGroupBox.TabIndex = 1;
            this.PluginInfoGroupBox.TabStop = false;
            this.PluginInfoGroupBox.Text = "Plugin Info";
            // 
            // ConfigurePluginButton
            // 
            this.ConfigurePluginButton.Location = new System.Drawing.Point(125, 241);
            this.ConfigurePluginButton.Name = "ConfigurePluginButton";
            this.ConfigurePluginButton.Size = new System.Drawing.Size(110, 23);
            this.ConfigurePluginButton.TabIndex = 8;
            this.ConfigurePluginButton.Text = "Configure Plugin";
            this.ConfigurePluginButton.UseVisualStyleBackColor = true;
            this.ConfigurePluginButton.Click += new System.EventHandler(this.ConfigurePluginButton_Click);
            // 
            // PluginWebsiteEditableLabel
            // 
            this.PluginWebsiteEditableLabel.AutoSize = true;
            this.PluginWebsiteEditableLabel.Location = new System.Drawing.Point(87, 63);
            this.PluginWebsiteEditableLabel.Name = "PluginWebsiteEditableLabel";
            this.PluginWebsiteEditableLabel.Size = new System.Drawing.Size(78, 13);
            this.PluginWebsiteEditableLabel.TabIndex = 7;
            this.PluginWebsiteEditableLabel.TabStop = true;
            this.PluginWebsiteEditableLabel.Text = "Plugin Website";
            // 
            // PluginDescriptionEditableLayer
            // 
            this.PluginDescriptionEditableLayer.AutoSize = true;
            this.PluginDescriptionEditableLayer.Location = new System.Drawing.Point(87, 85);
            this.PluginDescriptionEditableLayer.Name = "PluginDescriptionEditableLayer";
            this.PluginDescriptionEditableLayer.Size = new System.Drawing.Size(92, 13);
            this.PluginDescriptionEditableLayer.TabIndex = 6;
            this.PluginDescriptionEditableLayer.Text = "Plugin Description";
            // 
            // PluginVersionEditableLabel
            // 
            this.PluginVersionEditableLabel.AutoSize = true;
            this.PluginVersionEditableLabel.Location = new System.Drawing.Point(87, 41);
            this.PluginVersionEditableLabel.Name = "PluginVersionEditableLabel";
            this.PluginVersionEditableLabel.Size = new System.Drawing.Size(74, 13);
            this.PluginVersionEditableLabel.TabIndex = 5;
            this.PluginVersionEditableLabel.Text = "Plugin Version";
            // 
            // PluginNameEditableLabel
            // 
            this.PluginNameEditableLabel.AutoSize = true;
            this.PluginNameEditableLabel.Location = new System.Drawing.Point(87, 20);
            this.PluginNameEditableLabel.Name = "PluginNameEditableLabel";
            this.PluginNameEditableLabel.Size = new System.Drawing.Size(67, 13);
            this.PluginNameEditableLabel.TabIndex = 4;
            this.PluginNameEditableLabel.Text = "Plugin Name";
            // 
            // PluginWebsiteLabel
            // 
            this.PluginWebsiteLabel.AutoSize = true;
            this.PluginWebsiteLabel.Location = new System.Drawing.Point(7, 63);
            this.PluginWebsiteLabel.Name = "PluginWebsiteLabel";
            this.PluginWebsiteLabel.Size = new System.Drawing.Size(49, 13);
            this.PluginWebsiteLabel.TabIndex = 3;
            this.PluginWebsiteLabel.Text = "Website:";
            // 
            // PluginVersionLabel
            // 
            this.PluginVersionLabel.AutoSize = true;
            this.PluginVersionLabel.Location = new System.Drawing.Point(7, 41);
            this.PluginVersionLabel.Name = "PluginVersionLabel";
            this.PluginVersionLabel.Size = new System.Drawing.Size(45, 13);
            this.PluginVersionLabel.TabIndex = 2;
            this.PluginVersionLabel.Text = "Version:";
            // 
            // PluginDescriptionLabel
            // 
            this.PluginDescriptionLabel.AutoSize = true;
            this.PluginDescriptionLabel.Location = new System.Drawing.Point(7, 85);
            this.PluginDescriptionLabel.Name = "PluginDescriptionLabel";
            this.PluginDescriptionLabel.Size = new System.Drawing.Size(63, 13);
            this.PluginDescriptionLabel.TabIndex = 1;
            this.PluginDescriptionLabel.Text = "Description:";
            // 
            // PluginNameLabel
            // 
            this.PluginNameLabel.AutoSize = true;
            this.PluginNameLabel.Location = new System.Drawing.Point(7, 20);
            this.PluginNameLabel.Name = "PluginNameLabel";
            this.PluginNameLabel.Size = new System.Drawing.Size(38, 13);
            this.PluginNameLabel.TabIndex = 0;
            this.PluginNameLabel.Text = "Name:";
            // 
            // AvailablePluginsGroupBox
            // 
            this.AvailablePluginsGroupBox.Controls.Add(this.PluginsListBox);
            this.AvailablePluginsGroupBox.Location = new System.Drawing.Point(7, 7);
            this.AvailablePluginsGroupBox.Name = "AvailablePluginsGroupBox";
            this.AvailablePluginsGroupBox.Size = new System.Drawing.Size(183, 274);
            this.AvailablePluginsGroupBox.TabIndex = 0;
            this.AvailablePluginsGroupBox.TabStop = false;
            this.AvailablePluginsGroupBox.Text = "Available Plugins";
            // 
            // PluginsListBox
            // 
            this.PluginsListBox.FormattingEnabled = true;
            this.PluginsListBox.Location = new System.Drawing.Point(11, 20);
            this.PluginsListBox.Name = "PluginsListBox";
            this.PluginsListBox.Size = new System.Drawing.Size(160, 244);
            this.PluginsListBox.TabIndex = 0;
            // 
            // AboutTab
            // 
            this.AboutTab.Controls.Add(this.label2);
            this.AboutTab.Controls.Add(this.HomePageLink);
            this.AboutTab.Controls.Add(this.HomePageLabel);
            this.AboutTab.Controls.Add(this.ThanksToLabel);
            this.AboutTab.Controls.Add(this.AutomatorInfoGroupBox);
            this.AboutTab.Controls.Add(this.AutomatorDescriptionLabel);
            this.AboutTab.Controls.Add(this.AutomatorLabel);
            this.AboutTab.Location = new System.Drawing.Point(4, 22);
            this.AboutTab.Name = "AboutTab";
            this.AboutTab.Padding = new System.Windows.Forms.Padding(3);
            this.AboutTab.Size = new System.Drawing.Size(560, 287);
            this.AboutTab.TabIndex = 2;
            this.AboutTab.Text = "About";
            this.AboutTab.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(327, 236);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Copyright © 2009 Gabriel Barata";
            // 
            // HomePageLink
            // 
            this.HomePageLink.AutoSize = true;
            this.HomePageLink.Location = new System.Drawing.Point(179, 127);
            this.HomePageLink.Name = "HomePageLink";
            this.HomePageLink.Size = new System.Drawing.Size(169, 13);
            this.HomePageLink.TabIndex = 5;
            this.HomePageLink.TabStop = true;
            this.HomePageLink.Text = "http://blaze-wins.sourceforge.net/";
            // 
            // HomePageLabel
            // 
            this.HomePageLabel.AutoSize = true;
            this.HomePageLabel.Location = new System.Drawing.Point(75, 127);
            this.HomePageLabel.Name = "HomePageLabel";
            this.HomePageLabel.Size = new System.Drawing.Size(106, 13);
            this.HomePageLabel.TabIndex = 4;
            this.HomePageLabel.Text = "Blaze\'s homepage is:";
            // 
            // ThanksToLabel
            // 
            this.ThanksToLabel.AutoSize = true;
            this.ThanksToLabel.Location = new System.Drawing.Point(325, 259);
            this.ThanksToLabel.Name = "ThanksToLabel";
            this.ThanksToLabel.Size = new System.Drawing.Size(229, 13);
            this.ThanksToLabel.TabIndex = 3;
            this.ThanksToLabel.Text = "Thanks a lot naT, for the skin and the art work.";
            // 
            // AutomatorInfoGroupBox
            // 
            this.AutomatorInfoGroupBox.Controls.Add(this.LastIndexEditableLabel);
            this.AutomatorInfoGroupBox.Controls.Add(this.IndexingTimeEditableLabel);
            this.AutomatorInfoGroupBox.Controls.Add(this.LastIndexLabel);
            this.AutomatorInfoGroupBox.Controls.Add(this.IndexedItemsEditableLabel);
            this.AutomatorInfoGroupBox.Controls.Add(this.StartTimeEditableLabel);
            this.AutomatorInfoGroupBox.Controls.Add(this.MemoryEditableLabel);
            this.AutomatorInfoGroupBox.Controls.Add(this.IndexingDuratiomLabel);
            this.AutomatorInfoGroupBox.Controls.Add(this.StartTimeLabel);
            this.AutomatorInfoGroupBox.Controls.Add(this.IndexedItemsLabel);
            this.AutomatorInfoGroupBox.Controls.Add(this.MemoryLabel);
            this.AutomatorInfoGroupBox.Location = new System.Drawing.Point(6, 152);
            this.AutomatorInfoGroupBox.Name = "AutomatorInfoGroupBox";
            this.AutomatorInfoGroupBox.Size = new System.Drawing.Size(234, 129);
            this.AutomatorInfoGroupBox.TabIndex = 2;
            this.AutomatorInfoGroupBox.TabStop = false;
            this.AutomatorInfoGroupBox.Text = "Info";
            // 
            // IndexingTimeEditableLabel
            // 
            this.IndexingTimeEditableLabel.AutoSize = true;
            this.IndexingTimeEditableLabel.Location = new System.Drawing.Point(97, 83);
            this.IndexingTimeEditableLabel.Name = "IndexingTimeEditableLabel";
            this.IndexingTimeEditableLabel.Size = new System.Drawing.Size(47, 13);
            this.IndexingTimeEditableLabel.TabIndex = 7;
            this.IndexingTimeEditableLabel.Text = "Duration";
            // 
            // IndexedItemsEditableLabel
            // 
            this.IndexedItemsEditableLabel.AutoSize = true;
            this.IndexedItemsEditableLabel.Location = new System.Drawing.Point(97, 106);
            this.IndexedItemsEditableLabel.Name = "IndexedItemsEditableLabel";
            this.IndexedItemsEditableLabel.Size = new System.Drawing.Size(32, 13);
            this.IndexedItemsEditableLabel.TabIndex = 6;
            this.IndexedItemsEditableLabel.Text = "Items";
            // 
            // StartTimeEditableLabel
            // 
            this.StartTimeEditableLabel.AutoSize = true;
            this.StartTimeEditableLabel.Location = new System.Drawing.Point(97, 38);
            this.StartTimeEditableLabel.Name = "StartTimeEditableLabel";
            this.StartTimeEditableLabel.Size = new System.Drawing.Size(30, 13);
            this.StartTimeEditableLabel.TabIndex = 5;
            this.StartTimeEditableLabel.Text = "Time";
            // 
            // MemoryEditableLabel
            // 
            this.MemoryEditableLabel.AutoSize = true;
            this.MemoryEditableLabel.Location = new System.Drawing.Point(97, 16);
            this.MemoryEditableLabel.Name = "MemoryEditableLabel";
            this.MemoryEditableLabel.Size = new System.Drawing.Size(44, 13);
            this.MemoryEditableLabel.TabIndex = 4;
            this.MemoryEditableLabel.Text = "Memory";
            // 
            // IndexingDuratiomLabel
            // 
            this.IndexingDuratiomLabel.AutoSize = true;
            this.IndexingDuratiomLabel.Location = new System.Drawing.Point(6, 83);
            this.IndexingDuratiomLabel.Name = "IndexingDuratiomLabel";
            this.IndexingDuratiomLabel.Size = new System.Drawing.Size(93, 13);
            this.IndexingDuratiomLabel.TabIndex = 3;
            this.IndexingDuratiomLabel.Text = "Indexing Duration:";
            // 
            // StartTimeLabel
            // 
            this.StartTimeLabel.AutoSize = true;
            this.StartTimeLabel.Location = new System.Drawing.Point(6, 38);
            this.StartTimeLabel.Name = "StartTimeLabel";
            this.StartTimeLabel.Size = new System.Drawing.Size(58, 13);
            this.StartTimeLabel.TabIndex = 3;
            this.StartTimeLabel.Text = "Start Time:";
            // 
            // IndexedItemsLabel
            // 
            this.IndexedItemsLabel.AutoSize = true;
            this.IndexedItemsLabel.Location = new System.Drawing.Point(6, 106);
            this.IndexedItemsLabel.Name = "IndexedItemsLabel";
            this.IndexedItemsLabel.Size = new System.Drawing.Size(76, 13);
            this.IndexedItemsLabel.TabIndex = 3;
            this.IndexedItemsLabel.Text = "Indexed Items:";
            // 
            // MemoryLabel
            // 
            this.MemoryLabel.AutoSize = true;
            this.MemoryLabel.Location = new System.Drawing.Point(6, 16);
            this.MemoryLabel.Name = "MemoryLabel";
            this.MemoryLabel.Size = new System.Drawing.Size(75, 13);
            this.MemoryLabel.TabIndex = 3;
            this.MemoryLabel.Text = "Used Memory:";
            // 
            // AutomatorDescriptionLabel
            // 
            this.AutomatorDescriptionLabel.AutoSize = true;
            this.AutomatorDescriptionLabel.Location = new System.Drawing.Point(75, 58);
            this.AutomatorDescriptionLabel.Name = "AutomatorDescriptionLabel";
            this.AutomatorDescriptionLabel.Size = new System.Drawing.Size(108, 13);
            this.AutomatorDescriptionLabel.TabIndex = 1;
            this.AutomatorDescriptionLabel.Text = "AutomatorDescription";
            // 
            // AutomatorLabel
            // 
            this.AutomatorLabel.AutoSize = true;
            this.AutomatorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AutomatorLabel.Location = new System.Drawing.Point(206, 26);
            this.AutomatorLabel.Name = "AutomatorLabel";
            this.AutomatorLabel.Size = new System.Drawing.Size(142, 24);
            this.AutomatorLabel.TabIndex = 0;
            this.AutomatorLabel.Text = "AutomatorLabel";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(424, 331);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(505, 331);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // LastIndexLabel
            // 
            this.LastIndexLabel.AutoSize = true;
            this.LastIndexLabel.Location = new System.Drawing.Point(6, 61);
            this.LastIndexLabel.Name = "LastIndexLabel";
            this.LastIndexLabel.Size = new System.Drawing.Size(73, 13);
            this.LastIndexLabel.TabIndex = 8;
            this.LastIndexLabel.Text = "Last Indexing:";
            // 
            // LastIndexEditableLabel
            // 
            this.LastIndexEditableLabel.AutoSize = true;
            this.LastIndexEditableLabel.Location = new System.Drawing.Point(97, 61);
            this.LastIndexEditableLabel.Name = "LastIndexEditableLabel";
            this.LastIndexEditableLabel.Size = new System.Drawing.Size(58, 13);
            this.LastIndexEditableLabel.TabIndex = 9;
            this.LastIndexEditableLabel.Text = "Liast Index";
            // 
            this.autoUpdatesCheckBox.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(592, 366);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.tabControl.ResumeLayout(false);
            this.GeneralTab.ResumeLayout(false);
            this.autoUpdatesGroupBox.ResumeLayout(false);
            this.autoUpdatesGroupBox.PerformLayout();
            this.automationGroupBox.ResumeLayout(false);
            this.automationGroupBox.PerformLayout();
            this.indexingGroupBox.ResumeLayout(false);
            this.indexingGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updateTimeNumericUpDown)).EndInit();
            this.interfaceGroupBox.ResumeLayout(false);
            this.interfaceGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.suggestionsNumericUpDown)).EndInit();
            this.InteractionGroupBox.ResumeLayout(false);
            this.InteractionGroupBox.PerformLayout();
            this.SkinsTab.ResumeLayout(false);
            this.SkinsTab.PerformLayout();
            this.IndexingTab.ResumeLayout(false);
            this.DirectoryOptionsGroupBox.ResumeLayout(false);
            this.FileTypeGroupBox.ResumeLayout(false);
            this.DirectoriesGroupBox.ResumeLayout(false);
            this.PluginsTab.ResumeLayout(false);
            this.PluginInfoGroupBox.ResumeLayout(false);
            this.PluginInfoGroupBox.PerformLayout();
            this.AvailablePluginsGroupBox.ResumeLayout(false);
            this.AboutTab.ResumeLayout(false);
            this.AboutTab.PerformLayout();
            this.AutomatorInfoGroupBox.ResumeLayout(false);
            this.AutomatorInfoGroupBox.PerformLayout();
            this.autoUpdatesGroupBox.ResumeLayout(false);
            this.autoUpdatesGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage IndexingTab;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TabPage PluginsTab;
        private System.Windows.Forms.TabPage AboutTab;
        private System.Windows.Forms.TabPage GeneralTab;
        private System.Windows.Forms.TabPage SkinsTab;
        private System.Windows.Forms.GroupBox InteractionGroupBox;
        private System.Windows.Forms.Label BlazeHotKeyLabel;
        private System.Windows.Forms.Label AutomatorLabel;
        private System.Windows.Forms.GroupBox DirectoriesGroupBox;
        private System.Windows.Forms.ListBox DirectoriesListBox;
        private System.Windows.Forms.Button RemoveDirectoryButton;
        private System.Windows.Forms.Button AddDirectoryButton;
        private System.Windows.Forms.GroupBox FileTypeGroupBox;
        private System.Windows.Forms.Button RemoveTypeButton;
        private System.Windows.Forms.Button AddTypeButton;
        private System.Windows.Forms.ListBox FileTypesListBox;
        private System.Windows.Forms.GroupBox DirectoryOptionsGroupBox;
        private System.Windows.Forms.ComboBox MainKeyComboBox;
        private System.Windows.Forms.Label HotKeyPlusLabel;
        private System.Windows.Forms.ComboBox ModifierComboBox;
        private System.Windows.Forms.GroupBox AvailablePluginsGroupBox;
        private System.Windows.Forms.CheckedListBox PluginsListBox;
        private System.Windows.Forms.GroupBox PluginInfoGroupBox;
        private System.Windows.Forms.Label PluginVersionLabel;
        private System.Windows.Forms.Label PluginDescriptionLabel;
        private System.Windows.Forms.Label PluginNameLabel;
        private System.Windows.Forms.LinkLabel PluginWebsiteEditableLabel;
        private System.Windows.Forms.Label PluginDescriptionEditableLayer;
        private System.Windows.Forms.Label PluginVersionEditableLabel;
        private System.Windows.Forms.Label PluginNameEditableLabel;
        private System.Windows.Forms.Label PluginWebsiteLabel;
        private System.Windows.Forms.Button ConfigurePluginButton;
        private System.Windows.Forms.CheckedListBox OptionsListBox;
        private System.Windows.Forms.Label AutomatorDescriptionLabel;
        private System.Windows.Forms.GroupBox AutomatorInfoGroupBox;
        private System.Windows.Forms.Label MemoryEditableLabel;
        private System.Windows.Forms.Label IndexingDuratiomLabel;
        private System.Windows.Forms.Label StartTimeLabel;
        private System.Windows.Forms.Label IndexedItemsLabel;
        private System.Windows.Forms.Label MemoryLabel;
        private System.Windows.Forms.Label IndexingTimeEditableLabel;
        private System.Windows.Forms.Label IndexedItemsEditableLabel;
        private System.Windows.Forms.Label StartTimeEditableLabel;
        private System.Windows.Forms.GroupBox interfaceGroupBox;
        private System.Windows.Forms.Label suggestionsLabel;
        private System.Windows.Forms.GroupBox indexingGroupBox;
        private System.Windows.Forms.NumericUpDown updateTimeNumericUpDown;
        private System.Windows.Forms.Label updateTimeLabel;
        private System.Windows.Forms.NumericUpDown suggestionsNumericUpDown;
        private System.Windows.Forms.Label ThanksToLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel HomePageLink;
        private System.Windows.Forms.Label HomePageLabel;
        private System.Windows.Forms.Label AssistantHotKeyLabel;
        private System.Windows.Forms.ComboBox AssistantKeyComboBox;
        private System.Windows.Forms.Label manualUpdatesLabel;
        private System.Windows.Forms.CheckBox stopIndexingCheckBox;
        private System.Windows.Forms.GroupBox automationGroupBox;
        private System.Windows.Forms.RadioButton noAutomationRadioButton;
        private System.Windows.Forms.RadioButton yesAutomationRadioButton;
        private System.Windows.Forms.Label monitorLabel;
        private System.Windows.Forms.CheckBox stopMonitoringCheckBox;
        private System.Windows.Forms.GroupBox autoUpdatesGroupBox;
        private System.Windows.Forms.CheckBox autoUpdatesCheckBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label LastIndexLabel;
        private System.Windows.Forms.Label LastIndexEditableLabel;
    }
}