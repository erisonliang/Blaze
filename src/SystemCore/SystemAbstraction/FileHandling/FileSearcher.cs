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
        public static string[] SearchFullNames(string dir_path, List<string> extensions, bool recursive, bool include_dirs, bool relative_path)
        {
            DirectoryInfo root = new DirectoryInfo(dir_path);
            List<string> items = new List<string>();
            if (extensions.Contains(".*"))
                return SearchFullNames(dir_path, recursive, include_dirs, relative_path);
            foreach (FileInfo file in root.GetFiles())
            {
                if (extensions.Contains(file.Extension.ToLower()))
                {
                    items.Add(relative_path ?
                        FileNameManipulator.RelativePath(file.FullName, Environment.CurrentDirectory)
                        : file.FullName);
                }
            }
            if (include_dirs)
            {
                foreach (DirectoryInfo dir in root.GetDirectories())
                {
                    items.Add(relative_path ?
                        FileNameManipulator.RelativePath(dir.FullName + "\\", Environment.CurrentDirectory)
                        : dir.FullName + "\\");
                }
            }
            if (recursive)
            {
                foreach (DirectoryInfo directory in root.GetDirectories())
                {
                    items.AddRange(SearchFullNames(directory.FullName, extensions, recursive, include_dirs, relative_path));
                }
            }
            return items.ToArray();
        }

        public static string[] SearchFullNames(string dir_path, bool recursive, bool include_dirs, bool relative_path)
        {
            List<string> items = new List<string>();
            foreach (string file in Directory.GetFiles(dir_path, "*.*", (recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)))
            {
                items.Add(relative_path ?
                    FileNameManipulator.RelativePath(file, Environment.CurrentDirectory)
                    : file);
            }
            if (include_dirs)
            {
                foreach (string dir in Directory.GetDirectories(dir_path, "*", (recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)))
                {
                    items.Add(relative_path ?
                        FileNameManipulator.RelativePath(dir, Environment.CurrentDirectory)
                        : dir + "\\");
                }
            }
            return items.ToArray();
        }

        public static string GetItemFolder(string path)
        {
            if (Path.GetExtension(path) == ".lnk")
            {
                IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                IWshRuntimeLibrary.IWshShortcut link = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(path);
                if (Directory.Exists(link.TargetPath))
                {
                    string dir_str = string.Empty;
                    DirectoryInfo dir = null;
                    if (link.TargetPath[link.TargetPath.Length - 1] != '\\')
                        dir_str = link.TargetPath + "\\";
                    else
                        dir_str = link.TargetPath;
                    string temp_dir = Path.GetDirectoryName(dir_str);
                    if (temp_dir != null)
                        dir = Directory.GetParent(temp_dir);
                    if (dir != null)
                        return dir.FullName;
                    else
                        return dir_str;
                }
                return Path.GetDirectoryName(link.TargetPath);
            }
            else
            {
                if (Directory.Exists(path))
                {
                    string temp_dir = Path.GetDirectoryName(path + "\\");
                    DirectoryInfo dir = null;
                    if (temp_dir != null)
                        dir = Directory.GetParent(temp_dir);
                    if (dir != null)
                        return dir.FullName;
                    else
                        return path;
                }
                return Path.GetDirectoryName(path);
            }
        }
    }
}
