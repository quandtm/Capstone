using System;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Capstone.Editor.Data.ComponentTemplates
{
    public class TextureTemplate : ComponentTemplate
    {
        private string _texPath;

        public string TexturePath
        {
            get { return _texPath; }
            set
            {
                if (_texPath == value) return;
                _texPath = value;
                Preview = new BitmapImage(new Uri(_texPath));
            }
        }
        public ImageSource Preview { get; private set; }

        public TextureTemplate()
            : base("Texture")
        {
        }

        public override ComponentTemplate Clone()
        {
            return new TextureTemplate();
        }
    }
}
