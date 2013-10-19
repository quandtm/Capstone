using Capstone.Core;
using Capstone.Graphics;
using Capstone.Graphics.Sprites;
using Capstone.Objectives;
using Capstone.Resources;
using SharpDX;

namespace Capstone.Screens
{
    public class EditorScreen : IScreen
    {
        private readonly SpriteRenderer _spriteRenderer;
        private readonly ResourceCache _cache;
        private readonly EntitySet _entities;

        public Camera Camera { get; private set; }

        public ObjectiveManager Objectives { get; private set; }

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

            Objectives = _entities.Create("objectives").AddComponent<ObjectiveManager>();

            var camEntity = _entities.Create("camera");
            Camera = camEntity.AddComponent<Camera>();
            _spriteRenderer.CurrentCamera = Camera;
            Camera.Move(0, -100); // Initial offset to account for top toolbar

            // Load things here
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
            SpriteRenderer.Instance.Draw();
            XamlGraphicsDevice.Instance.Present();
        }

        public void OnNavigatedTo()
        {
            SpriteRenderer.Instance = _spriteRenderer;
            XamlGraphicsDevice.Instance.ClearColour = Color.DeepSkyBlue;
        }

        public void OnNavigatedFrom()
        {
            SpriteRenderer.Instance = null;
        }
    }
}
