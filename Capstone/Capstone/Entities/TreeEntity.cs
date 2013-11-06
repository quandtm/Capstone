using System.Collections.Generic;
using Capstone.Components;
using Capstone.Core;

namespace Capstone.Entities
{
    public class TreeEntity : IEntityGenerator
    {
        public string TypeName
        {
            get { return "treeentity"; }
        }

        public Entity Generate(EntitySet set, string entityName, Dictionary<string, object> parameters)
        {
            var e = set.Create(entityName);

            var clickArea = e.AddComponent<ClickArea>();
            clickArea.BaseWidth = parameters.GetOrDefault("BaseWidth", 1f);
            clickArea.BaseHeight = parameters.GetOrDefault("BaseHeight", 1f);

            return e;
        }
    }
}
