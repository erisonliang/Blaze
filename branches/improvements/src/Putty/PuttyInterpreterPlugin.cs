using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using SystemCore.CommonTypes;
using Configurator;
using Microsoft.Win32;
using System.Text;

/* Icon taken from http://tooparannoyed.deviantart.com/art/PuTTY-icon-74659186 */

namespace Putty
{
    [AutomatorPlugin("Putty: autocomplete for saved sessions")]
    public class PuttyInterpreterPlugin : InterpreterPlugin
    {
        private const string _Name = "Putty";

        public class Configuration
        {
            public string PuttyPath { get; set; }

            public Configuration()
            {
                PuttyPath = "putty.exe";
            }
        }
        private Configuration _configuration = new Configuration();
        private string ConfigurationFileName = CommonInfo.UserFolder + _Name + ".xml";

        public PuttyInterpreterPlugin() : base("adds putty saved sessions")
        {
            _configurable = true;
        }

        public override void Configure()
        {
            var dialog = new OpenFileDialog
                             {
                                 FileName = "putty.exe",
                                 Filter = "Putty executable (putty.exe)|putty.exe",
                                 Multiselect = false,
                                 RestoreDirectory = true,
                                 CheckFileExists = true
                             };

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                _configuration.PuttyPath = dialog.FileName;
                SaveSettings();
            }
            
        }

        private void SaveSettings()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
            using(TextWriter writer = new StreamWriter(ConfigurationFileName, false, Encoding.Default))
            {
                serializer.Serialize(writer, _configuration);
            }
        }

        protected override void SetupCommands()
        {
            LoadSettings();
            var sessionsKey = Registry.CurrentUser.OpenSubKey(@"Software\SimonTatham\PuTTY\Sessions");

            if (sessionsKey == null) return;

            var sessionNames = sessionsKey.GetSubKeyNames();
            foreach (var key in sessionNames)
            {
                string args = key;
                var putty_command = new Command("putty " + key, "Connect to " + key);
                putty_command.SetIsOwnerDelegate(str =>
                                                     {
                                                         var split = str.Split(new []{' '},1);
                                                         return split.Length > 0 &&
                                                                split[0].Trim().ToLowerInvariant() == "putty";
                                                     });
                putty_command.SetNameDelegate(str => str);
                putty_command.SetDescriptionDelegate( str => putty_command.Description);
                putty_command.SetIconDelegate(str => Resources.Icon.ToBitmap());
                putty_command.SetAutoCompleteDelegate(str => str);
                putty_command.SetUsageDelegate(str => new CommandUsage("putty <session>"));
                putty_command.SetExecuteDelegate(
                    (parameters, modifiers) => System.Diagnostics.Process.Start(_configuration.PuttyPath,
                                                                                string.Format("-load {0}", args)));
                _commands.Add(putty_command);
            }
        }

        private void LoadSettings()
        {
            _configuration = new Configuration();
            if (File.Exists(ConfigurationFileName))
            {
                var serializer = new XmlSerializer(typeof (Configuration));
                _configuration = (Configuration) serializer.Deserialize(new StreamReader(ConfigurationFileName, Encoding.Default));
            }
        }

        protected override string GetAssembyName()
        {
            return _Name;
        }

        protected override string GetAssemblyVersion()
        {
            return "1.0.0.0";
        }
    }
}
