using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCore.SystemAbstraction.FileHandling
{
    public static class INIManipulator
    {
        #region Public Methods
        public static List<string> GetCategories(string file)
        {
            string returnString = new string(' ', 65536);
            Win32.GetPrivateProfileString(null, null, null, returnString, 65536, file);
            List<string> result = new List<string>(returnString.Split('\0'));
            result.RemoveRange(result.Count - 2, 2);
            return result;
        }

        public static List<string> GetKeys(string file, string category)
        {
            string returnString = new string(' ', 32768);
            Win32.GetPrivateProfileString(category, null, null, returnString, 32768, file);
            List<string> result = new List<string>(returnString.Split('\0'));
            result.RemoveRange(result.Count-2,2);
            return result;
        }

        public static string GetValue(string file, string category, string key, string defaultValue)
        {
            string returnString = new string(' ', 1024);
            Win32.GetPrivateProfileString(category, key, defaultValue, returnString, 1024, file);
            return returnString.Split('\0')[0];
        }

        public static void WriteValue(string file, string category, string key, string value)
        {
            Win32.WritePrivateProfileString(category, key, value, file);
        }

        public static void DeteleKey(string file, string category, string key)
        {
            Win32.WritePrivateProfileString(category, key, null, file);
        }

        public static void DeleteCategory(string file, string category)
        {
            Win32.WritePrivateProfileString(category, null, null, file);
        }
        #endregion
    }
}
