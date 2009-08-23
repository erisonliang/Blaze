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

namespace SystemCore.CommonTypes
{
    public static class CommonInfo
    {
        public static string GUID = "{258b9f7e-f835-4b65-b833-906667ec51e6}";
        public static string AppName = "Blaze";
        public static string BinFolder = @"Bin\";
        public static string UserFolder = Environment.GetEnvironmentVariable("APPDATA") + @"\Blaze";
        public static string DocFolder = @"Docs\";
        public static string PluginsFolder = @"Plugins\";
        public static string UtilitiesFolder = @"Utilities\";
        public static string ScriptsFolder = @"Scripts\";
        public static string PythonFolder = @"IronPython\";
        public static string PythonTutorialFolder = @"IronPython\Tutorial\";
        public static string PythonLibFolder = @"IronPython\Lib\";
        public static string PythonSitePackagesFolder = @"IronPython\site-packages\";
        public static string PythonPath = @"IronPython\ipy.exe";
        public static string UserConfigFile = Environment.GetEnvironmentVariable("APPDATA") + @"\Blaze\user.ini";
        public static string IndexCacheFile = Environment.GetEnvironmentVariable("APPDATA") + @"\Blaze\index.db";
        public static string IndexerPath = @"BlazeIndexer.exe";
        public static string UserDesktop = Environment.GetEnvironmentVariable("USERPROFILE") + @"\Desktop";
        public static string UserHomeDrive = Environment.GetEnvironmentVariable("HOMEDRIVE");
    }
}