using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp.Wrapper.PhoneGap
{
    public delegate void CameraSuccessCallback(string imageDataOrDataURI);
    public delegate void CameraErrorCallback(string message);
    public static class navigator
    {
        public static class camera {
            public static void getPicture(CameraSuccessCallback success, CameraErrorCallback error, CameraSettings cameraSettings) { }
        }
    }
}
