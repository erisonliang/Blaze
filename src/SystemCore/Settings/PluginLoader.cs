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
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using SystemCore.CommonTypes;
using SystemCore.SystemAbstraction.FileHandling;

namespace SystemCore.Settings
{
    public class PluginLoader
    {
        #region Properties
        string _path;
        #endregion

        #region Accessors
        public string Path
        {
            get { return _path; }
        }
        #endregion

        #region Constructors
        public PluginLoader(string path)
        {
            _path = path;
        }
        #endregion

        #region Public Methods
        public List<Plugin> Load()
        {
            List<string> pluginFiles = new List<string>(FileSearcher.SearchFullNames(_path, new List<string>(new string[] { ".dll" }), true, false));

            //List<InterpreterPlugin> interpreterPlugins = new List<InterpreterPlugin>();
            //List<IndexerPlugin> indexerPlugins = new List<IndexerPlugin>();
            List<Plugin> plugins = new List<Plugin>();

            foreach (string dll in pluginFiles)
            {
                //string dll = pluginFiles[i];
                //Type objType = null;
                try
                {
                    // load
                    AssemblyName name = AssemblyName.GetAssemblyName(dll);
                    Assembly assembly = Assembly.Load(name);
                    List<Type> types = new List<Type>(assembly.GetTypes());
                    if (assembly != null)
                        foreach (Type type in types)
                        {
                            try
                            {
                                // create object
                                if (type.GetCustomAttributes(typeof(AutomatorPluginAttribute), false).Length == 1 &&
                                    (type.BaseType.Name == (typeof(InterpreterPlugin)).Name || type.BaseType.Name == (typeof(IndexerPlugin)).Name))
                                {
                                    Plugin plugin = (Plugin)Activator.CreateInstance(type);
                                    plugins.Add(plugin);
                                    //interpreterPlugins.Add((InterpreterPlugin)Activator.CreateInstance(type));
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Could not load " + name + " plugin. Reason: " + ex.ToString());
                            }
                        }
                    //if (assembly != null)
                    //    objType = assembly.GetType("AutomatorTestLib.TextIndexer");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            //return new PluginPackage(indexerPlugins.ToArray(), interpreterPlugins.ToArray());
            return plugins;
        }
        #endregion
    }
}
