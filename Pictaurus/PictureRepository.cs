using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Pictaurus
{
    public class PictureRepository
    {
        static PictureRepository _theInstance;
        Dictionary<string, PictureFolder> _folders = new Dictionary<string, PictureFolder>();
        List<Picture> _allPictures = new List<Picture>();
        Dictionary<string, Picture> _pictureLookup = new Dictionary<string, Picture>();
        Random _rand = new Random();

        public static PictureRepository Instance
        {
            get
            {
                if (_theInstance == null) _theInstance = new PictureRepository();
                return _theInstance;
            }
        }

        private PictureRepository()
        {
            foreach (string path in Settings.Folders)
            {
                ThreadPool.QueueUserWorkItem(FileLoadWorker, path);
            }         
        }

        void FileLoadWorker(object state)
        {
            try
            {
                AddFiles((string)state);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void AddFiles(string startPath)
        {
            foreach (string file in Directory.GetFiles(startPath))
            {
                string extension = Path.GetExtension(file).ToLower();
                if (extension == ".jpg" || extension == ".bmp" || extension == ".png")
                {
                    AddFile(file);
                }
            };

            foreach (string directory in Directory.GetDirectories(startPath))
            {
                AddFiles(directory);
            }
        }

        private void AddFile(string file)
        {
            lock (_allPictures)
            {
                if (_pictureLookup.ContainsKey(file)) return;
                string folderName = Path.GetDirectoryName(file).ToLower();

                Picture newPicture = new Picture(file);
                if (!_folders.ContainsKey(folderName)) _folders.Add(folderName, new PictureFolder(folderName));

                _allPictures.Add(newPicture);
                _folders[folderName].AddPicture(newPicture);
                _pictureLookup.Add(file, newPicture);
            }
        }

        internal Picture GetRandomPicture()
        {
            lock (_allPictures)
            {
                if (_allPictures.Count == 0) return null;
                int pictureNumber = _rand.Next(_allPictures.Count);
                return _allPictures[pictureNumber];
            }
        }

        internal Picture GetPictureByName(string name)
        {
            lock (_allPictures)
            {
                if (!_pictureLookup.ContainsKey(name)) return null;
                return _pictureLookup[name];
            }
        }

        internal void LoadRecentHints(string[] recentNames)
        {
            foreach (var name in recentNames) AddFile(name);
        }
    }
}
