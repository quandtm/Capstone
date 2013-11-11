using System.Collections.Generic;
using Capstone.Components;
using Capstone.Graphics.Sprites;

namespace Capstone.Entities
{
    public class HouseEntity : IEntityGenerator
    {
        public string TypeName
        {
            get { return "house"; }
        }

        public Core.Entity Generate(Core.EntitySet set, Resources.ResourceCache cache, string entityName, Dictionary<string, object> parameters)
        {
            var e = set.Create(entityName);

            var sprite = e.AddComponent<Sprite>();
            sprite.Load(cache, "Data/Texture/house1.png");
            sprite.Origin = new SharpDX.Vector2(160f / 2f);

            var rot = parameters.GetOrDefault("Rotation", 0f);
            e.Transform.LocalRotation = rot;

            var area = e.AddComponent<ClickArea>();
            area.BaseWidth = 180;
            area.BaseHeight = 180;

            return e;
        }
    }
}
