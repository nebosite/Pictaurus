using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pictaurus
{
    class Picture
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public PictureFolder Parent { get; set; }
        public Picture(string fileName) 
        {
            this.Name = fileName;  
        }
    }
}
