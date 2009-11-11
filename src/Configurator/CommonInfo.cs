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

namespace Configurator
{
    public static class CommonInfo
    {
        public static bool IsPortable = false;
        public static string GUID = "{258b9f7e-f835-4b65-b833-906667ec51e6}";
        public static string AppName = "Blaze";
        public static string BinFolder = @"Bin\";

        public static string UserFolder = (IsPortable ? @"User\" : Environment.GetEnvironmentVariable("APPDATA") + @"\Blaze\");

        public static string DocFolder = @"Docs\";
        public static string PluginsFolder = @"Plugins\";
        public static string UtilitiesFolder = @"Utilities\";
        public static string ScriptsFolder = @"Scripts\";
        public static string PythonFolder = @"IronPython\";
        public static string PythonTutorialFolder = @"IronPython\Tutorial\";
        public static string PythonLibFolder = @"IronPython\Lib\";
        public static string PythonSitePackagesFolder = @"IronPython\site-packages\";
        public static string PythonPath = @"IronPython\ipy.exe";
        public static string UserConfigFile = UserFolder + @"user.ini";
        public static string IndexCacheFile = UserFolder + @"index.db";
        public static string IndexerPath = @"BlazeIndexer.exe";
        public static string UserDesktop = Environment.GetEnvironmentVariable("USERPROFILE") + @"\Desktop";
        public static string UserHomeDrive = Environment.GetEnvironmentVariable("HOMEDRIVE");
        public static string BlazeWebsite = "http://blaze-wins.sourceforge.net/";
        public static string BlazeVersionUrl = (IsPortable ? "http://blaze-wins.sourceforge.net/latest-portable-version.php" : "http://blaze-wins.sourceforge.net/latest-version.php");
        public static string BlazeDownloadUrl = (IsPortable ? "http://blaze-wins.sourceforge.net/latest-portable-download.php" : "http://blaze-wins.sourceforge.net/latest-download.php");
        public static string BlazeTempPath = System.IO.Path.GetTempPath() + (IsPortable ? "Blaze Update.zip" : "Blaze Update.exe" );
        public static Version BlazeVersion = new Version("0.5.2.3");
        public static string BlazeUpdaterPath = @".\BlazeUpdater.exe";
    }
}
