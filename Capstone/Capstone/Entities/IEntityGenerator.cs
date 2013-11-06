using System.Collections.Generic;
using Capstone.Core;
using Capstone.Resources;

namespace Capstone.Entities
{
    public interface IEntityGenerator
    {
        string TypeName { get; }
        Entity Generate(EntitySet set, ResourceCache cache, string entityName, Dictionary<string, object> parameters);
    }
}
