using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Labs;
using Xamarin.Forms.Labs.Services;
using Xamarin.Forms.Labs.Services.Media;
namespace Stackoverflow.Views
{
    public class mediapicker : ContentPage
    {
        private readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private IMediaPicker _mediaPicker;
        private ImageSource _imageSource;
        Image img;

        private void Setup()
        {
            if (_mediaPicker != null)
            {
                return;
            }

            var device = Resolver.Resolve<IDevice>();

            _mediaPicker = DependencyService.Get<IMediaPicker>() ?? device.MediaPicker;
        }

        public mediapicker()
        {
            _mediaPicker = DependencyService.Get<IMediaPicker>();

            Button button = new Button()
            {
                Text = "Select Picture",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            img = new Image();

            button.Clicked += async (object sender, EventArgs e) =>
            {
                var action = DisplayActionSheet(null, "Cancel", null, "Gallery", "Camera");
                switch (await action)
                {
                    case "Gallery": await SelectPicture();
                        break;
                    case "Camera": await TakePicture();
                        break;
                    default:
                        break;
                }

            };

            Content = new StackLayout { Children = { button, img }, Orientation = StackOrientation.Vertical, VerticalOptions = LayoutOptions.FillAndExpand };
        }
        private async Task SelectPicture()
        {
            Setup();

            ImageSource = null;
            try
            {
                var mediaFile = await this._mediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions
                {
                    DefaultCamera = CameraDevice.Front
                });
                ImageSource = ImageSource.FromStream(() => mediaFile.Source);
            }
            catch (System.Exception ex)
            {

            }

        }
        public ImageSource ImageSource
        {
            get
            {
                return _imageSource;
            }
            set
            {
                // SetProperty(ref _imageSource, value);
                _imageSource = value;
                img.Source = ImageSource;
            }
        }
        private async Task TakePicture()
        {
            Setup();

            ImageSource = null;

            await this._mediaPicker.TakePhotoAsync(new CameraMediaStorageOptions { DefaultCamera = CameraDevice.Front, MaxPixelDimension = 400 }).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    var s = t.Exception.InnerException.ToString();
                }
                else if (t.IsCanceled)
                {
                    var canceled = true;
                }
                else
                {
                    var mediaFile = t.Result;

                    ImageSource = ImageSource.FromStream(() => mediaFile.Source);

                    return mediaFile;
                }

                return null;
            }, _scheduler);
        }
    }
}
