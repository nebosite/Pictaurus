using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pictaurus
{
    public static class Settings
    {
        const string FOLDERS_REG_VALUE_NAME = "FolderList";

        public static List<string> Folders;

        static Settings()
        {
            Folders = new List<string>();
            string folderNames = (string)Application.UserAppDataRegistry.GetValue(FOLDERS_REG_VALUE_NAME);
            if (folderNames == null) Folders.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
            else
            {
                Folders.AddRange(folderNames.Split(';'));
            }
        }

        internal static void SaveSettings()
        {
            Application.UserAppDataRegistry.SetValue(FOLDERS_REG_VALUE_NAME, string.Join(";", Folders.ToArray()));
        }
    }
}
