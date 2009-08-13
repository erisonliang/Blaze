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
using System.ComponentModel;
using System.Configuration.Install;
using System.Security.Permissions;
using System.Threading;
using SystemCore.CommonTypes;
using SystemCore.SystemAbstraction;
using Configurator;

namespace InstallerCustomActions
{
    [RunInstaller(true)]
    public partial class CustomInstaller : Installer
    {
        public CustomInstaller()
        {
            InitializeComponent();
        }

        [SecurityPermission(SecurityAction.Demand)]
        public override void Install(System.Collections.IDictionary stateSaver)
        {
            Mutex mutex;
            try
            {
                mutex = new Mutex(true, CommonInfo.GUID+"-running");
                if (!mutex.WaitOne(TimeSpan.Zero, true))
                {
                    Win32.SendMessageA(
                      (IntPtr)Win32.HWND_BROADCAST,
                      Win32.WM_KILLME,
                      IntPtr.Zero,
                      IntPtr.Zero);
                }
                mutex.ReleaseMutex();
                mutex.Close();
            }
            catch (Exception)
            {

            }

            base.Install(stateSaver);
        }

        [SecurityPermission(SecurityAction.Demand)]
        public override void Commit(System.Collections.IDictionary savedState)
        {
            string dir = Context.Parameters["dir"];

            //Config.Instance.SetBaseNewDir(dir);
            //Config.Instance.Configure(); // Create configuration file if needed

            //// Launch Blaze
            ///System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(Config.Instance.File + "Blaze.exe");
            //info.WorkingDirectory = Config.Instance.File;
            //System.Diagnostics.Process.Start(info);
            //info = null;

            base.Commit(savedState);
        }
    }
}