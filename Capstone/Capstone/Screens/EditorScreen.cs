using System.Collections.Generic;
using Capstone.Core;
using Capstone.Entities;
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

        private readonly Dictionary<string, IEntityGenerator> _generators;

        public EditorScreen()
        {
            _spriteRenderer = new SpriteRenderer();
            _cache = new ResourceCache();
            _entities = new EntitySet();
            _generators = new Dictionary<string, IEntityGenerator>();
            RegisterGenerators();
        }

        private void RegisterGenerators()
        {

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
        }

        public Entity AddObject(string entityTypeName, string entityName, Dictionary<string, object> parameters)
        {
            IEntityGenerator gen;
            if (_generators.TryGetValue(entityTypeName, out gen))
                return gen.Generate(_entities, parameters);
            return null;
        }

        public void RemoveObject(Entity e)
        {
            _entities.Destroy(e);
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
