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

        // from: http://mrpmorris.blogspot.com/2007/05/convert-absolute-path-to-relative-path.html
        // edited by me
        public static string RelativePath(string absolutePath, string relativeTo)
        {
            string[] absoluteDirectories = absolutePath.Split('\\');
            string[] relativeDirectories = relativeTo.Split('\\');

            //Get the shortest of the two paths
            int length = absoluteDirectories.Length < relativeDirectories.Length ? absoluteDirectories.Length : relativeDirectories.Length;

            //Use to determine where in the loop we exited
            int lastCommonRoot = -1;
            int index;

            //Find common root
            for (index = 0; index < length; index++)
                if (absoluteDirectories[index] == relativeDirectories[index])
                    lastCommonRoot = index;
                else
                    break;

            //If we didn't find a common prefix then throw
            if (lastCommonRoot == -1)
                return absolutePath;
                //throw new ArgumentException("Paths do not have a common base");

            //Build up the relative path
            StringBuilder relativePath = new StringBuilder();

            //Add on the ..
            for (index = lastCommonRoot + 1; index < relativeDirectories.Length; index++)
                if (relativeDirectories[index].Length > 0)
                    relativePath.Append("..\\");

            //Add on the folders
            for (index = lastCommonRoot + 1; index < absoluteDirectories.Length - 1; index++)
                relativePath.Append(absoluteDirectories[index] + "\\");
            if (index == absoluteDirectories.Length - 1)
                relativePath.Append(absoluteDirectories[absoluteDirectories.Length - 1]);

            if (relativePath.Length == 0)
                relativePath.Append(".\\");

            return relativePath.ToString();
        }

        public static string GetSpecialFolderPath(CSIDL_PROGRAMS prog)
        {
            StringBuilder path = new StringBuilder(260);
            Win32.SHGetSpecialFolderPath(IntPtr.Zero, path, (int)prog, false);
            return path.ToString();
        }
        #endregion
    }
}
