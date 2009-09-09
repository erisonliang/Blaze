using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCore.SystemAbstraction.FileHandling
{
    public static class FileNameManipulator
    {
        #region Public Methods
        public static string GetFileName(string filepath)
        {
            int i = filepath.LastIndexOf('\\') + 1;
            int f = filepath.LastIndexOf('.') - i;
            if (f < 0)
                return GetFileNameExt(filepath);
            else
                return filepath.Substring(i, f);
        }

        public static string GetFileNameExt(string filepath)
        {
            int i = filepath.LastIndexOf('\\') + 1;
            string ret = filepath.Substring(i);
            return ret;
        }

        public static string GetPath(string filepath)
        {
            int i = filepath.LastIndexOf('\\') + 1;
            if (i < filepath.Length)
                return filepath.Remove(i);
            return filepath;
        }

        public static string GetFolderName(string path)
        {
            string wpath;
            if (path[path.Length - 1] == '\\')
                wpath = path.Substring(0, path.Length - 1);
            else
                wpath = path;
            int i = wpath.LastIndexOf('\\') + 1;
            string ret = wpath.Substring(i);
            return ret;
        }
        #endregion
    }
}
