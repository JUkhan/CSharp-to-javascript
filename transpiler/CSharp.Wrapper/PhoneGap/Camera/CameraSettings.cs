using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Camera;

namespace CSharp.Wrapper.PhoneGap
{
   
   
    public class CameraSettings {        
        public int quality { get; set; }
        public DestinationType destinationType { get; set; }
        public PictureSourceType sourceType { get; set; }
        public bool allowEdit { get; set; }
        public EncodingType encodingType { get; set; }
        public int targetWidth { get; set; }
        public int targetHeight { get; set; }
        public CameraPopoverOptions popoverOptions { get; set; }
    }

    public class CameraPopoverOptions { 
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public PopoverArrowDirection arrowDir { get; set; }
    }
}
