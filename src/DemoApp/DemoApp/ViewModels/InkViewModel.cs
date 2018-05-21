using System;

using DemoApp.Helpers;
using DemoApp.Services;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace DemoApp.ViewModels
{
    public class InkViewModel : Observable
    {
        private RelayCommand _takePhotoCommand;

        public RelayCommand TakePhotoCommand => _takePhotoCommand ??
            (_takePhotoCommand = new RelayCommand(TakePhotoExecute));

        private ImageSource _photo;
        public ImageSource Photo
        {
            get { return _photo; }
            set { Set(ref _photo, value); }
        }

        public InkViewModel()
        {
        }

        private async void TakePhotoExecute()
        {
            var file = await Singleton<CameraService>.Instance.TakePhotoAsync();
            if (file == null)
            {
                return;
            }

            using (var s = await file.OpenReadAsync())
            {
                var bmp = new BitmapImage();
                await bmp.SetSourceAsync(s);
                Photo = bmp;
            }
        }



    }
}
