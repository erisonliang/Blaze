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
using SystemCore.SystemAbstraction.FileHandling;
using System.Threading;
using Configurator;

namespace BlazeIndexer
{
    static class Program
    {
        static Mutex mutex = new Mutex(true, CommonInfo.GUID + "-indexer");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
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
                try
                {
                    Indexer indexer = new Indexer();
                    indexer.LoadPlugins();
                    indexer.BuildIndex();
                    indexer.SaveIndex();
                    indexer.Clean();
                    mutex.ReleaseMutex();
                    mutex.Close();
                }
                catch (Exception e)
                {
                    SystemCore.SystemAbstraction.FileHandling.Logger logger = new Logger("BlazeIndexer_error_dump.log");
                    logger.WriteLine(e.ToString());
                    System.Windows.Forms.MessageBox.Show(
                        "BlazeIndexer has crashed: " + Environment.NewLine + Environment.NewLine + e.ToString(), "Error",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    logger = null;
                }
            }
        }
    }
}
