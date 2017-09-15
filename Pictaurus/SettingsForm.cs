using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pictaurus
{
    public partial class SettingsForm : Form
    {
        

        public SettingsForm()
        {
            InitializeComponent();
            listBoxFolders.DataSource = Settings.Folders;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Settings.SaveSettings();
            Close();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {

        }

        private void buttonDeleteFolder_Click(object sender, EventArgs e)
        {
            int index = listBoxFolders.SelectedIndex;
            if (index < 0 || index >= Settings.Folders.Count || Settings.Folders.Count == 0) return;
            Settings.Folders.RemoveAt(index);
            RefreshFolderListBox();
        }

        private void RefreshFolderListBox()
        {
            listBoxFolders.DataSource = null;
            listBoxFolders.DataSource = Settings.Folders;
            listBoxFolders.Invalidate();   
        }

        string lastDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

        private void buttonAddFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog picker = new FolderBrowserDialog();
            picker.SelectedPath = lastDirectory;
            
            DialogResult result = picker.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Settings.Folders.Add(picker.SelectedPath);
                lastDirectory = picker.SelectedPath;
                RefreshFolderListBox();
            }

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

    class ObservableList<T> : List<T>, IObservable<T>
    {

        public IDisposable Subscribe(IObserver<T> observer)
        {
            throw new NotImplementedException();
        }
    }
}
