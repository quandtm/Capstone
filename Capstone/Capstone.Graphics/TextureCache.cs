using System.Collections.Generic;
using SharpDX.Toolkit.Graphics;

namespace Capstone.Graphics
{
    public class TextureCache
    {
        private readonly Dictionary<string, Texture2D> _cache;
        private readonly GraphicsDevice _device;

        public TextureCache(GraphicsDevice toolkitDevice)
        {
            _cache = new Dictionary<string, Texture2D>();
            _device = toolkitDevice;
        }

        public Texture2D Load(string path)
        {
            Texture2D t;
            var key = path.ToLower();
            if (!_cache.TryGetValue(key, out t))
            {
                t = Texture2D.Load(_device, path);
                _cache.Add(key, t);
            }
            return t;
        }
    }
}
