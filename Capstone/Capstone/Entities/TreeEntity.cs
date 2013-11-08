using System.Collections.Generic;
using Capstone.Components;
using Capstone.Core;
using Capstone.Graphics.Sprites;
using Capstone.Resources;

namespace Capstone.Entities
{
    public class TreeEntity : IEntityGenerator
    {
        public string TypeName
        {
            get { return "tree"; }
        }

        public Entity Generate(EntitySet set, ResourceCache cache, string entityName, Dictionary<string, object> parameters)
        {
            var e = set.Create(entityName);

            var visual = e.AddComponent<TileSprite>();
            visual.Load(cache, "ms-appx:///Data/TestMap.map");

            var clickArea = e.AddComponent<ClickArea>();
            clickArea.TileSprite = visual;

            return e;
        }
    }
}
