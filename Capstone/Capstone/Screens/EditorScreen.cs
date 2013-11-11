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

        public ResourceCache Cache { get { return _cache; } }

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
            AddGenerator(new TreeEntity());
            AddGenerator(new HouseEntity());
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

        public Entity CreateEntity(string entityName, float x, float y, float depth)
        {
            var e = _entities.Create(entityName);
            e.Transform.LocalTranslation = new Vector3(x, y, depth);
            return e;
        }

        public Entity AddObject(string entityTypeName, string entityName, float x, float y, float depth, Dictionary<string, object> parameters)
        {
            IEntityGenerator gen;
            if (_generators.TryGetValue(entityTypeName.ToLower(), out gen))
            {
                var e = gen.Generate(_entities, _cache, entityName, parameters);
                e.Transform.LocalTranslation = new Vector3(x, y, depth);
                return e;
            }
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
            XamlGraphicsDevice.Instance.ClearColour = Color.Black;
        }

        public void OnNavigatedFrom()
        {
            SpriteRenderer.Instance = null;
        }

        private void AddGenerator(IEntityGenerator g)
        {
            _generators.Add(g.TypeName.ToLower(), g);
        }
    }
}
