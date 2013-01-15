using Capstone.Editor.Common;
using System;
using System.IO;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Capstone.Editor.Data
{
    public class SpritePreview : BindableBase
    {
        public ImageSource Image { get; private set; }
        public string Name { get; private set; }

        private SpritePreview()
        {
        }

        public static SpritePreview Load(string path)
        {
            var fn = Path.GetFileNameWithoutExtension(path);

            var sprite = new SpritePreview();
            sprite.Name = fn;
            sprite.Image = new BitmapImage(new Uri(path));
            return sprite;
        }
    }
}
