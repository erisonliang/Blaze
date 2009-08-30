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
using System.Threading;
using System.Windows.Forms;
using SystemCore.CommonTypes;
using SystemCore.SystemAbstraction;

namespace Blaze
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static Mutex mutex = new Mutex(true, CommonInfo.GUID + "-running");
        [STAThread]
        static void Main()
        {
            bool test = false;
            try
            {
                test = mutex.WaitOne(TimeSpan.Zero, true);
            }
            catch
            {
                test = true;
            }
            if (test)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainApplication());
                mutex.ReleaseMutex();
                mutex.Close();
            }
            else
            {
                Win32.SendMessageA(
                    (IntPtr)Win32.HWND_BROADCAST,
                    Win32.WM_SHOWME,
                    IntPtr.Zero,
                    IntPtr.Zero);
            }
        }
    }
}