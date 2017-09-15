using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pictaurus
{
    class PictureFolder
    {
        public string Name { get; set; }
        List<Picture> _allPictures = new List<Picture>();

        public PictureFolder(string folderName)
        {
            Name = folderName;
        }

        internal void AddPicture(Picture newPicture)
        {
            newPicture.Index = _allPictures.Count;
            _allPictures.Add(newPicture);
            newPicture.Parent = this;
        }

        internal Picture GetPrev(Picture picture, int stepSize)
        {
            int location = _allPictures.FindIndex(new Predicate<Picture>(p => p.Equals(picture)));
            location -= stepSize;
            if (location < 0) location = 0;
            return _allPictures[location];
        }
        internal Picture GetNext(Picture picture, int stepSize)
        {
            int location = _allPictures.FindIndex(new Predicate<Picture>(p => p.Equals(picture)));
            location += stepSize;
            if (location >= _allPictures.Count) location = _allPictures.Count-1;
            return _allPictures[location];
        }
    }
}
