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

            var visual = e.AddComponent<Sprite>();
            visual.Load(cache, "Data/Texture/tree.png");

            var clickArea = e.AddComponent<ClickArea>();
            clickArea.BaseWidth = 96;
            clickArea.BaseHeight = 96;

            return e;
        }
    }
}
