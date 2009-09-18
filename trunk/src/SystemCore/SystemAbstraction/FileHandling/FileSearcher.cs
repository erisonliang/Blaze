using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;

namespace SystemCore.SystemAbstraction.FileHandling
{
    public static class FileSearcher
    {
        public static string[] SearchFullNames(string dir_path, List<string> extensions, bool recursive, bool include_dirs)
        {
            DirectoryInfo root = new DirectoryInfo(dir_path);
            List<string> items = new List<string>();
            if (extensions.Contains(".*"))
                return SearchFullNames(dir_path, recursive, include_dirs);
            foreach (FileInfo file in root.GetFiles())
            {
                if (extensions.Contains(file.Extension.ToLower()))
                {
                    items.Add(file.FullName);
                }
            }
            if (include_dirs)
            {
                foreach (DirectoryInfo dir in root.GetDirectories())
                {
                    items.Add(dir.FullName + "\\");
                }
            }
            if (recursive)
            {
                foreach (DirectoryInfo directory in root.GetDirectories())
                {
                    items.AddRange(SearchFullNames(directory.FullName, extensions, recursive, include_dirs));
                }
            }
            return items.ToArray();
        }

        public static string[] SearchFullNames(string dir_path, bool recursive, bool include_dirs)
        {
            List<string> items = new List<string>();
            foreach (string file in Directory.GetFiles(dir_path, "*.*", (recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)))
            {
                items.Add(file);
            }
            if (include_dirs)
            {
                foreach (string dir in Directory.GetDirectories(dir_path, "*", (recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)))
                {
                    items.Add(dir + "\\");
                }
            }
            return items.ToArray();
        }
    }
}
