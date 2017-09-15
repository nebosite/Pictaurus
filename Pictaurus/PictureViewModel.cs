using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Threading;

namespace Pictaurus
{
    class PictureViewModel
    {
        const string RECENTPICTURES_REGNAME = "RecentPictures";
        public Picture CurrentPicture { get; set; }
        PictureRepository _repository = PictureRepository.Instance;
        List<Picture> _recentPictures = new List<Picture>();
        int _picturePointer = 0;

        public PictureViewModel()
        {
            string[] recentNames = (string[])Application.UserAppDataRegistry.GetValue(RECENTPICTURES_REGNAME);

            if (recentNames != null)
            {
                _repository.LoadRecentHints(recentNames);
                foreach (string name in recentNames)
                {
                    Picture recentPic = _repository.GetPictureByName(name);
                    if (recentPic != null) _recentPictures.Add(recentPic);
                }
                if(_recentPictures.Count > 0)
                {
                    _picturePointer = _recentPictures.Count - 1;
                    CurrentPicture = _recentPictures[_picturePointer];
                }
            }
        }

        public void SaveState()
        {

            List<string> recentNames = new List<string>();
            int i = _recentPictures.Count - 30;
            if (i < 0) i = 0;
            for(; i < _recentPictures.Count; i++)
            {
                recentNames.Add(_recentPictures[i].Name);
            }
            Application.UserAppDataRegistry.SetValue(RECENTPICTURES_REGNAME, recentNames.ToArray());
        }

        internal void GoBack(int steps)
        {
            _picturePointer-= steps;

            if (_picturePointer < 0) _picturePointer = 0;
            if(_recentPictures.Count > 0)  CurrentPicture = _recentPictures[_picturePointer];
            Debug.WriteLine("P: " + _picturePointer);
        }

        internal void GoForward(int steps)
        {
            _picturePointer += steps;

            if (_picturePointer >= _recentPictures.Count)
            {
                _picturePointer = _recentPictures.Count;
                Picture newPicture = _repository.GetRandomPicture();
                if (newPicture != null) _recentPictures.Add(newPicture);
            }

            if (_picturePointer <= _recentPictures.Count) CurrentPicture = _recentPictures[_picturePointer];
            else CurrentPicture = null;
            Debug.WriteLine("P: " + _picturePointer);
        }

        internal void GoUp(int stepSize)
        {
            PictureFolder parent = _recentPictures[_picturePointer].Parent;
            CurrentPicture = parent.GetPrev(_recentPictures[_picturePointer], stepSize);
            _recentPictures[_picturePointer] = CurrentPicture;
        }

        internal void GoDown(int stepSize)
        {
            PictureFolder parent = _recentPictures[_picturePointer].Parent;
            CurrentPicture = parent.GetNext(_recentPictures[_picturePointer], stepSize);
            _recentPictures[_picturePointer] = CurrentPicture;
            
        }
    }
}
