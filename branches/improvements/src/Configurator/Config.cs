// Blaze: Automated Desktop Experience
// Copyright (C) 2008,2009  Gabriel Barata
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.IO;
using System.Text;

namespace Configurator
{
    public class Config
    {
        #region Properties
        private static readonly Config _instance = new Config();
        private string _dir;
        private string _file;
        #endregion

        #region Accessors
        public static Config Instance { get { return _instance; } }
        public string Dir { get { return _dir; } }
        public string File { get { return _file; } }
        #endregion

        #region Constructors
        private Config()
        {
            _dir = CommonInfo.UserFolder;
            _file = CommonInfo.UserConfigFile;
        }
        #endregion

        #region Public Methods
        public void SetBaseNewDir(string dir)
        {
            //_file = dir;
            //if (_file[_file.Length - 1] != '\\')
            //    _file += '\\';
            //_dir = _file + _dir;
        }

        public void Clear()
        {
            CreateUserDir();
            StreamWriter file = new StreamWriter(_file,false);
            file.Close();
        }

        public bool ConfigFileAlreadyExists()
        {
                return System.IO.File.Exists(_file);
        }

        public void Configure()
        {
            OperatingSystem os = Environment.OSVersion;

            // Create configuration file if needed
            if (!Config.Instance.ConfigFileAlreadyExists())
            {
                Config.Instance.Clear();
                if (CommonInfo.IsPortable)
                {
                    Config.Instance.ConfigureBasic();
                    Config.Instance.ConfigurePortable();
                }
                else
                {
                    switch (os.Version.Major)
                    {
                        case 5:
                            Config.Instance.ConfigureBasic();
                            Config.Instance.ConfigureXP();
                            break;
                        case 6:
                            Config.Instance.ConfigureBasic();
                            Config.Instance.ConfigureVista();
                            break;
                        default:
                            Config.Instance.ConfigureBasic();
                            Config.Instance.ConfigureOther();
                            break;
                    }
                }
            }
        }
        #endregion

        #region Private Methods
        private void CreateUserDir()
        {
            if (!Directory.Exists(_dir))
                Directory.CreateDirectory(_dir);
        }

        private void ConfigureBasic()
        {
            CreateUserDir();
            StreamWriter file = new StreamWriter(_file, true, Encoding.Default);
            file.WriteLine("[interaction]");
            file.WriteLine("hotkeyMainkey=32");
            file.WriteLine("hotkeyModifierAlt=true");
            file.WriteLine("hotkeyModifierCtrl=true");
            file.WriteLine("hotkeyModifierShift=false");
            file.WriteLine("hotkeyModifierWin=false");
            file.WriteLine("[interface]");
            file.WriteLine("suggestions=10");
            file.WriteLine("[system]");
            file.WriteLine(CommonInfo.IsPortable ? "updateTime=0" : "updateTime=20");
            file.WriteLine("stopAutoUpdateOnBattery=true");
            file.WriteLine("autoUpdate=" + (CommonInfo.IsPortable ? "false" : "true"));
            file.WriteLine("[automation]");
            file.WriteLine("monitoringEnabled=true");
            file.WriteLine("stopMonitoringOnBattery=true");
            file.Close();

            //CreateShortcut(@"Utilities\C Drive", @"C:\");
            //CreateShortcut(@"Utilities\Mosca.lnk", @"control");
        }

        private void ConfigureXP()
        {
            CreateUserDir();
            StreamWriter file = new StreamWriter(_file, true, Encoding.Default);
            file.WriteLine("[indexer]");
            WriteDirectory(file, 1, "%ALLUSERSPROFILE%" + @"\Start Menu\", ".lnk", false, true, string.Empty);
            WriteDirectory(file, 2, "%USERPROFILE%" + @"\Start Menu\", ".lnk", false, true, string.Empty);
            WriteDirectory(file, 3, "%USERPROFILE%" + @"\Recent\", ".lnk", false, true, string.Empty);
            WriteDirectory(file, 4, "%APPDATA%" + @"\Microsoft\Internet Explorer\Quick Launch\", ".lnk", false, true, string.Empty);
            WriteDirectory(file, 5, @"C:\Windows\System32\", ".msc", false, false, string.Empty);
            WriteDirectory(file, 6, @"Utilities\", ".lnk", false, true, string.Empty);
            WriteDirectory(file, 7, "%USERPROFILE%" + @"\Favorites\", ".url", false, true, string.Empty);
            file.Close();
        }

        private void ConfigureVista()
        {
            CreateUserDir();
            StreamWriter file = new StreamWriter(_file, true, Encoding.Default);
            file.WriteLine("[indexer]");
            WriteDirectory(file, 1, "%SYSTEMDRIVE%" + @"\ProgramData\Microsoft\Windows\Start Menu\", ".lnk", false, true, string.Empty);
            WriteDirectory(file, 2, "%USERPROFILE%" + @"\AppData\Roaming\Microsoft\Windows\Start Menu\", ".lnk", false, true, string.Empty);
            WriteDirectory(file, 3, "%USERPROFILE%" + @"\AppData\Roaming\Microsoft\Windows\Recent\", ".lnk", false, true, string.Empty);
            WriteDirectory(file, 4, "%APPDATA%" + @"\Microsoft\Internet Explorer\Quick Launch\", ".lnk", false, true, string.Empty);
            WriteDirectory(file, 5, @"C:\Windows\System32\", ".msc", false, false, string.Empty);
            WriteDirectory(file, 6, @"Utilities\", ".lnk", false, true, string.Empty);
            WriteDirectory(file, 7, "%USERPROFILE%" + @"\Favorites\", ".url", false, true, string.Empty);
            file.Close();
        }

        private void ConfigureOther()
        {
            CreateUserDir();
            StreamWriter file = new StreamWriter(_file, true, Encoding.Default);
            file.WriteLine("[indexer]");
            WriteDirectory(file, 1, "%APPDATA%" + @"\Microsoft\Internet Explorer\Quick Launch\", ".lnk", false, true, string.Empty);
            WriteDirectory(file, 2, @"C:\Windows\System32\", ".msc", false, false, string.Empty);
            WriteDirectory(file, 3, @"Utilities\", ".lnk", false, true, string.Empty);
            WriteDirectory(file, 4, "%USERPROFILE%" + @"\Favorites\", ".url", false, true, string.Empty);
            file.Close();
        }

        private void ConfigurePortable()
        {
            CreateUserDir();
            StreamWriter file = new StreamWriter(_file, true, Encoding.Default);
            file.WriteLine("[indexer]");
            WriteDirectory(file, 1, "%ALLUSERSPROFILE%" + @"\Start Menu\", ".lnk", false, true, string.Empty);
            WriteDirectory(file, 2, "%SYSTEMDRIVE%" + @"\ProgramData\Microsoft\Windows\Start Menu\", ".lnk", false, true, string.Empty);
            WriteDirectory(file, 3, "%USERPROFILE%" + @"\Start Menu\", ".lnk", false, true, string.Empty);
            WriteDirectory(file, 4, "%USERPROFILE%" + @"\AppData\Roaming\Microsoft\Windows\Start Menu\", ".lnk", false, true, string.Empty);
            WriteDirectory(file, 5, "%USERPROFILE%" + @"\Recent\", ".lnk", false, true, string.Empty);
            WriteDirectory(file, 6, "%USERPROFILE%" + @"\AppData\Roaming\Microsoft\Windows\Recent\", ".lnk", false, true, string.Empty);
            WriteDirectory(file, 7, "%APPDATA%" + @"\Microsoft\Internet Explorer\Quick Launch\", ".lnk", false, true, string.Empty);
            WriteDirectory(file, 8, @"C:\Windows\System32\", ".msc", false, false, string.Empty);
            WriteDirectory(file, 9, @"Utilities\", ".lnk", false, true, string.Empty);
            WriteDirectory(file, 10, "%USERPROFILE%" + @"\Favorites\", ".url", false, true, string.Empty);
            file.Close();
        }

        private void WriteDirectory(StreamWriter file, int pos, string path, string extensions, bool includeDirectories, bool indexSubfolders, string plugins)
        {
            file.WriteLine(pos.ToString() + @"\name= " + path);
            file.WriteLine(pos.ToString() + @"\extensions= " + extensions);
            file.WriteLine(pos.ToString() + @"\includeDirectories= " + includeDirectories.ToString());
            file.WriteLine(pos.ToString() + @"\indexSubdirectories= " + indexSubfolders.ToString());
            file.WriteLine(pos.ToString() + @"\plugins= "+plugins);
        }
        #endregion
    }
}
