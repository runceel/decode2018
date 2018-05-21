using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Storage;

namespace DemoApp.Services
{
    public class CameraService
    {
        public Task<StorageFile> TakePhotoAsync()
        {
            var c = new CameraCaptureUI();
            return c.CaptureFileAsync(CameraCaptureUIMode.Photo).AsTask();
        }
    }
}
