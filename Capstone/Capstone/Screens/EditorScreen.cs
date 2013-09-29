using Capstone.Core;
using Capstone.Graphics;
using Capstone.Graphics.Sprites;
using Capstone.Resources;
using SharpDX;

namespace Capstone.Screens
{
    public class EditorScreen : IScreen
    {
        private readonly SpriteRenderer _spriteRenderer;
        private readonly ResourceCache _cache;
        private readonly EntitySet _entities;

        public EditorScreen()
        {
            _spriteRenderer = new SpriteRenderer();
            _cache = new ResourceCache();
            _entities = new EntitySet();
        }

        public void Initialise()
        {
            SpriteRenderer.Instance = _spriteRenderer;
            _spriteRenderer.Initialise();
            _spriteRenderer.ScreenOffset = new Vector2(0, 100); // Offset for top toolbar

            // TODO: Load things here
        }

        public void Destroy()
        {
            _entities.DestroyAllEntities();
            _entities.CollectGarbage();
        }

        public void Update(double elapsedSeconds)
        {
            _entities.CollectGarbage();
        }

        public void Draw(double elapsedSeconds)
        {
            XamlGraphicsDevice.Instance.Clear();

            // TODO: Draw things here
            SpriteRenderer.Instance.Draw();

            XamlGraphicsDevice.Instance.Present();
        }

        public void OnNavigatedTo()
        {
            SpriteRenderer.Instance = _spriteRenderer;
            XamlGraphicsDevice.Instance.ClearColour = Color.CornflowerBlue;
        }

        public void OnNavigatedFrom()
        {
            SpriteRenderer.Instance = null;
        }
    }
}
