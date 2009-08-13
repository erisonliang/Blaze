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
        public static FileInfo[] Search(string dir_path, List<string> extensions, bool recursive)
        {
            DirectoryInfo root = new DirectoryInfo(dir_path);
            List<FileInfo> subFiles = new List<FileInfo>();
            foreach (FileInfo file in root.GetFiles())
            {
                if (extensions.Contains(file.Extension.ToLower()))
                {
                    subFiles.Add(file);
                }
            }
            if (recursive)
            {
                foreach (DirectoryInfo directory in root.GetDirectories())
                {
                    subFiles.AddRange(Search(directory.FullName, extensions, recursive));
                }
            }
            return subFiles.ToArray();
        }

        public static FileInfo[] Search(string dir_path, List<string> extensions)
        {
            return Search(dir_path, extensions, true);
        }

        public static string[] SearchFullNames(string dir_path, List<string> extensions, bool recursive)
        {
            DirectoryInfo root = new DirectoryInfo(dir_path);
            List<string> subFiles = new List<string>();
            if (extensions.Contains(".*"))
                return SearchFullNames(dir_path, recursive);
            foreach (FileInfo file in root.GetFiles())
            {
                if (extensions.Contains(file.Extension.ToLower()))
                {
                    subFiles.Add(file.FullName);
                }
            }
            if (recursive)
            {
                foreach (DirectoryInfo directory in root.GetDirectories())
                {
                    subFiles.AddRange(SearchFullNames(directory.FullName, extensions, recursive));
                }
            }
            return subFiles.ToArray();
        }

        public static string[] SearchFullNames(string dir_path, List<string> extensions)
        {
            if (extensions.Contains(".*"))
                return SearchFullNames(dir_path, true);
            else
                return SearchFullNames(dir_path, extensions, true);
        }

        public static FileInfo[] Search(string dir_path, bool recursive)
        {
            DirectoryInfo root = new DirectoryInfo(dir_path);
            List<FileInfo> subFiles = new List<FileInfo>();
            foreach (FileInfo file in root.GetFiles())
            {
                subFiles.Add(file);
            }
            if (recursive)
            {
                foreach (DirectoryInfo directory in root.GetDirectories())
                {
                    subFiles.AddRange(Search(directory.FullName, recursive));
                }
            }
            return subFiles.ToArray();
        }

        public static string[] SearchFullNames(string dir_path, bool recursive)
        {
            if (recursive)
                return Directory.GetFiles(dir_path, "*.*", SearchOption.AllDirectories);
            else
                return Directory.GetFiles(dir_path, "*.*", SearchOption.TopDirectoryOnly);
        }
    }
}
