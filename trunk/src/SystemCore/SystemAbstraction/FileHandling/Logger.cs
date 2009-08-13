using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SystemCore.SystemAbstraction.FileHandling
{
    public class Logger
    {
        private string _filename = string.Empty;
        FileStream _file = null;
        private TextWriter _writer = null;

        public Logger(string filename)
        {
            _filename = filename;
        }

        public void Write(string format, params object[] arg)
        {
            Monitor.Enter(_file);
            _file = File.Open(_filename, FileMode.Append, FileAccess.Write, FileShare.Read);
            _writer = new StreamWriter(_file, Encoding.UTF8);
            string date = "["+DateTime.Now.ToString()+"] ";
            _writer.Write(string.Concat(date,format), arg);
            _writer.Close();
            Monitor.Exit(_file);
        }

        public void WriteLine(string format, params object[] arg)
        {
            Monitor.Enter(this);
            _file = File.Open(_filename, FileMode.Append, FileAccess.Write, FileShare.Read);
            _writer = new StreamWriter(_file, Encoding.UTF8);
            string date = "[" + DateTime.Now.ToString() + "] ";
            _writer.WriteLine(string.Concat(date, format), arg);
            _writer.Close();
            Monitor.Exit(this);
        }

        public void Clean()
        {
            Monitor.Enter(this);
            _file = File.Open(_filename, FileMode.Create, FileAccess.Write, FileShare.Read);
            _writer = new StreamWriter(_file, Encoding.UTF8);
            _writer.WriteLine("");
            _writer.Close();
            Monitor.Exit(this);
        }

    }
}
