using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Camera
{
    public enum DestinationType { DATA_URL = 0, FILE_URI = 1 }
    public enum PictureSourceType { PHOTOLIBRARY = 0, CAMERA = 1, SAVEDPHOTOALBUM = 2 }
    public enum EncodingType { JPEG = 0,  PNG = 1   }
    public enum PopoverArrowDirection {ARROW_UP = 1, ARROW_DOWN = 2, ARROW_LEFT = 4, ARROW_RIGHT = 8, ARROW_ANY = 15 }
}
