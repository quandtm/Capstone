using System;
using System.Diagnostics;
using Capstone.Resources;
using SharpDX.Toolkit.Graphics;

namespace Capstone.Graphics.Resources
{
    public class Texture : IResource
    {
        public string ResourceKey { get; set; }

        public Texture2D Texture2D { get; internal set; }

        public bool IsLoaded { get; private set; }

        public Texture()
        {
            IsLoaded = false;
        }

        public void Load(ResourceCache cache)
        {
            try
            {
                Texture2D = Texture2D.Load(XamlGraphicsDevice.Instance.ToolkitDevice, ResourceKey);
                IsLoaded = true;
            }
            catch (Exception ex)
            {
                IsLoaded = false;
                Debug.WriteLine("Unable to load Texture {0} - {1}", ResourceKey, ex.Message);
            }
        }

        public void Unload(ResourceCache cache)
        {
            if (Texture2D != null)
            {
                Texture2D.Dispose();
                Texture2D = null;
            }
            IsLoaded = false;
        }
    }
}
