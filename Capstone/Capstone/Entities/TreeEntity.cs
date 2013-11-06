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

            var clickArea = e.AddComponent<ClickArea>();
            clickArea.BaseWidth = parameters.GetOrDefault("BaseWidth", 1f);
            clickArea.BaseHeight = parameters.GetOrDefault("BaseHeight", 1f);
            clickArea.Selectable = parameters.GetOrDefault("Selectable", true);

            var visual = e.AddComponent<TileSprite>();
            visual.Load(cache, "ms-appx:///Data/TestMap.map");

            return e;
        }
    }
}
