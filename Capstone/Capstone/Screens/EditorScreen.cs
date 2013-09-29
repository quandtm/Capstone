using Capstone.Core;
using Capstone.Graphics.Sprites;
using Capstone.Resources;

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
            SpriteRenderer.Instance.Draw();
        }

        public void OnNavigatedTo()
        {
            SpriteRenderer.Instance = _spriteRenderer;
        }

        public void OnNavigatedFrom()
        {
            SpriteRenderer.Instance = null;
        }
    }
}
