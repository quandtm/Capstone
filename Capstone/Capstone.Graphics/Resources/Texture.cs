using Capstone.Resources;
using SharpDX.Toolkit.Graphics;

namespace Capstone.Graphics.Resources
{
    public class Texture : IResource
    {
        public string ResourceKey { get; set; }

        public Texture2D Texture2D { get; internal set; }

        public void Load(ResourceCache cache)
        {
            Texture2D = Texture2D.Load(XamlGraphicsDevice.Instance.ToolkitDevice, ResourceKey);
        }

        public void Unload(ResourceCache cache)
        {
            if (Texture2D != null)
            {
                Texture2D.Dispose();
                Texture2D = null;
            }
        }
    }
}
