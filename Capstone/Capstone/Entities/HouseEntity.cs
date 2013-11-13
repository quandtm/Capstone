using System;
using System.Collections.Generic;
using Capstone.Components;
using Capstone.Graphics.Sprites;

namespace Capstone.Entities
{
    public class HouseEntity : IEntityGenerator
    {
        private const int MaxIndex = 1;
        private static Random r;

        public string TypeName
        {
            get { return "house"; }
        }

        public Core.Entity Generate(Core.EntitySet set, Resources.ResourceCache cache, string entityName, Dictionary<string, object> parameters)
        {
            var e = set.Create(entityName);

            var sprite = e.AddComponent<Sprite>();
            if (r == null)
                r = new Random((int)(DateTime.Now.Ticks % (long)int.MaxValue));
            var path = "Data/Texture/house" + r.Next(500) % (MaxIndex + 1) + ".png";
            sprite.Load(cache, path);
            sprite.Origin = new SharpDX.Vector2(160f / 2f);

            var rot = parameters.GetOrDefault("Rotation", 0f);
            e.Transform.LocalRotation = rot;

            var area = e.AddComponent<ClickArea>();
            area.BaseWidth = 160;
            area.BaseHeight = 160;
            area.OriginX = sprite.Origin.X;
            area.OriginY = sprite.Origin.Y;

            return e;
        }
    }
}
