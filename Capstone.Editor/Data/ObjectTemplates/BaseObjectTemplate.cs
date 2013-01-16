using Capstone.Editor.Common;
using Capstone.Engine.Graphics;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Capstone.Editor.Data.ObjectTemplates
{
    public abstract class BaseObjectTemplate : BindableBase
    {
        public ImageSource PreviewImage { get; private set; }
        public string PreviewLabel { get; protected set; }

        protected async Task LoadImages(string path)
        {
            var ddsPath = path + ".dds";
            var previewPath = path + "_preview.png";

            var basePath = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;

            PreviewImage = new BitmapImage(new Uri(Path.Combine(basePath, previewPath)));
            SpriteRenderer.Instance.Preload(Path.Combine(basePath, ddsPath));
        }
    }
}
